using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2019_11.Ex6
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
            var p = ConsoleHelper.ReadLineAndSplitAsListOf<int>();

            var minV = int.MaxValue;
            var maxV = p.Max();
            var excluded = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                var line = ConsoleHelper.ReadLine();
                excluded[i] = string.IsNullOrEmpty(line) ? new List<int>() : line.Split(' ').Select(x => int.Parse(x)).ToList();
                foreach (var j in excluded[i])
                {
                    var v = p[i] + p[j];
                    minV = Math.Min(minV, v);
                }
                foreach (var j in Enumerable.Range(0, n).Except(excluded[i]))
                {
                    if (i == j)
                        continue;

                    var v = p[i] + p[j];
                    maxV = Math.Max(maxV, v);
                }
            }

            var res = minV > maxV ? maxV : -1;
            ConsoleHelper.WriteLine(res);
        }
    }
}
