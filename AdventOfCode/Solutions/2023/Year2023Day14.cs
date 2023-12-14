using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions;

public class Year2023Day14 : Solution
{
	private enum TileTypes
	{
		EMPTY,
		ROUND_ROCK,
		SQUARE_ROCK
	}

	private static TileTypes[,] ShiftMapNorth(TileTypes[,] map)
	{
		TileTypes[,] newMap = new TileTypes[map.GetLength(0), map.GetLength(1)];
		for (int x = 0; x < map.GetLength(0); x++)
			for (int y = 0; y < map.GetLength(1); y++)
				if (map[x, y] != TileTypes.ROUND_ROCK)
					newMap[x, y] = map[x, y];
				else
					newMap[x, y] = TileTypes.EMPTY;

		for (int x = 0; x < map.GetLength(0); x++)
		{
			for (int y = 0; y < map.GetLength(1); y++)
			{
				if (map[x, y] != TileTypes.ROUND_ROCK) continue;

				int probeY = y;
				while (probeY >= 0 && newMap[x, probeY] == TileTypes.EMPTY)
					probeY--;

				newMap[x, probeY + 1] = TileTypes.ROUND_ROCK;
			}
		}

		return newMap;
	}

	public override string Part1(string input)
	{
		string[] lines = input.Split('\n');
		TileTypes[,] map = new TileTypes[lines[0].Length, lines.Length];

		for (int y = 0; y < lines.Length; y++)
			for (int x = 0; x < lines[y].Length; x++)
				map[x, y] = lines[y][x] switch
				{
					'.' => TileTypes.EMPTY,
					'#' => TileTypes.SQUARE_ROCK,
					'O' => TileTypes.ROUND_ROCK,
					_ => throw new ArgumentException($"Invalid Tile Character: {lines[y][x]}")
				};


		map = ShiftMapNorth(map);

		int answer = 0;
		for (int x = 0; x < map.GetLength(0); x++)
			for (int y = 0; y < map.GetLength(1); y++)
				if (map[x, y] == TileTypes.ROUND_ROCK)
					answer += map.GetLength(1) - y;

		return answer.ToString();
	}

	private static TileTypes[,] RotateMap(TileTypes[,] input)
	{
		TileTypes[,] ret = new TileTypes[input.GetLength(1), input.GetLength(0)];

		for (int x = 0; x < input.GetLength(0); x++)
		{
			for (int y = 0; y < input.GetLength(1); y++)
			{
				ret[input.GetLength(1) - y - 1, x] = input[x, y];
			}
		}

		return ret;
	}

	private static ulong HashMap(TileTypes[,] map)
	{
		ulong hash = 1;
		for (int x = 0; x < map.GetLength(0); x++)
		{
			for (int y = 0; y < map.GetLength(1); y++)
			{
				hash <<= 1;
				if (map[x, y] == TileTypes.ROUND_ROCK)
					hash |= 1;
				// this is prime (or at least wolframalpha claims so)
				hash %= 9223372036854775783;
			}
		}

		return hash;
	}

	public override string Part2(string input)
	{
		string[] lines = input.Split('\n');
		TileTypes[,] map = new TileTypes[lines[0].Length, lines.Length];

		for (int y = 0; y < lines.Length; y++)
			for (int x = 0; x < lines[y].Length; x++)
				map[x, y] = lines[y][x] switch
				{
					'.' => TileTypes.EMPTY,
					'#' => TileTypes.SQUARE_ROCK,
					'O' => TileTypes.ROUND_ROCK,
					_ => throw new ArgumentException($"Invalid Tile Character: {lines[y][x]}")
				};

		Dictionary<ulong, int> seenStates = new();
		for (int i = 0; i < 1_000_000_000; i++)
		{
			ulong hash = HashMap(map);

			if (seenStates.ContainsKey(hash))
			{
				int lastSeen = seenStates[hash];
				int cycleLength = i - lastSeen;
				while (i + cycleLength < 1_000_000_000)
					i += cycleLength;
			}
			else
				seenStates[hash] = i;

			// we always shift "north" because rotating the map is less annoying than separating those 4 cases
			for (int d = 0; d < 4; d++)
			{
				map = ShiftMapNorth(map);
				map = RotateMap(map);
			}
		}

		// we end on being rotated so that the original west is "up", so correct for that.
		// map = RotateMap(map);
		// map = RotateMap(map);
		// map = RotateMap(map);

		int answer = 0;
		for (int x = 0; x < map.GetLength(0); x++)
			for (int y = 0; y < map.GetLength(1); y++)
				if (map[x, y] == TileTypes.ROUND_ROCK)
					answer += map.GetLength(1) - y;

		return answer.ToString();
	}
}
