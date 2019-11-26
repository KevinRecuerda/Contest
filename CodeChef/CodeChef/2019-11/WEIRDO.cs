using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2019_11.WEIRDO
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

        public const int FirstIndex = 'a';

        public static void Solve()
        {
            var l = ConsoleHelper.ReadLineAs<int>();

            var matrix = new int[l][];
            for (var i = 0; i < l; i++)
                matrix[i] = new int[26];

            // people [person] [occurence vs total] [letter]
            // people[0] = alice
            // people[1] = bob
            // people[x][0] = recipes occurence
            // people[x][1] = total count
            var people = new int[2][][];
            for (var i = 0; i < 2; i++)
            {
                people[i] = new int[2][];
                for (var j = 0; j < 2; j++)
                    people[i][j] = new int[26];
            }

            var peopleRecipes = new int[2];

            for (var i = 0; i < l; i++)
            {
                var line = matrix[i];

                var personIdx = 0;
                var lastConsonantIndex = -3;

                var word = ConsoleHelper.ReadLine();
                for (var j = 0; j < word.Length; j++)
                {
                    var character = word[j];

                    var idx = character - FirstIndex;
                    line[idx]++;

                    if (!(character == 'a' || character == 'e' || character == 'i' || character == 'o' ||
                          character == 'u'))
                    {
                        if (j - lastConsonantIndex < 3)
                            personIdx = 1;

                        lastConsonantIndex = j;
                    }
                }

                peopleRecipes[personIdx]++;

                var person = people[personIdx];
                for (var k = 0; k < 26; k++)
                {
                    if (line[k] > 0)
                    {
                        person[0][k]++;
                        person[1][k] += line[k];
                    }
                }
            }

            var scores = new double[2];
            for (var i = 0; i < 2; i++)
            {
                var recipes = peopleRecipes[i];
                var person = people[i];
                for (var k = 0; k < 26; k++)
                {
                    var letterOccurence = person[0][k];
                    var letterTotal = person[1][k];

                    if (letterOccurence > 0)
                    {
                        scores[i] += Math.Log(letterOccurence) - recipes * Math.Log(letterTotal);
                    }
                }
            }

            var score = Math.Exp(scores[0] - scores[1]);

            var res = score < Math.Pow(10, 7) ? score.ToString("0.0000000") : "Infinity";

            ConsoleHelper.WriteLine(res);
        }
    }
}