using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;
using AdventOfCode24.Models;

namespace AdventOfCode24.Day_08;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName, LogHandle logger)
	{
		var data = GetInput(fileName); 
		data.Antennae.Values
			.SelectMany(GetAntiNodes)
			.Where(p=> p.IsInBounds(data.Bounds))
			.Distinct()
			.Count()
			.Log(logger, count => $"Number of unique antinodes: {count}");
	}

	protected override void SolveTwo(string fileName, LogHandle logger)
	{
		var data = GetInput(fileName);
		data.Antennae.Values
			.SelectMany(points => GetHarmonicAntiNodes(points, data.Bounds))
			.Distinct()
			.Count()
			.Log(logger, count => $"Number of unique harmonic antinodes: {count}");
	}

	private Map GetInput(string fileName)
	{
		var map = InputReader.ReadAllLines(GetDay(), fileName)
			.Select(line => line.ToCharArray())
			.ToArray();

		var dict = new Dictionary<char, List<Point>>();
		map.ForEach((point, c) =>
		{
			if (c == '.')
				return;
			if (!dict.ContainsKey(c))
				dict.Add(c, []);
			dict[c].Add(point);
		});
		return new(new(map[0].Length - 1, map.Length - 1), dict);
	}

	private IEnumerable<Point> GetAntiNodes(List<Point> points)
	{
		if (points is { Count: > 1 })
		{
			for (var i = 0; i < points.Count; i++)
			{
				var point1 = points[i];
				foreach (var point2 in points[(i + 1)..])
				{
					var vector = point2 - point1;
					yield return point1 - vector;
					yield return point2 + vector;
				}
			}
		}
	}

	private IEnumerable<Point> GetHarmonicAntiNodes(List<Point> points, Bounds bounds)
	{
		if (points is { Count: > 1 })
		{
			for (var i = 0; i < points.Count; i++)
			{
				var point1 = points[i];
				foreach (var point2 in points[(i + 1)..])
				{
					var vector = point2 - point1;
					
					Point tmp = point1;
					while (tmp.IsInBounds(bounds))
					{
						yield return tmp;
						tmp -= vector;
					}

					tmp = point2;
					while (tmp.IsInBounds(bounds))
					{
						yield return tmp;
						tmp += vector;
					}
				}
			}
		}
	}
}

public record Map(Bounds Bounds, Dictionary<char, List<Point>> Antennae);