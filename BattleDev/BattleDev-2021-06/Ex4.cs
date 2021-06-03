using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev_2021_06.Ex4
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
    public static class Extensions {
        public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest) {

            first = list.Count > 0 ? list[0] : default;
            rest = list.Skip(1).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest) {
            first = list.Count > 0 ? list[0] : default;
            second = list.Count > 1 ? list[1] : default;
            rest = list.Skip(2).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out IList<T> rest) {
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
            var n = ConsoleHelper.ReadLineAs<int>();
            var detritus = ConsoleHelper.ReadLine();

            var grouped = detritus.GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());
            var current = grouped.Keys.ToDictionary(x => x, x => detritus.Substring(0, n / 2).Count(c => c == x));
            var res = 0;
            for (var i = 0; i < n; i++)
            {
                if (grouped.All(g => current[g.Key] * 2 == g.Value))
                    res++;

                current[detritus[i]]--;
                current[detritus[(i+n/2)%n]]++;
            }
            ConsoleHelper.WriteLine(res);
        }
    }
}