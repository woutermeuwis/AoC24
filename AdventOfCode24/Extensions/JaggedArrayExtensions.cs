using AdventOfCode24.Models;

namespace AdventOfCode24.Extensions;

public static class JaggedArrayExtensions
{
	public static bool IsInBounds<T>(this T[][] arr, long x, long y)
		=> x >= 0 && y >= 0 && y < arr.Length && x < arr[y].Length;

	public static bool IsInBounds<T>(this T[][] arr, Point p)
		=> IsInBounds(arr, p.X, p.Y);

	public static T Get<T>(this T[][] arr, long x, long y)
		=> arr[y][x];

	public static T Get<T>(this T[][] arr, Point p)
		=> arr.Get(p.X, p.Y);

	public static void Set<T>(this T[][] arr, long x, long y, T value)
		=> arr[y][x] = value;

	public static void Set<T>(this T[][] arr, Point p, T value)
		=> arr.Set(p.X, p.Y, value);

	public static bool Check<T>(this T[][] arr, long x, long y, T value)
		=> arr.IsInBounds(x, y)
		   && EqualityComparer<T>.Default.Equals(Get(arr, x, y), value);

	public static bool Check<T>(this T[][] arr, Point p, T value)
		=> arr.IsInBounds(p)
		   && EqualityComparer<T>.Default.Equals(Get(arr, p), value);

	public static void ForEach<T>(this T[][] arr, Action<Point, T> action)
	{
		for (var y = 0; y < arr.Length; y++)
		for (var x = 0; x < arr[y].Length; x++)
			action(new(x, y), arr[y][x]);
	}

	public static void Print<T>(this T[][] arr)
	{
		foreach (var line in arr)
		{
			foreach (var element in line)
				Console.Write(element?.ToString());
			Console.WriteLine();
		}
	}

	public static T[][] Copy<T>(this T[][] arr)
		=> arr.Select(row => row.ToArray()).ToArray();

	public static IEnumerable<Point> GetAllMatches<T>(this T[][] arr, T checkValue)
	{
		for (var y = 0; y < arr.Length; y++)
		{
			for (var x = 0; x < arr[y].Length; x++)
			{
				if (arr.Check(x, y, checkValue))
					yield return new(x, y);
			}
		}
	}

	public static bool All<T>(this T[][] arr, Func<T, bool> predicate)
		=> arr.All(line => line.All(predicate));

	public static Point FirstIndexOf<T>(this T[][] arr, T checkValue)
	{
		for (var y = 0; y < arr.Length; y++)
		{
			for (var x = 0; x < arr[y].Length; x++)
			{
				if (arr.Check(x, y, checkValue))
					return new(x, y);
			}
		}

		throw new("Element not found");
	}

	public static Point FirstIndexOf<T>(this T[][] arr, Func<T, bool> predicate)
	{
		for (var y = 0; y < arr.Length; y++)
		{
			for (var x = 0; x < arr[y].Length; x++)
			{
				if (predicate(arr.Get(x, y)))
					return new(x, y);
			}
		}
		throw new("No matches found in array");
	}
}