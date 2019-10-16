using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.MDF18.Ex2
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

            var dico = new Dictionary<string, Item>();
            for (var i = 0; i < n; i++)
            {
                var input = ConsoleHelper.ReadLineAndSplit();
                var p = input[0];
                var e = input[1];

                var pItem = GetOrAdd(p, dico);
                var eItem = GetOrAdd(e, dico);
                eItem.Parent = pItem;
                pItem.SubItems.Add(eItem);
            }

            var items = dico.Values.Where(x => x.Parent == null).ToList();
            items.ForEach(x => x.Do());

            var max = items.Select(x => x.Count).Max();
            var betters = items.Where(x => x.Count == max);

            var res = string.Join(" ", betters.Select(x => x.Id));
            ConsoleHelper.WriteLine(res);
        }

        public static Item GetOrAdd(string id, Dictionary<string, Item> dico)
        {
            if (!dico.ContainsKey(id))
                dico[id] = new Item(id);

            return dico[id];
        }

        public class Item
        {
            public string Id { get; set; }
            public Item Parent { get; set; }
            public List<Item> SubItems { get; set; }

            public int Count { get; set; }

            public Item(string id)
            {
                this.Id = id;
                this.Parent = null;
                this.SubItems = new List<Item>();
                this.Count = -1;
            }

            public int Do()
            {
                this.Count = this.Parent?.Parent != null ? 1 :0;
                foreach (var subItem in this.SubItems)
                {
                    this.Count += subItem.Do();
                }

                return this.Count;
            }
        }
    }
}
