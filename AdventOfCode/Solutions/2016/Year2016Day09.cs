using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions;

public class Year2016Day09 : Solution
{
    public override string Part1(string input)
    {
        // worth noting its unnecessary to compute the whole string here, but its such a minor difference in practice
        // that we might as well (for part 1, at least).
        
        input = input.Replace("\n", "").Replace(" ", "");
        StringBuilder decompressed = new();

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '(')
            {
                i++;
                string strNumChars = "";
                while (input[i] is >= '0' and <= '9')
                    strNumChars += input[i++];
                // (foo)x(bar)
                i++;
                string strNumReps = "";
                while (input[i] is >= '0' and <= '9')
                    strNumReps += input[i++];
                // closing paranthesis )
                i++;

                int numChars = int.Parse(strNumChars);
                int numReps = int.Parse(strNumReps);

                char[] repeated = new char[numChars];
                int repeatedIx = 0;
                for (; repeatedIx < repeated.Length; repeatedIx++)
                    repeated[repeatedIx] = input[i + repeatedIx];
                i += repeated.Length;

                string repeatedStr = new(repeated);

                for (int j = 0; j < numReps; j++)
                    decompressed.Append(repeatedStr);
                
                // because its getting incremented again in the outer loop.
                i--;
            }
            else
                decompressed.Append(input[i]);
        }

        return decompressed.Length.ToString();
    }

    public override string Part2(string input)
    {
        input = input.Replace("\n", "").Replace(" ", "");

        // (start, end, factor) (both ends included)
        List<(int, int, int)> multipliers = new();

        long count = 0;
        
        for (int i = 0; i < input.Length; i++)
        {
            List<(int, int, int)> toRemove = new();
            foreach ((int start, int end, int fac) in multipliers)
            {
                if (end < i)
                    toRemove.Add((start, end, fac));
            }

            foreach ((int, int, int) t in toRemove)
                multipliers.Remove(t);
            
            if (input[i] == '(')
            {
                string strLength = "";
                string strFac = "";

                i++;
                
                while (input[i] is >= '0' and <= '9')
                    strLength += input[i++];
                i++;
                while (input[i] is >= '0' and <= '9')
                    strFac += input[i++];

                int len = int.Parse(strLength);
                int fac = int.Parse(strFac);

                multipliers.Add((i + 1, i + len, fac));
            }
            else
            {
                count += multipliers.Aggregate(1, (a, t) => a * t.Item3);
            }
        }
        
        
        return count.ToString();
    }
}