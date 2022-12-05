using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day2 : IDay
    {
        public int Day => 2;

        public string TestInput => """
            A Y
            B X
            C Z
            """;

        public string Ex1TestResult => "15";

        public string Ex2TestResult => "12";

        public string Exercise1(StreamReader input)
        {
            var strategy = ParseEx1(input);
            return strategy.Sum(x => (int)CalcResult(x.opponent, x.you) + (int)x.you).ToString();
        }

        public string Exercise2(StreamReader input)
        {
            var strategy = ParseEx2(input);
            return strategy.Sum(x => (int)CalcYou(x.opponent, x.result) + (int)x.result).ToString();
        }

        private enum Shape
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        private enum Result
        {
            Lose = 0,
            Draw = 3,
            Win = 6,
        }

        private Result CalcResult(Shape opponent, Shape you) => (opponent, you) switch
        {
            (Shape.Rock, Shape.Paper) => Result.Win,
            (Shape.Rock, Shape.Scissors) => Result.Lose,
            (Shape.Paper, Shape.Rock) => Result.Lose,
            (Shape.Paper, Shape.Scissors) => Result.Win,
            (Shape.Scissors, Shape.Rock) => Result.Win,
            (Shape.Scissors, Shape.Paper) => Result.Lose,
            _ => Result.Draw
        };

        private Shape CalcYou(Shape opponent, Result result) => (opponent, result) switch
        {
            (Shape.Rock, Result.Lose) => Shape.Scissors,
            (Shape.Rock, Result.Win) => Shape.Paper,
            (Shape.Paper, Result.Lose) => Shape.Rock,
            (Shape.Paper, Result.Win) => Shape.Scissors,
            (Shape.Scissors, Result.Lose) => Shape.Paper,
            (Shape.Scissors, Result.Win) => Shape.Rock,
            _ => opponent
        };

        private IEnumerable<(Shape opponent, Shape you)> ParseEx1(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                string[] values = reader.ReadLine().Split(" ");
                Shape opponent = values[0] switch
                {
                    "A" => Shape.Rock,
                    "B" => Shape.Paper,
                    "C" => Shape.Scissors,
                    _ => throw new InvalidOperationException()
                };
                Shape you = values[1] switch
                {
                    "X" => Shape.Rock,
                    "Y" => Shape.Paper,
                    "Z" => Shape.Scissors,
                    _ => throw new InvalidOperationException()
                };

                yield return (opponent, you);
            }
        }

        private IEnumerable<(Shape opponent, Result result)> ParseEx2(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                string[] values = reader.ReadLine().Split(" ");
                Shape opponent = values[0] switch
                {
                    "A" => Shape.Rock,
                    "B" => Shape.Paper,
                    "C" => Shape.Scissors,
                    _ => throw new InvalidOperationException()
                };
                Result result = values[1] switch
                {
                    "X" => Result.Lose,
                    "Y" => Result.Draw,
                    "Z" => Result.Win,
                    _ => throw new InvalidOperationException()
                };

                yield return (opponent, result);
            }
        }
    }
}
