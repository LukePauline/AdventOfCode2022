using AdventOfCode2022;
using static AdventOfCode2022.InputHelpers;

IDay day = new Day2();

using StreamReader test = ConvertStringToStream(day.TestInput);
int ex1Test = day.Exercise1(test);

using StreamReader input = await GetInput(day.Day);

Console.WriteLine("--- Exercise 1 ---");
Console.WriteLine();

Console.WriteLine("Test:");
Console.WriteLine(ex1Test);
Console.WriteLine(ex1Test == day.Ex1TestResult ? "PASS" : "FAIL");
Console.WriteLine();

Console.WriteLine("Answer");
Console.WriteLine(day.Exercise1(input));
Console.WriteLine();
Console.WriteLine();

test.BaseStream.Position = 0;
input.BaseStream.Position = 0;
int ex2Test = day.Exercise2(test);

Console.WriteLine("--- Exercise 2 ---");
Console.WriteLine();

Console.WriteLine("Test:");
Console.WriteLine(ex2Test);
Console.WriteLine(ex2Test == day.Ex2TestResult ? "PASS" : "FAIL");
Console.WriteLine();

Console.WriteLine("Answer");
Console.WriteLine(day.Exercise2(input));