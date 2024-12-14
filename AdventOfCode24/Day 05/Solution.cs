using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24.Day_05;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var (rules, updates) = GetInput(fileName);
		updates
			.Where(u => IsSafe(rules, u))
			.Sum(u => u[u.Length / 2])
			.Log(Logger, sum => $"The sum of the middle page of each correctly numbered update is: {sum}");
	}

	protected override void SolveTwo(string fileName)
	{
		var (rules, updates) = GetInput(fileName);
		updates
			.Where(u => !IsSafe(rules, u))
			.Select(u => FixOrder(rules, u))
			.Sum(u => u[u.Length / 2])
			.Log(Logger, sum => $"The sum of the middle page of each correctly numbered update is: {sum}");
	}

	private (Dictionary<int, int[]> Rules, int[][] Updates) GetInput(string fileName)
	{
		var input = InputReader.ReadAllLines(GetDay(), fileName);
		var index = input.ToList().IndexOf("");

		var rules = input[0..index]
			.Select(r => r.Split("|").Select(int.Parse).ToArray())
			.GroupBy(r => r[0])
			.ToDictionary(g => g.Key, g => g.Select(r => r[1]).Distinct().ToArray());

		var updates = input[(index + 1)..]
			.Select(u => u.Split(",").Select(int.Parse).ToArray())
			.ToArray();

		return (rules, updates);
	}

	private bool IsSafe(Dictionary<int, int[]> rules, int[] update)
	{
		for (var i = 0; i < update.Length; i++)
		{
			if (!rules.ContainsKey(update[i]))
				continue;

			if (rules[update[i]].Any(r => update[..i].Contains(r)))
				return false;
		}

		return true;
	}

	private int[] FixOrder(Dictionary<int, int[]> rules, int[] update)
	{
		var ordered = new List<int>();
		while (update.Length > 0)
		{
			for (var i = 0; i < update.Length; i++)
			{
				var value = update[i];
				if (rules.TryGetValue(value, out var rule) && rule.Any(x => update.Contains(x)))
					continue;
				ordered.Add(value);
				update = update.WithoutIndex(i);
				break;
			}
		}

		ordered.Reverse();
		return ordered.ToArray();
	}
}