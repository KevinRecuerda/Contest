using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;

namespace CodeChef._2020_01.CHEFPSA
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
            var line = ReadLine();

            return ConvertTo<T>(line);
        }

        public string[] ReadLineAndSplit(char delimiter = ' ')
        {
            var splittedLine = ReadLine().Split(delimiter);
            return splittedLine;
        }

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = ReadLineAndSplit();

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
            SolveMultiple();
        }

        public static void SolveMultiple()
        {
            var t = ConsoleHelper.ReadLineAs<int>();
            for (var k = 0; k < t; k++)
            {
                Solve();
            }
        }

        public static void Solve()
        {
            var n = ConsoleHelper.ReadLineAs<int>();
            var x = ConsoleHelper.ReadLineAndSplitAsListOf<long>();

            var res = Count(n, x);
            ConsoleHelper.WriteLine(res);
        }

        private static long Count(int n, List<long> x)
        {
            var sequencesSum = x.Sum();
            if (sequencesSum % (n + 1) != 0)
                return 0; // wrong sequences

            var sum = sequencesSum / (n + 1);
            var middle = sum / 2.0;

            var dico = x.GroupBy(i => i).ToDictionary(g => g.Key, g => g.Count());
            if (!dico.ContainsKey(sum) || dico[sum] < 2)
                return 0; // wrong total sequences

            dico[sum] -= 2; // remove total
            if (dico[sum] == 0)
                dico.Remove(sum);

            // check wrong pair
            foreach (var item in dico)
            {
                var reverse = sum - item.Key;
                if (!dico.ContainsKey(reverse) || dico[reverse] != item.Value)
                    return 0;
            }

            // count
            var duplicatesCount = dico.Where(g => g.Key < middle && g.Value > 1).Select(g => g.Value).ToList();
            if (sum % 2 == 0 && dico.ContainsKey((long) middle))
                duplicatesCount.Add(dico[(long) middle] / 2);

            //var permutations = Factorial(n - 1) / duplicatesCount.Aggregate(1L, (all, v) => all * Factorial(v));
            var permutations = Factorial(n - 1) * duplicatesCount.Aggregate(1L, (all, v) => all * InvFactorial(v) % Mod);

            var distinctPairCount = dico.Where(g => g.Key < middle).Sum(g => g.Value);
            var permutationsPair = Pow(2, distinctPairCount, Mod);

            var count = permutations * permutationsPair;
            return count % Mod;
        }

        public static List<long> Factorials = new List<long> {1};

        public static long Factorial(int n)
        {
            for (var i = Factorials.Count; i <= n; i++)
            {
                var fac = Factorials[i - 1] * i % Mod;
                Factorials.Add(fac);
            }

            return Factorials[n];
        }

        public static List<long> InvFactorials = new List<long> { 1 };

        public static long InvFactorial(int n)
        {
            for (var i = InvFactorials.Count; i <= n; i++)
            {
                var modInverse = ModInverse(i, Mod);
                var invFac = InvFactorials[i - 1] * modInverse % Mod;
                InvFactorials.Add(invFac);
            }

            return InvFactorials[n];
        }
        public static long ModInverse(long a, long m)
        {
            var inv = Pow(a, m - 2, m);
            return inv;
        }

        // a^b % m
        public static long Pow(long a, long b, long m)
        {
            if (b == 0)
                return 1;

            var p = Pow(a, b / 2, m) % m;
            p = (p * p) % m;

            if (b % 2 == 1)
                p = a * p % m;

            return p;
        }

        public static long Pow(long n)
        {
            var pow = Math.Round(Math.Exp(n * Math.Log(2)));
            return (long) pow;
            //return 2 << n;
        }

        public static long Mod = 1000000007;
    }
}