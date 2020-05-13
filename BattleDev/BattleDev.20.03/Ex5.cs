using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2020_03.Ex5
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
            return (T) Convert.ChangeType(value, typeof(T));
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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = input[0];
            var k = input[1];

            var recipients = ConsoleHelper.ReadLine();
            InitMatrix(recipients);

            var best = Find(recipients, 0, k);

            var res = best.Item1 ? string.Join(" ", best.Item2) : "IMPOSSIBLE";
            
            ConsoleHelper.WriteLine(res);
        }

        private static bool[][] Matrix;

        private static void InitMatrix(string recipients)
        {
            Matrix =new bool[recipients.Length][];
            for (int i = recipients.Length-1; i >= 0; i--)
            {
                Matrix[i] = new bool[recipients.Length];
                Matrix[i][i] = true;
                for (int j = i+1; j < recipients.Length; j++)
                {
                    Matrix[i][j] = recipients[i] == recipients[j];
                    if (j - i > 1)
                        Matrix[i][j] = Matrix[i][j] && Matrix[i + 1][j - 1];
                }
            }
        }

        private static (bool, List<string>) Find(string recipients, int i, int k)
        {
            if (k == 1)
                return (Matrix[i][recipients.Length-1], new List<string>{recipients.Substring(i)});

            var max = recipients.Length - k;

            for (var j = max; j >= 0; j--)
            {
                if (!Matrix[i][j])
                    continue;

                var str = recipients.Substring(i, j - i + 1);
                var sub = Find(recipients, j+1, k-1);
                if (sub.Item1)
                {
                    sub.Item2.Insert(0, str);
                    return sub;
                }
            }

            return (false, null);
        }

        private static bool IsPalindrome(string str)
        {
            for (int i = 0; i < str.Length/2; i++)
            {
                if (str[i] != str[str.Length - 1 - i])
                    return false;
            }

            return true;
        }
    }
}