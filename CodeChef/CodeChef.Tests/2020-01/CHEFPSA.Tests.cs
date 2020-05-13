using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CodeChef._2020_01.CHEFPSA.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
6
1
3 2
1
-1 1
1
0 0
2
3 2 1 1
2
4 3 1 4
3
5 3 7 10 5 10", @"
0
0
1
0
2
4")]
        [TestCase(@"
2
3
-1 1 -2 2 -5 5
3
4 5 9 0 9 9", @"
0
8")]
        [TestCase(@"
5
2
10 10 1 9
3
10 10 1 9 2 8
4
10 10 1 9 2 8 3 7
5
10 10 1 9 2 8 3 7 4 6
6
10 10 1 9 2 8 3 7 4 6 5 5", @"
2
8
48
384
1920")]
        [TestCase(@"
4
3
10 10 1 9 1 9
4
10 10 1 9 1 9 2 8
5
10 10 1 9 1 9 1 9 2 8
5
10 10 1 9 1 9 2 8 3 7", @"
4
24
64
192")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Program.Main(null);
            sw.Stop();

            Console.WriteLine($"time: {sw.ElapsedMilliseconds}");

            Assert.AreEqual(output.Trim(), string.Join(Environment.NewLine, consoleHelperForTests.Output));
        }

        public class ConsoleHelperForTests : ConsoleHelper
        {
            private readonly string[] input;
            private int readIndex;
            public readonly List<string> Output;

            public ConsoleHelperForTests(string input)
                : this(input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
            }

            public ConsoleHelperForTests(string[] input)
            {
                this.input = input;
                this.readIndex = 0;
                this.Output = new List<string>();
            }

            public override string ReadLine()
            {
                if (this.readIndex < this.input.Length)
                    return this.input[this.readIndex++];

                return string.Empty;
            }

            public override void WriteLine(object obj)
            {
                this.Output.Add(obj.ToString());
            }
        }
    }
}