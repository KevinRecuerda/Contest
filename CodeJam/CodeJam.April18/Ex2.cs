using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam.April18.Ex2
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
            var t = ConsoleHelper.ReadLineAs<int>();
            for (var i = 0; i < t; i++)
            {
                var n = ConsoleHelper.ReadLineAs<int>();
                var numbers = ConsoleHelper.ReadLineAndSplitAsListOf<int>().ToArray();

                var index = Solve(numbers);
                var res = index >= 0 ? index.ToString() : "OK";
                ConsoleHelper.WriteLine($"Case #{i + 1}: {res}");
            }
        }

        public static int Solve(int[] numbers)
        {
            TroubleSort(numbers);

            for (var i = 0; i < numbers.Length - 1; i++)
            {
                if (numbers[i] > numbers[i + 1])
                    return i;
            }

            return -1;
        }

        private static void TroubleSort(int[] numbers)
        {
            var done = false;
            while (!done)
            {
                done = true;
                for (var i = 0; i < numbers.Length-2; i++)
                {
                    if (numbers[i] > numbers[i + 2])
                    {
                        var tmp = numbers[i + 2];
                        numbers[i + 2] = numbers[i];
                        numbers[i] = tmp;

                        done = false;
                    }
                }
            }
        }
    }
}
