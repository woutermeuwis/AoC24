using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24;

public abstract class BaseSolution
{
	public void SolveExampleOne()
	{
		using LogHandle logHandle = new($"Day {GetDay()}: Example 1");
		SolveOne("example.txt", logHandle);
		logHandle.Log("DONE");
	}

	public void SolveExampleTwo()
	{
		using LogHandle logHandle = new($"Day {GetDay()}: Example 2");
		SolveTwo("example.txt", logHandle);
		logHandle.Log("DONE");
	}

	public void SolvePartOne()
	{
		using LogHandle logHandle = new($"Day {GetDay()}: Part 1");
		SolveOne("input.txt", logHandle);
		logHandle.Log("DONE");
	}

	public void SolvePartTwo()
	{
		using LogHandle logHandle = new($"Day {GetDay()}: Part 2");
		SolveTwo("input.txt", logHandle);
		logHandle.Log("DONE");
	}

	protected int GetDay()
		=> GetType().Namespace?.Split('.')[^1].Split("_")[^1].ToInt() ?? throw new InvalidOperationException();

	protected abstract void SolveOne(string fileName, LogHandle logger);
	protected abstract void SolveTwo(string fileName, LogHandle logger);
}