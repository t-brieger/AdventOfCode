using System;
using System.Globalization;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day18 : Solution
{
	public override string Part1(string input)
	{
		(char dir, int len)[] directions = input.Split('\n').Select(line => line.Split(' '))
			.Select(line => (line[0][0], int.Parse(line[1]))).ToArray();

		directions = directions.Append(directions[0]).ToArray();

		(int x, int y)[] offsets = {(0, -1), (1, 0), (0, 1), (-1, 0)};
		
		long area = 0;
		(long x, long y) pos = (0, 0);
		
		for (int i = 0; i < directions.Length - 1; i++)
		{
			int dir = directions[i+1].dir switch
			{
				'U' => 0,
				'R' => 1,
				'D' => 2,
				'L' => 3,
				_ => throw new ArgumentException($"{directions[i].dir} is not a valid direction.")
			};
			
			(long ox, long oy) = (directions[i+1].len * offsets[dir].x,
				directions[i+1].len * offsets[dir].y);
			(long x, long y) next = (pos.x + ox, pos.y + oy);

			area += (pos.y + next.y) * (pos.x - next.x);
			pos = next;
		}

		area /= 2;
		// area is now our trench's area, except:
		//	walls are only counted as half a tile
		//  corners are counted 3/4 or 1/4 depending on direction
		
		// correct the former problem first
		area += directions.Skip(1).Sum(dir => dir.len + 1) / 2;
		
		// outside corners (like U1R1) are now counted 1.25 times (previously 1/4, but it's also the intersection of
		//		2 walls)
		// inside corners are analogously counted 1.75 times
		// since our polygon is closed, there are exactly 4 more outside than inside corners, so subtract 1 from the 
		// total area (i.e. 4 * (1.25 - 1)), then subtract 1 for every 2 corners left (since they have to match up for
		// 0.25 + 0.75 = 1 excess area per pair)
		area -= 1;
		area -= (directions.Length - 5) / 2;

		return area.ToString();
	}

	public override string Part2(string input)
	{
		(int dir, long len)[] directions = input.Split('\n').Select(line => line.Split(' '))
			.Select(line => ((line[2][7] - '0' + 1) % 4, long.Parse(line[2][2..7], NumberStyles.HexNumber))).ToArray();

		directions = directions.Append(directions[0]).ToArray();

		(int x, int y)[] offsets = {(0, -1), (1, 0), (0, 1), (-1, 0)};
		
		long area = 0;
		(long x, long y) pos = (0, 0);
		
		for (int i = 0; i < directions.Length - 1; i++)
		{
			(long ox, long oy) = (directions[i+1].len * offsets[directions[i+1].dir].x,
				directions[i+1].len * offsets[directions[i+1].dir].y);
			(long x, long y) next = (pos.x + ox, pos.y + oy);

			area += (pos.y + next.y) * (pos.x - next.x);
			pos = next;
		}

		area /= 2;
		area += directions.Skip(1).Sum(dir => dir.len + 1) / 2;
		
		area -= 1;
		area -= (directions.Length - 5) / 2;

		return area.ToString();
	}
}
