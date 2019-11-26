using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2019_11.Ex4
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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = input[0];
            var m = input[1];
            var c = input[2];

            var pierres = new Pierre[n];
            for (var i = 0; i < n; i++)
            {
                input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                pierres[i] = new Pierre(input[0], input[1]);
            }

            var poudres = new Poudre[m];
            for (var i = 0; i < m; i++)
            {
                input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                poudres[i] = new Poudre(input[0], input[1]);
            }

            var items = pierres.OfType<Item>().Concat(poudres.OfType<Item>()).OrderByDescending(i => i.Renta).ToArray();

            var cash = 0;
            foreach (var item in items)
            {
                if (c < item.MinWeight)
                    continue;

                if (item is Pierre)
                {
                    cash += item.Price;
                    c -= item.Weight;
                }
                else
                {
                    var quantity = Math.Min(c, item.Weight);
                    cash += item.Price * quantity;
                    c -= quantity;
                }
            }

            ConsoleHelper.WriteLine(cash);
        }

        private abstract class Item
        {
            public Item(int price, int weight)
            {
                Price = price;
                Weight = weight;
            }

            public int Price { get; set; }
            public int Weight { get; set; }
            public abstract int MinWeight { get; }
            public abstract double Renta { get; }
        }

        private class Pierre : Item
        {
            public override int MinWeight => Weight;
            public override double Renta => Price * 1.0 / Weight;

            public Pierre(int price, int weight) : base(price, weight)
            {
            }
        }

        private class Poudre : Item
        {
            public override int MinWeight => 1;
            public override double Renta => Price;

            public Poudre(int price, int weight) : base(price, weight)
            {
            }
        }
    }
}
