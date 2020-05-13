using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_01.CHFDORA
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

        public static int n, m;
        public static int[][] matrix;

        public static void Solve()
        {
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            n = input[0];
            m = input[1];

            matrix = new int[n][];
            for (var i = 0; i < n; i++)
                matrix[i] = ConsoleHelper.ReadLineAndSplitAsListOf<int>().ToArray();

            var count = n * m;
            for (var i = 1; i < n; i++)
                for (var j = 1; j < m; j++)
                    count += CalculatePalindromeSize(i, j);

            ConsoleHelper.WriteLine(count);
        }

        public static int CalculatePalindromeSize(int i, int j)
        {
            var maxSize = new[]
                {
                    i, n-1-i,
                    j, m-1-j
                }
                .Min();

            var size = 0;
            for (var k = 1; k <= maxSize; k++)
            {
                if (matrix[i][j-k] != matrix[i][j+k]
                    || matrix[i-k][j] != matrix[i+k][j])
                    break;

                size = k;
            }

            return size;
        }
    }
}