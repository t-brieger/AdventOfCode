using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day16 : Solution
{
	private enum TileTypes
	{
		VERTICAL_SPLITTER,
		HORIZONTAL_SPLITTER,
		BACK_MIRROR,
		MIRROR,
		EMPTY
	}

	private static readonly Dictionary<TileTypes, int[][]> BounceRules;

	static Year2023Day16()
	{
		// 0: north, 1: east, 2: south, 3: west

		BounceRules = new();

		BounceRules[TileTypes.VERTICAL_SPLITTER] = new int[4][];
		BounceRules[TileTypes.VERTICAL_SPLITTER][0] = new[] {0};
		BounceRules[TileTypes.VERTICAL_SPLITTER][1] = new[] {0, 2};
		BounceRules[TileTypes.VERTICAL_SPLITTER][2] = new[] {2};
		BounceRules[TileTypes.VERTICAL_SPLITTER][3] = new[] {0, 2};

		BounceRules[TileTypes.HORIZONTAL_SPLITTER] = new int[4][];
		BounceRules[TileTypes.HORIZONTAL_SPLITTER][0] = new[] {1, 3};
		BounceRules[TileTypes.HORIZONTAL_SPLITTER][1] = new[] {1};
		BounceRules[TileTypes.HORIZONTAL_SPLITTER][2] = new[] {1, 3};
		BounceRules[TileTypes.HORIZONTAL_SPLITTER][3] = new[] {3};

		BounceRules[TileTypes.BACK_MIRROR] = new int[4][];
		BounceRules[TileTypes.BACK_MIRROR][0] = new[] {3};
		BounceRules[TileTypes.BACK_MIRROR][1] = new[] {2};
		BounceRules[TileTypes.BACK_MIRROR][2] = new[] {1};
		BounceRules[TileTypes.BACK_MIRROR][3] = new[] {0};

		BounceRules[TileTypes.MIRROR] = new int[4][];
		BounceRules[TileTypes.MIRROR][0] = new[] {1};
		BounceRules[TileTypes.MIRROR][1] = new[] {0};
		BounceRules[TileTypes.MIRROR][2] = new[] {3};
		BounceRules[TileTypes.MIRROR][3] = new[] {2};

		BounceRules[TileTypes.EMPTY] = new int[4][];
		BounceRules[TileTypes.EMPTY][0] = new[] {0};
		BounceRules[TileTypes.EMPTY][1] = new[] {1};
		BounceRules[TileTypes.EMPTY][2] = new[] {2};
		BounceRules[TileTypes.EMPTY][3] = new[] {3};
	}

	private static int GetEnergisedCount(TileTypes[,] map, int startX, int startY, int startDir)
	{
		HashSet<(int x, int y, int dir)> energised = new();
		HashSet<(int x, int y, int dir)> beams = new();
		foreach (var beam in BounceRules[map[startX, startX]][startDir])
		{
			beams.Add((startX, startY, beam));
			energised.Add((startX, startY, beam));
		}

		while (beams.Count > 0)
		{
			(int x, int y, int dir) beam = beams.First();
			beams.Remove(beam);

			int newX = beam.x + beam.dir switch
			{
				1 => 1,
				3 => -1,
				_ => 0
			};
			int newY = beam.y + beam.dir switch
			{
				0 => -1,
				2 => 1,
				_ => 0
			};

			beam = (newX, newY, beam.dir);
            
			if (beam.x < 0 || beam.y < 0 || beam.x >= map.GetLength(0) || beam.y >= map.GetLength(1))
				continue;

			if (!energised.Add((beam.x, beam.y, beam.dir)))
				continue;

			foreach (int newBeam in BounceRules[map[beam.x, beam.y]][beam.dir])
				beams.Add((beam.x, beam.y, newBeam));
		}

		return energised.DistinctBy(b => (b.x, b.y)).Count();
	}
	
	public override string Part1(string input)
	{
		string[] lines = input.Split('\n');
		TileTypes[,] map = new TileTypes[lines[0].Length, lines.Length];

		for (int y = 0; y < lines.Length; y++)
		{
			for (int x = 0; x < lines[y].Length; x++)
			{
				map[x, y] = lines[y][x] switch
				{
					'.' => TileTypes.EMPTY,
					'|' => TileTypes.VERTICAL_SPLITTER,
					'-' => TileTypes.HORIZONTAL_SPLITTER,
					'\\' => TileTypes.BACK_MIRROR,
					'/' => TileTypes.MIRROR
				};
			}
		}

		return GetEnergisedCount(map, 0, 0, 1).ToString();
	}

	public override string Part2(string input)
	{
		string[] lines = input.Split('\n');
		TileTypes[,] map = new TileTypes[lines[0].Length, lines.Length];

		for (int y = 0; y < lines.Length; y++)
		{
			for (int x = 0; x < lines[y].Length; x++)
			{
				map[x, y] = lines[y][x] switch
				{
					'.' => TileTypes.EMPTY,
					'|' => TileTypes.VERTICAL_SPLITTER,
					'-' => TileTypes.HORIZONTAL_SPLITTER,
					'\\' => TileTypes.BACK_MIRROR,
					'/' => TileTypes.MIRROR
				};
			}
		}

		int maxEnergised = 0;
		for (int x = 0; x < map.GetLength(0); x++)
			maxEnergised = Math.Max(maxEnergised, GetEnergisedCount(map, x, 0, 2));
		for (int x = 0; x < map.GetLength(0); x++)
			maxEnergised = Math.Max(maxEnergised, GetEnergisedCount(map, x, map.GetLength(1) - 1, 0));
		for (int y = 0; y < map.GetLength(1); y++)
			maxEnergised = Math.Max(maxEnergised, GetEnergisedCount(map, 0, y, 1));
		for (int y = 0; y < map.GetLength(1); y++)
			maxEnergised = Math.Max(maxEnergised, GetEnergisedCount(map, map.GetLength(0) - 1, y, 3));

		return maxEnergised.ToString();
	}
}
