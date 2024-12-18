using System.Runtime.Intrinsics.Arm;
using AdventOfCode24.Enums;
using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;
using AdventOfCode24.Models;

namespace AdventOfCode24.Day_16;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var maze = GetMaze(fileName);
		DijkstraSolve(maze)
			.Where(dp => dp.Position == maze.End)
			.Min(dp => dp.Cost)
			.Log(Logger, cost => $"The cost for the cheapest path is: {cost}");
	}

	protected override void SolveTwo(string fileName)
	{
		var maze = GetMaze(fileName);
		var solution = DijkstraSolve(maze);
		CountAllPointsOnShortestPath(solution, maze)
			.Log(Logger, tileCount => $"The amount of tiles on any of the shortest paths is: {tileCount}");
		// 249 low
		// 511 high
	}

	private Maze GetMaze(string fileName)
	{
		var map = InputReader.ReadAllLines(GetDay(), fileName)
			.Select(line => line.ToCharArray())
			.ToArray();

		List<Point> walls = [];
		Point start = Point.Zero, end = Point.Zero;
		map.ForEach((p, c) =>
		{
			if (c == '#')
				walls.Add(p);
			if (c == 'S')
				start = p;
			if (c == 'E')
				end = p;
		});
		return new(Bounds.FromJaggedArray(map), walls.ToArray(), start, end);
	}

	private List<DijkstraPoint> DijkstraSolve(Maze maze)
	{
		// A is the list of points of which we found the shortest path
		List<DijkstraPoint> determinedPoints = [new(maze.Start, Direction.Right, 0)];
		// X is the list of points, of which we have found a path, not necessarily the shortest.
		var activePoints = GetNeighboursWithCost(determinedPoints[0], maze).ToList();

		while (activePoints.Count > 0)
		{
			var cur = activePoints.MinBy(x => x.Cost) ?? throw new();
			activePoints.Remove(cur);
			determinedPoints.Add(cur);
			if (cur.Position == maze.End)
				return determinedPoints;

			var neighbours = GetNeighboursWithCost(cur, maze).ToArray();
			// Find neighbours for current point, and compare them to A
			foreach (var n in neighbours)
			{
				// validate against visited!
				// if visited exactly --> ignore
				if (determinedPoints.Any(dp => dp.Position == n.Position))
					continue;

				// cannot have an active path from other direction!
				if (activePoints.Any(dp => dp.Position == n.Position && dp.Direction == n.Direction.TurnClockwise(180)))
					continue;

				var match = activePoints.FirstOrDefault(dp => dp.Matches(n));
				if (match is not null)
				{
					if (match.Cost <= n.Cost)
						continue;

					activePoints.Remove(match);
				}

				activePoints.Add(n);
			}
		}

		return determinedPoints;
	}

	private DijkstraPoint? GetNeighboursForReconstruction(List<DijkstraPoint> remaining, List<DijkstraPoint> visited, DijkstraPoint cur, Direction direction)
	{
		var neighbourPos = cur.Position.GetNeighbour(direction);
		var neighbour = remaining.FirstOrDefault(dp => dp.Position == neighbourPos);
		if (neighbour is null)
			return null;

		if (cur.Direction == direction.TurnClockwise(180))
			return neighbour;

		if (neighbour.Direction == direction.TurnClockwise(180))
		{
			var otherNeighbourPos = cur.Position.GetNeighbour(direction.TurnClockwise(180));
			var otherNeighbour = visited.FirstOrDefault(dp => dp.Position == otherNeighbourPos);
			if (otherNeighbour is not null)
				return neighbour;
		}

		return null;
	}

	private long CountAllPointsOnShortestPath(List<DijkstraPoint> determinedPoints, Maze maze)
	{
		var start = determinedPoints.FirstOrDefault(dp => dp.Position == maze.End) ?? throw new();
		var remaining = determinedPoints.ToList();
		remaining.Remove(start);
		List<DijkstraPoint> active = [start];
		List<DijkstraPoint> visited = [];

		while (active.Count > 0)
		{
			var cur = active[0];
			active.Remove(cur);
			visited.Add(cur);

			var left = GetNeighboursForReconstruction(remaining, visited, cur, Direction.Left);
			if (left is not null)
			{
				remaining.Remove(left);
				active.Add(left);
			}

			var right = GetNeighboursForReconstruction(remaining, visited, cur, Direction.Right);
			if (right is not null)
			{
				remaining.Remove(right);
				active.Add(right);
			}

			var upper = GetNeighboursForReconstruction(remaining, visited, cur, Direction.Up);
			if (upper is not null)
			{
				remaining.Remove(upper);
				active.Add(upper);
			}

			var lower = GetNeighboursForReconstruction(remaining, visited, cur, Direction.Down);
			if (lower is not null)
			{
				remaining.Remove(lower);
				active.Add(lower);
			}
		}

		// Print
		var map = maze.Bounds.ToJaggedArray('.');
		maze.Walls.ForEach(p => map.Set(p, '#'));
		visited.ForEach(v => map.Set(v.Position, 'O'));
		map.Print();

		return visited.DistinctBy(v => v.Position).Count();
	}

	private IEnumerable<DijkstraPoint> GetNeighboursWithCost(DijkstraPoint cur, Maze maze)
	{
		var neighbourSpaces = cur.Position.GetOrthogonalNeighbours(maze.Bounds)
			.WhereNot(maze.Walls.Contains)
			.ToArray();

		var dataPoints = neighbourSpaces.Select(cur.GetDataForPoint).ToArray();
		var validPoints = dataPoints.WhereNot(dp => Math.Abs(dp.Direction - cur.Direction) == 180).ToArray();
		return validPoints;
	}

	private DijkstraPoint[] GetReverseNeighbours(Point cur, Maze maze)
	{
		List<DijkstraPoint> neighours = [];

		var left = cur.GetLeft();
		if (left.IsInBounds(maze.Bounds) && !left.In(maze.Walls))
			neighours.Add(new(left, Direction.Right, -1));

		var right = cur.GetRight();
		if (right.IsInBounds(maze.Bounds) && !right.In(maze.Walls))
			neighours.Add(new(right, Direction.Left, -1));

		var top = cur.GetUp();
		if (top.IsInBounds(maze.Bounds) && !top.In(maze.Walls))
			neighours.Add(new(top, Direction.Down, -1));

		var bottom = cur.GetDown();
		if (bottom.IsInBounds(maze.Bounds) && !bottom.In(maze.Walls))
			neighours.Add(new(bottom, Direction.Up, -1));

		return neighours.ToArray();
	}

	private record DijkstraPoint(Point Position, Direction Direction, long Cost)
	{
		public bool Matches(DijkstraPoint dp)
		{
			return dp.Position == Position && dp.Direction == Direction;
		}

		public DijkstraPoint GetDataForPoint(Point point)
		{
			if (point.X != Position.X && point.Y != Position.Y)
				throw new();
			if (!point.IsNeighbour(Position))
				throw new();

			var dir = point.DirectionComingFrom(Position);
			var cost = dir == Direction ? Cost + 1 : Cost + 1001;
			return new(point, dir, cost);
		}
	}

	private record Maze(Bounds Bounds, Point[] Walls, Point Start, Point End);
}