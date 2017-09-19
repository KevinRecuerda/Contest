using System;
using System.Collections.Generic;
using System.Linq;

namespace MDF17.Ex1
{
    #region ConsoleHelper
    public interface IConsoleHelper
    {
        string ReadLine();
        T ReadLineAs<T>();
        List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ');
        void WriteLine(object obj);
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

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = this.ReadLine().Split(delimiter);

            return splittedLine.Select(ConvertTo<T>).ToList();
        }

        public virtual void WriteLine(object obj)
        {
            Console.WriteLine(obj);
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
            var p = new List<int>();
            for (var i = 0; i < n; i++)
            {
                var size = ConsoleHelper.ReadLineAs<int>();
                p.Add(size);
            }
            p = p.OrderBy(x => x).ToList();
            var result = "";
            for (var i = 0; i < n/2; i++)
            {
                result += p[i] + " ";
                result += p[n-1-i] + " ";
            }
            if (n%2 == 1)
                result += p[n/2];
            
            ConsoleHelper.WriteLine(result);
        }
    }
}
