using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day8 : IDay
    {
        public int Day => 8;

        public string TestInput => """
            30373
            25512
            65332
            33549
            35390
            """;

        public string Ex1TestResult => "21";

        public string Ex2TestResult => "8";

        public string Exercise1(StreamReader input)
        {
            var trees = Parse(input);
            int visible = trees.GetLength(0) * 2 + trees.GetLength(1) * 2 - 4;
            for (int i = 1; i < trees.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < trees.GetLength(1) - 1; j++)
                {
                    if (TreeVisibile(trees, i, j))
                        visible++;
                }
            }
            return visible.ToString();
        }

        public string Exercise2(StreamReader input)
        {
            int maxScenicScore = 0;
            var trees = Parse(input);
            for (int i = 1; i < trees.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < trees.GetLength(1) - 1; j++)
                {
                    int scenicScore = CalcScenicScore(trees, i, j);
                    if (scenicScore > maxScenicScore)
                        maxScenicScore = scenicScore;
                }
            }
            return maxScenicScore.ToString();
        }
        private int[,] Parse(StreamReader input)
        {
            var grid = input.ReadToEnd().SplitByLineBreak(StringSplitOptions.RemoveEmptyEntries);
            var trees = new int[grid[0].Length, grid.Length];
            for (int i = 0; i < grid.Length; i++)
            {
                int[] row = grid[i].Select(x => (int)char.GetNumericValue(x)).ToArray();
                trees.SetRow(row, i);
            }
            return trees;
        }

        private bool TreeVisibile(int[,] trees, int x, int y)
        {
            int tree = trees[x, y];
            return trees.Row(y)[0..x].All(t => t < tree)
                || trees.Row(y)[(x + 1)..trees.GetLength(0)].All(t => t < tree)
                || trees.Col(x)[0..y].All(t => t < tree)
                || trees.Col(x)[(y + 1)..trees.GetLength(1)].All(t => t < tree);
        }

        private int CalcScenicScore(int[,] trees, int x, int y)
        {
            int tree = trees[x, y];
            int[] leftTrees = trees.Row(y)[0..x];
            int[] aboveTrees = trees.Col(x)[0..y];
            int[] rightTrees = trees.Row(y)[(x + 1)..trees.GetLength(0)];
            int[] belowTrees = trees.Col(x)[(y + 1)..trees.GetLength(1)];

            var left = leftTrees.Reverse().TakeWhile(t => t < tree).Count();
            var right = rightTrees.TakeWhile(t => t < tree).Count();
            var above = aboveTrees.Reverse().TakeWhile(t => t < tree).Count();
            var below = belowTrees.TakeWhile(t => t < tree).Count();

            if (left < leftTrees.Length) left++;
            if (right < rightTrees.Length) right++;
            if (above < aboveTrees.Length) above++;
            if (below < belowTrees.Length) below++;

            return left * right * above * below;
        }

    }
}
