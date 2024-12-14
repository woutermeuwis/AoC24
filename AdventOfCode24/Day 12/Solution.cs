using AdventOfCode24.Enums;
using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;
using AdventOfCode24.Models;

namespace AdventOfCode24.Day_12;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var map = GetInput(fileName);
		var zones = GetZones(map)
			.ToArray();
		zones
			.Select(GetZoneFencePrice)
			.Sum()
			.Log(Logger, sum => $"The total price of fencing for all regions is: {sum}");
	}

	protected override void SolveTwo(string fileName)
	{
		var map = GetInput(fileName);
		var zones = GetZones(map)
			.ToArray();
		zones
			.Select(GetZoneFencePriceWithDiscount)
			.Sum()
			.Log(Logger, sum => $"The total price of fencing for all regions is: {sum}");
	}


	private char[][] GetInput(string fileName)
		=> InputReader.ReadAllLines(GetDay(), fileName)
			.Select(line => line.ToCharArray())
			.ToArray();

	private IEnumerable<Point[]> GetZones(char[][] input)
	{
		var data = input.Copy();
		var bounds = Bounds.FromJaggedArray(data);
		while (!data.All(c => c == '.'))
		{
			var start = data.FirstIndexOf(c => c != '.');
			var c = data.Get(start);

			HashSet<Point> zone = [start];
			List<Point> cur = [start];
			data.Set(start, '.');

			while (cur.Count != 0)
			{
				cur = cur.SelectMany(p => p.GetOrthogonalNeighbours(bounds))
					.Distinct()
					.Where(n => data.Check(n, c))
					.Where(zone.Add)
					.ToList();
				cur.ForEach(p => data.Set(p, '.'));
			}

			yield return zone.ToArray();
		}
	}

	private long GetZoneFencePrice(Point[] zone)
	{
		var area = zone.Length;
		var perimeter = zone.Sum(p => p.GetOrthogonalNeighbours().Count(point => !zone.Contains(point)));
		return perimeter * area;
	}

	private long GetZoneFencePriceWithDiscount(Point[] zone)
	{
		return zone.Length * GetPerimeterStrokes(zone);
	}


	private long GetPerimeterStrokes(Point[] zone)
	{
		var corners = 0;
		foreach (var point in zone)
		{
			// surrounded! no corners!
			if(point.GetAllNeighbours().Count(zone.Contains) == 8)
				continue;
			
			// check outer corners!
			if (!point.GetLeft().In(zone))
			{
				if (!point.GetUp().In(zone))
					corners++;
				if (!point.GetDown().In(zone))
					corners++;
			}
			if (!point.GetRight().In(zone))
			{
				if (!point.GetUp().In(zone))
					corners++;
				if (!point.GetDown().In(zone))
					corners++;
			}
			
			// check inner corners!
			if (point.GetLeft().In(zone) && point.GetUp().In(zone) && !point.GetUpperLeft().In(zone))
				corners++;
			if (point.GetLeft().In(zone) && point.GetDown().In(zone) && !point.GetLowerLeft().In(zone))
				corners++;
			if (point.GetRight().In(zone) && point.GetUp().In(zone) && !point.GetUpperRight().In(zone))
				corners++;
			if (point.GetRight().In(zone) && point.GetDown().In(zone) && !point.GetLowerRight().In(zone))
				corners++;
		}

		return corners;
	}
}