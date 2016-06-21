﻿namespace RsListDecoding.Extensions
{
    using System;
    using System.Collections.Generic;
    using GfPolynoms.Extensions;

    public static class BiVariablePolynomialExtensions
    {
        private static BiVariablePolynomial Pow(IDictionary<int, BiVariablePolynomial> powersCache, BiVariablePolynomial polynomial, int degree)
        {
            BiVariablePolynomial result;

            if (powersCache.TryGetValue(degree, out result) == false)
            {
                if (degree == 0)
                    result = new BiVariablePolynomial(polynomial.Field) {[new Tuple<int, int>(0, 0)] = polynomial.Field.One()};
                else
                    result = Pow(powersCache, polynomial, degree - 1)*polynomial;
                powersCache[degree] = result;
            }

            return result;
        }

        public static BiVariablePolynomial PerformVariablesSubstitution(this BiVariablePolynomial polynomial,
            BiVariablePolynomial xSubstitution, BiVariablePolynomial ySubstitution)
        {
            if(polynomial.Field.Equals(xSubstitution.Field) == false)
                throw new ArithmeticException(nameof(xSubstitution));
            if (polynomial.Field.Equals(ySubstitution.Field) == false)
                throw new ArithmeticException(nameof(ySubstitution));

            var result = new BiVariablePolynomial(polynomial.Field);
            var xCache = new Dictionary<int, BiVariablePolynomial>();
            var yCache = new Dictionary<int, BiVariablePolynomial>();

            foreach (var coefficient in polynomial)
                result += coefficient.Value
                          *Pow(xCache, xSubstitution, coefficient.Key.Item1)
                          *Pow(yCache, ySubstitution, coefficient.Key.Item2);

            return result;
        }
    }
}