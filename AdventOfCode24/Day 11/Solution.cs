using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24.Day_11;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		GetInput(fileName).Select(s => Blink(s, 25))
			.Sum()
			.Log(Logger, count => $"After blinking 25 times the amount of stones is: {count}");
	}

	protected override void SolveTwo(string fileName)
	{
		GetInput(fileName).Select(s => Blink(s, 75))
			.Sum()
			.Log(Logger, count => $"After blinking 75 times the amount of stones is: {count}");
	}

	private long[] GetInput(string fileName)
		=> InputReader
			.ReadAllText(GetDay(), fileName).Split(' ')
			.Select(stringExtensions.ToLong)
			.ToArray();

	private StonePile[] ProcessStone(StonePile stone)
	{
		if (stone is { Value: 0 })
			return [stone with { Value = 1 }];

		var str = stone.Value.ToString();
		var l = str.Length;
		if (l % 2 is 0)
			return
			[
				stone with { Value = str[..(l / 2)].ToLong() },
				stone with { Value = str[(l / 2)..].ToLong() }
			];

		return [stone with { Value = stone.Value * 2024 }];
	}

	private long Blink(long stone, long amountOfBlinks)
	{
		StonePile[] stones = [new(stone, 1)];
		for (var i = 0; i < amountOfBlinks; i++)
		{
			stones = stones
				.SelectMany(ProcessStone)
				.GroupBy(s => s.Value)
				.Select(g => new StonePile(g.Key, g.Sum(s => s.Count)))
				.ToArray();
		}

		return stones.Sum(s => s.Count);
	}

	private record StonePile(long Value, long Count);
}