using System;
using System.Text;

namespace AdventOfCode.Solutions;

public class Year2016Day16 : Solution
{
    public override string Part1(string input)
    {
        while (input.Length < 272)
        {
            string reverse = input.Reverse();
            reverse = reverse.Replace('1', '2').Replace('0', '1').Replace('2', '0');
            input += "0" + reverse;
        }

        input = input.Substring(0, 272);

        while (input.Length % 2 == 0)
        {
            string checkSum = "";
            for (int i = 0; i < input.Length-1; i += 2)
            {
                checkSum += input[i] == input[i + 1] ? '1' : '0';
            }

            input = checkSum;
        }
        
        return input;
    }

    public override string Part2(string input)
    {
        // there is almost certain something clever possible here because the pattern is *almost* periodic:
        // it's "a 0 b 0 a 1 b 0 a 1 b 0 a 0 b ..." - the only thing that changes is the joiner
        // one could, for example, pre-compute the first-level checksums of "a0", "a1", "b0" and "b1", for some speedup,
        // and then just concatenate those appropriately (and pre-compute a level deeper, then one more, etc)
        
        // however, this runs in under half a second on my somewhat old laptop, so I won't bother, despite its abhorrent
        // memory usage.
        
        while (input.Length < 35651584)
        {
            string reverse = input.Reverse();
            reverse = reverse.Replace('1', '2').Replace('0', '1').Replace('2', '0');
            input += "0" + reverse;
        }

        input = input.Substring(0, 35651584);
        
        while (input.Length % 2 == 0)
        {
            StringBuilder checkSum = new StringBuilder();
            for (int i = 0; i < input.Length-1; i += 2)
            {
                checkSum.Append(input[i] == input[i + 1] ? '1' : '0');
            }

            input = checkSum.ToString();
        }
        
        return input;
    }
}
