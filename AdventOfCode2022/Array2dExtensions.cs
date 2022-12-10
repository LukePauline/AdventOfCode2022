using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public static class Array2dExtensions
    {
        public static T[] Row<T>(this T[,] input, int y)
        {
            var row = new T[input.GetLength(0)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                row[i] = input[i, y];
            }
            return row;
        }

        public static T[] Col<T>(this T[,] input, int x)
        {
            var col = new T[input.GetLength(1)];
            for (int i = 0; i < input.GetLength(1); i++)
            {
                col[i] = input[x, i];
            }
            return col;
        }

        public static void SetRow<T>(this T[,] input, T[] value, int y)
        {
            for (int i = 0; i < input.GetLength(0); i++)
            {
                input[i, y] = value[i];
            }
        }

        public static string Render<T>(this T[,] values)
        {
            StringBuilder output = new();
            for (int y = 0; y < values.GetLength(1); y++)
            {
                for (int x = 0; x < values.GetLength(0); x++)
                {
                    output.Append(values[x, y]);
                }
                output.AppendLine();
            }
            return output.ToString();
        }
    }
}
