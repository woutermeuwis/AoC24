using AdventOfCode24.Enums;

namespace AdventOfCode24.Extensions;

public static class DirectionExtensions
{
	/// <summary>
	/// Turn clockwise to the next direction value.
	/// Currently supports only multiples of 45 degrees
	/// </summary>
	/// <param name="direction">The current direction</param>
	/// <param name="degrees">The amount of degrees to turn clockwise</param>
	/// <returns></returns>
	public static Direction TurnClockwise(this Direction direction, int degrees)
	{
		if (degrees % 45 != 0)
			throw new ArgumentException(nameof(degrees));

		return (Direction)NormalizeDegrees((int)direction + degrees);
	}

	public static int GetPositiveDifferenceTo(this Direction dir1, Direction dir2)
		=> NormalizeDegrees(dir1 - dir2);

	public static int GetSmallesDifferenceTo(this Direction dir1, Direction dir2)
		=> NormalizeDegrees(dir1 - dir2) % 180;

	private static int NormalizeDegrees(int degrees)
	{
		while (degrees < 0) degrees += 360;
		return degrees % 360;
	}
}