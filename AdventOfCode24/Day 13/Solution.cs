using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;
using AdventOfCode24.Models;

namespace AdventOfCode24.Day_13;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		GetMachines(fileName)
			.Sum(FindMinTokensToWin)
			.Log(Logger, tokens => $"The minimum amount of tokens to spend to win all prizes is: {tokens}");
	}

	protected override void SolveTwo(string fileName)
	{
		GetMachines(fileName, 10000000000000)
			.Sum(FindMinTokensToWinWithoutLimit)
			.Log(Logger, tokens => $"The minimum amount of tokens to spend to win all prizes is: {tokens}");
	}

	private IEnumerable<ClawMachine> GetMachines(string fileName, long offset = 0)
	{
		var lines = InputReader.ReadAllLines(GetDay(), fileName);
		for (var i = 0; i < lines.Length; i += 4)
		{
			var a = lines[i].GetRegexMatch(@"Button A: X\+(\d*), Y\+(\d*)").Groups;
			var b = lines[i + 1].GetRegexMatch(@"Button B: X\+(\d*), Y\+(\d*)").Groups;
			var prize = lines[i + 2].GetRegexMatch(@"Prize: X=(\d*), Y=(\d*)").Groups;

			yield return new(
				new(a[1].Value.ToInt(), a[2].Value.ToInt()),
				new(b[1].Value.ToInt(), b[2].Value.ToInt()),
				new(prize[1].Value.ToInt() + offset, prize[2].Value.ToInt() + offset)
			);
		}
	}

	private long FindMinTokensToWin(ClawMachine machine)
	{
		var (a, b, prize) = machine;

		// validate possibility! No button can be pressed more than 100 times
		var max = a * 100 + b * 100;
		if (max.IsLeftOf(prize) || max.IsAbove(prize))
			return 0;

		// brute force it...
		var minTokens = long.MaxValue;
		for (var i = 0; i <= 100; i++)
		{
			var remainder = prize - i * a;
			if (remainder % b == Point.Zero && remainder.IsDivisibleBy(b) && remainder / b <= 100)
			{
				minTokens = Math.Min(minTokens, 3 * i + remainder / b);
			}
		}

		return minTokens == long.MaxValue
			? 0
			: minTokens;
	}

	private long FindMinTokensToWinWithoutLimit(ClawMachine machine)
	{
		var ((aX, aY), (bX, bY), (pX, pY)) = machine;

		var b = (pY * aX - pX * aY) / (bY * aX - bX * aY);
		var a = (pX - bX * b) / aX;

		if (a * machine.A + b * machine.B != machine.Prize)
			return 0;
		return 3 * a + b;
	}

	private record ClawMachine(Point A, Point B, Point Prize);
}