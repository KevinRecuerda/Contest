using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CodeChef.Oct18.CCIRCLES.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
2 3
0 0 5
8 3 2
0
10
20", @"
0
1
0")]
        [TestCase(@"
2 4
0 0 5
0 1 2
0
2
8
9", @"
0
1
1
0")]
        [TestCase(@"
2 4
0 0 5
0 4 2
0
5
10
12", @"
1
1
1
0")]
        [TestCase(@"
3 3
0 0 5
8 3 2
-5 1 3
0
10
20", @"
1
3
0")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Solve();

            var actual = string.Join(Environment.NewLine, consoleHelperForTests.Output);
            Assert.AreEqual(output.Trim(), actual);
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
