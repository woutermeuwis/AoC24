namespace AdventOfCode24.Extensions;

public static class stringExtensions
{
	public static long ToLong(this string str) => long.Parse(str);
	public static int ToInt(this string str) => int.Parse(str);
}