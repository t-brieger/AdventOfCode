using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day16 : Solution
    {
        public override string Part1(string input)
        {
            Dictionary<(int, string), int> auntData = new();
            string[] aunts = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (string aunt in aunts)
            {
                int i = int.Parse(aunt[4..].Split(':', 2)[0]);
                string dataPart = aunt.Split(": ", 2)[1];
                foreach (string[] split in dataPart.Split(", ").Select(field => field.Split(": ")))
                    auntData.Add((i, split[0]), int.Parse(split[1]));
            }

            for (int i = 1; i <= 500; i++)
            {
                //lol
                if (auntData.ContainsKey((i, "children")) && auntData[(i, "children")] != 3) continue;
                if (auntData.ContainsKey((i, "cats")) && auntData[(i, "cats")] != 7) continue;
                if (auntData.ContainsKey((i, "samoyeds")) && auntData[(i, "samoyeds")] != 2) continue;
                if (auntData.ContainsKey((i, "pomeranians")) && auntData[(i, "pomeranians")] != 3) continue;
                if (auntData.ContainsKey((i, "akitas")) && auntData[(i, "akitas")] != 0) continue;
                if (auntData.ContainsKey((i, "vizslas")) && auntData[(i, "vizslas")] != 0) continue;
                if (auntData.ContainsKey((i, "goldfish")) && auntData[(i, "goldfish")] != 5) continue;
                if (auntData.ContainsKey((i, "trees")) && auntData[(i, "trees")] != 3) continue;
                if (auntData.ContainsKey((i, "cars")) && auntData[(i, "cars")] != 2) continue;
                if (auntData.ContainsKey((i, "perfumes")) && auntData[(i, "perfumes")] != 1) continue;
                return i.ToString();
            }

            return null;
        }

        public override string Part2(string input)
        {
            Dictionary<(int, string), int> auntData = new();
            string[] aunts = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (string aunt in aunts)
            {
                int i = int.Parse(aunt[4..].Split(':', 2)[0]);
                string dataPart = aunt.Split(": ", 2)[1];
                foreach (string[] split in dataPart.Split(", ").Select(field => field.Split(": ")))
                    auntData.Add((i, split[0]), int.Parse(split[1]));
            }

            for (int i = 1; i <= 500; i++)
            {
                if (auntData.ContainsKey((i, "children")) && auntData[(i, "children")] != 3) continue;
                if (auntData.ContainsKey((i, "cats")) && auntData[(i, "cats")] <= 7) continue;
                if (auntData.ContainsKey((i, "samoyeds")) && auntData[(i, "samoyeds")] != 2) continue;
                if (auntData.ContainsKey((i, "pomeranians")) && auntData[(i, "pomeranians")] >= 3) continue;
                if (auntData.ContainsKey((i, "akitas")) && auntData[(i, "akitas")] != 0) continue;
                if (auntData.ContainsKey((i, "vizslas")) && auntData[(i, "vizslas")] != 0) continue;
                if (auntData.ContainsKey((i, "goldfish")) && auntData[(i, "goldfish")] >= 5) continue;
                if (auntData.ContainsKey((i, "trees")) && auntData[(i, "trees")] <= 3) continue;
                if (auntData.ContainsKey((i, "cars")) && auntData[(i, "cars")] != 2) continue;
                if (auntData.ContainsKey((i, "perfumes")) && auntData[(i, "perfumes")] != 1) continue;
                return i.ToString();
            }

            return null;
        }
    }
}