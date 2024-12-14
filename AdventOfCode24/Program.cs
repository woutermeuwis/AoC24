using AdventOfCode24;
using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

var config = new
{
	Day = 12,
	Days = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 },
	RunMultiple = false,
	RunExamples = true,
	RunInput = true,
	RunPartOne = true,
	RunPartTwo = true,
};

using var outerLogger = new LogHandle();
if (config.RunMultiple)
	config.Days.ForEach(RunDay);
else
	RunDay(config.Day);
outerLogger.Log("Finished running");


void RunDay(int day)
{
	var runner = Type.GetType($"AdventOfCode24.Day_{day:D2}.Solution") ?? throw new InvalidOperationException();
	var instance = Activator.CreateInstance(runner) as BaseSolution ?? throw new InvalidOperationException();

	if (config is { RunPartOne: true, RunExamples: true })
		instance.SolveExampleOne();
	if (config is { RunPartOne: true, RunInput: true })
		instance.SolvePartOne();
	if (config is { RunPartTwo: true, RunExamples: true })
		instance.SolveExampleTwo();
	if (config is { RunPartTwo: true, RunInput: true })
		instance.SolvePartTwo();
}