using System.Collections.Generic;

namespace AdventOfCode.Solutions;

class Year2017Day21 : Solution
{
    //how the fuck does this work???
    private static HashSet<(int, int)> EnhanceGrid(IReadOnlySet<(int, int)> enabled,
        IReadOnlyDictionary<(bool, bool, bool, bool), bool[]> twos,
        Dictionary<(bool, bool, bool, bool, bool, bool, bool, bool, bool), bool[]> threes, int size)
    {
        HashSet<(int, int)> newEnabled = new();
        if (size % 2 == 0)
        {
            for (int i = 0; i < size; i += 2)
            {
                for (int j = 0; j < size; j += 2)
                {
                    bool[] ourPattern = twos[
                        (enabled.Contains((i, j + 1)), enabled.Contains((i + 1, j + 1)), enabled.Contains((i, j)),
                            enabled.Contains((i + 1, j)))
                    ];
                    if (ourPattern[0])
                        newEnabled.Add((i * 3 / 2, j * 3 / 2 + 2));
                    if (ourPattern[1])
                        newEnabled.Add((i * 3 / 2 + 1, j * 3 / 2 + 2));
                    if (ourPattern[2])
                        newEnabled.Add((i * 3 / 2 + 2, j * 3 / 2 + 2));

                    if (ourPattern[3])
                        newEnabled.Add((i * 3 / 2, j * 3 / 2 + 1));
                    if (ourPattern[4])
                        newEnabled.Add((i * 3 / 2 + 1, j * 3 / 2 + 1));
                    if (ourPattern[5])
                        newEnabled.Add((i * 3 / 2 + 2, j * 3 / 2 + 1));

                    if (ourPattern[6])
                        newEnabled.Add((i * 3 / 2, j * 3 / 2));
                    if (ourPattern[7])
                        newEnabled.Add((i * 3 / 2 + 1, j * 3 / 2));
                    if (ourPattern[8])
                        newEnabled.Add((i * 3 / 2 + 2, j * 3 / 2));
                }
            }
        }
        else
        {
            for (int i = 0; i < size; i += 3)
            {
                for (int j = 0; j < size; j += 3)
                {
                    bool[] ourPattern = threes[
                        (enabled.Contains((i, j + 2)), enabled.Contains((i + 1, j + 2)),
                            enabled.Contains((i + 2, j + 2)), enabled.Contains((i, j + 1)),
                            enabled.Contains((i + 1, j + 1)), enabled.Contains((i + 2, j + 1)),
                            enabled.Contains((i, j)), enabled.Contains((i + 1, j)), enabled.Contains((i + 2, j)))];
                    if (ourPattern[0])
                        newEnabled.Add((i * 4 / 3, j * 4 / 3 + 3));
                    if (ourPattern[1])
                        newEnabled.Add((i * 4 / 3 + 1, j * 4 / 3 + 3));
                    if (ourPattern[2])
                        newEnabled.Add((i * 4 / 3 + 2, j * 4 / 3 + 3));
                    if (ourPattern[3])
                        newEnabled.Add((i * 4 / 3 + 3, j * 4 / 3 + 3));
                        
                    if (ourPattern[4])
                        newEnabled.Add((i * 4 / 3, j * 4 / 3 + 2));
                    if (ourPattern[5])
                        newEnabled.Add((i * 4 / 3 + 1, j * 4 / 3 + 2));
                    if (ourPattern[6])
                        newEnabled.Add((i * 4 / 3 + 2, j * 4 / 3 + 2));
                    if (ourPattern[7])
                        newEnabled.Add((i * 4 / 3 + 3, j * 4 / 3 + 2));
                        
                    if (ourPattern[8])
                        newEnabled.Add((i * 4 / 3, j * 4 / 3 + 1));
                    if (ourPattern[9])
                        newEnabled.Add((i * 4 / 3 + 1, j * 4 / 3 + 1));
                    if (ourPattern[10])
                        newEnabled.Add((i * 4 / 3 + 2, j * 4 / 3 + 1));
                    if (ourPattern[11])
                        newEnabled.Add((i * 4 / 3 + 3, j * 4 / 3 + 1));
                        
                    if (ourPattern[12])
                        newEnabled.Add((i * 4 / 3, j * 4 / 3));
                    if (ourPattern[13])
                        newEnabled.Add((i * 4 / 3 + 1, j * 4 / 3));
                    if (ourPattern[14])
                        newEnabled.Add((i * 4 / 3 + 2, j * 4 / 3));
                    if (ourPattern[15])
                        newEnabled.Add((i * 4 / 3 + 3, j * 4 / 3));
                }
            }
        }

        return newEnabled;
    }

    public override string Part1(string input)
    {
        HashSet<(int, int)> grid = new()
        {
            (0, 0),
            (1, 0),
            (2, 0),
            (2, 1),
            (1, 2)
        };

        string[] rules = input.Split('\n');

        Dictionary<(bool, bool, bool, bool), bool[]> twoRules = new();
        Dictionary<(bool, bool, bool, bool, bool, bool, bool, bool, bool), bool[]> threeRules = new();

        foreach (string rule in rules)
        {
            string[] parts = rule.Replace("/", "").Split(" => ");
            bool[] from = new bool[parts[0].Length];
            bool[] to = new bool[parts[1].Length];
            for (int i = 0; i < from.Length; i++)
                from[i] = parts[0][i] == '#';
            for (int i = 0; i < to.Length; i++)
                to[i] = parts[1][i] == '#';

            for (int i = 0; i < 4; i++)
            {
                if (from.Length == 4)
                {
                    twoRules.TryAdd((from[0], from[1], from[2], from[3]), to);
                    from = new[] { from[3], from[1], from[2], from[0] };
                    twoRules.TryAdd((from[0], from[1], from[2], from[3]), to);
                    from = new[] { from[2], from[3], from[0], from[1] };
                }
                else
                {
                    threeRules.TryAdd(
                        (from[0], from[1], from[2], from[3], from[4], from[5], from[6], from[7], from[8]), to);
                    from = new[]
                        { from[8], from[5], from[2], from[7], from[4], from[1], from[6], from[3], from[0] };
                    threeRules.TryAdd(
                        (from[0], from[1], from[2], from[3], from[4], from[5], from[6], from[7], from[8]), to);
                    from = new[]
                        { from[6], from[7], from[8], from[3], from[4], from[5], from[0], from[1], from[2] };
                }
            }
        }

        int currentSize = 3;
        for (int i = 0; i < 5; i++)
        {
            grid = EnhanceGrid(grid, twoRules, threeRules, currentSize);
            if (currentSize % 2 == 0)
                currentSize = currentSize * 3 / 2;
            else
                currentSize = currentSize * 4 / 3;
        }

        return grid.Count.ToString();
    }

    public override string Part2(string input)
    {
        HashSet<(int, int)> grid = new()
        {
            (0, 0),
            (1, 0),
            (2, 0),
            (2, 1),
            (1, 2)
        };

        string[] rules = input.Split('\n');

        Dictionary<(bool, bool, bool, bool), bool[]> twoRules = new();
        Dictionary<(bool, bool, bool, bool, bool, bool, bool, bool, bool), bool[]> threeRules = new();

        foreach (string rule in rules)
        {
            string[] parts = rule.Replace("/", "").Split(" => ");
            bool[] from = new bool[parts[0].Length];
            bool[] to = new bool[parts[1].Length];
            for (int i = 0; i < from.Length; i++)
                from[i] = parts[0][i] == '#';
            for (int i = 0; i < to.Length; i++)
                to[i] = parts[1][i] == '#';

            for (int i = 0; i < 4; i++)
            {
                if (from.Length == 4)
                {
                    twoRules.TryAdd((from[0], from[1], from[2], from[3]), to);
                    from = new[] { from[3], from[1], from[2], from[0] };
                    twoRules.TryAdd((from[0], from[1], from[2], from[3]), to);
                    from = new[] { from[2], from[3], from[0], from[1] };
                }
                else
                {
                    threeRules.TryAdd(
                        (from[0], from[1], from[2], from[3], from[4], from[5], from[6], from[7], from[8]), to);
                    from = new[]
                        { from[8], from[5], from[2], from[7], from[4], from[1], from[6], from[3], from[0] };
                    threeRules.TryAdd(
                        (from[0], from[1], from[2], from[3], from[4], from[5], from[6], from[7], from[8]), to);
                    from = new[]
                        { from[6], from[7], from[8], from[3], from[4], from[5], from[0], from[1], from[2] };
                }
            }
        }

        int currentSize = 3;
        for (int i = 0; i < 18; i++)
        {
            grid = EnhanceGrid(grid, twoRules, threeRules, currentSize);
            if (currentSize % 2 == 0)
                currentSize = currentSize * 3 / 2;
            else
                currentSize = currentSize * 4 / 3;
        }

        return grid.Count.ToString();
    }
}