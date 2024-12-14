using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24.Day_07;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		GetInput(fileName)
			.Where(r => CanBeSolved(r, false))
			.Sum(report => report.Calibration)
			.Log(Logger, sum => $"The total calibration result is {sum}");
	}

	protected override void SolveTwo(string fileName)
	{
		GetInput(fileName)
			.Where(r => CanBeSolved(r, true))
			.Sum(report => report.Calibration)
			.Log(Logger, sum => $"The total calibration result is {sum}");
	}

	private (long Calibration, long[] Operators)[] GetInput(string fileName) =>
		InputReader.ReadAllLines(GetDay(), fileName)
			.Select(calibration => calibration.Split(": "))
			.Select(arr => (arr[0].ToLong(), arr[1].Split(' ').Select(x => x.ToLong()).ToArray()))
			.ToArray();

	private bool CanBeSolved((long calibration, long[] operators) report, bool canConcatenate)
	{
		List<long> memory = [];
		var (calibration, operators) = report;
		memory.Add(operators[0]);
		foreach (var op in operators[1..])
		{
			var backup = memory.ToList();
			memory = backup
				.SelectMany(b => Process(b, op, canConcatenate))
				.ToList();
		}

		return memory.Contains(calibration);
	}

	private long[] Process(long op1, long op2, bool canConcatenate)
	{
		return canConcatenate
			? [op1 + op2, op1 * op2, $"{op1}{op2}".ToLong()]
			: [op1 + op2, op1 * op2];
	}
}