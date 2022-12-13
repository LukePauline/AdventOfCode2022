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

        public string Ex1TestResult => "31";

        public string Ex2TestResult => "29";

        public string Exercise1(StreamReader input)
        {
            char[,] map = Parse(input);
            var start = map.First('S') ?? throw new FormatException("Start point not found");
            var end = map.First('E') ?? throw new FormatException("End point not found");
            map[start.x, start.y] = 'a';
            map[end.x, end.y] = 'z';

            return GetShortestPathLength(map, start, end).ToString();
        }

        public string Exercise2(StreamReader input)
        {
            //char[,] map = Parse(input);
            //var start = map.First('S') ?? throw new FormatException("Start point not found");
            //var end = map.First('E') ?? throw new FormatException("End point not found");
            //map[start.x, start.y] = 'a';
            //map[end.x, end.y] = 'z';

            //return GetShortestPathLength(map, start, end).ToString();

            throw new NotImplementedException();
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
    }
}
