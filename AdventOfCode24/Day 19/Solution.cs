using System.Text.RegularExpressions;
using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24.Day_19;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var (towels, combinations) = GetInput(fileName);
		var regex = new Regex(@"^(" + string.Join('|', towels) + @")*$");
		combinations
			.Count(regex.IsMatch)
			.Log(Logger, cnt => $"The amount of valid combinations is: {cnt}");
	}

	protected override void SolveTwo(string fileName)
	{
		var (towels, combinations) = GetInput(fileName);
		var regex = new Regex(@"^(" + string.Join('|', towels) + @")*$");
		combinations
			.Where(c => regex.IsMatch(c))
			.Sum(c => GetAmountOfCombinations(towels, c))
			.Log(Logger, cnt => $"The amount of ways the valid combinations can be created is: {cnt}");
		
		//8464 low
	}

	private (string[] towels, string[] combinations) GetInput(string fileName)
	{
		var lines = InputReader.ReadAllLines(GetDay(), fileName);
		var towels = lines[0].Split(", ");
		var combinations = lines[2..];
		return (towels, combinations);
	}

	private long GetAmountOfCombinations(string[] towels, string combination)
	{
		List<(string combi, long cnt)> running = towels.Select(t => (t, 1L)).ToList(), updatedValues = [];
		long found = 0;
		while (running.Any())
		{
			foreach (var (cur, cnt) in running)
			{
				if (cur == combination)
				{
					found+= cnt;
					continue;
				}
				updatedValues.AddRange(towels.Select(t => (combi: cur + t, cnt)).Where(x=> combination.StartsWith(x.combi)));
			}

			running = updatedValues.GroupBy(x=>x.combi).Select(g=> (g.Key, g.Sum(x=>x.cnt))).ToList();
			updatedValues = [];
		}
		return found;
	}
}