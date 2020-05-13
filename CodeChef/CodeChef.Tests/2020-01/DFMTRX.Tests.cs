using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CodeChef._2020_01.DFMTRX.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
3
1
2
3", @"
Hooray
1
Hooray
1 2
3 1
Boo")]
        [TestCase(@"
1
4", @"
Hooray
1 2 3 4
5 1 4 3
6 7 1 2
7 6 5 1")]
        [TestCase(@"
1
6", @"
Hooray
1 2 3 4 5 6
7 1 4 5 6 3
8 9 1 6 2 5
9 10 11 1 3 2
10 11 7 8 1 4
11 8 10 7 9 1")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Main(null);

            Console.WriteLine($@"Expected:
{output.Trim()}
");
            Console.WriteLine($@"Actual:
{string.Join(Environment.NewLine, consoleHelperForTests.Output)}
");

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