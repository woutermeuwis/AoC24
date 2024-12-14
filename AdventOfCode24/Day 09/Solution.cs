using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24.Day_09;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		ByteDefragment(GetInput(fileName))
			.Select((v, i) => v * i)
			.Sum()
			.Log(Logger, checksum => $"Defragmented memory checksum: {checksum}");
	}

	protected override void SolveTwo(string fileName)
	{
		FileDefragment(GetInput(fileName).ToList())
			.Select((v, i) => v * i)
			.Sum()
			.Log(Logger, checksum => $"Defragmented memory checksum: {checksum}");
	}

	private FileDescriptor[] GetInput(string fileName)
		=> InputReader.ReadAllText(GetDay(), fileName)
			.ToCharArray()
			.Select(c => c - '0')
			.Select((c, i) => new FileDescriptor(i % 2 == 0 ? i / 2 : -1, c, i % 2 == 1))
			.ToArray();

	private List<long> ByteDefragment(FileDescriptor[] diskMap)
	{
		var freeSpace = diskMap.Where(fd => fd.IsFreeSpace).ToArray();
		var files = diskMap.Where(fd => !fd.IsFreeSpace).ToArray();

		var fileSize = files.Sum(fd => fd.Length);

		var disk = new List<long>(fileSize);
		var patcher = files.Last();
		var patchIndex = 0;

		for (var i = 0; i < files.Length; i++)
		{
			var file = files[i];

			// if finishing up... add remaining entries of current patcher
			if (file.Id == patcher.Id)
			{
				if (patchIndex < patcher.Length)
					for (var j = 0; j < patcher.Length - patchIndex; j++)
						disk.Add(patcher.Id);
				return disk;
			}

			// add current file
			for (var j = 0; j < file.Length; j++)
				disk.Add(file.Id);

			// fill up whitespace with patches
			var emptySpace = freeSpace[i].Length;
			while (emptySpace > 0)
			{
				if (patchIndex == patcher.Length)
				{
					// no more patches...
					if (patcher.Id == file.Id + 1)
						return disk;

					patcher = files.First(fd => fd.Id == patcher.Id - 1);
					patchIndex = 0;
				}

				disk.Add(patcher.Id);
				patchIndex++;
				emptySpace--;
			}
		}

		return disk;
	}

	private IEnumerable<long> FileDefragment(List<FileDescriptor> diskMap)
	{
		// makes iterating easier!
		diskMap.Add(new(-1, 1, true));
		var id = diskMap.Max(fd => fd.Id);

		while (id > 0)
		{
			// get file and index
			var fileToMove = diskMap.Single(fd => fd.Id == id);
			var indexToMove = diskMap.IndexOf(fileToMove);

			// find a space where it might fit
			for (var i = 0; i < indexToMove; i++)
			{
				// if not free, or too small, skip...
				if (!diskMap[i].IsFreeSpace || diskMap[i].Length < fileToMove.Length)
					continue;

				if (diskMap[indexToMove - 1].IsFreeSpace && diskMap[indexToMove + 1].IsFreeSpace)
				{
					var newFreeSpace = diskMap[indexToMove - 1].Length + fileToMove.Length + diskMap[indexToMove + 1].Length;
					diskMap.RemoveRange(indexToMove - 1, 3);
					diskMap.Insert(indexToMove - 1, new(-1, newFreeSpace, true));
				}
				else if (diskMap[indexToMove - 1].IsFreeSpace)
				{
					var newFreeSpace = diskMap[indexToMove - 1].Length + fileToMove.Length;
					diskMap.RemoveRange(indexToMove - 1, 2);
					diskMap.Insert(indexToMove - 1, new(-1, newFreeSpace, true));
				}
				else if (diskMap[indexToMove + 1].IsFreeSpace)
				{
					var newFreeSpace = diskMap[indexToMove + 1].Length + fileToMove.Length;
					diskMap.RemoveRange(indexToMove, 2);
					diskMap.Insert(indexToMove, new(-1, newFreeSpace, true));
				}
				else
				{
					diskMap.RemoveAt(indexToMove);
					diskMap.Insert(indexToMove, new(-1, fileToMove.Length, true));
				}


				var freeSpace = diskMap[i];
				diskMap.Remove(freeSpace);
				if (freeSpace.Length > fileToMove.Length)
					diskMap.Insert(i, freeSpace with { Length = freeSpace.Length - fileToMove.Length });

				diskMap.Insert(i, fileToMove);
				break;
			}

			id--;
		}

		return DumpDisk(diskMap);
	}

	private IEnumerable<long> DumpDisk(List<FileDescriptor> diskMap)
	{
		var lastFileIndex = diskMap.IndexOf(diskMap.Last(fd => !fd.IsFreeSpace));
		var usefulDiskMap = diskMap.Take(lastFileIndex + 1).ToList();

		foreach (var file in usefulDiskMap)
		{
			if (file.IsFreeSpace)
			{
				for (var i = 0; i < file.Length; i++)
					yield return 0;
			}
			else
			{
				for (var i = 0; i < file.Length; i++)
					yield return file.Id;
			}
		}
	}

	private record FileDescriptor(int Id, int Length, bool IsFreeSpace);
}