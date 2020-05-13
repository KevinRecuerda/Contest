using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CodeChef._2020_05.SORTVS
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
            var m = input[1];

            var p = ConsoleHelper.ReadLineAndSplitAsListOf<int>().ToArray();
            var order = p.OrderBy(x => x).Select((x, i) => new {x, i}).ToDictionary(x => x.x, x => x.i);
            var array = p.Select(x => order[x]).ToArray();

            var groupsByKey = array.Select((x, i) => new Group(i, x)).ToArray();
            var groupsByValue = groupsByKey.OrderBy(g => g.Values.First()).ToArray();

            for (var i = 0; i < m; i++)
            {
                input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var x = order[input[0]];
                var y = order[input[1]];

                Combine(x, y, groupsByKey, groupsByValue);
            }

            var toChecks = groupsByKey.Distinct().ToList();
            toChecks.ForEach(g => g.CalcWeight());
            
            var totalCost = 0;
            while (toChecks.Count > 0)
            {
                var min = toChecks.Min(g => g.Weight);
                var minGroups = toChecks.Where(g => g.Weight == min).ToList();
                
                // target
                if (min == 0)
                {
                    minGroups.ForEach(g => toChecks.Remove(g));
                    continue;
                }
                
                // try find efficient swap
                if (min >= 2)
                {
                    var find = false;
                    foreach (var toCheck in minGroups)
                    {
                        if (find)
                            break;
                        
                        foreach (var wrongKey in toCheck.WrongKeys)
                        {
                            var otherGroup = groupsByValue[wrongKey];
                            var inter = toCheck.WrongValues.Intersect(otherGroup.WrongKeys).ToList();
                            if (inter.Count > 0)
                            {
                                var wrongValue = inter.First();
                                
                                Swap(wrongKey, wrongValue, groupsByKey, groupsByValue);
                                totalCost++;
                                
                                find = true;
                                break;
                            }
                        }
                    }
                    
                    if (find)
                        continue;
                }
                
                // simple swap
                var group = minGroups.First();

                var key = group.WrongKeys.First();
                var value = group.WrongValues.First();
                
                Swap(key, value, groupsByKey, groupsByValue);
                totalCost++;
            }

            ConsoleHelper.WriteLine(totalCost);
        }

        private static void Swap(int key, int value, Group[] groupsByKey, Group[] groupsByValue)
        {
            var group1 = groupsByKey[key];
            var group2 = groupsByValue[key];

            group1.Values.Remove(value);
            group1.Values.Add(key);
            group1.CalcWeight();
            groupsByValue[key] = group1;
                
            group2.Values.Remove(key);
            group2.Values.Add(value);
            group2.CalcWeight();
            groupsByValue[value] = group2;
        }

        private static void Combine(int i, int j, Group[] groupsByKey, Group[] groupsByValue)
        {
            var group1 = groupsByKey[i];
            var group2 = groupsByKey[j];

            if (group1 == group2)
                return;

            var group = new Group(group1, group2);
            foreach (var key in group.Keys)
                groupsByKey[key] = group;
            foreach (var value in group.Values)
                groupsByValue[value] = group;
        }

        public class Group
        {
            public HashSet<int> Keys { get; set; }
            public HashSet<int> Values { get; set; }


            public int Weight { get; set; }
            public HashSet<int> WrongKeys { get; set; }
            public HashSet<int> WrongValues { get; set; }

            public Group(int key, int value)
            {
                Keys = new HashSet<int> {key};
                Values = new HashSet<int> {value};
            }

            public Group(Group group1, Group group2)
            {
                Keys = new HashSet<int>(group1.Keys.Concat(group2.Keys));
                Values = new HashSet<int>(group1.Values.Concat(group2.Values));
            }

            public void CalcWeight()
            {
                WrongKeys = new HashSet<int>(Keys.Except(Values));
                WrongValues = new HashSet<int>(Values.Except(Keys));
                Weight = WrongKeys.Count;
            }

            public override string ToString()
            {
                return $"W={Weight} | [{string.Join(",", Keys)}]={string.Join(",", Values)}";
            }
        }
    }
}