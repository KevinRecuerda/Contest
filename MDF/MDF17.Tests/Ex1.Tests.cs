﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
3
...
###
...", @"
-2")]
        [TestCase(@"
4
....
#!!#
#!!#
....", @"
4")]
        [TestCase(@"
5
.....
##!!#
#!!!#
#!!##
.....", @"
-1")]
        [TestCase(@"
5
.#.!.
.#.#!
.!.!.
.#.#.
.!.#.", @"
5")]
        [TestCase(@"
5
.#.#.
.#.#!
.!.!.
.#.#.
.!.#.", @"
-1")]
        [TestCase(@"
20
..!..!!!!...!#.!!!.#
.#!.#.##.#.!..!.#!!!
!.!.#!#!!!!.!!.!.#!!
!!#!!..##!!!#...#!.!
#.!!!!.!..!.!!!!!!!.
#!!!.!....!...!!!.!#
!.#.!.!.#.!!!!..!!.#
.!.!!!..!.!.##!..#!!
.!!!.!!..###.####..!
!!.!!..!.......#.!!.
!#!..!.!!....!.!#.#.
!#..!.!...!...!#!.!.
.!!!.#!.!!!!!..!.!.#
.#!.!..!.!.!!!.#.###
#.!.!!!#.!...#.#.!#.
!#!..!!#..#!#!#!!!!!
.......#!.!!!!#..!!.
!!!.!.!#!.!#!!#!!!.!
!.!!.!.#.!.#!..!!..!
!.....!!.....##.#!!.", @"
18")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Solve();

            var expected = output.Trim().Replace("\n", string.Empty).Replace("\r", string.Empty);
            Assert.AreEqual(expected, consoleHelperForTests.Output.First());
        }

        public class ConsoleHelperForTests : ConsoleHelper
        {
            private readonly string[] input;
            private int readIndex;
            public readonly List<string> Output;

            public ConsoleHelperForTests(string input)
                : this(input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
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
