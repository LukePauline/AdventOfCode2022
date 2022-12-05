using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day1 : IDay
    {
        public int Day { get; } = 1;
        public string TestInput { get; } = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000";
        public string Ex1TestResult { get; } = "24000";
        public string Ex2TestResult { get; } = "45000";

        public string Exercise1(StreamReader input)
        {
            var elves = GetElves(input);
            return elves.Select(x => x.Sum()).Max().ToString();
        }

        public string Exercise2(StreamReader input)
        {
            var elves = GetElves(input).ToList();
            return elves.Select(x => x.Sum()).OrderByDescending(x => x).Take(3).Sum().ToString();
        }

        private IEnumerable<List<int>> GetElves(StreamReader input)
        {
            List<int> elf = new();

            while (!input.EndOfStream)
            {
                string line = input.ReadLine()!;
                if (string.IsNullOrWhiteSpace(line))
                {
                    yield return elf;
                    elf = new List<int>();
                }
                else
                {
                    elf.Add(int.Parse(line));
                }
            }
            yield return elf;
        }
    }
}
