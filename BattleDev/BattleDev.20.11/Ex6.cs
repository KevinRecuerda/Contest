using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2020_11.Ex6
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

            var similar1 = 0;
            var similar2 = 0;
            for (var i = 0; i < n; i++)
            {
                var input = ConsoleHelper.ReadLineAndSplit();
                var f = input[0];
                var d = input[1];

                var occurenceF = new int[26];
                var occurenceD = new int[26];

                foreach (var c in f)
                    occurenceF[c - 'a']++;
                foreach (var c in d)
                    occurenceD[c - 'a']++;

                var similar = 0;
                for (var k = 0; k < 26; k++)
                    similar += Math.Min(occurenceD[k], occurenceF[k]);

                if (similar == 1)
                    similar1++;
                else if (similar >= 2)
                    similar2++;
            }

            var prio = (similar1 + similar2 - 1) % 2;
            var res = prio == 0 ? "DEBUNK" : "FAKE";
            ConsoleHelper.WriteLine(res);
        }
    }
}
