using AdventOfCode2022;
using System.Reflection;
using static AdventOfCode2022.Helpers.InputHelpers;

int dayNo = 15;// DateTime.UtcNow.Day;

Assembly assembly = Assembly.GetExecutingAssembly();
Type tDay = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IDay))).Single(t => t.Name == $"Day{dayNo}");

IDay day = (IDay)Activator.CreateInstance(tDay);

using StreamReader test = ConvertStringToStream(day.TestInput);
object ex1Test = day.Exercise1(test, true);

using StreamReader input = await GetInput(day.Day);

Console.WriteLine("--- Exercise 1 ---");
Console.WriteLine();

Console.WriteLine("Test:");
Console.WriteLine(ex1Test);
Console.WriteLine(ex1Test.Equals(day.Ex1TestResult) ? "PASS" : "FAIL");
Console.WriteLine();

Console.WriteLine("Answer");
Console.WriteLine(day.Exercise1(input, false));

Console.WriteLine();
Console.WriteLine();

try
{
    test.BaseStream.Position = 0;
    input.BaseStream.Position = 0;
    object ex2Test = day.Exercise2(test, true);

    Console.WriteLine("--- Exercise 2 ---");
    Console.WriteLine();

    Console.WriteLine("Test:");
    Console.WriteLine(ex2Test);
    Console.WriteLine(ex2Test.Equals(day.Ex2TestResult) ? "PASS" : "FAIL");
    Console.WriteLine();

    Console.WriteLine("Answer");
    Console.WriteLine(day.Exercise2(input, false));
}
catch (NotImplementedException) { }
