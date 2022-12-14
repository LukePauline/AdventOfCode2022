using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day13 : IDay
    {
        public int Day => 13;

        public string TestInput => """
            [1,1,3,1,1]
            [1,1,5,1,1]

            [[1],[2,3,4]]
            [[1],4]

            [9]
            [[8,7,6]]

            [[4,4],4,4]
            [[4,4],4,4,4]

            [7,7,7,7]
            [7,7,7]

            []
            [3]

            [[[]]]
            [[]]

            [1,[2,[3,[4,[5,6,7]]]],8,9]
            [1,[2,[3,[4,[5,6,0]]]],8,9]
            """;

        public string Ex1TestResult => "13";

        public string Ex2TestResult => throw new NotImplementedException();

        public string Exercise1(StreamReader input)
        {
            int sum = 0;
            var pairs = Parse(input);
            for (int i = 0; i < pairs.Length; i++)
            {
                if (CompareLists(pairs[i].left, pairs[i].right).Value)
                    sum += i + 1;
            }
            return sum.ToString();
        }

        public string Exercise2(StreamReader input)
        {
            throw new NotImplementedException();
        }

        private (object[] left, object[] right)[] Parse(StreamReader input)
        {
            var pairs = input.ReadToEnd().SplitByEmptyLine();
            return pairs.Select(pair =>
            {
                var lines = pair.SplitByLineBreak().Select(x => new Queue<char>(x)).ToArray();
                return (ParseList(lines[0]).ToArray(), ParseList(lines[1]).ToArray());
            }).ToArray();
        }

        public static IEnumerable<object> ParseList(Queue<char> line)
        {
            line.Dequeue();
            while (line.Count > 0)
            {
                char next = line.Peek();
                if (next == '[')
                {
                    object[] list = ParseList(line).ToArray();
                    yield return list;
                }
                else if (next == ']')
                {
                    line.Dequeue();
                    break;
                }
                else if (next == ',')
                {
                    line.Dequeue();
                }
                else
                {
                    yield return ReadNumber(line);
                }
            }
        }

        private static int ReadNumber(Queue<char> line)
        {
            char next = line.Peek();
            string num = string.Empty;

            while (next != ',' && next != ']')
            {
                num += line.Dequeue();
                next = line.Peek();
            }
            return int.Parse(num);
        }

        private bool? CompareLists(object[] left, object[] right)
        {
            for (int i = 0; i < left.Length; i++)
            {
                if (i >= right.Length)
                    return false;

                if ((left[i], right[i]) is (int lNum, int rNum))
                {
                    var result = CompareNumbers(lNum, rNum);
                    if (result == null)
                        continue;
                    return result.Value;
                }

                object[] l = left[i] as object[] ?? new object[] { left[i] };
                object[] r = right[i] as object[] ?? new object[] { right[i] };
                bool? listResult = CompareLists(l, r);
                if (listResult.HasValue)
                    return listResult.Value;
            }

            return right.Length > left.Length ? true : null;
        }

        private bool? CompareNumbers(int left, int right) => Math.Sign(left - right) switch
        {
            -1 => true,
            0 => null,
            1 => false,
            _ => throw new NotImplementedException()
        };
    }
}
