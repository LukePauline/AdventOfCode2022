using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022
{
    public class Day10 : IDay
    {
        public int Day => 10;

        public string TestInput => """
            addx 15
            addx -11
            addx 6
            addx -3
            addx 5
            addx -1
            addx -8
            addx 13
            addx 4
            noop
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx -35
            addx 1
            addx 24
            addx -19
            addx 1
            addx 16
            addx -11
            noop
            noop
            addx 21
            addx -15
            noop
            noop
            addx -3
            addx 9
            addx 1
            addx -3
            addx 8
            addx 1
            addx 5
            noop
            noop
            noop
            noop
            noop
            addx -36
            noop
            addx 1
            addx 7
            noop
            noop
            noop
            addx 2
            addx 6
            noop
            noop
            noop
            noop
            noop
            addx 1
            noop
            noop
            addx 7
            addx 1
            noop
            addx -13
            addx 13
            addx 7
            noop
            addx 1
            addx -33
            noop
            noop
            noop
            addx 2
            noop
            noop
            noop
            addx 8
            noop
            addx -1
            addx 2
            addx 1
            noop
            addx 17
            addx -9
            addx 1
            addx 1
            addx -3
            addx 11
            noop
            noop
            addx 1
            noop
            addx 1
            noop
            noop
            addx -13
            addx -19
            addx 1
            addx 3
            addx 26
            addx -30
            addx 12
            addx -1
            addx 3
            addx 1
            noop
            noop
            noop
            addx -9
            addx 18
            addx 1
            addx 2
            noop
            noop
            addx 9
            noop
            noop
            noop
            addx -1
            addx 2
            addx -37
            addx 1
            addx 3
            noop
            addx 15
            addx -21
            addx 22
            addx -6
            addx 1
            noop
            addx 2
            addx 1
            noop
            addx -10
            noop
            noop
            addx 20
            addx 1
            addx 2
            addx 2
            addx -6
            addx -11
            noop
            noop
            noop
            """;

        public object Ex1TestResult => "13140";

        public object Ex2TestResult => """
            ##..##..##..##..##..##..##..##..##..##..
            ###...###...###...###...###...###...###.
            ####....####....####....####....####....
            #####.....#####.....#####.....#####.....
            ######......######......######......####
            #######.......#######.......#######.....
            """;

        public object Exercise1(StreamReader input, bool isTest)
        {
            var instructions = Parse(input);
            Dictionary<int, int> process = ExecuteInstructions(instructions);
            return new[]
            {
                GetRegisterValue(process, 20) * 20,
                GetRegisterValue(process, 60) * 60,
                GetRegisterValue(process, 100) * 100,
                GetRegisterValue(process, 140) * 140,
                GetRegisterValue(process, 180) * 180,
                GetRegisterValue(process, 220) * 220
            }.Sum();
        }

        public object Exercise2(StreamReader input, bool isTest)
        {
            var instructions = Parse(input);
            Dictionary<int, int> process = ExecuteInstructions(instructions);

            char[,] display = new char[40, 6];

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    int cycle = i * 40 + j + 1;
                    int regValue = GetRegisterValue(process, cycle);
                    display[j, i] = j >= regValue - 1 && j <= regValue + 1 ? '#' : '.';
                }
            }
            return display.Render();
        }

        private static Dictionary<int, int> ExecuteInstructions(IEnumerable<Instruction> instructions)
        {
            Dictionary<int, int> process = new() { { 0, 1 } };
            int value = 1;
            int cycle = 1;
            foreach (var instruction in instructions)
            {
                switch (instruction.Operation)
                {
                    case Operation.NoOp:
                        cycle++;
                        break;
                    case Operation.AddX:
                        value += instruction.Operand;
                        cycle += 2;
                        process.Add(cycle, value);
                        break;
                }
            }

            return process;
        }

        private int GetRegisterValue(Dictionary<int, int> process, int cycle)
        {
            var key = process.Keys.Where(x => x <= cycle).Max();
            return process[key];
        }

        private IEnumerable<Instruction> Parse(StreamReader input) =>
            input.ReadToEnd().SplitByLineBreak(StringSplitOptions.RemoveEmptyEntries).Select(x => Instruction.Parse(x));

        public record Instruction(Operation Operation, int Operand = 0)
        {
            public static Instruction Parse(string input)
            {
                int operand = 0;
                Operation operation = input[..4] switch
                {
                    "addx" => Operation.AddX,
                    "noop" => Operation.NoOp,
                    _ => throw new FormatException(),
                };
                if (operation == Operation.AddX)
                {
                    operand = int.Parse(input[5..]);
                }
                return new(operation, operand);
            }
        }

        public enum Operation
        {
            AddX,
            NoOp
        }
    }
}
