using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24.Day_02;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName, LogHandle logger)
		=> GetReports(fileName)
			.Count(ValidateReportSafety)
			.Log(logger, count => $"Number of safe reports: {count}");

	protected override void SolveTwo(string fileName, LogHandle logger)
		=> GetReports(fileName)
			.Count(ValidateReportDampenedSafety)
			.Log(logger, count => $"Number of dampened safe reports: {count}");

	private int[][] GetReports(string fileName)
	{
		return InputReader.ReadAllLines(GetDay(), fileName)
			.Select(line => line.Split(" ").Select(int.Parse).ToArray())
			.ToArray();
	}

	private bool ValidateReportSafety(int[] report)
	{
		int upCounter = 0, downCounter = 0;
		for (var i = 1; i < report.Length; i++)
		{
			var diff = report[i] - report[i - 1];
			if (Math.Abs(diff) > 3 || diff == 0) return false;

			switch (diff)
			{
				case > 0:
					upCounter++;
					break;
				case < 0:
					downCounter++;
					break;
			}
		}

		return upCounter == 0 || downCounter == 0;
	}

	private bool ValidateReportDampenedSafety(int[] report)
	{
		int up = 0, down = 0;
		for (var i = 1; i < report.Length; i++)
		{
			var diff = report[i] - report[i - 1];
			if (diff > 0) up++;
			if (diff < 0) down++;
		}

		var isAscending = up > down;
		var nodeData = report.Select((e, i) =>
			new NodeData(i, i == 0 ? null : report[i] - report[i - 1], i == report.Length - 1 ? null : report[i + 1] - report[i])
		).ToArray();

		var faultyNodes = nodeData
			.Where(nd => (isAscending && (nd.DiffTo <= 0 || nd.DiffFrom <= 0))
			             || (!isAscending && (nd.DiffTo >= 0 || nd.DiffFrom >= 0))
			             || (nd.DiffTo.HasValue && Math.Abs(nd.DiffTo.Value) is < 1 or > 3)
			             || (nd.DiffFrom.HasValue && Math.Abs(nd.DiffFrom.Value) is < 1 or > 3))
			.DistinctBy(nd => nd.Index)
			.OrderBy(nd => nd.Index)
			.ToList();

		// if there is 1 faulty node, we remove it
		// if there are 3 faulty nodes, we remove the center
		// if there are 2 faulty nodes, we try each one
		return faultyNodes.Count switch
		{
			0 => true,
			1 => ValidateReportSafety(report.WithoutIndex(faultyNodes[0].Index)),
			2 => ValidateReportSafety(report.WithoutIndex(faultyNodes[0].Index)) || ValidateReportSafety(report.WithoutIndex(faultyNodes[1].Index)),
			3 => ValidateReportSafety(report.WithoutIndex(faultyNodes[1].Index)),
			_ => false
		};
	}

	public record NodeData(int Index, int? DiffTo, int? DiffFrom);
}