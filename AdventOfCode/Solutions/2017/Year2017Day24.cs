using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2017
{
    //TODO: try to do something like the following:
    /*  remove all the "double-components" (cant think of a better word rn), like 0/0, 1/1, 2/2 etc, from the original list of components
     *  then, from the recursive function, also get a list of pin amounts that occur *anywhere* in the longest/strongest 
     *  (depending on which part) bridge, and for each double-component whose number occurs, add twice that to the total - that 
     *  way, we'll have to check way fewer bridges, I estimate that It'd give roughly a 10-15x speed improvement on my input (also its applicable to both parts)
     */
    class Year2017Day24 : Solution
    {
        (int strength, int length) Build(int strength, int length, int port, List<(int pins1, int pins2)> allComponents, bool byLength)
        {
            List<(int pins1, int pins2)> usable = allComponents.Where(x => x.pins1 == port || x.pins2 == port).ToList();

            if (usable.Count == 0) return (strength, length);

            List<(int strength, int length)> bridges = new List<(int, int)>();

            foreach ((int pins1, int pins2) comp in usable)
            {
                int strength1 = strength + comp.pins1 + comp.pins2;
                int length1 = length + 1;
                int nextPort = port == comp.Item1 ? comp.Item2 : comp.Item1;
                List<(int pins1, int pins2)> remaining = allComponents.ToList(); remaining.Remove(comp);
                bridges.Add(Build(strength1, length1, nextPort, remaining, byLength));
            }
            return bridges.OrderBy(x => byLength ? x.Item2 : 0).ThenBy(x => x.Item1).Last();
        }

        public override string Part1(string input)
        {
            List<(int l, int r)> components = new List<(int, int)>();

            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in lines)
            {
                string[] split = s.Split('/');
                components.Add((int.Parse(split[0]), int.Parse(split[1])));
            }

            return Build(0, 0, 0, components, false).Item1.ToString();
        }

        public override string Part2(string input)
        {
            List<(int l, int r)> components = new List<(int, int)>();

            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in lines)
            {
                string[] split = s.Split('/');
                components.Add((int.Parse(split[0]), int.Parse(split[1])));
            }

            return Build(0, 0, 0, components, true).Item1.ToString();
        }
    }
}
