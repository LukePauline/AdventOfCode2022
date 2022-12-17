using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    internal class Day6 : IDay
    {
        public int Day => 6;

        public string TestInput => "mjqjpqmgbljsphdztnvjfqwrcgsmlb";

        public object Ex1TestResult => 7;

        public object Ex2TestResult => 19;

        public object Exercise1(StreamReader input, bool isTest)
        {
            string parsed = input.ReadToEnd();
            return (Enumerable.Range(0, parsed.Length).First(i => parsed.Substring(i, 4).Distinct().Count() == 4) + 4);
        }

        public object Exercise2(StreamReader input, bool isTest)
        {
            string parsed = input.ReadToEnd();
            return (Enumerable.Range(0, parsed.Length).First(i => parsed.Substring(i, 14).Distinct().Count() == 14) + 14);
        }
    }
}
