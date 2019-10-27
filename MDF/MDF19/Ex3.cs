//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Contest.MDF19.Ex3
//{
//    #region ConsoleHelper
//    public interface IConsoleHelper
//    {
//        string ReadLine();
//        T ReadLineAs<T>();

//        string[] ReadLineAndSplit(char delimiter = ' ');
//        List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ');

//        void WriteLine(object obj);
//        void Debug(object obj);
//    }

//    public class ConsoleHelper : IConsoleHelper
//    {
//        public virtual string ReadLine()
//        {
//            return Console.ReadLine();
//        }

//        public T ReadLineAs<T>()
//        {
//            var line = this.ReadLine();

//            return ConvertTo<T>(line);
//        }

//        public string[] ReadLineAndSplit(char delimiter = ' ')
//        {
//            var splittedLine = this.ReadLine().Split(delimiter);
//            return splittedLine;
//        }

//        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
//        {
//            var splittedLine = this.ReadLineAndSplit();

//            return splittedLine.Select(ConvertTo<T>).ToList();
//        }

//        public virtual void WriteLine(object obj)
//        {
//            Console.WriteLine(obj);
//        }

//        public void Debug(object obj)
//        {
//            Console.Error.WriteLine(obj);
//        }

//        private static T ConvertTo<T>(string value)
//        {
//            return (T)Convert.ChangeType(value, typeof(T));
//        }
//    }
//    #endregion

//    public static class Program
//    {
//        public static IConsoleHelper ConsoleHelper;

//        static Program()
//        {
//            ConsoleHelper = new ConsoleHelper();
//        }

//        public static void Main(string[] args)
//        {
//            Solve();
//        }

//        public static void Solve()
//        {
//            var size = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
//            var n = size[0];
//            var m = size[1];

//            var friends = Enumerable.Range(1, n).ToDictionary(i => i, i => new HashSet<int>());

//            var sexes = ConsoleHelper.ReadLineAndSplitAsListOf<int>();

//            for (int i = 0; i < m; i++)
//            {
//                var edge = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
//                var p1 = edge[0];
//                var p2 = edge[1];

//                friends[p1].Add(p2);
//                friends[p2].Add(p1);
//            }

//            var toChecks = Enumerable.Range(1, n).ToList();
//            //Solve(toChecks, friends, sexes);

//            ConsoleHelper.WriteLine(p);
//        }

//        public static int Solve(HashSet<int> toChecks, Dictionary<int, HashSet<int>> friends, List<int> sexes)
//        {
//            if (toChecks.Count == 0)
//                return 0;

//            var first = toChecks.First();
//            var sexe = sexes[first - 1];
//            toChecks.Remove(first);

//            var friend = friends[first].Where(x => sexes[x - 1] != sexe).ToList();
//            //if (friend.Count == 0)
//            //    return Solve(toChecks)
//            return 1;
//        }
//    }
//}
