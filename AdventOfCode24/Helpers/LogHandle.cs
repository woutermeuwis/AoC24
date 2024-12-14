using System.Diagnostics;

namespace AdventOfCode24.Helpers;

public class LogHandle : IDisposable
{
	private readonly long _start;

	public LogHandle()
	{
		_start = Stopwatch.GetTimestamp();
	}
	
	public LogHandle(string initLog)
	{
		_start = Stopwatch.GetTimestamp();
		if(initLog is {Length:>0})
			Log(initLog);
	}

	public void Log(string message)
	{
		Console.WriteLine($"[{GetTimestamp()}] {message}");
	}

	public void Dispose()
	{
		Console.WriteLine("------------------------------------------------------");
		Console.WriteLine();
	}

	private string GetTimestamp()
	{
		var elapsed = Stopwatch.GetElapsedTime(_start, Stopwatch.GetTimestamp());
		return elapsed.TotalMilliseconds switch
		{
			< 1000 => $"{elapsed.Milliseconds}ms",
			< 60000 => $"{elapsed.Seconds}s {elapsed.Milliseconds}ms",
			_ => $"{elapsed.Minutes}m {elapsed.Seconds}s)"
		};
	}
}