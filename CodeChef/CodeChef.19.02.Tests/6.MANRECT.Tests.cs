using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CodeChef.Feb19.MANRECT.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
1
10
8
0
999999988
999999982", @"
Q 0 0
Q 10 0
Q 6 4
Q 1000000000 4
Q 6 1000000000
A 6 4 12 18")]
        [TestCase(@"
1
3
3
1
999999996
999999996", @"
Q 0 0
Q 3 0
Q 1 2
Q 1000000000 3
Q 0 1000000000
A 0 3 4 4")]
        [TestCase(@"
1
10
11
0
999999995
999999993", @"
Q 0 0
Q 10 0
Q 4 6
Q 1000000000 6
Q 4 1000000000
A 4 6 5 7")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Main(null);

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
