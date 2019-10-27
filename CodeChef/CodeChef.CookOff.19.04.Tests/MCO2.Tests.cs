using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CodeChef.April19.MCO2.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
5
3
0 0 4
3 0 1
0 1 0
2
0 5
3 2
2
1 3
5 0
3
0 3 4
4 0 5
6 5 0
4
3 0 2 4
0 2 3 5
3 4 3 0
3 4 0 3", @"
YES
NO
NO
YES
YES")]
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
