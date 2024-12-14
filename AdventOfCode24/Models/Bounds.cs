namespace AdventOfCode24.Models;

public record Bounds(int X, int Y, int Width, int Height)
{
	public int Left => Width > 0
		? X
		: X + Width;

	public int Right => Width > 0
		? X + Width
		: X;

	public int Top => Height > 0
		? Y
		: Y + Height;

	public int Bottom => Height > 0
		? Y + Height
		: Y;

	public static Bounds FromJaggedArray<T>(T[][] array) 
		=> new(0, 0, array[0].Length - 1, array.Length - 1);

	public Bounds(int width, int height) : this(0, 0, width, height)
	{
	}
}