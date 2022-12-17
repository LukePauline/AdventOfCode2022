using AdventOfCode2022.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day15 : IDay
    {
        public int Day => 15;

        public string TestInput => """
            Sensor at x=2, y=18: closest beacon is at x=-2, y=15
            Sensor at x=9, y=16: closest beacon is at x=10, y=16
            Sensor at x=13, y=2: closest beacon is at x=15, y=3
            Sensor at x=12, y=14: closest beacon is at x=10, y=16
            Sensor at x=10, y=20: closest beacon is at x=10, y=16
            Sensor at x=14, y=17: closest beacon is at x=10, y=16
            Sensor at x=8, y=7: closest beacon is at x=2, y=10
            Sensor at x=2, y=0: closest beacon is at x=2, y=10
            Sensor at x=0, y=11: closest beacon is at x=2, y=10
            Sensor at x=20, y=14: closest beacon is at x=25, y=17
            Sensor at x=17, y=20: closest beacon is at x=21, y=22
            Sensor at x=16, y=7: closest beacon is at x=15, y=3
            Sensor at x=14, y=3: closest beacon is at x=15, y=3
            Sensor at x=20, y=1: closest beacon is at x=15, y=3
            """;

        public object Ex1TestResult => 26;

        public object Ex2TestResult => 56000011L;

        public object Exercise1(StreamReader input, bool isTest)
        {
            int targetY = isTest ? 10 : 2000000;
            HashSet<Position> noBeacons = new();
            var results = Parse(input).ToList();
            var beacons = results.Select(x => x.beacon).ToHashSet();

            foreach (var (sensor, beacon) in results)
            {
                var (dX, dY) = sensor - beacon;
                int dist = Math.Abs(dX) + Math.Abs(dY);
                if (sensor.Y + dist >= targetY && sensor.Y - dist <= targetY)
                {
                    int yy = targetY - sensor.Y;
                    int maxXx = dist - Math.Abs(yy);
                    for (int xx = -maxXx; xx <= maxXx; xx++)
                    {
                        if (!beacons.Contains(new Position(sensor.X + xx, targetY)))
                            noBeacons.Add(new Position(sensor.X + xx, targetY));
                    }
                }
            }
            return noBeacons.Count;
        }

        public object Exercise2(StreamReader input, bool isTest)
        {
            int max = isTest ? 20 : 4000000;
            Ranges[] ranges = Enumerable.Range(0, max + 1).Select(x => new Ranges()).ToArray();
            var results = Parse(input).ToList();

            foreach (var (sensor, beacon) in results)
            {
                var (dX, dY) = sensor - beacon;
                int dist = Math.Abs(dX) + Math.Abs(dY);
                for (int targetY = (sensor.Y - dist).Min(0); targetY <= (sensor.Y + dist).Max(max); targetY++)
                {
                    int yy = targetY - sensor.Y;
                    int maxXx = dist - Math.Abs(yy);
                    ranges[targetY].Add((sensor.X - maxXx).Min(0), (sensor.X + maxXx).Max(max));
                }
            }
            int y = 0;
            int x = 0;
            for (int i = 0; i <= max; i++)
            {
                if (ranges[i].Current.Count > 1)
                {
                    y = i;
                    break;
                }
            }
            x = ranges[y].Current.OrderBy(r => r.Min).ElementAt(0).Max + 1;
            return (long)x * 4000000L + (long)y;
        }

        public IEnumerable<(Position sensor, Position beacon)> Parse(StreamReader input)
        {
            Regex regex = new(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");
            while (!input.EndOfStream)
            {
                var match = regex.Match(input.ReadLine());
                if (!match.Success)
                    continue;
                var sensor = new Position(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                var beacon = new Position(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
                yield return (sensor, beacon);
            }
        }

        public record Range(int Min, int Max)
        {
            public bool Overlaps(Range range) => Min - 1 <= range.Max && Max + 1 >= range.Min;
            public bool Inside(int value) => value >= Min && value <= Max;
        }

        public class Ranges
        {
            private List<Range> _ranges = new();
            public ReadOnlyCollection<Range> Current => new(_ranges);

            public void Add(int min, int max) => Add(new Range(min, max));

            public void Add(Range range)
            {
                var crossovers = _ranges.Where(r => r.Overlaps(range)).ToArray();
                foreach (Range crossover in crossovers)
                {
                    _ranges.Remove(crossover);
                }
                List<Range> toCombine = new List<Range>(crossovers) { range };
                Range combined = new(toCombine.Min(x => x.Min), toCombine.Max(x => x.Max));
                _ranges.Add(combined);
            }
        }
    }
}
