using System;

namespace AdventOfCode.Solutions;

public static class Assembunny
{
    public static void Run(long[] registers, string[][] instructions)
    {
        for (long i = 0; i < instructions.Length; i++)
        {
            string[] instruction = instructions[i];

            try
            {
                switch (instruction[0])
                {
                    case "cpy":
                        long valueA = instruction[1][0] is >= 'a' and <= 'd'
                            ? registers[instruction[1][0] - 'a']
                            : int.Parse(instruction[1]);
                        registers[instruction[2][0] - 'a'] = valueA;
                        break;
                    case "inc":
                        registers[instruction[1][0] - 'a']++;
                        break;
                    case "dec":
                        registers[instruction[1][0] - 'a']--;
                        break;
                    case "jnz":
                        long jumpAmount = (instruction[2][0] is >= 'a' and <= 'd' ? registers[instruction[2][0] - 'a'] : int.Parse(instruction[2])) - 1;

                        bool jump = (instruction[1][0] is >= 'a' and <= 'd'
                            ? registers[instruction[1][0] - 'a']
                            : int.Parse(instruction[1])) != 0;

                        if (jump)
                            i += jumpAmount;
                        break;
                    case "tgl":
                        long index = i;
                        if (instruction[1][0] is >= 'a' and <= 'd')
                            index += registers[instruction[1][0] - 'a'];
                        else
                            index += int.Parse(instruction[1]);

                        if (index >= instructions.Length || index < 0)
                            break;

                        if (instructions[index].Length == 2)
                            instructions[index][0] = instructions[index][0] == "inc" ? "dec" : "inc";
                        if (instructions[index].Length == 3)
                            instructions[index][0] = instructions[index][0] == "jnz" ? "cpy" : "jnz";

                        break;
                }
            }
            catch (Exception e)
            {
                // just quietly ignore exceptions (which are caused by "tgl" producing invalid instructions)
            }
        }
    }
}
