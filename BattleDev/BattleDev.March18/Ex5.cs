using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BattleDev.March18.Ex5
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
            var matrix = new int[n][];
            for (var i = 0; i < n; i++)
            {
                var line = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                matrix[i] = line.ToArray();

            }

            var best = GetBestValue(n, matrix);

            ConsoleHelper.WriteLine(best);
        }

        private static int GetBestValue(int n, int[][] matrix)
        {
            var bestMatrix = new int[n][];
            for (var k = 0; k < n; k++)
                bestMatrix[k] = new int[n];

            for (var i = 0; i < n; i++)
            {
                for (var k = 0; k < n; k++)
                {
                    bestMatrix[k][i] = matrix[k][(k+i)%n];
                    for (var j = 2; j <= i; j++)
                    {
                        var gain = bestMatrix[k][j-1] + bestMatrix[(k + j) % n][i-j];
                        //var gain = bestMatrix[k][j-1] + matrix[(k+j)%n][(k+i)%n];
                        bestMatrix[k][i] = Math.Max(bestMatrix[k][i], gain);
                    }

                    if (i >= 3)
                    {
                        var gain = bestMatrix[(k + 1) % n][i - 2] + matrix[k][(k + i) % n];
                        bestMatrix[k][i] = Math.Max(bestMatrix[k][i], gain);
                    }
                }
            }

            var max = 0;
            for (var i = 0; i < n; i++)
                max = Math.Max(bestMatrix[i][n - 1], max);

            return max;
        }
    }
}
