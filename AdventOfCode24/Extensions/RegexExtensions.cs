using System.Text.RegularExpressions;

namespace AdventOfCode24.Extensions;

public static class RegexExtensions
{
	public static MatchCollection GetRegexMatches(this string input, string pattern)
		=> Regex.Matches(input, pattern);
}