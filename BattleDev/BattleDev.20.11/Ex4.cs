using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2020_11.Ex4
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
            var input = ConsoleHelper.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var n = input[0];
            var m = input[1];
            
            var keys = ConsoleHelper.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var cache = new int[n];
            cache[0] = keys[0];
            for (var i = 1; i < n; i++)
                cache[i] = keys[i] ^ cache[i - 1];
            
            var res = new int[256];
            for (var i = 0; i < m; i++)
            {
                input = ConsoleHelper.ReadLine().Split(' ').Select(int.Parse).ToArray();
                var l = input[0];
                var r = input[1];
                var xor = cache[r];
                if (l > 0)
                    xor ^= cache[l-1];
                res[xor]++;
            }
            
            ConsoleHelper.WriteLine(string.Join(" ", res));
        }
    }
}