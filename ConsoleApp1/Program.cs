using DecimalMath;
using System;
using System.Reflection;

namespace Program
{
    class Prog
    {
        public static decimal Derivative(Func<decimal, decimal> func, decimal x, decimal epsilon)
        {
            return (func(x + epsilon) - func(x - epsilon)) / (2 * epsilon);
        }

        // Метод половинного деления
        public static void BisectionMethod(Func<decimal, decimal> func, decimal epsilon, decimal leftRange, decimal rightRange)
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

        // Метод Ньютона 
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

        public static decimal SecondDerivative(Func<decimal, decimal> func, decimal x, decimal epsilon)
        {
            return (Derivative(func, x + epsilon, epsilon) - Derivative(func, x - epsilon, epsilon)) / (2 * epsilon);
        }

        // Метод простых итераций
        public static void SimpleIterationMethod(Func<decimal, decimal> func, decimal epsilon, decimal leftRange, decimal rightRange)
        {
            decimal h = (rightRange - leftRange) / 20;
            decimal maxDerivative = 0;
            decimal derivativeSign = 0;
            List<decimal> criticalPoints = new List<decimal> { leftRange, rightRange };
            for (int i = 1; i < 20; i++)
            {
                decimal x = leftRange + i * h;
                decimal secondDer = SecondDerivative(func, x, epsilon);
                decimal secondDerNext = SecondDerivative(func, x + h, epsilon);
                if (secondDer * secondDerNext < 0) criticalPoints.Add(x);
            }

            foreach (decimal x in criticalPoints)
            {
                decimal derivative = Derivative(func, x, epsilon);
                decimal absDer = Math.Abs(derivative);
                if (absDer > maxDerivative)
                {
                    maxDerivative = absDer;
                    derivativeSign = (derivative > 0) ? 1 : -1;
                }
            }
            decimal lambda = -derivativeSign / maxDerivative;
            List<decimal> results = new List<decimal>();
            decimal xn = leftRange + (rightRange - leftRange) / 2;
            decimal xnplusone = xn + lambda * func(xn);
            results.Add(xn);
            results.Add(xnplusone);
            while (Math.Abs(xnplusone - xn) >= epsilon)
            {
                xn = xnplusone;
                xnplusone = xn + lambda * func(xn);
                results.Add(xnplusone);
            }
            Console.WriteLine("Метод простых итераций");
            Console.WriteLine("Промежуточные результаты:");
            int idx = 0;
            foreach (decimal x in results)
            {
                Console.WriteLine($"x{idx}: {x}");
                idx++;
            }
            Console.WriteLine($"Результат: {xnplusone}");
        }

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            decimal epsilon = 0.0001m;
            decimal left_bound = 1;
            decimal right_bound = 2;
            Func<decimal, decimal> initial_func = x => DecimalEx.Pow(2, x) - 5 * DecimalEx.Pow(x, 2) + 10;
            BisectionMethod(initial_func, epsilon, left_bound, right_bound);
            Console.WriteLine("\n\n\n");
            NutonMethod(initial_func, epsilon, left_bound, right_bound);
            Console.WriteLine("\n\n\n");
            SimpleIterationMethod(initial_func, epsilon, left_bound, right_bound);
        }
    }
}