using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev_2021_06.Ex5
{
    #region ConsoleHelper

    public interface IConsoleHelper
    {
        string ReadLine();
        T ReadLineAs<T>();

        string[] ReadLineAndSplit(char delimiter = ' ');
        List<T> ReadLineAndSplitAs<T>(char delimiter = ' ');

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
            var line = this.ReadLine();

            return ConvertTo<T>(line);
        }

        public string[] ReadLineAndSplit(char delimiter = ' ')
        {
            var splittedLine = this.ReadLine().Split(delimiter);
            return splittedLine;
        }

        public List<T> ReadLineAndSplitAs<T>(char delimiter = ' ')
        {
            var splittedLine = this.ReadLineAndSplit();

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

    #region Extensions

    public static class Extensions
    {
        public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default;
            rest = list.Skip(1).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default;
            second = list.Count > 1 ? list[1] : default;
            rest = list.Skip(2).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default;
            second = list.Count > 1 ? list[1] : default;
            third = list.Count > 2 ? list[2] : default;
            rest = list.Skip(3).ToList();
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
            Solve();
        }

        public static void Solve()
        {
            var (n, a, c, _) = ConsoleHelper.ReadLineAndSplitAs<int>();
            // var observations = ConsoleHelper.ReadLineAndSplitAs<int>();
            var observations = ConsoleHelper.ReadLine().Split().Select(int.Parse).ToList();

            // var matrix = new int [a + c, n];
            // matrix[0, 0] = observations[0];
            // matrix[1, 0] = 0;
            // for (var j = 1; j < n; j++)
            // {
            //     var previous = matrix[0, j - 1];
            //     if (j >= a + c)
            //         previous = Math.Min(previous, matrix[a + c - 1, j - 1]);
            //     matrix[0, j] = previous + observations[j];
            //
            //     for (var i = 1; i < Math.Min(a + c, j + 2); i++)
            //     {
            //         matrix[i, j] = matrix[i - 1, j - 1];
            //         if (i>a)
            //             matrix[i, j] += observations[j];
            //     }
            // }

            // var min = int.MaxValue;
            // for (var i = 0; i < a+c; i++)
            //     if (matrix[i, n - 1] < min)
            //         min = matrix[i, n - 1];

            var matrix = new int[n];
            for (var i = 0; i < n; i++)
                matrix[i] = int.MaxValue;

            var w = 0;
            for (var i = a; i < a + c; i++)
                w += observations[i];

            matrix[0] = observations[0];
            var next = Math.Min(a + c - 1, n - 1);
            matrix[next] = Math.Min(matrix[next], w);

            w -= observations[a];
            if (a + c < n)
                w += observations[a + c];

            for (var i = 1; i < n; i++)
            {
                matrix[i] = Math.Min(matrix[i], matrix[i - 1] + observations[i]);

                next = Math.Min(i + a + c - 1, n - 1);
                matrix[next] = Math.Min(matrix[next], matrix[i - 1] + w);

                if (i + a < n)
                    w -= observations[i + a];
                if (i + a + c < n)
                    w += observations[i + a + c];
            }

            ConsoleHelper.WriteLine(matrix[n - 1]);
        }
    }
}