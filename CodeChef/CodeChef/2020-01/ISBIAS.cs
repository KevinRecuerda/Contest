using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_01.ISBIAS
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
            var line = ReadLine();

            return ConvertTo<T>(line);
        }

        public string[] ReadLineAndSplit(char delimiter = ' ')
        {
            var splittedLine = ReadLine().Split(delimiter);
            return splittedLine;
        }

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = ReadLineAndSplit();

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

        public static void SolveMultiple()
        {
            var t = ConsoleHelper.ReadLineAs<int>();
            for (var k = 0; k < t; k++)
            {
                Solve();
            }
        }

        public static void Solve()
        {
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = input[0];
            var q = input[1];

            var a = ConsoleHelper.ReadLineAndSplitAsListOf<long>();

            var ratingSequences = new int[n-1];
            var senses = new List<int>();

            var sequenceId = -1;
            var sequenceSens = 0;

            for (var i = 0; i < n-1; i++)
            {
                var sens = a[i] < a[i+1] ? 1 : -1;
                if (sens != sequenceSens)
                {
                    sequenceId++;
                    sequenceSens = sens;
                    senses.Add(sequenceSens);
                }

                ratingSequences[i] = sequenceId;
            }

            for (var i = 0; i < q; i++)
            {
                input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var l = input[0]-1;
                var r = input[1]-2;

                var start = ratingSequences[l];
                var end = ratingSequences[r];

                var res = senses[start] != senses[end] ? "YES" : "NO";
                ConsoleHelper.WriteLine(res);
            }
        }
    }
}