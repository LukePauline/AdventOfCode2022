using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public static class StringExtensions
    {
        public static string[] SplitByLineBreak(this string input, StringSplitOptions stringSplitOptions = StringSplitOptions.None) => input.Split(new[] { "\r\n", "\r", "\n" }, stringSplitOptions);
        public static string[] SplitByEmptyLine(this string input, StringSplitOptions stringSplitOptions = StringSplitOptions.None) => input.Split(new[] { "\r\n\r\n", "\r\r", "\n\n" }, stringSplitOptions);
    }
}
