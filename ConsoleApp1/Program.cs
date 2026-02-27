using DecimalMath;
using System;
using System.Reflection;

namespace Program
{
    class Prog
    {

        public static void Half_Delenie(Func<decimal, decimal> func, decimal epsilon, decimal leftRange, decimal rightRange)
        {
            List<decimal> result = new List<decimal>();

            decimal middle = leftRange + (rightRange - leftRange) / 2;

            while (rightRange - leftRange >= 2 * epsilon)
            {
                middle = leftRange + (rightRange - leftRange) / 2;

                result.Add(middle);

                if ((func(leftRange) * func(middle)) < 0)
                {
                    rightRange = middle;
                }
                else if ((func(rightRange) * func(middle)) < 0)
                {
                    leftRange = middle;
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Метод половинного деления");

            Console.WriteLine("Промежуточные результаты:");

            int i = 1;

            foreach (decimal x in result)
            {
                Console.WriteLine($"{i}: {x}");
                i++;
            }

            Console.WriteLine($"Результат: {middle}");
            
        }

        public static decimal Derivative(Func<decimal, decimal> func, decimal x, decimal epsilon)
        {
            return (func(x + epsilon) - func(x - epsilon)) / (2 * epsilon);
        }

        public static void NutonMethod(Func<decimal, decimal> func, decimal epsilon, decimal leftRange, decimal rightRange)
        {
            List<decimal> results = new List<decimal>();

            decimal xn;

            if (func(leftRange) < 0)
            {
                xn = rightRange;
            }
            else if(func(rightRange) < 0)
            {
                xn = leftRange;
            }
            else
            {
                throw new ArgumentException();
            }

            results.Add(xn);

            decimal xnplusone = xn - (func(xn) / Derivative(func, xn, epsilon));

            results.Add(xnplusone);

            while (Math.Abs(xn - xnplusone) >= epsilon)
            {
                decimal buffer = xnplusone;

                xnplusone = xnplusone - (func(xnplusone) / Derivative(func, xnplusone, epsilon));

                xn = buffer;

                results.Add(xnplusone);
            }

            Console.WriteLine("Метод Ньютона");

            Console.WriteLine("Промежуточные результаты:");

            int i = 0;

            foreach (decimal x in results)
            {
                Console.WriteLine($"x{i}: {x}");
                i++;
            }

            Console.WriteLine($"Результат: {xnplusone}");


        }

        public static void SimpleIterationMethod(Func<decimal, decimal> phi, decimal epsilon, decimal leftRange, decimal rightRange)
        {
            List<decimal> results = new List<decimal>();
            decimal xn = leftRange + (rightRange - leftRange) / 2;
            decimal xnplusone = phi(xn);
            results.Add(xn);
            results.Add(xnplusone);
            while (Math.Abs(xnplusone - xn) >= epsilon)
            {
                xn = xnplusone;
                xnplusone = phi(xnplusone);
                results.Add(xnplusone);
            }
            Console.WriteLine("Метод простых итераций");
            Console.WriteLine("Промежуточные результаты:");
            int i = 0;
            foreach (decimal x in results)
            {
                Console.WriteLine($"x{i}: {x}");
                i++;
            }
            Console.WriteLine($"Результат: {xnplusone}");
        }

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Half_Delenie((x) => DecimalEx.Pow(2, x) - 5 * DecimalEx.Pow(x, 2) + 10, 0.0001m, 1, 2);

            Console.WriteLine("\n\n\n");

            NutonMethod((x) => DecimalEx.Pow(2, x) - 5 * DecimalEx.Pow(x, 2) + 10, 0.0001m, 1, 2);

            Console.WriteLine("\n\n\n");

            SimpleIterationMethod((x) => (DecimalEx.Pow(2, x) + 10) / (5 * x), 0.0001m, 1, 2); // преобразовали исх. ур., выразив х, получили фи(х)
        }
    }
}