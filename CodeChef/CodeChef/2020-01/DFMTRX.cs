using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_01.DFMTRX
{
    #region ConsoleHelper

    public interface IConsoleHelper
    {
        string ReadLine();
        T ReadLineAs<T>();

        string[] ReadLineAndSplit(char delimiter = ' ');
        List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ');

        void WriteLine(object obj);
        void Debug(object obj);
    }

    public class ConsoleHelper : IConsoleHelper
    {
        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }

        public T ReadLineAs<T>()
        {
            var line = ReadLine();

            return ConvertTo<T>(line);
        }

        public string[] ReadLineAndSplit(char delimiter = ' ')
        {
            var splittedLine = ReadLine().Split(delimiter);
            return splittedLine;
        }

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = ReadLineAndSplit();

            return splittedLine.Select(ConvertTo<T>).ToList();
        }

        public virtual void WriteLine(object obj)
        {
            Console.WriteLine(obj);
        }

        public void Debug(object obj)
        {
            Console.Error.WriteLine(obj);
        }

        private static T ConvertTo<T>(string value)
        {
            return (T) Convert.ChangeType(value, typeof(T));
        }
    }

    #endregion

    public static class Program
    {
        public static IConsoleHelper ConsoleHelper;

        static Program()
        {
            ConsoleHelper = new ConsoleHelper();
        }

        public static void Main(string[] args)
        {
            SolveMultiple();
        }

        public static void SolveMultiple()
        {
            var t = ConsoleHelper.ReadLineAs<int>();
            for (var k = 0; k < t; k++)
            {
                Solve();
            }
        }

        public static void Solve()
        {
            var n = ConsoleHelper.ReadLineAs<int>();

            if (n % 2 == 1 && n > 1)
            {
                ConsoleHelper.WriteLine("Boo");
                return;
            }

            ConsoleHelper.WriteLine("Hooray");

            if (n == 1)
                ConsoleHelper.WriteLine("1");
            else
            {
                var matrix = new int[n][];
                for (var i = 0; i < n; i++)
                {
                    matrix[i] = new int[n];
                    matrix[i][i] = 1;
                }

                for (var i = 1; i < n; i++)
                {
                    for (var j = 0; j < i; j++)
                    {
                        // top/right corner
                        var value = i + j + 1;
                        if (value > n)
                            value -= n-1;

                        matrix[j][i] = value;

                        // bottom/left corner
                        value = i + j + n;
                            if (value > 2 * n - 1)
                                value -= n-1;

                        matrix[i][j] = value;
                    }
                }

                for (var i = 1; i < n / 2; i++)
                {
                    var odd = 2 * i + 1;
                    var even = odd - 1;
                    matrix[i][n - 1] = odd;
                    matrix[i + n / 2 - 1][n - 1] = even;

                    matrix[n - 1][i] = even + n;
                    matrix[n - 1][i + n / 2 - 1] = odd + n - 2;
                }

                Display(matrix);
            }
        }

        private static void Display(int[][] matrix)
        {
            foreach (var line in matrix) 
                ConsoleHelper.WriteLine(string.Join(" ", line));
        }
    }
}