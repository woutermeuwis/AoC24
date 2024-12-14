using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24.Day_01;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var (left, right) = GetInput(fileName);
		left.Sort();
		right.Sort();
		left.Zip(right, MathHelper.AbsDiff)
			.Sum()
			.Log(Logger, sum => $"Total Distance: {sum}");
	}

	protected override void SolveTwo(string fileName)
	{
		var (left, right) = GetInput(fileName);

		var distinct = left.Distinct();

		var lookupTable = right
			.GroupBy(x => x)
			.ToDictionary(g => g.Key, g => g.Count());

		distinct
			.Select(i => i * lookupTable.GetValueOrDefault(i, 0))
			.Sum()
			.Log(Logger, sum => $"Total Similarity: {sum}");
	}

	private (List<long> left, List<long> right) GetInput(string file)
	{
		List<long> left = [], right = [];
		foreach (var line in InputReader.ReadAllLines(GetDay(), file))
		{
			var split = line.Split("   ");
			left.Add(split[0].ToLong());
			right.Add(split[1].ToLong());
		}

		return (left, right);
	}
}