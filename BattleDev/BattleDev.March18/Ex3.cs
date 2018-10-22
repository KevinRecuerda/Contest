using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BattleDev.March18.Ex3
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
            var stars = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = ConsoleHelper.ReadLineAs<int>();
            var k = ConsoleHelper.ReadLineAs<int>();

            var friends = new List<Friend>();
            for (var i = 0; i < n; i++)
            {
                var line = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var dist = 0;
                for (var j = 0; j < 5; j++)
                {
                    dist += Math.Abs(stars[j] - line[j]);
                }
                var friend = new Friend()
                {
                    Dist = dist,
                    NewEpisodeStar = line[5]
                };
                friends.Add(friend);
            }

            var average = friends.OrderBy(f => f.Dist).Take(k).Average(x => x.NewEpisodeStar);
            ConsoleHelper.WriteLine(Math.Floor(average));
        }
    }

    public class Friend
    {
        public int Dist { get; set; }
        public int NewEpisodeStar { get; set; }
    }
}