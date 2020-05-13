using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2020_03.Ex3.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
6
1 08:45-12:59
1 12:30-13:59
3 11:09-11:28
5 09:26-09:56
5 16:15-16:34
3 08:40-10:12", @"
1 14:00-14:59")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Solve();

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
