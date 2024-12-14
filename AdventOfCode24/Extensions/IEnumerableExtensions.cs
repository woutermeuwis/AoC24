namespace AdventOfCode24.Extensions;

public static class IEnumerableExtensions
{
	public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool condition, Func<T, bool> predicate)
		=> condition
			? enumerable.Where(predicate)
			: enumerable;

	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> predicate)
	{
		foreach (var element in enumerable)
			predicate(element);
	}

	public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
		=> enumerable.Where(x => !predicate(x));
}