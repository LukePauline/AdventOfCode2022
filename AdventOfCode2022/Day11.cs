using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022
{
    public class Day11 : IDay
    {
        public int Day => 11;

        public string TestInput => """
            Monkey 0:
              Starting items: 79, 98
              Operation: new = old * 19
              Test: divisible by 23
                If true: throw to monkey 2
                If false: throw to monkey 3

            Monkey 1:
              Starting items: 54, 65, 75, 74
              Operation: new = old + 6
              Test: divisible by 19
                If true: throw to monkey 2
                If false: throw to monkey 0

            Monkey 2:
              Starting items: 79, 60, 97
              Operation: new = old * old
              Test: divisible by 13
                If true: throw to monkey 1
                If false: throw to monkey 3

            Monkey 3:
              Starting items: 74
              Operation: new = old + 3
              Test: divisible by 17
                If true: throw to monkey 0
                If false: throw to monkey 1
            """;

        public object Ex1TestResult => 10605;

        public object Ex2TestResult => 2713310158;

        public object Exercise1(StreamReader input, bool isTest)
        {
            var monkeys = Parse(input);
            int[] inspections = new int[monkeys.Count];
            for (int round = 0; round < 20; round++)
            {
                for (int m = 0; m < monkeys.Count; m++)
                {
                    inspections[m] += monkeys[m].InpsectItems(monkeys);
                }
            }
            return inspections.OrderByDescending(x => x).Take(2).Aggregate(1, (a, b) => a * b);
        }

        public object Exercise2(StreamReader input, bool isTest)
        {
            var monkeys = Parse(input);
            Monkey.MaxWorry = monkeys.Select(x => x.Value.TestNum).Aggregate(1, (a, b) => a * b);
            long[] inspections = new long[monkeys.Count];
            for (int round = 0; round < 10000; round++)
            {
                for (int m = 0; m < monkeys.Count; m++)
                {
                    inspections[m] += monkeys[m].InpsectItemsEx2(monkeys);
                }
            }
            return inspections.OrderByDescending(x => x).Take(2).Aggregate(1, (long a, long b) => a * b);
        }

        private Dictionary<int, Monkey> Parse(StreamReader input)
        {
            var text = input.ReadToEnd();
            var items = text.SplitByEmptyLine();
            return items.Select(x => Monkey.Parse(x)).ToDictionary(m => m.Id, m => m);
        }

        private class Monkey
        {
            public int Id { get; set; }
            public List<long> Items { get; set; }
            public Func<long, long> Operation { get; set; }
            public int TestNum { get; set; }
            public int TrueThrow { get; set; }
            public int FalseThrow { get; set; }

            public static long MaxWorry { get; set; } = 0;

            public bool Test(long item) => item % TestNum == 0;

            public int InpsectItems(Dictionary<int, Monkey> monkeys)
            {
                int inspections = Items.Count;
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i] = Operation(Items[i]);
                    Items[i] /= 3;
                    if (Test(Items[i]))
                        monkeys[TrueThrow].Items.Add(Items[i]);
                    else
                        monkeys[FalseThrow].Items.Add(Items[i]);
                }
                Items.Clear();
                return inspections;
            }
            public int InpsectItemsEx2(Dictionary<int, Monkey> monkeys)
            {
                int inspections = Items.Count;
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i] = Operation(Items[i]);
                    if (Test(Items[i]))
                    {
                        monkeys[TrueThrow].Items.Add(Items[i] % MaxWorry);
                    }
                    else
                    {
                        monkeys[FalseThrow].Items.Add(Items[i] % MaxWorry);
                    }
                }
                Items.Clear();
                return inspections;
            }

            public static Monkey Parse(string input)
            {
                Monkey monkey = new Monkey();
                string[] lines = input.SplitByLineBreak();
                monkey.Id = (int)char.GetNumericValue(lines[0][7]);

                monkey.Items = lines[1][18..].Split(",", StringSplitOptions.TrimEntries).Select(x => long.Parse(x)).ToList();

                Regex opRegex = new(@"  Operation: new = ((?:old)|(?:\d+)) ([*+/-]) ((?:old)|(?:\d+))");
                Match opMatch = opRegex.Match(lines[2]);
                ParameterExpression old = Expression.Parameter(typeof(long), "old");
                Expression right = opMatch.Groups[3].Value == "old" ? old : Expression.Constant(long.Parse(opMatch.Groups[3].Value));
                var operation = opMatch.Groups[2].Value switch
                {
                    "*" => Expression.Multiply(old, right),
                    "+" => Expression.Add(old, right),
                    "-" => Expression.Subtract(old, right),
                    "/" => Expression.Divide(old, right),
                    _ => throw new InvalidOperationException()
                };
                monkey.Operation = Expression.Lambda<Func<long, long>>(operation, old).Compile();

                Regex testRegex = new(@"  Test: divisible by (\d+)");
                Match testMatch = testRegex.Match(lines[3]);
                monkey.TestNum = int.Parse(testMatch.Groups[1].Value);

                monkey.TrueThrow = (int)char.GetNumericValue(lines[4].Last());
                monkey.FalseThrow = (int)char.GetNumericValue(lines[5].Last());


                return monkey;
            }
        }
    }
}
