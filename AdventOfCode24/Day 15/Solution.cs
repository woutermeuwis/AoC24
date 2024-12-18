using System.Diagnostics;
using AdventOfCode24.Enums;
using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;
using AdventOfCode24.Models;

namespace AdventOfCode24.Day_15;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var (warehouse, instructions) = Part1.GetInput(fileName);
		instructions.ForEach(i => warehouse = Part1.ExecuteInstruction(warehouse, i));
		warehouse.Obstacles
			.Sum(Part1.GetGPSCoordinate)
			.Log(Logger, sum => $"The sum of all box GPS coordinates after moving is {sum}");
	}

	protected override void SolveTwo(string fileName)
	{
		var (warehouse, instructions) = Part2.GetInput(fileName);
		instructions.ForEach(i => warehouse = Part2.ExecuteInstruction(warehouse, i));
		warehouse.Obstacles
			.OrderBy(o=>o.P1.Y)
			.ThenBy(o=>o.P1.X)
			.Sum(o => Part2.GetGPSCoordinate(o, warehouse.Bounds))
			.Log(Logger, sum => $"The sum of all box GPS coordinates after moving is {sum}");
	}

	private static int FindDay() => new Solution().GetDay();


	private static class Part1
	{
		public static (WareHouse WareHouse, Direction[] Instructions) GetInput(string fileName)
		{
			var lines = InputReader.ReadAllLines(FindDay(), fileName);
			List<char[]> warehouse = [];
			List<char[]> instructions = [];

			var foundEmptyLine = false;
			foreach (var line in lines)
			{
				if (line is { Length: 0 })
				{
					foundEmptyLine = true;
					continue;
				}

				if (foundEmptyLine) instructions.Add(line.ToCharArray());
				else warehouse.Add(line.ToCharArray());
			}

			var instructionSet = instructions
				.SelectMany(line => line)
				.Select(c => c switch
				{
					'^' => Direction.Up,
					'v' => Direction.Down,
					'>' => Direction.Right,
					'<' => Direction.Left,
					_ => throw new()
				}).ToArray();

			var map = warehouse.ToArray();
			List<Point> walls = [], obstacles = [];
			var robot = Point.Zero;

			map.ForEach((pos, c) =>
			{
				if (c == '#')
					walls.Add(pos);
				if (c == 'O')
					obstacles.Add(pos);
				if (c == '@')
					robot = pos;
			});

			return (new(new(warehouse[0].Length, warehouse.Count), walls.ToArray(), obstacles.ToArray(), robot), instructionSet);
		}

		public static WareHouse ExecuteInstruction(WareHouse warehouse, Direction direction)
		{
			var robot = warehouse.Robot;
			var cur = robot;
			bool? canMove = null;

			// determine if robot can move
			while (canMove is null)
			{
				cur = cur.GetNeighbour(direction);
				if (cur.In(warehouse.Obstacles))
					continue;
				canMove = !cur.In(warehouse.Walls);
			}

			if (canMove is false)
				return warehouse;

			// move obstacles and robot!
			var dir = direction.TurnClockwise(180);
			var prev = cur.GetNeighbour(dir);
			var obstacles = warehouse.Obstacles.ToList();
			while (prev.In(obstacles))
			{
				obstacles.Remove(prev);
				obstacles.Add(cur);
				cur = prev;
				prev = cur.GetNeighbour(dir);
			}

			return warehouse with { Robot = cur, Obstacles = obstacles.ToArray() };
		}

		// ReSharper disable once InconsistentNaming
		public static long GetGPSCoordinate(Point point)
			=> 100 * point.Y + point.X;

		public static void PrintWarehouse(WareHouse warehouse)
		{
			var wh = warehouse.Bounds.ToJaggedArray('.');
			warehouse.Walls.ForEach(w => wh.Set(w, '#'));
			warehouse.Obstacles.ForEach(o => wh.Set(o, 'O'));
			wh.Set(warehouse.Robot, '@');
			wh.Print();
		}

		public record WareHouse(Bounds Bounds, Point[] Walls, Point[] Obstacles, Point Robot);
	}

	private static class Part2
	{
		public static (WareHouse Warehouse, Direction[] instructionSet) GetInput(string fileName)
		{
			var lines = InputReader.ReadAllLines(FindDay(), fileName);
			List<string> warehouse = [];
			List<char[]> instructions = [];

			var foundEmptyLine = false;
			foreach (var line in lines)
			{
				if (line is { Length: 0 })
				{
					foundEmptyLine = true;
					continue;
				}

				if (foundEmptyLine) instructions.Add(line.ToCharArray());
				else warehouse.Add(line);
			}

			var instructionSet = instructions
				.SelectMany(line => line)
				.Select(c => c switch
				{
					'^' => Direction.Up,
					'v' => Direction.Down,
					'>' => Direction.Right,
					'<' => Direction.Left,
					_ => throw new()
				}).ToArray();

			var map = warehouse
				.Select(line => line.Replace("#", "##").Replace(".", "..").Replace("O", "[]").Replace("@", "@.").ToCharArray())
				.ToArray();
			List<Point> walls = [];
			List<Obstacle> obstacles = [];
			var robot = Point.Zero;

			map.ForEach((pos, c) =>
			{
				if (c == '#')
					walls.Add(pos);
				if (c == '[')
					obstacles.Add(new(pos, pos.GetNeighbour(Direction.Right)));
				if (c == '@')
					robot = pos;
			});

			return (new(new(2 * warehouse[0].Length, warehouse.Count), walls.ToArray(), obstacles.ToArray(), robot), instructionSet);
		}


		public static void PrintWarehouse(WareHouse warehouse)
		{
			var wh = warehouse.Bounds.ToJaggedArray('.');
			warehouse.Walls.ForEach(w => wh.Set(w, '#'));
			warehouse.Obstacles.ForEach(o =>
			{
				wh.Set(o.P1, '[');
				wh.Set(o.P2, ']');
			});
			wh.Set(warehouse.Robot, '@');
			wh.Print();
		}

		public static WareHouse ExecuteInstruction(WareHouse warehouse, Direction direction)
		{
			var robot = warehouse.Robot;
			var next = robot.GetNeighbour(direction);

			// check for wall
			if (next.In(warehouse.Walls))
				return warehouse;

			// check for empty space
			var obstacle = warehouse.Obstacles.FirstOrDefault(o => o.Contains(next));
			if (obstacle is null)
				return warehouse with { Robot = next };

			var obstacles = warehouse.Obstacles.ToList();
			Stack<Obstacle> workingObstacles = [];
			workingObstacles.Push(obstacle);

			while (workingObstacles.Count > 0)
			{
				var cur = workingObstacles.Pop();
				Point? col1 = null, col2 = null;
				switch (direction)
				{
					case Direction.Left:
						col1 = cur.P1.GetNeighbour(Direction.Left);
						break;
					case Direction.Right:
						col2 = cur.P2.GetNeighbour(Direction.Right);
						break;
					case Direction.Down:
						col1 = cur.P1.GetNeighbour(Direction.Down);
						col2 = cur.P2.GetNeighbour(Direction.Down);
						break;
					case Direction.Up:
						col1 = cur.P1.GetNeighbour(Direction.Up);
						col2 = cur.P2.GetNeighbour(Direction.Up);
						break;
					default: throw new();
				}

				if (col1 is not null)
				{
					// cannot push obstacle into wall!
					if (col1.In(warehouse.Walls))
						return warehouse;

					var obs1 = obstacles.FirstOrDefault(o => o.Contains(col1));
					if (obs1 is not null && !workingObstacles.Contains(obs1))
						workingObstacles.Push(obs1);
				}

				if (col2 is not null)
				{
					// cannot push obstacle into wall!
					if (col2.In(warehouse.Walls))
						return warehouse;

					var obs2 = obstacles.FirstOrDefault(o => o.Contains(col2));
					if (obs2 is not null && !workingObstacles.Contains(obs2))
						workingObstacles.Push(obs2);
				}

				obstacles.Remove(cur);
				var newObstacle = cur.Move(direction);
				obstacles.Add(newObstacle);
			}

			return warehouse with { Robot = warehouse.Robot.GetNeighbour(direction), Obstacles = obstacles.ToArray() };
		}

		// ReSharper disable once InconsistentNaming
		// This method does not follow the rules for the coordinates as presented in part two...
		// However, neither does the correct solution... it expects us to calculate the coordinates of the left point of the obstacle as in part one for some reason...
		public static long GetGPSCoordinate(Obstacle point, Bounds bounds) 
			=> 100 * point.P1.Y + point.P1.X;

		public record Obstacle(Point P1, Point P2)
		{
			public bool Contains(Point p)
				=> p == P1 || p == P2;

			public Obstacle Move(Direction direction)
				=> new(P1.GetNeighbour(direction), P2.GetNeighbour(direction));
		}

		public record WareHouse(Bounds Bounds, Point[] Walls, Obstacle[] Obstacles, Point Robot);
	}
}