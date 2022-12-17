using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day14 : IDay
    {
        public int Day => 14;

        public string TestInput => """
            498,4 -> 498,6 -> 496,6
            503,4 -> 502,4 -> 502,9 -> 494,9
            """;

        public string Ex1TestResult => "24";

        public string Ex2TestResult => "93";

        public string Exercise1(StreamReader input)
        {
            HashSet<Position> blockedCells = Parse(input);
            Position origin = new(500, 0);
            var bb = GetBoundingBox(blockedCells);
            int count = 0;
            Position curr = origin;
            while (true)
            {
                var next = GetNewPosition(curr, blockedCells);
                if (next == null)
                {
                    blockedCells.Add(curr);
                    count++;
                    curr = origin;
                    continue;
                }
                if (IsOutsideBoundingBox(bb, next))
                    return count.ToString();
                curr = next;
            }
            throw new InvalidProgramException("You done goofed");
        }

        public string Exercise2(StreamReader input)
        {
            HashSet<Position> blockedCells = Parse(input);
            Position origin = new(500, 0);
            var bb = GetBoundingBox(blockedCells);
            int count = 0;
            Position curr = origin;
            while (true)
            {
                var next = GetNewPosition(curr, blockedCells);
                if (next == null || next.Y == bb.bottom + 2)
                {
                    blockedCells.Add(curr);
                    count++;

                    if (curr == origin)
                        return count.ToString();

                    curr = origin;
                    continue;
                }
                curr = next;
            }
            throw new InvalidProgramException("You done goofed");
        }

        private HashSet<Position> Parse(StreamReader input)
        {
            var blockedCells = new HashSet<Position>();
            var lines = input.ReadToEnd().SplitByLineBreak(StringSplitOptions.RemoveEmptyEntries);
            var parts = lines.Select(x => x.Split("->", StringSplitOptions.TrimEntries)).ToArray();
            foreach (var line in parts)
            {
                for (int i = 0; i < line.Length - 1; i++)
                {
                    var p1 = Position.Parse(line[i]);
                    var p2 = Position.Parse(line[i + 1]);
                    var (dX, dY) = p2 - p1;
                    var points = (dX, dY) switch
                    {
                        (0, _) => Range(p1.Y, dY).Select(y => new Position(p1.X, y)),
                        (_, 0) => Range(p1.X, dX).Select(x => new Position(x, p1.Y)),
                        _ => throw new NotImplementedException("Your assumptions about the input may be wrong.")
                    };
                    blockedCells.AddRange(points);
                }
            }
            return blockedCells;
        }

        private Position? GetNewPosition(Position current, HashSet<Position> blockedCells)
        {
            Position below = current with { Y = current.Y + 1 };
            if (!blockedCells.Contains(below))
                return below;

            Position diagLeft = below with { X = current.X - 1 };
            if (!blockedCells.Contains(diagLeft))
                return diagLeft;

            Position diagRight = below with { X = current.X + 1 };
            if (!blockedCells.Contains(diagRight))
                return diagRight;

            return null;
        }

        private (int top, int left, int bottom, int right) GetBoundingBox(HashSet<Position> positions)
        {
            var top = positions.Min(p => p.Y);
            var bottom = positions.Max(p => p.Y);
            var left = positions.Min(p => p.X);
            var right = positions.Max(p => p.X);
            if (top > 0)
                top = 0;
            if (left > 500)
                left = 500;
            if (right < 500)
                right = 500;
            if (bottom < 0)
                bottom = 0;
            return (top, left, bottom, right);
        }
        private bool IsOutsideBoundingBox((int top, int left, int bottom, int right) boundingBox, Position position) =>
            position.Y < boundingBox.top || position.Y > boundingBox.bottom || position.X < boundingBox.left || position.X > boundingBox.right;

        private IEnumerable<int> Range(int start, int dx)
        {
            var range = Enumerable.Range(0, Math.Abs(dx) + 1);
            return range.Select(x => start + Math.Sign(dx) * x);
        }

        private record Position(int X, int Y)
        {
            public static (int dX, int dY) operator -(Position a, Position b) => (a.X - b.X, a.Y - b.Y);
            public static Position Parse(string input)
            {
                var parts = input.Split(',');
                return new(int.Parse(parts[0]), int.Parse(parts[1]));
            }
        }
    }
}
