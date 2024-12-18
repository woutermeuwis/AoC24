using System.Drawing;
using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;
using AdventOfCode24.Models;
using Point = AdventOfCode24.Models.Point;

namespace AdventOfCode24.Day_14;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var bounds = GetBounds(fileName);
		var robotPositions = GetRobots(fileName)
			.Select(r => MoveRobot(r, bounds, 100).Position)
			.GroupBy(p => p)
			.Select(g => (g.Key, g.Count()))
			.ToArray();

		int first = 0, second = 0, third = 0, fourth = 0;
		var centerX = (bounds.Width - 1) / 2;
		var centerY = (bounds.Height - 1) / 2;

		foreach (var (pos, count) in robotPositions)
		{
			if (pos.X < centerX)
			{
				if (pos.Y < centerY)
					first += count;
				if (pos.Y > centerY)
					second += count;
			}

			if (pos.X > centerX)
			{
				if (pos.Y < centerY)
					third += count;
				if (pos.Y > centerY)
					fourth += count;
			}
		}

		Logger($"The safety factor after 100 seconds will be: {first * second * third * fourth}");
	}

	protected override void SolveTwo(string fileName)
	{
		var robots = GetRobots(fileName).ToArray();
		var bounds = GetBounds(fileName);

		var i = 0;
		while (true)
		{
			if (robots.Length == robots.DistinctBy(r => r.Position).Count())
			{
				Logger($"The image is shown at second {i}");

				var map = Enumerable.Range(1, (int)bounds.Height)
					.Select(i => Enumerable.Range(1, (int)bounds.Width).Select(j => ' ').ToArray())
					.ToArray();
				robots.ForEach(r => map.Set(r.Position, '#'));
				map.ForEach(line => Logger(new(line)));

				return;
			}

			robots = robots.Select(r => MoveRobot(r, bounds, 1)).ToArray();
			i++;
		}
	}

	private IEnumerable<Robot> GetRobots(string fileName)
	{
		var lines = InputReader.ReadAllLines(GetDay(), fileName);
		foreach (var line in lines)
		{
			var split = line.Split(' ');
			var pos = split[0][2..].Split(',').Select(stringExtensions.ToLong).ToArray();
			var vel = split[1][2..].Split(',').Select(stringExtensions.ToLong).ToArray();
			yield return new(new(pos[0], pos[1]), new(vel[0], vel[1]));
		}
	}

	private Bounds GetBounds(string fileName)
	{
		if (fileName.Contains("example.txt"))
			return new(0, 0, 11, 7);
		return new(0, 0, 101, 103);
	}

	private Robot MoveRobot(Robot robot, Bounds bounds, int seconds)
	{
		var newPos = robot.Position + seconds * robot.Velocity;

		var x = newPos.X % bounds.Width;
		while (x < 0)
			x += bounds.Width;

		var y = newPos.Y % bounds.Height;
		while (y < 0)
			y += bounds.Height;

		return robot with { Position = new(x, y) };
	}

	private record Robot(Point Position, Point Velocity);
}