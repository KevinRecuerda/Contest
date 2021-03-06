using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.MDF18.Ex1
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
            var m = ConsoleHelper.ReadLineAs<int>();
            var capacity = m * 10;

            var lasTime = 0;
            var people = 0;
            var sonore = false;

            var duration = 0;
            for (var i = 0; i < n; i++)
            {
                var input = ConsoleHelper.ReadLineAndSplit();
                var h = input[0].Split(':');
                var time = int.Parse(h[0]) * 60 + int.Parse(h[1]);
                var p = input[1];

                if (sonore)
                    duration += time - lasTime;

                if (p == "E")
                    people++;
                else
                    people--;

                if (people > capacity)
                    sonore = true;
                else
                    sonore = false;

                lasTime = time;

            }

            if (sonore)
                duration += 23 * 60 - lasTime;

            ConsoleHelper.WriteLine(duration);
        }
    }
}
