using System.Text.RegularExpressions;
using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24.Day_03;

public class Solution : BaseSolution
{
	private const string RegexPattern = @"mul\((\d{1,3}),(\d{1,3})\)";

	protected override void SolveOne(string fileName)
		=> InputReader.ReadAllText(GetDay(), fileName)
			.GetRegexMatches(RegexPattern)
			.Sum(Calculate)
			.Log(Logger, sum => $"Uncorrupted Multiplications: {sum}");

	protected override void SolveTwo(string fileName)
		=> InputReader.ReadAllText(GetDay(), fileName)
			.Split("do()")
			.SelectMany(d => d.Split("don't()")[0].GetRegexMatches(RegexPattern))
			.Sum(Calculate)
			.Log(Logger, sum => $"Uncorrupted Extended result: {sum}");

	private int Calculate(Match m)
		=> m.Groups[1].Value.ToInt() * m.Groups[2].Value.ToInt();
}