using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Chef_and_Adventures.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
5
2 2 1 2
11 10 5 9
11 11 5 9
12 11 5 9
1 2 1 100", @"
Chefirnemo
Chefirnemo
Pofik
Chefirnemo
Pofik")]
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
