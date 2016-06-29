﻿namespace WaveletCodesTools.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GfAlgorithms.CombinationsCountCalculator;
    using GfAlgorithms.LinearSystemSolver;
    using GfPolynoms;
    using GfPolynoms.Extensions;
    using GfPolynoms.GaloisFields;
    using JetBrains.Annotations;
    using ListDecoderForFixedDistanceCodes;
    using RsCodesTools.ListDecoder;
    using RsCodesTools.ListDecoder.GsDecoderDependencies.InterpolationPolynomialBuilder;
    using RsCodesTools.ListDecoder.GsDecoderDependencies.InterpolationPolynomialFactorisator;
    using Xunit;

    public class GsBasedDecoderTests
    {
        private readonly GsBasedDecoder _decoder;

        [UsedImplicitly]
        public static readonly IEnumerable<object[]> DecoderTestsData;

        private static Tuple<FieldElement, FieldElement>[] GenerateCodeword(int n, Polynomial generationPolynomial, Polynomial informationPolynomial)
        {
            var field = informationPolynomial.Field;

            var m = new Polynomial(field, 1).RightShift(n);
            m[0] = field.InverseForAddition(1);
            var c = (informationPolynomial.RaiseVariableDegree(2) * generationPolynomial) % m;

            var i = 0;
            var codeword = new Tuple<FieldElement, FieldElement>[n];
            for (; i <= c.Degree; i++)
                codeword[i] = new Tuple<FieldElement, FieldElement>(new FieldElement(field, field.GetGeneratingElementPower(i)),
                    new FieldElement(field, c[i]));
            for (; i < n; i++)
                codeword[i] = new Tuple<FieldElement, FieldElement>(new FieldElement(field, field.GetGeneratingElementPower(i)),
                    field.Zero());

            return codeword;
        }

        private static Tuple<FieldElement, FieldElement>[] AddRandomNoise(Tuple<FieldElement, FieldElement>[] codeword, int errorsCount)
        {
            var random = new Random();
            var errorsPositions = new HashSet<int>();
            while (errorsPositions.Count < errorsCount)
                errorsPositions.Add(random.Next(codeword.Length));

            var one = codeword[0].Item1.Field.One();
            foreach (var errorPosition in errorsPositions)
                codeword[errorPosition].Item2.Add(one);

            return codeword;
        }

        private static object[] PrepareTestData(int n, int k, int d, Polynomial generationPolynomial, Polynomial informationPolynomial, int randomErrorsCount)
        {
            return new object[]
                   {
                       n, k, d, generationPolynomial,
                       AddRandomNoise(GenerateCodeword(n, generationPolynomial, informationPolynomial), randomErrorsCount), n - randomErrorsCount,
                       informationPolynomial
                   };
        }

        static GsBasedDecoderTests()
        {
            var gf7 = new PrimeOrderField(7);
            var generationPolynomial1 = new Polynomial(gf7, 4, 2, 6, 4, 3, 4)
                                        + new Polynomial(gf7, 1, 2, 1, 5, 2, 1).RightShift(2);
            var gf11 = new PrimeOrderField(11);
            var generationPolynomial2 = new Polynomial(gf11, 0, 0, 7, 3, 4, 1, 8, 1, 8, 2, 7, 5);
            var generationPolynomial3 = new Polynomial(gf11, 0, 0, 2, 0, 10, 9, 3, 9, 3, 10, 2, 2);

            var gf13 = new PrimeOrderField(13);
            var generationPolynomial4 = new Polynomial(gf13, 0, 0, 0, 8, 1, 12, 2, 11, 5, 6, 4, 2, 3, 12, 2, 4);

            DecoderTestsData = new[]
                               {
                                   PrepareTestData(6, 3, 3, generationPolynomial1, new Polynomial(gf7, 4, 0, 2), 1),
                                   PrepareTestData(6, 3, 3, generationPolynomial1, new Polynomial(gf7, 1, 2, 3), 1),
                                   PrepareTestData(6, 3, 3, generationPolynomial1, new Polynomial(gf7, 6, 4, 1), 1),
                                   PrepareTestData(6, 3, 3, generationPolynomial1, new Polynomial(gf7, 0, 2), 1),
                                   PrepareTestData(6, 3, 3, generationPolynomial1, new Polynomial(gf7, 0, 0, 3), 1),
                                   PrepareTestData(10, 5, 6, generationPolynomial2, new Polynomial(gf11, 1, 2, 3, 4, 5), 3),
                                   PrepareTestData(10, 5, 5, generationPolynomial3, new Polynomial(gf11, 1, 2, 3, 4, 5), 2),
                                   PrepareTestData(12, 6, 6, generationPolynomial4, new Polynomial(gf13, 1, 2, 3, 4, 5, 6), 3),
                                   PrepareTestData(12, 6, 6, generationPolynomial4, new Polynomial(gf13, 0, 2, 0, 2, 11), 3)
                               };
        }

        public GsBasedDecoderTests()
        {
            var linearSystemsSolver = new GaussSolver();
            _decoder = new GsBasedDecoder(new GsDecoder(
                    new SimplePolynomialBuilder(new PascalsTriangleBasedCalcualtor(), linearSystemsSolver),
                    new RrFactorizator()),
                linearSystemsSolver);
        }

        [Theory]
        [MemberData(nameof(DecoderTestsData))]
        public void ShouldFindOriginalInformationWordAmongPossibleVariants(int n, int k, int d, Polynomial generatingPolynomial,
            Tuple<FieldElement, FieldElement>[] decodedCodeword, int minCorrectValuesCount, Polynomial expectedInformationPolynomial)
        {
            // When
            var possibleVariants = _decoder.Decode(n, k, d, generatingPolynomial, decodedCodeword, minCorrectValuesCount);

            // Then
            Assert.True(possibleVariants.Contains(expectedInformationPolynomial));
        }
    }
}