using AdventOfCode24.Extensions;
using AdventOfCode24.Helpers;

namespace AdventOfCode24.Day_17;

public class Solution : BaseSolution
{
	protected override void SolveOne(string fileName)
	{
		var (rom, a, b, c) = GetComputer(fileName);
		var computer = new Computer();
		computer.LoadProgram(rom);
		computer.ResetWithRegisters(a, b, c);
		computer.RunToCompletion();
		Logger(computer.Print());
	}

	protected override void SolveTwo(string fileName)
	{
		var (rom, a, b, c) = GetComputer(fileName);
		var singleRun = rom[..^2];

		var computer = new Computer();
		computer.LoadProgram(rom);

		if (fileName.Contains("example"))
		{
			List<long> values = [0, 1, 2, 3, 4, 5, 6, 7];
			for (var o = 0; o < rom.Length; o++)
			{
				var test = string.Join(',', rom.Take(o + 1));
				var newValues = new List<long>();
				for (var i = 0; i < 8; i++)
				{
					foreach (var value in values)
					{
						var cur = value + (long)Math.Pow(8, o + 1) * i;
						computer.ResetWithRegisters(cur, b, c);
						computer.RunToCompletion();
						if ((test.EndsWith('0') ? test : test + ",0") == computer.Print())
							newValues.Add(cur);
					}
				}

				values = newValues;
			}

			Logger($"The value of register A is supposed to be: {values.Min()}");
		}
		else
		{
			var max = 1024;
			List<long> values = Enumerable.Range(0, max).Select(i => (long)i).ToList();
			List<long> foundValues = [];
			for (var o = 0; o < rom.Length; o++)
			{
				var test = rom.Take(o + 1).ToArray();
				var newValues = new List<long>();
				for (long i = 0; i < 8; i++)
				{
					foreach (var value in values)
					{
						var cur = value + (long)Math.Pow(8, o) * i * max;
						computer.ResetWithRegisters(cur, b, c);
						computer.RunToCompletion();
						var print = computer.StdOut.ToArray();

						if (string.Join(",", print) == string.Join(",", rom))
							foundValues.Add(cur);
						else if (print.Length >= o + 1 && Enumerable.Range(0, o + 1).All(x => print[x] == test[x]))
							newValues.Add(cur);
					}
				}
				values = newValues;
			}

			Logger($"The value of register A is supposed to be: {foundValues.Min()}");
		}
	}

	private (long[] Program, long A, long B, long C) GetComputer(string fileName)
	{
		var lines = InputReader.ReadAllLines(GetDay(), fileName).Select(l => l.Split(' ').Last()).ToArray();
		var a = lines[0].ToLong();
		var b = lines[1].ToLong();
		var c = lines[2].ToLong();
		var rom = lines[4].Split(',').Select(instr => instr.ToLong()).ToArray();
		return (rom, a, b, c);
	}

	private class Computer
	{
		// ROM
		private long[] Program { get; set; } = [];

		// Registers
		private long A { get; set; }
		private long B { get; set; }
		private long C { get; set; }

		// Instruction Pointer
		private long CNT { get; set; }

		public List<long> StdOut { get; } = [];
		public bool IsHalted => CNT >= Program.Length;

		public void LoadProgram(long[] program)
			=> Program = program;

		public void ResetWithRegisters(long a, long b, long c)
		{
			StdOut.Clear();
			CNT = 0;
			A = a;
			B = b;
			C = c;
		}

		public void RunToCompletion()
		{
			while (!IsHalted)
				Step();
		}

		public void Step()
		{
			Execute(Program[CNT], Program[CNT + 1]);
			CNT += 2;
		}

		public string Print()
			=> string.Join(',', StdOut);

		private void Execute(long instruction, long operand)
		{
			switch (instruction)
			{
				case 0:
					Adv(operand);
					break;
				case 1:
					Bxl(operand);
					break;
				case 2:
					Bst(operand);
					break;
				case 3:
					Jnz(operand);
					break;
				case 4:
					Bxc();
					break;
				case 5:
					Out(operand);
					break;
				case 6:
					Bdv(operand);
					break;
				case 7:
					Cdv(operand);
					break;
			}
		}

		private long GetComboOperand(long op)
			=> op switch
			{
				<= 3 => op,
				4 => A,
				5 => B,
				6 => C,
				_ => throw new()
			};

		private void Output(long value)
		{
			StdOut.Add(value);
		}

		#region Instructions

		// Instr 0
		private void Adv(long op)
			=> A = (long)Math.Truncate(A / Math.Pow(2, GetComboOperand(op)));

		// Instr 1
		private void Bxl(long operand)
			=> B ^= operand;

		// Instr 2
		private void Bst(long op)
			=> B = GetComboOperand(op) % 8;

		// Instr 3
		private void Jnz(long op)
		{
			if (A == 0) return;
			// -2 offsets the CNT inc!
			CNT = op - 2;
		}

		// Instr 4
		private void Bxc()
			=> B ^= C;

		// Instr 5
		private void Out(long op)
			=> Output(GetComboOperand(op) % 8);

		// Instr 6
		private void Bdv(long op)
			=> B = (long)Math.Truncate(A / Math.Pow(2, GetComboOperand(op)));

		// Instr 7
		private void Cdv(long op)
			=> C = (long)Math.Truncate(A / Math.Pow(2, GetComboOperand(op)));

		#endregion
	}
}