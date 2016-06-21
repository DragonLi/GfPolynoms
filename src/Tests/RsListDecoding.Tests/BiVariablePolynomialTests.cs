﻿namespace RsListDecoding.Tests
{
    using System;
    using System.Collections.Generic;
    using GfPolynoms;
    using GfPolynoms.Extensions;
    using GfPolynoms.GaluaFields;
    using JetBrains.Annotations;
    using Xunit;

    public class BiVariablePolynomialTests
    {
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> EvaluateTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> AddTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> MultiplyPolynomialsTestsData;
        [UsedImplicitly]
        public static readonly IEnumerable<object[]> MultiplyByFieldElementTestsData;

        static BiVariablePolynomialTests()
        {
            var gf5 = new PrimeOrderField(5);

            var polynomialForEvoluation = new BiVariablePolynomial(gf5)
                             {
                                 [new Tuple<int, int>(1, 1)] = new FieldElement(gf5, 2),
                                 [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 1),
                                 [new Tuple<int, int>(2, 0)] = new FieldElement(gf5, 1),
                                 [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 1),
                                 [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 4)
                             };
            EvaluateTestsData = new[]
                                {
                                    new object[] {polynomialForEvoluation, new FieldElement(gf5, 1), new FieldElement(gf5, 3), gf5.Zero()},
                                    new object[] {polynomialForEvoluation, new FieldElement(gf5, 2), new FieldElement(gf5, 4), gf5.Zero()},
                                    new object[] {polynomialForEvoluation, new FieldElement(gf5, 3), new FieldElement(gf5, 2), gf5.Zero()},
                                    new object[] {polynomialForEvoluation, new FieldElement(gf5, 2), new FieldElement(gf5, 3), gf5.Zero()},
                                };

            AddTestsData = new[]
                           {
                               new object[]
                               {
                                   new BiVariablePolynomial(gf5)
                                   {
                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 2),
                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 3)
                                   },
                                   new BiVariablePolynomial(gf5)
                                   {
                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 3),
                                       [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 4)
                                   },
                                   new BiVariablePolynomial(gf5)
                                   {
                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 3),
                                       [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 4)
                                   }
                               },
                               new object[]
                               {
                                   new BiVariablePolynomial(gf5)
                                   {
                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 2),
                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 3),
                                       [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 4)
                                   },
                                   new BiVariablePolynomial(gf5)
                                   {
                                       [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 4),
                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 4),
                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 4)

                                   },
                                   new BiVariablePolynomial(gf5)
                                   {
                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 2),
                                       [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 3),
                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 1)
                                   }
                               }
                           };

            MultiplyPolynomialsTestsData = new[]
                                           {
                                               new object[]
                                               {
                                                   new BiVariablePolynomial(gf5)
                                                   {
                                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 2),
                                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 3)
                                                   },
                                                   new BiVariablePolynomial(gf5)
                                                   {
                                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 3),
                                                       [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 4)
                                                   },
                                                   new BiVariablePolynomial(gf5)
                                                   {
                                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 1),
                                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 4),
                                                       [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 3),
                                                       [new Tuple<int, int>(1, 1)] = new FieldElement(gf5, 2)
                                                   }
                                               },
                                               new object[]
                                               {
                                                   new BiVariablePolynomial(gf5)
                                                   {
                                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 2),
                                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 3),
                                                       [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 4)
                                                   },
                                                   new BiVariablePolynomial(gf5)
                                                   {
                                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 4),
                                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 1),
                                                       [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 2)
                                                   },
                                                   new BiVariablePolynomial(gf5)
                                                   {
                                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 3),
                                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 4),
                                                       [new Tuple<int, int>(2, 0)] = new FieldElement(gf5, 3),
                                                       [new Tuple<int, int>(0, 2)] = new FieldElement(gf5, 3)
                                                   }
                                               },
                                               new object[]
                                               {
                                                   new BiVariablePolynomial(gf5)
                                                   {
                                                       [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 2),
                                                       [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 3),
                                                       [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 4)
                                                   },
                                                   new BiVariablePolynomial(gf5),
                                                   new BiVariablePolynomial(gf5)
                                               }
                                           };

            var polynomialForMultiplication = new BiVariablePolynomial(gf5)
                                              {
                                                  [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 3),
                                                  [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 2),
                                                  [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 4),
                                                  [new Tuple<int, int>(1, 1)] = new FieldElement(gf5, 1)
                                              };
            MultiplyByFieldElementTestsData = new[]
                                              {
                                                  new object[] {polynomialForMultiplication, gf5.Zero(), new BiVariablePolynomial(gf5)},
                                                  new object[]
                                                  {
                                                      polynomialForMultiplication, new FieldElement(gf5, 2),
                                                      new BiVariablePolynomial(gf5)
                                                      {
                                                          [new Tuple<int, int>(0, 0)] = new FieldElement(gf5, 1),
                                                          [new Tuple<int, int>(1, 0)] = new FieldElement(gf5, 4),
                                                          [new Tuple<int, int>(0, 1)] = new FieldElement(gf5, 3),
                                                          [new Tuple<int, int>(1, 1)] = new FieldElement(gf5, 2)
                                                      }
                                                  }
                                              };
        }

        [Theory]
        [MemberData(nameof(EvaluateTestsData))]
        public void ShouldEvaluateValue(BiVariablePolynomial polynomial, FieldElement xValue, FieldElement yValue, FieldElement expectedResult)
        {
            Assert.Equal(expectedResult, polynomial.Evaluate(xValue, yValue));
        }

        [Theory]
        [MemberData(nameof(AddTestsData))]
        public void ShouldAddTwoPolynomials(BiVariablePolynomial a, BiVariablePolynomial b, BiVariablePolynomial expectedResult)
        {
            Assert.Equal(expectedResult, a + b);
        }

        [Theory]
        [MemberData(nameof(MultiplyPolynomialsTestsData))]
        public void ShouldMultiplyTwoPolynomials(BiVariablePolynomial a, BiVariablePolynomial b, BiVariablePolynomial expectedResult)
        {
            Assert.Equal(expectedResult, a*b);
        }

        [Theory]
        [MemberData(nameof(MultiplyByFieldElementTestsData))]
        public void ShouldMultiplyByFieldElement(BiVariablePolynomial a, FieldElement b, BiVariablePolynomial expectedResult)
        {
            Assert.Equal(expectedResult, a * b);
        }
    }
}