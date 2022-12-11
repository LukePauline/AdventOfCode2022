using System.Linq.Expressions;
using System.Text.RegularExpressions;

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

        public string Ex1TestResult => "10605";

        public string Ex2TestResult => throw new NotImplementedException();

        public string Exercise1(StreamReader input)
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
            return inspections.OrderByDescending(x => x).Take(2).Aggregate(1, (a, b) => a * b).ToString();
        }

        public string Exercise2(StreamReader input)
        {
            throw new NotImplementedException();
        }

        private Dictionary<int, Monkey> Parse(StreamReader input)
        {
            var text = input.ReadToEnd();
            var items = text.SplitByEmptyLines();
            return items.Select(x => Monkey.Parse(x)).ToDictionary(m => m.Id, m => m);
        }

        private class Monkey
        {
            public int Id { get; set; }
            public List<int> Items { get; set; }
            public Func<int, int> Operation { get; set; }
            public Func<int, bool> Test { get; set; }
            public int TrueThrow { get; set; }
            public int FalseThrow { get; set; }

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

            public static Monkey Parse(string input)
            {
                Monkey monkey = new Monkey();
                string[] lines = input.SplitByLineBreak();
                monkey.Id = (int)char.GetNumericValue(lines[0][7]);

                monkey.Items = lines[1][18..].Split(",", StringSplitOptions.TrimEntries).Select(x => int.Parse(x)).ToList();

                Regex opRegex = new(@"  Operation: new = ((?:old)|(?:\d+)) ([*+/-]) ((?:old)|(?:\d+))");
                Match opMatch = opRegex.Match(lines[2]);
                ParameterExpression old = Expression.Parameter(typeof(int), "old");
                Expression right = opMatch.Groups[3].Value == "old" ? old : Expression.Constant(int.Parse(opMatch.Groups[3].Value));
                var operation = opMatch.Groups[2].Value switch
                {
                    "*" => Expression.Multiply(old, right),
                    "+" => Expression.Add(old, right),
                    "-" => Expression.Subtract(old, right),
                    "/" => Expression.Divide(old, right),
                    _ => throw new InvalidOperationException()
                };
                monkey.Operation = Expression.Lambda<Func<int, int>>(operation, old).Compile();

                Regex testRegex = new(@"  Test: divisible by (\d+)");
                Match testMatch = testRegex.Match(lines[3]);
                monkey.Test = (val) => val % int.Parse(testMatch.Groups[1].Value) == 0;

                monkey.TrueThrow = (int)char.GetNumericValue(lines[4].Last());
                monkey.FalseThrow = (int)char.GetNumericValue(lines[5].Last());


                return monkey;
            }
        }
    }
}
