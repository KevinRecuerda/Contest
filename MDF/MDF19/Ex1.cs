using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.MDF19.Ex1
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
            var capa = ConsoleHelper.ReadLineAs<int>();
            var conso = ConsoleHelper.ReadLineAs<int>();

            var s1 = ConsoleHelper.ReadLineAs<int>();
            var s2 = ConsoleHelper.ReadLineAs<int>();
            var s3 = ConsoleHelper.ReadLineAs<int>();

            var dest = ConsoleHelper.ReadLineAs<int>();

            var points = new[] {s1, s2, s3, dest};
            points = points.Where(x => x <= dest).OrderBy(x => x).ToArray();

            var result = true;
            var current = 0;
            foreach (var point in points)
            {
                if ((point-current) * conso > capa * 100)
                    result = false;

                current = point;
            }

            ConsoleHelper.WriteLine(result ? "OK" : "KO");
        }
    }
}