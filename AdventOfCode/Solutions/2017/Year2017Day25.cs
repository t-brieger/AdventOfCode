using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions;

class Year2017Day25 : Solution
{
    public override string Part1(string input)
    {
        //this implementation assumes that all inputs are structured like this (no parts are optional)
        //Begin in state <X>.
        //Perform a diagnostic checksum after <N> steps.
        //(
        //
        //In state <X>:
        //  If the current value is 0:
        //    - Write the value <B>.
        //    - Move one slot to the [right,left].
        //    - Continue with state <X>.
        //  If the current value is 1:
        //    - Write the value <B>.
        //    - Move one slot to the [right,left].
        //    - Continue with state <X>.
        //)+

        //state -> if 0: value, direction, next, if 1: value, direction, next
        Dictionary<char, (bool, bool, char, bool, bool, char)> stateBehaviours = new();
        int numIters = 0;
        for (int i = 54; input[i] >= '0' && input[i] <= '9'; i++)
        {
            numIters *= 10;
            numIters += input[i] - '0';
        }

        string[] rawBehaviours = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < rawBehaviours.Length; i++)
        {
            string behaviourStr = rawBehaviours[i];
            char id = behaviourStr[9];
            string[] lines = behaviourStr.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            bool write0 = lines[2][22] == '1';
            bool dir0 = lines[3][27] == 'r';
            char next0 = lines[4][26];
            bool write1 = lines[6][22] == '1';
            bool dir1 = lines[7][27] == 'r';
            char next1 = lines[8][26];
            stateBehaviours.Add(id, (write0, dir0, next0, write1, dir1, next1));
        }

        char currentState = input[15];
        int pos = 0;
        HashSet<int> onCells = new();

        for (int i = 0; i < numIters; i++)
        {
            (bool write0, bool dir0, char next0, bool write1, bool dir1, char next1) currentRules =
                stateBehaviours[currentState];
            if (onCells.Contains(pos))
            {
                //we dont do anything if we should write a 1, since there already is a 1 at this position
                if (!currentRules.write1)
                    onCells.Remove(pos);
                if (currentRules.dir1)
                    pos++;
                else
                    pos--;
                currentState = currentRules.next1;
            }
            else
            {
                //we dont do anything if we should write a 0, since there already is a 0 at this position
                if (currentRules.write0)
                    onCells.Add(pos);
                if (currentRules.dir0)
                    pos++;
                else
                    pos--;
                currentState = currentRules.next0;
            }
        }

        return onCells.Count.ToString();
    }

    public override string Part2(string input)
    {
        return "";
    }
}