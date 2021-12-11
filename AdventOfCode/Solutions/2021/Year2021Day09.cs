using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2021Day09 : Solution
    {
        public override string Part1(string input)
        {
            Dictionary<(int, int), int> heightMap = new();
            string[] lines = input.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            for (int j = 0; j < lines[i].Length; j++)
                heightMap.Add((i, j), lines[i][j] - '0');

            int lows = 0;

            foreach (KeyValuePair<(int, int), int> kvp in heightMap)
            {
                (int x, int y) = kvp.Key;
                if (heightMap.ContainsKey((x - 1, y)) && heightMap[(x - 1, y)] <= kvp.Value) continue;
                if (heightMap.ContainsKey((x + 1, y)) && heightMap[(x + 1, y)] <= kvp.Value) continue;
                if (heightMap.ContainsKey((x, y - 1)) && heightMap[(x, y - 1)] <= kvp.Value) continue;
                if (heightMap.ContainsKey((x, y + 1)) && heightMap[(x, y + 1)] <= kvp.Value) continue;

                lows += kvp.Value + 1;
            }

            return lows.ToString();
        }

        public override string Part2(string input)
        {
            Dictionary<(int, int), int> heightMap = new();
            string[] lines = input.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            for (int j = 0; j < lines[i].Length; j++)
                heightMap.Add((i, j), lines[i][j] - '0');

            HashSet<(int, int)> lows = new();

            foreach (KeyValuePair<(int, int), int> kvp in heightMap)
            {
                (int x, int y) = kvp.Key;
                if (heightMap.ContainsKey((x - 1, y)) && heightMap[(x - 1, y)] <= kvp.Value) continue;
                if (heightMap.ContainsKey((x + 1, y)) && heightMap[(x + 1, y)] <= kvp.Value) continue;
                if (heightMap.ContainsKey((x, y - 1)) && heightMap[(x, y - 1)] <= kvp.Value) continue;
                if (heightMap.ContainsKey((x, y + 1)) && heightMap[(x, y + 1)] <= kvp.Value) continue;

                lows.Add((x, y));
            }

            List<int> basinSizes = new();

            foreach ((int x, int y) in lows)
            {
                HashSet<(int, int)> seen = new();
                FindBasin(x, y, heightMap, seen);
                basinSizes.Add(seen.Count);
            }

            return basinSizes.OrderByDescending(e => e).Take(3).Aggregate(1, (t, e) => t * e).ToString();
        }

        private static void FindBasin(int x, int y, IReadOnlyDictionary<(int, int), int> heightMap,
            ISet<(int, int)> seen)
        {
            if (heightMap.ContainsKey((x - 1, y)) && heightMap[(x - 1, y)] != 9 && !seen.Contains((x - 1, y)))
            {
                seen.Add((x - 1, y));
                FindBasin(x - 1, y, heightMap, seen);
            }

            if (heightMap.ContainsKey((x + 1, y)) && heightMap[(x + 1, y)] != 9 && !seen.Contains((x + 1, y)))
            {
                seen.Add((x + 1, y));
                FindBasin(x + 1, y, heightMap, seen);
            }

            if (heightMap.ContainsKey((x, y - 1)) && heightMap[(x, y - 1)] != 9 && !seen.Contains((x, y - 1)))
            {
                seen.Add((x, y - 1));
                FindBasin(x, y - 1, heightMap, seen);
            }

            if (heightMap.ContainsKey((x, y + 1)) && heightMap[(x, y + 1)] != 9 && !seen.Contains((x, y + 1)))
            {
                seen.Add((x, y + 1));
                FindBasin(x, y + 1, heightMap, seen);
            }
        }
    }
}