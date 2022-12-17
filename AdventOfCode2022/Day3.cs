using System.Net.Sockets;

namespace AdventOfCode2022
{
    public class Day3 : IDay
    {
        public int Day => 3;

        public string TestInput => """
            vJrwpWtwJgWrhcsFMMfFFhFp
            jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
            PmmdzqPrVvPwwTWBwg
            wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
            ttgJtRGJQctTZtZT
            CrZsJsPPZsGzwwsLwLmpwMDw
            """;

        public object Ex1TestResult => 157;

        public object Ex2TestResult => 70;

        public object Exercise1(StreamReader input, bool isTest)
        {
            return ParseEx1(input).Sum(GetItemPriority);
        }

        public object Exercise2(StreamReader input, bool isTest)
        {
            return ParseEx2(input).Sum(GetItemPriority);
        }

        private int GetItemPriority(char item)
        {
            if (item < 65 || item > 122)
                throw new ArgumentException(nameof(item));

            //lowercase
            if (item > 96)
                return item - 96;

            //Uppercase
            return item - 38;
        }

        private IEnumerable<char> ParseEx1(StreamReader input)
        {
            while (!input.EndOfStream)
            {
                string line = input.ReadLine();
                int pocketLength = line.Length / 2;
                string pocket1 = line[0..pocketLength];
                string pocket2 = line[pocketLength..];

                yield return GetDuplicate(pocket1, pocket2);
            }

            char GetDuplicate(string pocket1, string pocket2)
            {
                foreach (var item in pocket1)
                {
                    if (pocket2.Contains(item))
                        return item;
                }
                throw new Exception();
            }
        }

        private IEnumerable<char> ParseEx2(StreamReader input)
        {
            Dictionary<char, int> items = new();
            while (!input.EndOfStream)
            {
                string line1 = input.ReadLine();
                string line2 = input.ReadLine();
                string line3 = input.ReadLine();

                yield return GetDuplicate(line1, line2, line3);
            }

            char GetDuplicate(string line1, string line2, string line3)
            {
                foreach (var item in line1)
                {
                    if (!line2.Contains(item))
                        continue;
                    if (line3.Contains(item))
                        return item;
                }
                throw new Exception();
            }
        }
    }
}
