using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_05.TRPLSRT
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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = input[0];
            var k = input[1];

            var p = ConsoleHelper.ReadLineAndSplitAsListOf<int>().ToArray();

            var permutations = TripleSort(p);
            if (permutations == null || permutations.Count > k)
            {
                ConsoleHelper.WriteLine(-1);
                return;
            }

            ConsoleHelper.WriteLine(permutations.Count);
            permutations.ForEach(ConsoleHelper.WriteLine);
        }

        private static List<Permutation> TripleSort(int[] array)
        {
            array = array.Select(x => x - 1).ToArray();
            var index = array.Select((x, i) => new {x, i}).OrderBy(x => x.x).Select(x => x.i).ToArray();
            var done = Enumerable.Range(0, array.Length).Select(i => array[i] == i).ToArray();

            var perm1 = FindPerm(array, done, index, true).ToList();
            var perm2 = FindPerm(array, done, index, false).ToList();

            var permutations = perm1.Concat(perm2).ToList();

            return done.All(x => x) ? permutations : null;
        }

        private static IEnumerable<Permutation> FindPerm(int[] array, bool[] done, int[] index, bool ignoreTwiceRef)
        {
            for (var i = 0; i < array.Length; i++)
            {
                if (done[i])
                    continue;

                var a = index[i];
                var b = i;
                var c = array[b];

                if (a == c)
                {
                    if (ignoreTwiceRef)
                        continue;
                    
                    var found = false;
                    for (var k = i + 1; k < array.Length; k++)
                    {
                        if (k == c || done[k])
                            continue;

                        found = true;
                        c = k;
                        break;
                    }

                    if (!found)
                        continue;
                }

                var perm = new Permutation(a, b, c);
                yield return perm;

                ApplyPermutation(perm, array, index, done);
            }
        }

        private static void ApplyPermutation(Permutation perm, int[] array, int[] index, bool[] done)
        {
            index[array[perm.C]] = perm.A;
            index[array[perm.B]] = perm.C;
            index[array[perm.A]] = perm.B;

            var tmp = array[perm.C];
            array[perm.C] = array[perm.B];
            array[perm.B] = array[perm.A];
            array[perm.A] = tmp;

            done[perm.A] = array[perm.A] == perm.A;
            done[perm.B] = array[perm.B] == perm.B;
            done[perm.C] = array[perm.C] == perm.C;
        }
    }

    public class Permutation
    {
        public Permutation(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        public override string ToString()
        {
            return $"{A + 1} {B + 1} {C + 1}";
        }
    }
}