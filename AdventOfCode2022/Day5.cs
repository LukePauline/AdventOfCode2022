using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day5 : IDay
    {
        public int Day => 5;

        public string TestInput => """
                [D]    
            [N] [C]    
            [Z] [M] [P]
             1   2   3 

            move 1 from 2 to 1
            move 3 from 1 to 3
            move 2 from 2 to 1
            move 1 from 1 to 2
            """;

        public string Ex1TestResult => "CMZ";

        public string Ex2TestResult => "MCD";

        public string Exercise1(StreamReader input)
        {
            var (stacks, moves) = Parse(input);
            foreach (var move in moves)
            {
                move.Apply(stacks);
            }
            string top = string.Empty;
            foreach (var stack in stacks)
            {
                if (stack.Any())
                    top += stack.Pop();
            }
            return top;
        }

        public string Exercise2(StreamReader input)
        {
            var (stacks, moves) = Parse(input);
            foreach (var move in moves)
            {
                move.ApplyEx2(stacks);
            }
            string top = string.Empty;
            foreach (var stack in stacks)
            {
                if (stack.Any())
                    top += stack.Pop();
            }
            return top;
        }

        private (List<Stack<char>> stacks, IEnumerable<Move> moves) Parse(StreamReader input)
        {
            // Parse stacks
            string line = input.ReadLine();
            List<Stack<char>> stacks = new();
            for (int i = 0; i < (line.Length + 1) / 4; i++)
                stacks.Add(new Stack<char>());
            while (line[1] != '1')
            {
                for (int i = 0; i * 4 + 1 < line.Length; i += 1)
                {
                    char contents = line[i * 4 + 1];
                    if (contents != ' ')
                        stacks[i].Push(contents);
                }
                line = input.ReadLine();
            };
            stacks = stacks.Select(s => new Stack<char>(s)).ToList();

            input.ReadLine();
            Regex regex = new(@"move (\d+) from (\d+) to (\d+)");
            List<Move> moves = new();
            while (!input.EndOfStream)
            {
                string moveLine = input.ReadLine();
                var match = regex.Match(moveLine);
                int count = int.Parse(match.Groups[1].Value);
                int from = int.Parse(match.Groups[2].Value) - 1;
                int to = int.Parse(match.Groups[3].Value) - 1;

                moves.Add(new Move(count, from, to));
            }

            return (stacks, moves);
        }

        private record Move(int Count, int From, int To)
        {
            public void Apply(List<Stack<char>> stacks)
            {
                var crates = stacks[From].PopMany(Count);
                stacks[To].PushMany(crates);
            }
            public void ApplyEx2(List<Stack<char>> stacks)
            {
                var crates = stacks[From].PopMany(Count).Reverse();
                stacks[To].PushMany(crates);
            }
        }
    }

    public static class StackExtensions
    {
        public static IEnumerable<T> PopMany<T>(this Stack<T> stack, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return stack.Pop();
            }
        }
        public static void PushMany<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                stack.Push(item);
            }
        }
    }
}
