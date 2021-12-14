using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2015Day19 : Solution
{
    public override string Part1(string input)
    {
        string[] inputParts = input.Split("\n\n", 2);
        IEnumerable<string[]> replacementsList = inputParts[0].Split('\n').Select(line => line.Split(" => ", 2));
        string ourMolecule = inputParts[1];

        Dictionary<string, List<string>> replacements = new();
        foreach (string[] repLine in replacementsList)
        {
            if (!replacements.ContainsKey(repLine[0]))
                replacements[repLine[0]] = new List<string>();
            replacements[repLine[0]].Add(repLine[1]);
        }

        HashSet<string> seenMolecules = new();

        for (int i = 0; i < ourMolecule.Length; i++)
        {
            string currentElement = ourMolecule[i].ToString();
            if (i != ourMolecule.Length - 1 && ourMolecule[i + 1] is >= 'a' and <= 'z')
            {
                currentElement += ourMolecule[i + 1];
            }

            if (!replacements.ContainsKey(currentElement)) continue;
            foreach (string rep in replacements[currentElement])
            {
                seenMolecules.Add(ourMolecule[..i] + rep + ourMolecule[(i + currentElement.Length)..]);
            }
        }

        return seenMolecules.Count.ToString();
    }

    public override string Part2(string input)
    {
        string molec = input.Split("\n\n")[1];
        int movesNeeded = 0;
        for (int i = 0; i < molec.Length; i++)
        {
            string element = molec[i].ToString();
            if (i != molec.Length - 1 && molec[i + 1] is >= 'a' and <= 'z')
            {
                element += molec[++i];
            }

            switch (element)
            {
                case "Rn" or "Ar":
                    continue;
                case "Y":
                    movesNeeded--;
                    break;
                default:
                    movesNeeded++;
                    break;
            }
        }

        return (movesNeeded - 1).ToString();
    }
}