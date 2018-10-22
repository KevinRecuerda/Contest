using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CodeChef.Oct18.BITOBYT.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
13
0
1
2
3
10
11
26
27
29
37
53
104
105", @"
0 0 0
1 0 0
1 0 0
0 1 0
0 1 0
0 0 1
0 0 1
2 0 0
0 2 0
0 0 2
4 0 0
0 0 8
16 0 0")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.SolveMultiple();

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
