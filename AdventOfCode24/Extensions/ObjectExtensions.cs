namespace AdventOfCode24.Extensions;

public static class ObjectExtensions
{
	public static bool In<T>(this T value, IEnumerable<T> enumerable)
		=> enumerable.Contains(value);

	public static bool NotIn<T>(this T value, IEnumerable<T> enumerable)
		=> !enumerable.Contains(value);
}