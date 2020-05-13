using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_05.CHANDF.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
2
7 12 4 17
7 12 0 8", @"
15
7")]
        [TestCase(@"
1
2 4 0 8", @"
6")]
        [TestCase(@"
2
0 4 0 8
0 4 7 8", @"
0
7")]
        [TestCase(@"
1
15 15 0 8", @"
8")]
        [TestCase(@"
1
9 10 0 9", @"
9")]
        [TestCase(@"
1
1 6 0 6", @"
5")]
        [TestCase(@"
1
0 4 1 6", @"
1")]
        [TestCase(@"
2
9 17 0 24
1 24 0 24", @"
24
17")]
        [TestCase(@"
2
11 15 0 12
11 15 12 12", @"
11
12")]
        [TestCase(@"
1
5 3 8 16", @"
15")]
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