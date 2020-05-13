using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace BattleDev._2020_03.Ex3
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
            var n = ConsoleHelper.ReadLineAs<int>();

            var input = new List<string>();
            var unavailables = Enumerable.Range(1, 5).ToDictionary(i => i, i => new List<(int, int)>());
            for (int i = 0; i < n; i++)
            {
                var line = ConsoleHelper.ReadLine();
                input.Add(line);
                var unavailable = line.Split(' ');
                var day = int.Parse(unavailable[0]);

                var time = unavailable[1].Split('-');
                var start = ParseTime(time[0]);
                var end = ParseTime(time[1]);

                unavailables[day].Add((start, end));
            }

            var res = Find(unavailables);
            ConsoleHelper.WriteLine(res);
        }

        private static string Find(Dictionary<int, List<(int, int)>> unavailables)
        {
            for (int i = 1; i <= 5; i++)
            {
                var ko = unavailables[i];
                var current = 8 * 60;
                foreach (var (s, e) in ko.OrderBy(x => x.Item1))
                {
                    if (s - current >= 59)
                        return FormatRes(i, current);

                    current = Math.Max(current, e+1);
                }

                var end = 18 * 60 -1;
                if (end - current >= 59)
                    return FormatRes(i, current);
            }

            return "error";
        }

        private static string FormatRes(int day, int start)
        {
            return $"{day} {FormatTime(start)}-{FormatTime(start + 59)}";
        }

        private static string FormatTime(int time)
        {
            return $"{time / 60:00}:{time % 60:00}";
        }

        private static int ParseTime(string time)
        {
            var s = time.Split(':');

            return int.Parse(s[0]) * 60 + int.Parse(s[1]);
        }
    }
}