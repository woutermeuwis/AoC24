namespace AdventOfCode24.Extensions;

public static class ObjectExtensions
{
	public static bool In<T>(this T value, T[] array)
		=> array.Contains(value);
}