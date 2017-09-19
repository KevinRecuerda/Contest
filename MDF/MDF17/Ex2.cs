using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDF17.Ex2
{
    #region ConsoleHelper
    public interface IConsoleHelper
    {
        string ReadLine();
        T ReadLineAs<T>();
        List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ');
        void WriteLine(object obj);
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

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = this.ReadLine().Split(delimiter);

            return splittedLine.Select(ConvertTo<T>).ToList();
        }

        public virtual void WriteLine(object obj)
        {
            Console.WriteLine(obj);
        }

        private static T ConvertTo<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
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
            var str = new StringBuilder();
            var map = new char[n, n];
            map[0, 0] = '.';
            map[0, n-1] = '.';
            map[n-1, 0] = '.';
            map[n-1, n-1] = '.';
            var format = true;
            for (var k = n; k > 0; k--)
            {
                for (var i = n - k; i < k; i++)
                {
                    for (var j = n - k; j < k; j++)
                    {
                        if (i == n - k && j == n - k
                            || i == n - k && j == k - 1
                            || i == k - 1 && j == n - k
                            || i == k - 1 && j == k - 1)
                            continue;

                        map[i, j] = format ? '*' : '#';
                    }
                }
                format = !format;
            }

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                    str.Append(map[i, j]);
                str.AppendLine(string.Empty);
            }

            ConsoleHelper.WriteLine(str.ToString());
        }
    }
}
