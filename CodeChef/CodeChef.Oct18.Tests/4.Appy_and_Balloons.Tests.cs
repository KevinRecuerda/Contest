﻿using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CodeChef.Oct18.HMAPPY.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
5 3
1 2 3 4 5
1 2 3 4 5", @"
15")]
        [TestCase(@"
5 20
1 2 3 4 5
1 2 3 4 5", @"
0")]
        [TestCase(@"
5 5
1 2 3 5 5
1 2 3 5 5", @"
15")]
        [TestCase(@"
5 10
1 2 3 4 5
1 2 3 4 5", @"
4")]
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
