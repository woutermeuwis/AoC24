namespace AdventOfCode24.Helpers;

public static class InputReader
{
	public static string[] ReadAllLines(int day, string file)
		=> File.ReadAllLines(GetFile(day, file));

	public static string ReadAllText(int day, string file)
		=> File.ReadAllText(GetFile(day, file));

	private static string GetFile(int day, string file)
		=> $"./../../../Day {day:D2}/{file}";
}