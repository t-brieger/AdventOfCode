using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day22 : Solution
{
    public override string Part1(string input)
    {
        (bool on, int x1, int x2, int y1, int y2, int z1, int z2)[] commands = input.Split('\n')
            .Select(s => s.Split(new[] {' ', ',', '=', '.'}, StringSplitOptions.RemoveEmptyEntries)).Select(x =>
                (x[0].Length == 2, Math.Max(int.Parse(x[2]), -50), Math.Min(int.Parse(x[3]), 50),
                    Math.Max(int.Parse(x[5]), -50), Math.Min(int.Parse(x[6]), 50), Math.Max(int.Parse(x[8]), -50),
                    Math.Min(int.Parse(x[9]), 50))).ToArray();

        HashSet<(int, int, int)> enabled = new HashSet<(int, int, int)>();

        foreach ((bool on, int x1, int x2, int y1, int y2, int z1, int z2) in commands)
        {
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    for (int z = z1; z <= z2; z++)
                    {
                        if (on)
                            enabled.Add((x, y, z));
                        else
                            enabled.Remove((x, y, z));
                    }
                }
            }
        }

        return enabled.Count.ToString();
    }

    private static (bool, int, int, int, int, int, int) Intersection(
        (bool, int x1, int x2, int y1, int y2, int z1, int z2) cuboid1,
        (bool, int x1, int x2, int y1, int y2, int z1, int z2) cuboid2, bool outState)
    {
        int x1 = Math.Max(cuboid1.x1, cuboid2.x1);
        int x2 = Math.Min(cuboid1.x2, cuboid2.x2);
        int y1 = Math.Max(cuboid1.y1, cuboid2.y1);
        int y2 = Math.Min(cuboid1.y2, cuboid2.y2);
        int z1 = Math.Max(cuboid1.z1, cuboid2.z1);
        int z2 = Math.Min(cuboid1.z2, cuboid2.z2);
        if (x1 <= x2 && y1 <= y2 && z1 <= z2)
            return (outState, x1, x2, y1, y2, z1, z2);
        return (outState, 0, -1, 0, -1, 0, -1);
    }

    private static long PatchArea((bool on, int x1, int x2, int y1, int y2, int z1, int z2) p)
    {
        int x = p.x2 - p.x1 + 1;
        int y = p.y2 - p.y1 + 1;
        int z = p.z2 - p.z1 + 1;
        return (p.on ? 1L : -1L) * x * y * z;
    }

    public override string Part2(string input)
    {
        (bool on, int x1, int x2, int y1, int y2, int z1, int z2)[] commands = input.Split('\n')
            .Select(s => s.Split(new[] {' ', ',', '=', '.'}, StringSplitOptions.RemoveEmptyEntries)).Select(x =>
                (x[0].Length == 2, int.Parse(x[2]), int.Parse(x[3]), int.Parse(x[5]), int.Parse(x[6]), int.Parse(x[8]),
                    int.Parse(x[9]))).ToArray();

        List<(bool, int, int, int, int, int, int)> patches = new List<(bool, int, int, int, int, int, int)>
            {commands[0]};

        foreach ((bool on, int x1, int x2, int y1, int y2, int z1, int z2) newPatch in commands.Skip(1))
        {
            List<(bool, int, int, int, int, int, int)> oldPatches = patches.ToArray().ToList();
            if (newPatch.on)
                patches.Add(newPatch);
            foreach ((bool on, int x1, int x2, int y1, int y2, int z1, int z2) patch in oldPatches)
            {
                if (newPatch.on && patch.on)
                    patches.Add(Intersection(newPatch, patch, false));
                else if (newPatch.on && !patch.on)
                    patches.Add(Intersection(newPatch, patch, true));
                else if (!newPatch.on && patch.on)
                    patches.Add(Intersection(newPatch, patch, false));
                else if (!newPatch.on && !patch.on)
                    patches.Add(Intersection(newPatch, patch, true));
            }

            patches = patches.Where(p => PatchArea(p) != 0).ToList();
        }

        return patches.Aggregate(0L, (sum, patch) => sum + PatchArea(patch)).ToString();
    }
}