using AdventOfCode24.Enums;
using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;
using AdventOfCode24.Models;

namespace AdventOfCode24.Day_06;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var map = GetInput(fileName);
		var guardPos = GetGuardPosition(map);
		var direction = Direction.Up;

		Logger("Parsed input data!");

		while (true)
		{
			var next = guardPos.GetNeighbour(direction);
			map.Set(guardPos, 'X');

			if (!map.IsInBounds(next))
				break;

			if (map.Check(next, '#'))
				direction = direction.TurnClockwise(90);
			else
				guardPos = next;
		}


		map.Sum(l => l.Count(c => c == 'X'))
			.Log(Logger, sum => $"Distinct spaces patrolled by guard: {sum}");
	}

	protected override void SolveTwo(string fileName)
	{
		var map = GetInput(fileName);
		var guardStart = GetGuardPosition(map);

		Logger("Parsed input data!");

		map.Select((line, y)
				=> line.Select((_, x) => CheckObstacleLooping(map, new(x, y), guardStart))
					.Count(x => x)
			)
			.Sum()
			.Log(Logger, sum => $"Amount of possible loop-creating obstructions: {sum}");
	}

	private char[][] GetInput(string fileName) =>
		InputReader.ReadAllLines(GetDay(), fileName)
			.Select(l => l.ToCharArray())
			.ToArray();

	private Point GetGuardPosition(char[][] map)
	{
		for (var y = 0; y < map.Length; y++)
		for (var x = 0; x < map[y].Length; x++)
			if (map.Check(x, y, '^'))
				return new(x, y);

		throw new();
	}

	private bool CheckObstacleLooping(char[][] map, Point obstacle, Point guardStart)
	{
		if (obstacle == guardStart)
			return false;

		map = map.Copy();
		map.Set(obstacle, '#');

		var guardLog = new Dictionary<Point, List<Direction>>();
		var direction = Direction.Up;
		var guardPos = guardStart;

		while (true)
		{
			if (!guardLog.ContainsKey(guardPos))
				guardLog.Add(guardPos, []);

			if (guardLog[guardPos].Contains(direction))
				return true;

			guardLog[guardPos].Add(direction);
			var next = guardPos.GetNeighbour(direction);
			if (!map.IsInBounds(next))
				return false;

			if (map.Check(next, '#'))
				direction = direction.TurnClockwise(90);
			else
				guardPos = next;
		}
	}
}