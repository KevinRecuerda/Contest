using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_05.SORTVS.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
3
3 1
2 3 1
2 3
5 10
2 4 5 1 3
1 2
1 3
1 4
1 5
2 3
2 4
2 5
3 4
3 5
4 5
4 1
3 1 4 2
1 2", @"
1
0
2")]
        [TestCase(@"
2
3 0
2 3 1
4 0
4 3 1 2", @"
2
3")]
        [TestCase(@"
2
3 1
2 3 1
1 2
4 2
4 3 1 2
1 2
2 4", @"
1
1")]
        [TestCase(@"
1
4 0
3 0 1 2", @"
3")]
        [TestCase(@"
1
6 3
4 5 0 1 2 3
0 1
2 3
4 5", @"
4")]
        [TestCase(@"
2
8 4
6 7 0 1 2 3 4 5
0 1
2 3
4 5
6 7
8 4
6 7 4 1 2 3 0 5
0 1
2 3
4 5
6 7", @"
6
5")]
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