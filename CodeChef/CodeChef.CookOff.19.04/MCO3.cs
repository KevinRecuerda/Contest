using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CodeChef.April19.MCO3
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
            return (T) Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
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

        private static void Solve()
        {
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<long>();
            var n = input[0];
            var a = input[1];
            var b = input[2];
            var x = input[3];
            var y = input[4];
            var z = input[5];

            var c = ConsoleHelper.ReadLineAndSplitAsListOf<long>();
            var cByLevel = c.GroupBy(i => (int)Math.Log(i)).ToDictionary(i => i.Key, i => i.ToList());

            var finalRound = (z - b) / y;

            var ppUser = a + x * finalRound;
            var missingUsers = z - ppUser;
            var contrib = 0L;

            var maxLevel = cByLevel.Keys.Max();
            for (var lvl = maxLevel; lvl >= 0 && missingUsers > 0; lvl--)
            {
                if (!cByLevel.ContainsKey(lvl-1))
                    cByLevel[lvl-1] = new List<long>();

                var sortedC = cByLevel[lvl].OrderByDescending(i => i).ToList();
                for (var i = 0; i < sortedC.Count && missingUsers>0; i++)
                {
                    missingUsers -= sortedC[i];
                    contrib++;
                    cByLevel[lvl-1].Add(sortedC[i]/2);
                }
            }

            var result = missingUsers > 0 ? "RIP" : contrib.ToString();
            ConsoleHelper.WriteLine(result);
        }
    }
}