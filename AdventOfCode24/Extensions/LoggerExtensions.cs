using AdventOfCode24.Helpers;

namespace AdventOfCode24.Extensions;

public static class LoggerExtensions
{
	public static void Log(this int i, LogHandle handle)
		=> handle.Log(i.ToString());

	public static void Log(this int i, LogHandle handle, Func<int, string> formatter)
		=> handle.Log(formatter(i));

	public static void Log(this long l, LogHandle handle)
		=> handle.Log(l.ToString());

	public static void Log(this long l, LogHandle handle, Func<long, string> formatter)
		=> handle.Log(formatter(l));

	public static void Log(this string str, LogHandle handle)
		=> handle.Log(str);

	public static void Log(this string str, LogHandle handle, Func<string, string> formatter)
		=> handle.Log(formatter(str));
}