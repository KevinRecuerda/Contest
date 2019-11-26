using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BattleDev._2019_11.Ex5
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
            var lostTemple = ConsoleHelper.ReadLine()[0];

            var temples = new Dictionary<string, List<string>>();
            var oldTemples = Enumerable.Range('A', 14).Select(x => (char) x).ToDictionary(x => x, x => "");

            var edges = new (string, string)[21];
            for (var idx = 0; idx < 21; idx++)
            {
                var input = ConsoleHelper.ReadLineAndSplit();
                edges[idx] = (input[0], input[1]);

                if (!temples.ContainsKey(input[0]))
                    temples[input[0]] = new List<string>();
                if (!temples.ContainsKey(input[1]))
                    temples[input[1]] = new List<string>();

                temples[input[0]].Add(input[1]);
                temples[input[1]].Add(input[0]);
            }

            // search extrem groups
            var extremGroup1 = new List<string>();
            var extremGroup2 = new List<string>();

            foreach (var temple1 in temples.Keys)
            {
                if (extremGroup1.Contains(temple1) || extremGroup2.Contains(temple1))
                    continue;

                foreach (var temple2 in temples[temple1])
                {
                    if (extremGroup1.Contains(temple1) || extremGroup2.Contains(temple1))
                        continue;

                    foreach (var temple3 in temples[temple2])
                    {
                        if (temples[temple3].Contains(temple1))
                        {
                            var listToAdd = extremGroup1.Count == 0 ? extremGroup1 : extremGroup2;
                            listToAdd.Add(temple1);
                            listToAdd.Add(temple2);
                            listToAdd.Add(temple3);
                        }
                    }
                }
            }

            var common1 = "";
            var common2 = "";

            foreach (var temple1 in extremGroup1)
            {
                var intersect = temples[temple1].Intersect(extremGroup2).ToList();
                if (intersect.Count > 0)
                {
                    common1 = temple1;
                    common2 = intersect.First();
                }
            }

            // following (2 items per)
            var following1 = extremGroup1.Where(t => t != common1).SelectMany(t => temples[t]).Distinct().Except(extremGroup1).ToList();
            var following2 = extremGroup2.Where(t => t != common2).SelectMany(t => temples[t]).Distinct().Except(extremGroup2).ToList();

            // pivot = K
            var pivot1 = temples[following1[0]].Intersect(temples[following1[1]]).SingleOrDefault();
            var pivot2 = temples[following2[0]].Intersect(temples[following2[1]]).SingleOrDefault();

            if (pivot1 == null)
            {
                Switch(ref pivot1, ref pivot2);
                Switch(ref following1, ref following2);
                Switch(ref common1, ref common2);
                Switch(ref extremGroup1, ref extremGroup2);
            }

            var k = pivot1;
            oldTemples['K'] = k;

            var f = temples[k].Except(following1).Single();
            oldTemples['F'] = f;

            // set following 2
            var a = temples[f].Intersect(following2).Single();
            oldTemples['A'] = a;

            var j = following2.Single(t => t != a);
            oldTemples['J'] = j;

            // set extrem groups
            var m = extremGroup2.Single(t => temples[t].Contains(a));
            oldTemples['M'] = m;

            var n = extremGroup2.Single(t => temples[t].Contains(j));
            oldTemples['N'] = n;

            var e = common2;
            oldTemples['E'] = e;

            var g = common1;
            oldTemples['G'] = g;

            // middle
            var h = temples[j].Except(new[] {a, n}).Single();
            oldTemples['H'] = h;

            var b = temples[h].Intersect(following1).Single();
            oldTemples['B'] = b;

            var i = temples[h].Except(new[] {b, j}).Single();
            oldTemples['I'] = i;

            var l = following1.Single(t => t != b);
            oldTemples['L'] = l;

            var c = temples[b].Intersect(extremGroup1).Single();
            oldTemples['C'] = c;

            var d = temples[l].Intersect(extremGroup1).Single();
            oldTemples['D'] = d;

            ConsoleHelper.WriteLine(oldTemples[lostTemple]);
        }

        public static void Switch<T>(ref T value1, ref T value2)
        {
            var tmp = value1;
            value1 = value2;
            value2 = tmp;
        }
    }
}