using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;
using AdventOfCode24.Models;

namespace AdventOfCode24.Day_10;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		Solve<HashSet<Point>>(fileName)
			.Log(Logger, score => $"The sum of scores of all trail heads is: {score}");
	}

	protected override void SolveTwo(string fileName)
	{
		Solve<List<Point>>(fileName)
			.Log(Logger, score => $"The advanced sum of scores of all trail heads is: {score}");
	}

	private int Solve<T>(string fileName) where T : ICollection<Point>, new()
	{
		var (map, trailHeads) = GetInput(fileName);
		return trailHeads
			.Select(th => GetTrailScore<T>(map, th))
			.Sum();
	}

	private (int[][] Map, Point[] TrailHeads) GetInput(string fileName)
	{
		var map = InputReader.ReadAllLines(GetDay(), fileName)
			.Select(line => line.ToCharArray().Select(c => c - '0').ToArray())
			.ToArray();

		var trailHeads = map.GetAllMatches(0).ToArray();
		return (map, trailHeads);
	}

	private int GetTrailScore<T>(int[][] map, Point trailHead) where T : ICollection<Point>, new()
	{
		var bounds = Bounds.FromJaggedArray(map);

		T cur = [], next = [];
		cur.Add(trailHead);

		for (var i = 1; i <= 9; i++)
		{
			var nextValue = i;
			foreach (var cp in cur)
			{
				var neighbours = cp.GetOrthogonalNeighbours()
					.Where(n => n.IsInBounds(bounds) && map.Check(n, nextValue));

				foreach (var neighbour in neighbours)
				{
					next.Add(neighbour);
				}
			}

			cur = next;
			next = [];
		}

		return cur.Count;
	}
}