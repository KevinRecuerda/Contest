using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_05.TRPLSRT.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
1
4 2
3 2 4 1", @"
1
4 1 3")]
        [TestCase(@"
1
4 2
1 2 4 3", @"
-1")]
        [TestCase(@"
1
5 2
5 4 3 2 1", @"
2
5 1 2
4 2 5")]
        [TestCase(@"
1
8 4
8 7 6 5 4 3 2 1", @"
4
8 1 2
7 2 8
6 3 4
5 4 6")]
        [TestCase(@"
1
7 3
7 6 4 3 2 5 1", @"
3
5 2 6
7 1 3
4 3 7")]
        [TestCase(@"
1
11 8
11 10 9 8 7 6 5 4 3 2 1", @"
-1")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Main(null);

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