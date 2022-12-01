
namespace AdventOfCode2022
{
    public interface IDay
    {
        int Day { get; }
        string TestInput { get; }
        int Ex1TestResult { get; }
        int Ex2TestResult { get; }

        int Exercise1(StreamReader input);
        int Exercise2(StreamReader input);
    }
}