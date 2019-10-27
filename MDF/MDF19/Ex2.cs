using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.MDF19.Ex2
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
            var size = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = size[0];
            var m = size[1];

            var friends = Enumerable.Range(1, n).ToDictionary(i => i, i => new HashSet<int>());

            for (int i = 0; i < m; i++)
            {
                var edge = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var p1 = edge[0];
                var p2 = edge[1];

                friends[p1].Add(p2);
                friends[p2].Add(p1);
            }

            var myFriends = friends[1];

            var max = 0;
            var p = -1;
            foreach (var friend in friends)
            {
                if (friend.Key == 1 || !myFriends.Contains(friend.Key))
                    continue;

                var common = friend.Value.Count(x => myFriends.Contains(x));
                if (common == 0)
                    continue;

                if (common > max)
                {
                    max = common;
                    p = friend.Key;
                }
                else if (common == max && friend.Key > p)
                {
                    max = common;
                    p = friend.Key;
                }
            }

            ConsoleHelper.WriteLine(p);
        }
    }
}
