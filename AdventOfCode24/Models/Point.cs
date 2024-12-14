using AdventOfCode24.Enums;

namespace AdventOfCode24.Models;

/// <summary>
/// Represents points in a X/Y coordinate system
/// X goes from left to right (so a positive X is to the right of the origin)
/// Y goes from top to bottom (so a positive Y is below the origin) 
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public record Point(int X, int Y)
{
	public static Point operator +(Point p1, Point p2)
		=> new(p1.X + p2.X, p1.Y + p2.Y);

	public static Point operator -(Point p1, Point p2)
		=> new(p1.X - p2.X, p1.Y - p2.Y);

	public static Point operator *(Point p, int scalar)
		=> new(p.X * scalar, p.Y * scalar);

	public static Point operator *(int scalar, Point p)
		=> new(p.X * scalar, p.Y * scalar);

	public static Point operator /(Point p, int scalar)
		=> new(p.X / scalar, p.Y / scalar);

	public bool IsNeighbour(Point p)
		=> Math.Abs(p.X - X) <= 1 && Math.Abs(p.Y - Y) < 1;

	public decimal Size
		=> (decimal)Math.Sqrt((int)Math.Pow(X, 2) + (int)Math.Pow(Y, 2));

	public Point GetLeft()
		=> this + new Point(-1, 0);

	public Point GetRight()
		=> this + new Point(1, 0);

	public Point GetUp()
		=> this + new Point(0, -1);

	public Point GetDown()
		=> this + new Point(0, 1);

	public Point GetUpperLeft()
		=> this + new Point(-1, -1);

	public Point GetUpperRight()
		=> this + new Point(1, -1);

	public Point GetLowerLeft()
		=> this + new Point(-1, 1);

	public Point GetLowerRight()
		=> this + new Point(1, 1);

	public Point GetNeighbour(Direction direction)
		=> direction switch
		{
			Direction.Up => GetUp(),
			Direction.Down => GetDown(),
			Direction.Left => GetLeft(),
			Direction.Right => GetRight(),
			Direction.UpperLeft => GetUpperLeft(),
			Direction.UpperRight => GetUpperRight(),
			Direction.LowerLeft => GetLowerLeft(),
			Direction.LowerRight => GetLowerRight(),
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};

	public Point[] GetOrthogonalNeighbours(Bounds? bounds = null)
		=> new[] { GetLeft(), GetUp(), GetRight(), GetDown() }
			.Where(p => bounds == null || p.IsInBounds(bounds))
			.ToArray();

	public Point[] GetDiagonalNeighbours(Bounds? bounds = null)
		=> new[] { GetUpperLeft(), GetUpperRight(), GetLowerRight(), GetLowerLeft() }
			.Where(p => bounds == null || p.IsInBounds(bounds))
			.ToArray();


	public Point[] GetAllNeighbours(Bounds? bounds = null)
		=> new[] { GetLeft(), GetUpperLeft(), GetUp(), GetUpperRight(), GetRight(), GetLowerRight(), GetDown(), GetLowerLeft() }
			.Where(p => bounds == null || p.IsInBounds(bounds))
			.ToArray();


	public bool IsInBounds(int x1, int y1, int x2, int y2)
		=> x1 <= X && X <= x2 && y1 <= Y && Y <= y2;

	public bool IsInBounds(Bounds bounds)
		=> IsInBounds(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);


	public override string ToString()
		=> $"({X}, {Y})";
}