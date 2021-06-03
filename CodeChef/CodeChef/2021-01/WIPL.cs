using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2021_01.WIPL
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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = input[0];
            var k = input[1];

            var h = ConsoleHelper.ReadLineAndSplitAsListOf<int>().OrderByDescending(x => x).ToArray();

            var res = FindMinimalBoxes(k, h);
            ConsoleHelper.WriteLine(res);
        }

        private static int FindMinimalBoxes(int k, int[] h)
        {
            if (h.Sum() < 2 * k)
                return -1;
            
            // minimal size for 1st tower
            var boxes = 0;
            var sum = 0;
            while (sum < 2 * k)
                sum += h[boxes++];

            // var minSum = FindMinimalSum(k, h.Take(boxes).ToArray());
            var matrix = new Matrix(h.Take(boxes).ToArray(), k);
            var minSum = matrix[k];

            // missing height for 2nd tower
            var delta = k - (sum - minSum);
            if (delta > h.Skip(boxes).Sum())
                return -1;
            
            while (delta > 0)
                delta -= h[boxes++];
            
            return boxes;
        }
    }
    public class Matrix
    {
        private int[] array;
        private int[] cumSum;

        private int?[,] matrix;

        public Matrix(int[] array, int max)
        {
            this.array = array.OrderBy(x => x).ToArray();

            var sum = 0;
            cumSum = this.array.Select(x => sum += x).ToArray();
            
            matrix = new int?[array.Length, max+1];
        }

        public int this[int k] => k + Find(array.Length-1, k);

        private int Find(int i, int j)
        {
            if (i < 0)
                return int.MaxValue;
            
            if (matrix[i, j].HasValue)
                return matrix[i, j].Value;

            var value = array[i];
            if (j == 0 || j == value)
                return 0;
            
            if (j > this.cumSum[i])
                return int.MaxValue;

            var take = j > value ? Find(i - 1, j - value) : value-j;
            var ignore = Find(i - 1, j);
            return Math.Min(take, ignore);
        }
    }
}