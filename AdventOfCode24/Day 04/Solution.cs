using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;
using AdventOfCode24.Models;

namespace AdventOfCode24.Day_04;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var puzzle = GetInput(fileName);
		var positions = GetCharPositions(puzzle);

		positions['X']
			.Sum(x => positions['M']
				.Where(x.IsNeighbour)
				.Count(m => puzzle.Check(2 * m - x, 'A') && puzzle.Check(3 * m - 2 * x, 'S')))
			.Log(Logger, counter => $"Total XMAS occurences: {counter}");
	}

	protected override void SolveTwo(string fileName)
	{
		var puzzle = GetInput(fileName);
		var positions = GetCharPositions(puzzle);
		var counter = 0;

		foreach (var a in positions['A'])
		{
			var diagonalOne = (puzzle.Check(a.GetUpperLeft(), 'M') && puzzle.Check(a.GetLowerRight(), 'S'))
			                  || (puzzle.Check(a.GetUpperLeft(), 'S') && puzzle.Check(a.GetLowerRight(), 'M'));

			var diagonalTwo = (puzzle.Check(a.GetLowerLeft(), 'M') && puzzle.Check(a.GetUpperRight(), 'S'))
			                  || (puzzle.Check(a.GetLowerLeft(), 'S') && puzzle.Check(a.GetUpperRight(), 'M'));

			if (diagonalOne && diagonalTwo)
				counter++;
		}

		Logger($"Total actual XMAS occurences: {counter}");
	}

	private char[][] GetInput(string fileName) =>
		InputReader
			.ReadAllLines(GetDay(), fileName)
			.Select(s => s.ToCharArray())
			.ToArray();

	private Dictionary<char, List<Point>> GetCharPositions(char[][] puzzle)
	{
		var dict = new Dictionary<char, List<Point>>();
		var points = Enumerable.Range(0, puzzle.Length)
			.SelectMany(y => Enumerable.Range(0, puzzle[0].Length).Select(x => new Point(x, y))
				.ToArray());

		foreach (var point in points)
		{
			var c = puzzle.Get(point);
			if (!dict.ContainsKey(c))
				dict.Add(c, []);
			dict[c].Add(point);
		}

		return dict;
	}
}