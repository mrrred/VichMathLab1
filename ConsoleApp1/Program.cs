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

            string time = DateTime.Now.ToString("HHmmss");
            string filename = $"BisectionMethod_{time}.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (decimal x in result)
                {
                    writer.WriteLine(x);
                }
                writer.WriteLine(middle);
            }
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

            string time = DateTime.Now.ToString("HHmmss");
            string filename = $"NutonMethod_{time}.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (decimal x in results)
                {
                    writer.WriteLine(x);
                }
            }
        }

        public static decimal SecondDerivative(Func<decimal, decimal> func, decimal x, decimal epsilon)
        {
            return (Derivative(func, x + epsilon, epsilon) - Derivative(func, x - epsilon, epsilon)) / (2 * epsilon);
        }

        public static void SimpleIteration2(Func<decimal, decimal> phi, decimal epsilon, decimal leftRange, decimal rightRange)
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
            Console.WriteLine("Метод простых итераций через аналитически определенные эквивалентные функции");
            Console.WriteLine("Промежуточные результаты:");
            int idx = 0;
            foreach (decimal x in results)
            {
                Console.WriteLine($"x{idx}: {x}");
                idx++;
            }
            Console.WriteLine($"Результат: {xnplusone}");

            string time = DateTime.Now.ToString("HHmmss");
            string filename = $"SimpleIteration2_{time}.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (decimal x in results)
                {
                    writer.WriteLine(x);
                }
            }
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
            Console.WriteLine("Метод простых итераций через обобщенный случай эквивалентной функции");
            Console.WriteLine("Промежуточные результаты:");
            int idx = 0;
            foreach (decimal x in results)
            {
                Console.WriteLine($"x{idx}: {x}");
                idx++;
            }
            Console.WriteLine($"Результат: {xnplusone}");

            string time = DateTime.Now.ToString("HHmmss");
            string filename = $"SimpleIterationMethod_{time}.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (decimal x in results)
                {
                    writer.WriteLine(x);
                }
            }
        }

        public static void Menu()
        {
            while (true)
            {
                Console.WriteLine("Выберите метод для решения:");
                Console.WriteLine("1.Метод половинного деления");
                Console.WriteLine("2.Метод Ньютона");
                Console.WriteLine("3.Метод простых итераций через обобщенный случай эквивалентной функции");
                Console.WriteLine("4.Метод простых итераций через аналитически определенные эквивалентные функции");
                Console.WriteLine("Иной ввод - каждый метод последовательно");
                Console.WriteLine("q - выход");
                Console.Write("Ваш выбор: ");

                string methodChoice = Console.ReadLine();

                if (methodChoice?.ToLower() == "q") break;

                Console.Write("\nВведите точность (Enter - по умолчанию 0.0001): ");
                string epsilonInput = Console.ReadLine() ?? "";
                decimal epsilon = string.IsNullOrWhiteSpace(epsilonInput) ? 0.0001m : decimal.Parse(epsilonInput);

                Console.Write("Введите левую границу интервала (Enter - по умолчанию 1): ");
                string leftInput = Console.ReadLine() ?? "";
                decimal leftBound = string.IsNullOrWhiteSpace(leftInput) ? 1m : decimal.Parse(leftInput);

                Console.Write("Введите правую границу интервала (Enter - по умолчанию 2): ");
                string rightInput = Console.ReadLine() ?? "";
                decimal rightBound = string.IsNullOrWhiteSpace(rightInput) ? 2m : decimal.Parse(rightInput);

                Func<decimal, decimal> func = x => DecimalEx.Pow(2, x) - 5 * DecimalEx.Pow(x, 2) + 10;
                Console.WriteLine($"\nПараметры: точность = {epsilon}, [a, b] = [{leftBound}, {rightBound}]\n");
                switch (methodChoice)
                {
                    case "1":
                        BisectionMethod(func, epsilon, leftBound, rightBound);
                        break;
                    case "2":
                        NutonMethod(func, epsilon, leftBound, rightBound);
                        break;
                    case "3":
                        SimpleIterationMethod(func, epsilon, leftBound, rightBound);
                        break;
                    case "4":
                        Func<decimal, decimal> phi = null;
                        string phiInfo = "";
                        if (leftBound == -2 && rightBound == -1)
                        {
                            phi = x => -DecimalEx.Sqrt((DecimalEx.Pow(2, x) + 10) / 5);
                            phiInfo = "fi(x) = -sqrt((2^x + 10) / 5)";
                            Console.WriteLine($"Эквивалентное преобразование: {phiInfo}\n");
                            SimpleIteration2(phi, epsilon, leftBound, rightBound);
                        }
                        else if (leftBound == 1 && rightBound == 2)
                        {
                            phi = x => DecimalEx.Sqrt((DecimalEx.Pow(2, x) + 10) / 5);
                            phiInfo = "fi(x) = sqrt((2^x + 10) / 5)";
                            Console.WriteLine($"Эквивалентное преобразование: {phiInfo}\n");
                            SimpleIteration2(phi, epsilon, leftBound, rightBound);
                        }
                        else if (leftBound == 8 && rightBound == 9)
                        {
                            phi = x => DecimalEx.Log(5 * DecimalEx.Pow(x, 2) - 10) / DecimalEx.Log(2);
                            phiInfo = "fi(x) = ln(5x^2 - 10) / ln(2)";
                            Console.WriteLine($"Эквивалентное преобразование: {phiInfo}\n");
                            SimpleIteration2(phi, epsilon, leftBound, rightBound);
                        }
                        else
                        {
                            Console.WriteLine($"Заданный интервал не соответствует аналитически определённым начальным приближениям\n");
                            SimpleIterationMethod(func, epsilon, leftBound, rightBound);
                        }
                        break;
                    default:
                        BisectionMethod(func, epsilon, leftBound, rightBound);
                        Console.WriteLine("\n\n\n");
                        NutonMethod(func, epsilon, leftBound, rightBound);
                        Console.WriteLine("\n\n\n");
                        SimpleIterationMethod(func, epsilon, leftBound, rightBound);
                        break;
                }
                Console.Write("Нажмите Enter для возврата в меню или введите q для выхода: ");
                string continueChoice = Console.ReadLine();

                if (continueChoice?.ToLower() == "q") break;
            }
        }

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Menu();
        }
    }
}