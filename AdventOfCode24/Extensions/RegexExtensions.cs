using System.Text.RegularExpressions;

namespace AdventOfCode24.Extensions;

public static class RegexExtensions
{
	public static MatchCollection GetRegexMatches(this string input, string pattern)
		=> Regex.Matches(input, pattern);
	
	public static Match GetRegexMatch(this string input, string pattern)
		=> Regex.Match(input, pattern);
}