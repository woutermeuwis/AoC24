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

		var newValue = (int)direction + degrees;
		while (newValue < 0)
			newValue += 360;
		return (Direction)(newValue % 360);
	}
}