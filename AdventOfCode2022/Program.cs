using AdventOfCode2022;
using System.Reflection;
using static AdventOfCode2022.InputHelpers;

int dayNo = DateTime.UtcNow.Day;

Assembly assembly = Assembly.GetExecutingAssembly();
Type tDay = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IDay))).Single(t => t.Name == $"Day{dayNo}");

IDay day = (IDay)Activator.CreateInstance(tDay);

using StreamReader test = ConvertStringToStream(day.TestInput);
string ex1Test = day.Exercise1(test);

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
string ex2Test = day.Exercise2(test);

Console.WriteLine("--- Exercise 2 ---");
Console.WriteLine();

Console.WriteLine("Test:");
Console.WriteLine(ex2Test);
Console.WriteLine(ex2Test == day.Ex2TestResult ? "PASS" : "FAIL");
Console.WriteLine();

Console.WriteLine("Answer");
Console.WriteLine(day.Exercise2(input));
