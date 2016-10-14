﻿namespace GfPolynoms.GaloisFields
{
    using System;
    using System.Collections.Generic;

    public abstract class GaloisField
    {
        /// <summary>
        /// Представление элементов по соответствующим им степеням порождающего элемента
        /// </summary>
        protected int[] ElementsByPowers { get; private set; }
        /// <summary>
        /// Степени порождающего элемента по соответствующим им элементам
        /// </summary>
        protected int[] PowersByElements { get; private set; }


        /// <summary>
        ///     Порядок поля, простое число в некоторой степени
        /// </summary>
        public int Order { get; private set; }
        /// <summary>
        ///     Характеристика поля, степень простого числа в порядке поля
        /// </summary>
        public int Characteristic { get; private set; }

        /// <summary>
        /// Метод для разложения переданного порядка поля на множители
        /// </summary>
        /// <param name="order">Порядок поля, переданный в конструктор</param>
        /// <returns>Частичное разложение порядка на множители</returns>
        protected Dictionary<int, int> AnalyzeOrder(int order)
        {
            var fractions = new Dictionary<int, int>();

            for (var i = 2; i*i <= order && order > 1; i++)
            {
                if(order % i != 0)
                    continue;

                fractions[i] = 0;
                if (fractions.Count > 1)
                    return fractions;

                while (order % i == 0)
                {
                    fractions[i]++;
                    order /= i;
                }
            }

            if (order != 1)
                fractions[order] = 1;
            return fractions;
        }

        /// <summary>
        /// Метод для инициализации внутренних объектов поля
        /// </summary>
        /// <param name="order">Порядок поля</param>
        /// <param name="characteristic">Характеристика поля</param>
        protected void Initialize(int order, int characteristic)
        {
            Order = order;
            Characteristic = characteristic;

            PowersByElements = new int[order];
            PowersByElements[0] = -1;

            ElementsByPowers = new int[order - 1];
        }

        /// <summary>
        /// Проверка, являются ли переданные операнды элементами поля
        /// </summary>
        protected void ValidateArguments(int a, int b)
        {
            if (IsFieldElement(a) == false)
                throw new ArgumentException($"Element {a} is not field member");
            if (IsFieldElement(b) == false)
                throw new ArgumentException($"Element {b} is not field member");
        }

        /// <summary>
        ///     Проверка, входит ли переданное значение в поле
        /// </summary>
        public bool IsFieldElement(int a)
        {
            return a >= 0 && a < Order;
        }

        /// <summary>
        ///     Сложение двух элементов поля
        /// </summary>
        /// <param name="a">Первое слагаемое</param>
        /// <param name="b">Второе слагаемое</param>
        /// <returns>Сумма</returns>
        public abstract int Add(int a, int b);

        /// <summary>
        ///     Вычетание двух элементов поля
        /// </summary>
        /// <param name="a">Уменьшаемое</param>
        /// <param name="b">Вычетаемое</param>
        /// <returns>Разность</returns>
        public abstract int Subtract(int a, int b);

        /// <summary>
        ///     Умножение двух элементов поля
        /// </summary>
        /// <param name="a">Первый множитель</param>
        /// <param name="b">Второй множитель</param>
        /// <returns>Произведение</returns>
        public int Multiply(int a, int b)
        {
            ValidateArguments(a, b);

            if (a == 0 || b == 0)
                return 0;
            return ElementsByPowers[(PowersByElements[a] + PowersByElements[b]) % (Order - 1)];
        }

        /// <summary>
        ///     Деление двух элементов поля
        /// </summary>
        /// <param name="a">Делимое</param>
        /// <param name="b">Делитель</param>
        /// <returns>Частное</returns>
        public int Divide(int a, int b)
        {
            ValidateArguments(a, b);
            if (b == 0)
                throw new ArgumentException(nameof(b));

            return a == 0
                ? 0
                : ElementsByPowers[(PowersByElements[a] - PowersByElements[b] + Order - 1) % (Order - 1)];
        }

        /// <summary>
        /// Inverts field element
        /// </summary>
        /// <param name="a">Invetible element</param>
        /// <returns>Inverse osite element</returns>
        public abstract int InverseForAddition(int a);

        /// <summary>
        /// Inverts field element
        /// </summary>
        /// <param name="a">Invetible element</param>
        /// <returns>Inverse osite element</returns>
        public int InverseForMultiplication(int a)
        {
            if (IsFieldElement(a) == false)
                throw new ArgumentException($"Element {a} is not field member");
            if (a == 0)
                throw new ArgumentException("Can't inverse zero");

            return ElementsByPowers[(Order - 1 - PowersByElements[a]) % (Order - 1)];
        }

        /// <summary>
        /// Exponentiation of the generation element to the specified degree
        /// </summary>
        /// <param name="degree">Power for exponentiation</param>
        /// <returns>Exponentiation result</returns>
        public int GetGeneratingElementPower(int degree)
        {
            return degree >= 0
                ? ElementsByPowers[degree % (Order - 1)]
                : InverseForMultiplication(ElementsByPowers[(-degree) % (Order - 1)]);
        }

        /// <summary>
        /// Exponentiation of given element to the specified degree
        /// </summary>
        /// <param name="element">Element for exponentiation</param>
        /// <param name="degree">Power for exponentiation</param>
        public int Pow(int element, int degree)
        {
            if (IsFieldElement(element) == false)
                throw new ArgumentException($"Element {element} is not field member");

            if (degree == 0)
                return 1;
            return element == 0 ? 0 : GetGeneratingElementPower(PowersByElements[element]*degree);
        }
    }
}