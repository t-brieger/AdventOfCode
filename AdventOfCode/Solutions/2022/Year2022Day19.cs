using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day19 : Solution
{
    private class Blueprint
    {
        public int oreCostOre, clayCostOre, obsidianCostOre, obsidianCostClay, geodeCostOre, geodeCostObsidian;
    }

    private int MostGeodesMinable(Blueprint bp, int time = 24, int oreBots = 1, int clayBots = 0,
        int obsidianBots = 0, int geodeBots = 0, int ore = 0, int clay = 0, int obsidian = 0,
        bool didntBuildOre = false, bool didntBuildClay = false, bool didntBuildObsidian = false,
        bool didntBuildGeode = false)
    {
        if (time == 0)
            return 0;

        int mostOreCost = Math.Max(bp.oreCostOre,
            Math.Max(bp.clayCostOre, Math.Max(bp.obsidianCostOre, bp.geodeCostOre)));
        
        bool canBuildOre = bp.oreCostOre <= ore && oreBots < mostOreCost;
        bool canBuildClay = bp.clayCostOre <= ore && clayBots < bp.obsidianCostClay;
        bool canBuildObsidian = bp.obsidianCostOre <= ore && bp.obsidianCostClay <= clay && obsidianBots < bp.geodeCostObsidian;
        bool canBuildGeode = bp.geodeCostOre <= ore && bp.geodeCostObsidian <= obsidian;

        int max = 0;
        max = Math.Max(max, MostGeodesMinable(bp, time - 1, oreBots, clayBots, obsidianBots, geodeBots,
            ore + oreBots, clay + clayBots, obsidian + obsidianBots, canBuildOre, canBuildClay, canBuildObsidian,
            canBuildGeode));
        if (canBuildOre && !didntBuildOre)
        {
            max = Math.Max(max, MostGeodesMinable(bp, time - 1, oreBots + 1, clayBots, obsidianBots, geodeBots,
                ore - bp.oreCostOre + oreBots, clay + clayBots, obsidian + obsidianBots));
        }

        if (canBuildClay && !didntBuildClay)
        {
            max = Math.Max(max, MostGeodesMinable(bp, time - 1, oreBots, clayBots + 1, obsidianBots, geodeBots,
                ore - bp.clayCostOre + oreBots, clay + clayBots, obsidian + obsidianBots));
        }

        if (canBuildObsidian && !didntBuildObsidian)
        {
            max = Math.Max(max, MostGeodesMinable(bp, time - 1, oreBots, clayBots, obsidianBots + 1,
                geodeBots,
                ore - bp.obsidianCostOre + oreBots, clay - bp.obsidianCostClay + clayBots, obsidian + obsidianBots));
        }

        if (canBuildGeode && !didntBuildGeode)
        {
            max = Math.Max(max, MostGeodesMinable(bp, time - 1, oreBots, clayBots, obsidianBots,
                geodeBots + 1,
                ore - bp.geodeCostOre + oreBots, clay + clayBots, obsidian - bp.geodeCostObsidian + obsidianBots));
        }

        return geodeBots + max;
    }

    public override string Part1(string input)
    {
        string[] bluePrintStrings = input.Split('\n');
        Blueprint[] blueprintInfo = new Blueprint[bluePrintStrings.Length];
        for (int i = 0; i < bluePrintStrings.Length; i++)
        {
            string[][] parts = bluePrintStrings[i].Split('.', ':').Select(x => x.Split(' ')).ToArray();
            int oreCost = int.Parse(parts[1][^2]);
            int clayCost = int.Parse(parts[2][^2]);
            int obsCostO = int.Parse(parts[3][^5]);
            int obsCostC = int.Parse(parts[3][^2]);
            int geoCostO = int.Parse(parts[4][^5]);
            int geoCostOb = int.Parse(parts[4][^2]);
            blueprintInfo[i] = new Blueprint
            {
                oreCostOre = oreCost,
                clayCostOre = clayCost,
                obsidianCostOre = obsCostO,
                obsidianCostClay = obsCostC,
                geodeCostOre = geoCostO,
                geodeCostObsidian = geoCostOb
            };
        }

        RescaleBar(blueprintInfo.Length);

        int qualitySum = 0;
        for (int i = 0; i < blueprintInfo.Length; i++)
        {
            int geodeCount = MostGeodesMinable(blueprintInfo[i]);
            qualitySum += (i + 1) * geodeCount;
            IncreaseBar();
        }


        return qualitySum.ToString();
    }

    public override string Part2(string input)
    {
        string[] bluePrintStrings = input.Split('\n');
        Blueprint[] blueprintInfo = new Blueprint[bluePrintStrings.Length];
        for (int i = 0; i < bluePrintStrings.Length; i++)
        {
            string[][] parts = bluePrintStrings[i].Split('.', ':').Select(x => x.Split(' ')).ToArray();
            int oreCost = int.Parse(parts[1][^2]);
            int clayCost = int.Parse(parts[2][^2]);
            int obsCostO = int.Parse(parts[3][^5]);
            int obsCostC = int.Parse(parts[3][^2]);
            int geoCostO = int.Parse(parts[4][^5]);
            int geoCostOb = int.Parse(parts[4][^2]);
            blueprintInfo[i] = new Blueprint
            {
                oreCostOre = oreCost,
                clayCostOre = clayCost,
                obsidianCostOre = obsCostO,
                obsidianCostClay = obsCostC,
                geodeCostOre = geoCostO,
                geodeCostObsidian = geoCostOb
            };
        }

        RescaleBar(3);

        int score = 1;
        for (int i = 0; i < 3; i++)
        {
            int geodeCount = MostGeodesMinable(blueprintInfo[i], 32);
            score *= geodeCount;
            IncreaseBar();
        }


        return score.ToString();
    }
}