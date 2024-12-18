using AdventOfCode24.Extensions;

namespace AdventOfCode24.Models;

public record Bounds(long X, long Y, long Width, long Height)
{
	public long Left => Width > 0
		? X
		: X + Width - 1;

	public long Right => Width > 0
		? X + Width - 1
		: X;

	public long Top => Height > 0
		? Y
		: Y + Height - 1;

	public long Bottom => Height > 0
		? Y + Height - 1
		: Y;

	public static Bounds FromJaggedArray<T>(T[][] array)
		=> new(0, 0, array[0].Length, array.Length);

	public T[][] ToJaggedArray<T>(T value)
	{
		var arr = new T[Height][];
		for (var y = 0; y < Height; y++)
		{
			arr[y] = new T[Width];
			for (var x = 0; x < Width; x++)
				arr.Set(x, y, value);
		}

		return arr;
	}


	public Bounds(long width, long height) : this(0, 0, width, height)
	{
	}

	public Bounds(long side) : this(side, side)
	{
	}
}