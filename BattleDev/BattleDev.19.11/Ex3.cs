using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2019_11.Ex3
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

            var requests = new (int, int, int)[m];
            for (var i = 0; i < m; i++)
            {
                input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var start = input[0];
                var end = input[1];

                requests[i] = (i, start, end);
            }

            requests = requests.OrderBy(r => r.Item2).ThenBy(r => r.Item3).ToArray();

            var cables = new Queue<int>(Enumerable.Range(1, n));
            var currentRequests = new List<((int, int, int), int)>();
            var choices = new int[m];
            for (var i = 0; i < m; i++)
            {
                var request = requests[i];
                var endedRequests = currentRequests.Where(x => x.Item1.Item3 <= request.Item2).ToList();
                foreach (var endedRequest in endedRequests)
                {
                    cables.Enqueue(endedRequest.Item2);
                    currentRequests.Remove(endedRequest);
                }

                if (cables.Count == 0)
                {
                    choices = null;
                    break;
                }

                var cable = cables.Dequeue();
                currentRequests.Add((requests[i], cable));
                choices[request.Item1] = cable;
            }

            var res = choices != null
                ? string.Join(" ", choices)
                : "pas possible";

            ConsoleHelper.WriteLine(res);
        }
    }
}
