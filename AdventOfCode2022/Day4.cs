using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day4 : IDay
    {
        public int Day => 4;

        public string TestInput => """
            2-4,6-8
            2-3,4-5
            5-7,7-9
            2-8,3-7
            6-6,4-6
            2-6,4-8
            """;

        public string Ex1TestResult => "2";

        public string Ex2TestResult => "4";

        public string Exercise1(StreamReader input)
        {
            return Parse(input).Count(x => x.Item1.Includes(x.Item2) || x.Item2.Includes(x.Item1)).ToString();
        }

        public string Exercise2(StreamReader input)
        {
            return Parse(input).Count(x => x.Item1.Overlaps(x.Item2)).ToString();
        }

        private IEnumerable<(Area, Area)> Parse(StreamReader input)
        {
            while (!input.EndOfStream)
            {
                string line = input.ReadLine();
                var areas = line.Split(',');
                yield return (ParseArea(areas[0]), ParseArea(areas[1]));
            }
        }

        private Area ParseArea(string area)
        {
            var bounds = area.Split('-');
            return new Area(int.Parse(bounds[0]), int.Parse(bounds[1]));
        }

        private record Area(int Start, int End)
        {
            public bool Includes(Area area) => Start <= area.Start && End >= area.End;
            public bool Overlaps(Area area) => Start <= area.Start && End >= area.Start
                                            || Start <= area.End && End >= area.End
                                            || Includes(area)
                                            || area.Includes(this);
        }
    }
}
