using System.Collections.Generic;

namespace AdventOfCode.Solutions;

public class Year2023Day17 : Solution
{
	public override string Part1(string input)
	{
		string[] lines = input.Split('\n');
		int[,] map = new int[lines[0].Length, lines.Length];
        
		for (int y = 0; y < lines.Length; y++)
			for (int x = 0; x < lines[y].Length; x++)
				map[x, y] = lines[y][x] - '0';

		(int x, int y)[] directions =
		{
			(0, -1), (1, 0), (0, 1), (-1, 0)
		};
		
        
		// state: (x, y, straight_count, dir)
		(_, int heatLoss) = Util.Djikstra((0, 0, 0, 1), (state, cost) =>
		{
			(int x, int y, int straight, int dir) = state;

			if (straight == 4)
				return new ((int, int, int, int), int)[] {};
			
			List<((int, int, int, int), int)> ret = new();

			for (int d = 0; d < 4; d++)
			{
				if (d == (dir + 2) % 4)
					continue;
				
				(int newX, int newY) = (x + directions[d].x, y + directions[d].y);

				if (newX < 0 || newY < 0)
					continue;
				if (newX >= map.GetLength(0) || newY >= map.GetLength(1))
					continue;
                
				ret.Add(((newX, newY, d == dir ? straight + 1 : 1, d), cost + map[newX, newY]));
			}
			
			return ret;
		}, state => (state.Item1, state.Item2) == (map.GetLength(0) - 1, map.GetLength(1) - 1) && state.Item3 < 4);

		return heatLoss.ToString();
	}

	public override string Part2(string input)
	{
		string[] lines = input.Split('\n');
		int[,] map = new int[lines[0].Length, lines.Length];
        
		for (int y = 0; y < lines.Length; y++)
			for (int x = 0; x < lines[y].Length; x++)
				map[x, y] = lines[y][x] - '0';

		(int x, int y)[] directions =
		{
			(0, -1), (1, 0), (0, 1), (-1, 0)
		};
		
        
		// state: (x, y, straight_count, dir)
		(_, int heatLoss) = Util.Djikstra((0, 0, 0, -1), (state, cost) =>
		{
			(int x, int y, int straight, int dir) = state;

			if (straight > 10)
				return new ((int, int, int, int), int)[] {};
			
			List<((int, int, int, int), int)> ret = new();

			for (int d = 0; d < 4; d++)
			{
				// only happens on our initial tile, because of the start state we defined (first tile after turning
				// has straight=1, not 0)
				if (straight == 0)
					dir = d;
				
				if (d == (dir + 2) % 4)
					continue;
				if (d != dir && straight < 4)
					continue;
				
				(int newX, int newY) = (x + directions[d].x, y + directions[d].y);

				if (newX < 0 || newY < 0)
					continue;
				if (newX >= map.GetLength(0) || newY >= map.GetLength(1))
					continue;
                
				ret.Add(((newX, newY, d == dir ? straight + 1 : 1, d), cost + map[newX, newY]));
			}
			
			return ret;
		}, state => (state.Item1, state.Item2) == (map.GetLength(0) - 1, map.GetLength(1) - 1) && state.Item3 is < 11 and > 3);

		return heatLoss.ToString();
	}
}
