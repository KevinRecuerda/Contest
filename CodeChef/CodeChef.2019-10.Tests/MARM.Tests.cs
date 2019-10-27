using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef.October2019.MARM.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
1
2 2
1 2", @"
3 1")]
        [TestCase(@"
1
3 0
2 4 8", @"
2 4 8")]
        [TestCase(@"
1
3 1
2 4 8", @"
10 4 8")]
        [TestCase(@"
1
3 2
2 4 8", @"
10 0 8")]
        [TestCase(@"
1
3 3
2 4 8", @"
10 0 2")]
        [TestCase(@"
1
3 4
2 4 8", @"
8 0 2")]
        [TestCase(@"
1
3 5
2 4 8", @"
8 0 2")]
        [TestCase(@"
1
3 6
2 4 8", @"
8 0 10")]
        [TestCase(@"
1
3 7
2 4 8", @"
2 0 10")]
        [TestCase(@"
1
3 8
2 4 8", @"
2 0 10")]
        [TestCase(@"
1
3 9
2 4 8", @"
2 0 8")]
        [TestCase(@"
1
4 0
1 2 3 4", @"
1 2 3 4")]
        [TestCase(@"
1
4 1
1 2 3 4", @"
5 2 3 4")]
        [TestCase(@"
1
4 2
1 2 3 4", @"
5 1 3 4")]
        [TestCase(@"
1
4 3
1 2 3 4", @"
5 1 2 4")]
        [TestCase(@"
1
4 4
1 2 3 4", @"
5 1 2 1")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Main(null);

            Assert.AreEqual(output.Trim(), consoleHelperForTests.Output.First());
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
