using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Contest.MDF18.Ex4
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
            var line = this.ReadLine();

            return ConvertTo<T>(line);
        }

        public string[] ReadLineAndSplit(char delimiter = ' ')
        {
            var splittedLine = this.ReadLine().Split(delimiter);
            return splittedLine;
        }

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
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
            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
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
            var size = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = size[0];
            var m = size[1];
            M = m;
            var prices = new double[n][];
            for (var i = 0; i < n; i++)
            {
                prices[i] = ConsoleHelper.ReadLineAndSplitAsListOf<double>().ToArray();
            }

            couples = new Dictionary<int, List<double>>();
            for (var i = 0; i < m; i++)
                    couples[i] = new List<double>();

            // find best
            //for (var k = 1; k <= m; k++)
            //{
            //    Find(prices, 1, 0, k, new HashSet<int>());
            //}
            Find(prices, 1, 0, m, new HashSet<int>());
            //var r = Find(prices, 1, 0, m);

            var dico = couples.ToDictionary(x => x.Key, x => x.Value.Max());
            var ratio = dico.OrderByDescending(x => Math.Pow(x.Value, m / x.Key)).ToList();
            var res = ratio.Select(x => Math.Pow(x.Value, m / x.Key)).First();
            ConsoleHelper.WriteLine(Math.Max(res*10000,10000));
        }

        private static int M;
        private static Dictionary<int, List<double>> couples; 
        public static void Find(double[][] prices, double fxRate, int current, int m, HashSet<int> alreadySeen)
        {
            alreadySeen.Add(current);
            if (m == 1 || current == 0 && alreadySeen.Count > 1)
            {
                //return fxRate*prices[current][0];
                couples[M-m].Add(fxRate*prices[current][0]);
                return;
            }

            for (var i = 0; i < prices.Length; i++)
            {
                if (i == current)
                    continue;

                var copy = new HashSet<int>(alreadySeen);
                var adjustedFxRate = fxRate * prices[current][i];
                Find(prices, adjustedFxRate, i, m - 1, copy);
            }
        }
    }
}
