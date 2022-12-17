using AdventOfCode2022.Helpers;

namespace AdventOfCode2022
{
    public class Day12 : IDay
    {
        public int Day => 12;

        public string TestInput => """
            Sabqponm
            abcryxxl
            accszExk
            acctuvwj
            abdefghi
            """;

        public object Ex1TestResult => 31;

        public object Ex2TestResult => 29;

        public object Exercise1(StreamReader input, bool isTest)
        {
            char[,] map = Parse(input);
            var start = map.FirstIndex(x => x == 'S') ?? throw new FormatException("Start point not found");
            var end = map.FirstIndex(x => x == 'E') ?? throw new FormatException("End point not found");
            map[start.x, start.y] = 'a';
            map[end.x, end.y] = 'z';

            return GetShortestPathLength(map, start, end);
        }

        public object Exercise2(StreamReader input, bool isTest)
        {
            char[,] map = Parse(input);
            var start = map.FirstIndex(x => x == 'S') ?? throw new FormatException("Start point not found");
            var end = map.FirstIndex(x => x == 'E') ?? throw new FormatException("End point not found");
            map[start.x, start.y] = 'a';
            map[end.x, end.y] = 'z';

            var candidates = map.WhereIndices(x => x == 'a');

            return GetShortestPathLengthInvertedEx2(map, end, candidates);
        }

        private char[,] Parse(StreamReader input)
        {
            var lines = input.ReadToEnd().SplitByLineBreak(StringSplitOptions.RemoveEmptyEntries);
            var grid = new char[lines[0].Length, lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                grid.SetRow(lines[i].ToCharArray(), i);
            }
            return grid;
        }

        private int GetShortestPathLength(char[,] map, (int x, int y) start, (int x, int y) end)
        {
            bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];
            visited.Fill(false);
            visited[start.x, start.y] = true;

            int[,] distance = new int[map.GetLength(0), map.GetLength(1)];
            distance.Fill(int.MaxValue - 1);
            distance[start.x, start.y] = 0;

            HashSet<(int x, int y)> unvisited = Enumerable.Range(0, map.GetLength(0))
                .SelectMany(x => Enumerable.Range(0, map.GetLength(1))
                    .Select(y => (x, y)))
                .ToHashSet();

            var node = start;
            while (node != end)
            {
                var (x, y) = node;
                var candidates = new[]
                {
                    (x-1,y),
                    (x+1,y),
                    (x,y-1),
                    (x,y+1)
                }.Where(((int x, int y) n) => n.x >= 0 && n.x < map.GetLength(0)
                                           && n.y >= 0 && n.y < map.GetLength(1)
                                           && unvisited.Contains(n)
                                           && map[n.x, n.y] - map[x, y] <= 1);
                foreach (var (cx, cy) in candidates)
                {
                    if (distance[x, y] + 1 < distance[cx, cy])
                        distance[cx, cy] = distance[x, y] + 1;
                }
                unvisited.Remove((x, y));
                visited[x, y] = true;

                node = unvisited.Aggregate((m, n) => distance[n.x, n.y] < distance[m.x, m.y] ? n : m);
            }

            return distance[end.x, end.y];
        }

        private int GetShortestPathLengthInvertedEx2(char[,] map, (int x, int y) start, IEnumerable<(int x, int y)> ends)
        {
            bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];
            visited.Fill(false);
            visited[start.x, start.y] = true;

            int[,] distance = new int[map.GetLength(0), map.GetLength(1)];
            distance.Fill(int.MaxValue - 1);
            distance[start.x, start.y] = 0;

            HashSet<(int x, int y)> unvisited = Enumerable.Range(0, map.GetLength(0))
                .SelectMany(x => Enumerable.Range(0, map.GetLength(1))
                    .Select(y => (x, y)))
                .ToHashSet();

            var node = start;
            while (ends.Any(x => unvisited.Contains(x)))
            {
                var (x, y) = node;
                var candidates = new[]
                {
                    (x-1,y),
                    (x+1,y),
                    (x,y-1),
                    (x,y+1)
                }.Where(((int x, int y) n) => n.x >= 0 && n.x < map.GetLength(0)
                                           && n.y >= 0 && n.y < map.GetLength(1)
                                           && unvisited.Contains(n)
                                           && map[n.x, n.y] - map[x, y] >= -1);
                foreach (var (cx, cy) in candidates)
                {
                    if (distance[x, y] + 1 < distance[cx, cy])
                        distance[cx, cy] = distance[x, y] + 1;
                }
                unvisited.Remove((x, y));
                visited[x, y] = true;

                if (!unvisited.Any())
                    break;

                node = unvisited.Aggregate((m, n) => distance[n.x, n.y] < distance[m.x, m.y] ? n : m);
            }

            return ends.Min(n => distance[n.x, n.y]);
        }
    }
}
