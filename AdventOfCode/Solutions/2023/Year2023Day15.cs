using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day15 : Solution
{
	// ReSharper disable once InconsistentNaming
	private static byte HASH(string s)
	{
		ushort val = 0;
		foreach (char c in s)
		{
			val += c;
			val *= 17;
			val %= 256;
		}

		return (byte) val;
	}

	public override string Part1(string input) => input.Split(',').Select(HASH).Sum(b => b).ToString();

	public override string Part2(string input)
	{
		List<(string, int)>[] boxes = new List<(string, int)>[256];
		for (int i = 0; i < 256; i++)
			boxes[i] = new();

		foreach (string command in input.Split(','))
		{
			string label = new string(command.TakeWhile(c => c != '-' && c != '=').ToArray());
			char operation = command[label.Length];
			int power = operation == '=' ? command[^1] - '0' : -1;

			List<(string label, int power)> box = boxes[HASH(label)];

			if (operation == '-')
				box.RemoveAll(t => t.label == label);
			else if (operation == '=')
			{
				int i = box.FirstIndexOf(t => t.label == label);
				if (i == -1)
					box.Add((label, power));
				else
					box[i] = (label, power);
			}
		}

		long answer = 0;

		for (int i = 0; i < 256; i++)
		{
			List<(string label, int power)> box = boxes[i];
			for (int j = 0; j < box.Count; j++)
				answer += (i + 1) * (j + 1) * box[j].power;
		}

		return answer.ToString();
	}
}
