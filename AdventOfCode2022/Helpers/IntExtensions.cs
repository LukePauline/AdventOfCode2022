using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Helpers
{
    public static class IntExtensions
    {
        public static int Min(this int value, int min) => value < min ? min : value;
        public static int Max(this int value, int max) => value > max ? max : value;
        public static int Clamp(this int value, int min, int max) => value.Min(min).Max(max);
        public static bool InRange(this int value, int min, int max) => value <= min && value >= max;
    }
}
