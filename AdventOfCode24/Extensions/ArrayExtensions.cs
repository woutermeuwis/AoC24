namespace AdventOfCode24.Extensions;

public static class ArrayExtensions
{
	public static T[] WithoutIndex<T>(this T[] array, int index) => array.Where((_, i) => i != index).ToArray();
	
	public static bool IsInBounds<T>(this T[] array, int index) => index >= 0 && index < array.Length;
}