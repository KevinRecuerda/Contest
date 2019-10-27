using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev.Mar18.Ex4
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
            var words = new string[n];
            for (var i = 0; i < n; i++)
            {
                words[i] = ConsoleHelper.ReadLine();
            }

            var res = Find(words);
            if (res == "")
                res = "KO";

            ConsoleHelper.WriteLine(res);
        }

        private static string Find(string[] words)
        {
            var w = words[0];
            if (w.Length == 0)
                return "";

            // Ignore
            var ignoreWords = new string[words.Length];
            ignoreWords[0] = w.Substring(1);
            for (var i = 1; i < words.Length; i++)
                ignoreWords[i] = words[i];
            var max = Find(ignoreWords);

            // Take
            var isOk = true;
            var takeWords = new string[words.Length];
            takeWords[0] = w.Substring(1);
            for (var i = 1; i < words.Length; i++)
            {
                var idx = words[i].IndexOf(w[0]);
                if (idx < 0)
                {
                    isOk = false;
                    break;
                }

                takeWords[i] = words[i].Substring(idx+1);
            }

            if (isOk)
            {
                var take = w[0] + Find(takeWords);
                if (take.Length > max.Length)
                    max = take;
            }

            return max;
        }
    }
}
