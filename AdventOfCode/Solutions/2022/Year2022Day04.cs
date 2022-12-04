using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;

namespace AdventOfCode.Solutions;

public class Year2022Day04 : Solution
{
    public override string Part1(string input)
    {
        // oh no
        IEnumerable<((int, int), (int, int))> lines = input.Split('\n').Select(x => x.Split(",").Select(y => y.Split('-').Select(int.Parse).ToArray()).ToArray()).Select(z => ((z[0][0], z[0][1]), ((z[1][0], z[1][1]))));

        int count = 0;
        foreach (((int s1, int e1), (int s2, int e2)) in lines)
        {
            if ((s1 <= s2 && e1 >= e2) || (s2 <= s1 && e2 >= e1))
                count++;
        }
        
        return count.ToString();
    }

    public override string Part2(string input)
    {
        IEnumerable<((int, int), (int, int))> lines = input.Split('\n').Select(x => x.Split(",").Select(y => y.Split('-').Select(int.Parse).ToArray()).ToArray()).Select(z => ((z[0][0], z[0][1]), ((z[1][0], z[1][1]))));

        int count = 0;
        foreach (((int s1, int e1), (int s2, int e2)) in lines)
        {
            if ((s1 <= s2 && e1 >= s2) || (s2 <= s1 && e2 >= s1))
                count++;
        }
        
        return count.ToString();
    }
}