using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24;

public abstract class BaseSolution
{
	protected Action<string> Logger;

	public void SolveExampleOne()
	{
		using LogHandle logHandle = new($"Day {GetDay()}: Example 1");
		Logger = logHandle.Log;
		SolveOne("example.txt");
		logHandle.Log("DONE");
	}

	public void SolveExampleTwo()
	{
		using LogHandle logHandle = new($"Day {GetDay()}: Example 2");
		Logger = logHandle.Log;
		SolveTwo("example.txt");
		logHandle.Log("DONE");
	}

	public void SolvePartOne()
	{
		using LogHandle logHandle = new($"Day {GetDay()}: Part 1");
		Logger = logHandle.Log;
		SolveOne("input.txt");
		logHandle.Log("DONE");
	}

	public void SolvePartTwo()
	{
		using LogHandle logHandle = new($"Day {GetDay()}: Part 2");
		Logger = logHandle.Log;
		SolveTwo("input.txt");
		logHandle.Log("DONE");
	}

	protected int GetDay()
		=> GetType().Namespace?.Split('.')[^1].Split("_")[^1].ToInt() ?? throw new InvalidOperationException();

	protected abstract void SolveOne(string fileName);
	protected abstract void SolveTwo(string fileName);
}