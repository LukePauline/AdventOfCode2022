
namespace AdventOfCode2022
{
    public interface IDay
    {
        int Day { get; }
        string TestInput { get; }
        object Ex1TestResult { get; }
        object Ex2TestResult { get; }

        object Exercise1(StreamReader input, bool isTest);
        object Exercise2(StreamReader input, bool isTest);
    }
}