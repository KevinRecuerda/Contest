using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev.March18.Ex2
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
            var price = ConsoleHelper.ReadLineAs<int>();
            var n = ConsoleHelper.ReadLineAs<int>();

            var gain = 0.0;
            for (var i = 0; i < n; i++)
            {
                var personCount = ConsoleHelper.ReadLineAs<int>();
                double bill = personCount * price;
                if (personCount >= 10)
                    bill *= 0.7;
                else if (personCount >= 6)
                    bill *= 0.8;
                else if (personCount >= 4)
                    bill *= 0.9;

                gain += bill;
            }

            ConsoleHelper.WriteLine(Math.Ceiling(gain));
        }
    }
}
