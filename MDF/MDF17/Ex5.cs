using System;
using System.Collections.Generic;
using System.Linq;

namespace MDF17.Ex5
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
            // map
            var n = ConsoleHelper.ReadLineAs<int>();
            var map = new int[n, n];
            var totalGates = 0;
            for (var i = 0; i < n; i++)
            {
                var line = ConsoleHelper.ReadLine();
                for (var j = 0; j < n; j++)
                {
                    var cellScore = line[j] == 'X' ? 1 : 0;

                    map[i, j] = cellScore;
                    totalGates += cellScore;
                }
            }
            var count = 0;
            var gatesReached = 0;
            while (gatesReached < totalGates)
            {
                var minCol = 0;
                for (var i = 0; i < n; i++)
                {
                    for (var j = minCol; j < n; j++)
                    {
                        if (map[i, j] == 0)
                            continue;

                        map[i, j] = 0;
                        minCol = j;
                        gatesReached++;
                    }
                }
                count++;
            }

            ConsoleHelper.WriteLine(count);
        }
    }
}
