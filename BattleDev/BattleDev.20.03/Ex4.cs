using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2020_03.Ex4
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
            var n = ConsoleHelper.ReadLineAs<int>();

            var sachaCards = ConsoleHelper.ReadLineAndSplit();
            var myCards = ConsoleHelper.ReadLineAndSplit();

            var best = FindBest(myCards.ToList(), sachaCards, 0);

            var res = best.Item1 ? string.Join(" ", best.Item2) : "-1";

            ConsoleHelper.WriteLine(res);
        }

        private static (bool, List<string>) FindBest(List<string> myCards, string[] sachaCards, int i)
        {
            if (i >= sachaCards.Length)
                return (myCards.Count > 0, myCards.ToList());

            var sachaCard = sachaCards[i];
            var cards = myCards.Distinct()
                .Select(x => new {card = x, fight = Fight(x, sachaCard)})
                .OrderByDescending(x => x.fight)
                .ToList();
            foreach (var card in cards)
            {
                var depth = i + (card.fight >= 0 ? 1 : 0);

                var fight = card.fight;
                if (fight == 1)
                {
                    for (; depth < sachaCards.Length; depth++)
                    {
                        fight = Fight(card.card, sachaCards[depth]);
                        if (fight == -1)
                            break;

                        if (fight == 0)
                        {
                            depth++;
                            break;
                        }
                    }
                }

                if (fight != 1)
                    myCards.Remove(card.card);

                var sub = FindBest(myCards, sachaCards, depth);
                if (sub.Item1)
                {
                    if (fight != 1)
                        sub.Item2.Insert(0, card.card);
                    return sub;
                }

                if (fight != 1)
                    myCards.Add(card.card);
            }

            return (false, new List<string>());
        }

        private static Dictionary<string, int> Rules = new Dictionary<string, int>()
        {
            ["feu-eau"] = -1,
            ["feu-plante"] = 1,
            ["feu-glace"] = 1,
            ["eau-plante"] = -1,
            ["eau-sol"] = -1,
            ["plante-poison"] = 1,
            ["plante-sol"] = -1,
            ["plante-vol"] = 1
        };

        private static int Fight(string p1, string p2)
        {
            var value1 = $"{p1}-{p2}";
            var value2 = $"{p2}-{p1}";

            if (Rules.TryGetValue(value1, out var res))
                return res;

            if (Rules.TryGetValue(value2, out res))
                return -res;

            return 0;
        }
    }
}