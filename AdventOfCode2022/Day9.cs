using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day9 : IDay
    {
        public int Day => 9;

        public string TestInput => """
            R 4
            U 4
            L 3
            D 1
            R 4
            D 1
            L 5
            R 2
            """;

        public string Ex1TestResult => "13";

        public string Ex2TestResult => "1";

        public string Exercise1(StreamReader input)
        {
            IEnumerable<Move> moves = Parse(input);
            IEnumerable<Point> tailPositions = TrackRope(moves, 2);
            return tailPositions.Distinct().Count().ToString();
        }

        public string Exercise2(StreamReader input)
        {
            IEnumerable<Move> moves = Parse(input);
            IEnumerable<Point> tailPositions = TrackRope(moves, 10);
            return tailPositions.Distinct().Count().ToString();
        }

        private IEnumerable<Point> TrackRope(IEnumerable<Move> moves, int length)
        {
            // Initial conditions
            Point[] knots = Enumerable.Repeat(new Point(0, 0), length).ToArray();

            foreach (var move in moves)
            {
                // Apply movement
                for (int i = 0; i < move.Distance; i++)
                {
                    switch (move.Direction)
                    {
                        case Direction.Up:
                            knots[0].Y++;
                            break;
                        case Direction.Down:
                            knots[0].Y--;
                            break;
                        case Direction.Left:
                            knots[0].X--;
                            break;
                        case Direction.Right:
                            knots[0].X++;
                            break;
                    }

                    for (int j = 1; j < knots.Length; j++)
                    {
                        Point head = knots[j - 1];
                        Point tail = knots[j];

                        knots[j] = (head.X - tail.X, head.Y - tail.Y) switch
                        {
                            (2, 2) => new(tail.X + 1, tail.Y + 1),
                            (2, -2) => new(tail.X + 1, tail.Y - 1),
                            (-2, 2) => new(tail.X - 1, tail.Y + 1),
                            (-2, -2) => new(tail.X - 1, tail.Y - 1),
                            (2, _) => new(tail.X + 1, head.Y),
                            (-2, _) => new(tail.X - 1, head.Y),
                            (_, 2) => new(head.X, tail.Y + 1),
                            (_, -2) => new(head.X, tail.Y - 1),
                            _ => tail
                        };
                    }

                    yield return knots.Last();
                }
            }
        }

        private IEnumerable<Move> Parse(StreamReader input) =>
            input.ReadToEnd()
            .SplitByLineBreak(StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Move.Parse(x));

        private record Move(Direction Direction, int Distance)
        {
            public static Move Parse(string input)
            {
                Direction direction = input[0] switch
                {
                    'U' => Direction.Up,
                    'D' => Direction.Down,
                    'L' => Direction.Left,
                    'R' => Direction.Right,
                    _ => throw new InvalidOperationException($"Inavlid direction {input[0]}")
                };
                int distance = int.Parse(input[2..]);

                return new Move(direction, distance);
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
