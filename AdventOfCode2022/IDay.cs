
namespace AdventOfCode2022
{
    public interface IDay
    {
        int Day { get; }
        string TestInput { get; }
        string Ex1TestResult { get; }
        string Ex2TestResult { get; }

        string Exercise1(StreamReader input);
        string Exercise2(StreamReader input);
    }
}