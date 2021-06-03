using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev_2021_06.Ex3
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
            var matrix = new string[20];
            for (var i = 0; i < 20; i++)
                matrix[i] = ConsoleHelper.ReadLine();

            var min = new int[10];
            for (var i = 0; i < 10; i++)
            {
                min[i] = 0;
                while (min[i] < 20 && matrix[min[i]][i] != '#')
                    min[i]++;
            }

            var sorted = min.OrderByDescending(x => x).ToList();
            var res = "NOPE";
            if (sorted[0] - 4 >= sorted[1])
            {
                var index = min.Select((x, i) => (x, i)).Where((g => g.x == sorted[0])).First().i;
                
                if (matrix[sorted[0] -1].Count(x => x == '#') == 9
                && matrix[sorted[0] -2].Count(x => x == '#') == 9
                && matrix[sorted[0] -3].Count(x => x == '#') == 9
                && matrix[sorted[0] -4].Count(x => x == '#') == 9
                )
                    res = $"BOOM {index+1}";
            }

            ConsoleHelper.WriteLine(res);
        }
    }
}