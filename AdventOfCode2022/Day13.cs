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

        public string Ex2TestResult => "140";

        public string Exercise1(StreamReader input)
        {
            int sum = 0;
            var packets = Parse(input);
            for (int i = 0; i < packets.Length; i += 2)
            {
                if (PacketComparer.CompareLists(packets[i], packets[i + 1]).Value)
                    sum += i / 2 + 1;
            }
            return sum.ToString();
        }

        public string Exercise2(StreamReader input)
        {
            var packets = new List<object[]>(Parse(input))
            {
                new object[] { new object[] { 2 } },
                new object[] { new object[] { 6 } }
            };

            packets.Sort(new PacketComparer());

            int distressCall2 = -1;
            int distressCall6 = -1;
            for (int i = 0; i < packets.Count; i++)
            {
                string p = StringifyPacket(packets[i]);
                if (p == "[[2]]")
                    distressCall2 = i + 1;
                else if (p == "[[6]]")
                    distressCall6 = i + 1;
            }
            return (distressCall2 * distressCall6).ToString();
        }


        private string StringifyPacket(object[] packet)
        {
            string line = "[";
            foreach (var item in packet)
            {
                if (item is int)
                    line += item.ToString();
                if (item is object[] obj)
                    line += StringifyPacket(obj);
                line += ",";
            }
            int lastComma = line.LastIndexOf(',');
            if (lastComma != -1)
                line = line[..^1];
            line += "]";
            return line;
        }

        private object[][] Parse(StreamReader input) => input.ReadToEnd().SplitByLineBreak(StringSplitOptions.RemoveEmptyEntries).Select(x => ParseList(new Queue<char>(x)).ToArray()).ToArray();

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

        private class PacketComparer : IComparer<object[]>
        {
            public static bool? CompareLists(object[] left, object[] right)
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

            private static bool? CompareNumbers(int left, int right) => Math.Sign(left - right) switch
            {
                -1 => true,
                0 => null,
                1 => false,
                _ => throw new NotImplementedException()
            };
            public int Compare(object[] x, object[] y)
            {
                return CompareLists(x, y) switch
                {
                    true => -1,
                    null => 0,
                    false => 1
                };
            }
        }
    }
}
