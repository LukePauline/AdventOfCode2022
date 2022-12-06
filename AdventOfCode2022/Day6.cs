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

        public string Ex1TestResult => "7";

        public string Ex2TestResult => "19";

        public string Exercise1(StreamReader input)
        {
            string parsed = input.ReadToEnd();
            return (Enumerable.Range(0, parsed.Length).First(i => parsed.Substring(i, 4).Distinct().Count() == 4) + 4).ToString();
        }

        public string Exercise2(StreamReader input)
        {
            string parsed = input.ReadToEnd();
            return (Enumerable.Range(0, parsed.Length).First(i => parsed.Substring(i, 14).Distinct().Count() == 14) + 14).ToString();
        }
    }
}
