using AdventOfCode24.Helpers;

namespace AdventOfCode24.Extensions;

public static class LoggerExtensions
{
	public static void Log(this int i,  Action<string> logger)
		=> logger(i.ToString());

	public static void Log(this int i,  Action<string> logger, Func<int, string> formatter)
		=> logger(formatter(i));

	public static void Log(this long l,  Action<string> logger)
		=> logger(l.ToString());

	public static void Log(this long l, Action<string> logger, Func<long, string> formatter)
		=> logger(formatter(l));

	public static void Log(this string str,  Action<string> logger)
		=> logger(str);

	public static void Log(this string str,  Action<string> logger, Func<string, string> formatter)
		=> logger(formatter(str));
}