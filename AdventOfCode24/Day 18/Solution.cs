using AdventOfCode24.Enums;
using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;
using AdventOfCode24.Models;

namespace AdventOfCode24.Day_18;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var (bytes, bounds) = GetInput(fileName);
		var dataSize = IsExample(fileName)
			? 12
			: 1024;

		var corruptedSpaces = bytes.Take(dataSize).ToArray();
		DijkstraSolve(bounds, corruptedSpaces, Point.Zero, new Point(bounds.Right, bounds.Bottom))
			.Last()
			.Cost
			.Log(Logger, steps => $"The required amount of steps on the shortest path to the exit after {dataSize} bytes have fallen is: {steps}");
	}

	protected override void SolveTwo(string fileName)
	{
		var (bytes, bounds) = GetInput(fileName);

		var mazeBlocker = new MazeBlocker(bounds);
		foreach (var corruptedByte in bytes)
		{
			if (!mazeBlocker.AddCorruptedSpace(corruptedByte)) 
				continue;
			
			Logger($"The path is blocked off, when the byte falls on position {corruptedByte.X},{corruptedByte.Y}");
			return;
		}
	}

	private (Point[] bytes, Bounds bounds) GetInput(string filename)
	{
		Bounds bounds = IsExample(filename)
			? new(7)
			: new(71);

		var bytes = InputReader.ReadAllLines(GetDay(), filename)
			.Select(coord => coord.Split(','))
			.Select(split => new Point(split[0].ToLong(), split[1].ToLong()))
			.ToArray();

		return (bytes, bounds);
	}

	private List<DijkstraPoint> DijkstraSolve(Bounds bounds, Point[] walls, Point start, Point end)
	{
		// A is the list of points of which we found the shortest path
		List<DijkstraPoint> determinedPoints = [new(start, 0)];
		// X is the list of points, of which we have found a path, not necessarily the shortest.
		var activePoints = GetNeighboursWithCost(determinedPoints[0], bounds, walls).ToList();

		while (activePoints.Count > 0)
		{
			var cur = activePoints.MinBy(x => x.Cost) ?? throw new();
			// Remove active point from list of points
			activePoints.Remove(cur);

			// Any other path to this point pass the other known points and are more costly by definition
			// This means that we have found the cheapest way to reach this point!
			determinedPoints.Add(cur);

			if (cur.Position == end)
				return determinedPoints;

			// Find neighbours for current point, and compare them to A
			foreach (var n in GetNeighboursWithCost(cur, bounds, walls).Where(n => determinedPoints.None(dp => dp.Position == n.Position)))
			{
				var match = activePoints.FirstOrDefault(dp => dp.Position == n.Position);
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

	private IEnumerable<DijkstraPoint> GetNeighboursWithCost(DijkstraPoint cur, Bounds bounds, Point[] walls)
		=> cur.Position.GetOrthogonalNeighbours(bounds)
			.WhereNot(walls.Contains)
			.Select(p => new DijkstraPoint(p, cur.Cost + 1))
			.ToArray();
	private record DijkstraPoint(Point Position, long Cost);


	// this class assumes the start to be at top left, and end to be at bottom right
	private class MazeBlocker(Bounds bounds)
	{
		private Bounds Bounds { get; } = bounds;
		private List<Block> Blocks { get; } = [];

		public bool AddCorruptedSpace(Point space)
		{
			var isBottomLeft = space.X == Bounds.Left || space.Y == Bounds.Bottom;
			var isTopRight = space.X == Bounds.Right || space.Y == Bounds.Top;

			var neighbourBlocks = Blocks.Where(b => b.Points.Any(p => p.IsNeighbour(space))).ToList();


			if (neighbourBlocks.Any())
			{
				isBottomLeft |= neighbourBlocks.Any(b => b.IsBottomLeftEdge);
				isTopRight |= neighbourBlocks.Any(b => b.IsTopRightEdge);
				neighbourBlocks.ForEach(b => Blocks.Remove(b));
				var newCluster = neighbourBlocks.SelectMany(b => b.Points).Append(space).ToArray();
				Blocks.Add(new(newCluster, isBottomLeft, isTopRight));
			}
			else Blocks.Add(new([space], isBottomLeft, isTopRight));

			return Blocks.Any(b => b is { IsBottomLeftEdge: true, IsTopRightEdge: true });
		}

		private record Block(Point[] Points, bool IsBottomLeftEdge, bool IsTopRightEdge);
	}
}