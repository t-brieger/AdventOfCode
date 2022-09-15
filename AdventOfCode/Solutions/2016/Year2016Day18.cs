namespace AdventOfCode.Solutions;

public class Year2016Day18 : Solution
{
    public override string Part1(string input)
    {
        bool[,] safeTiles = new bool[40, input.Length];

        int safeCount = 0;
        
        for (int i = 0; i < input.Length; i++)
        {
            safeTiles[0, i] = input[i] == '.';

            if (safeTiles[0, i])
                safeCount++;
        }

        for (int i = 1; i < safeTiles.GetLength(0); i++)
        {
            for (int j = 0; j < safeTiles.GetLength(1); j++)
            {
                bool leftSafe = j == 0 || safeTiles[i - 1, j - 1];
                bool centerSafe = safeTiles[i - 1, j];
                bool rightSafe = j == safeTiles.GetLength(1) - 1 || safeTiles[i - 1, j + 1];

                safeTiles[i, j] = !((!leftSafe && !centerSafe && rightSafe) 
                                  || (leftSafe && !centerSafe && !rightSafe) 
                                  || (!leftSafe && centerSafe && rightSafe) 
                                  || (leftSafe && centerSafe && !rightSafe));

                if (safeTiles[i, j])
                    safeCount++;
            }
        }
        
        return safeCount.ToString();
    }

    public override string Part2(string input)
    {
        bool[] lastSafeTiles = new bool[input.Length];
        bool[] nextSafeTiles = new bool[input.Length];

        long safeCount = 0;
        
        for (int i = 0; i < input.Length; i++)
        {
            lastSafeTiles[i] = input[i] == '.';

            if (lastSafeTiles[i])
                safeCount++;
        }

        for (int i = 1; i < 400_000; i++)
        {
            for (int j = 0; j < lastSafeTiles.Length; j++)
            {
                bool leftSafe = j == 0 || lastSafeTiles[j - 1];
                bool centerSafe = lastSafeTiles[j];
                bool rightSafe = j == lastSafeTiles.Length - 1 || lastSafeTiles[j + 1];

                nextSafeTiles[j] = !((!leftSafe && !centerSafe && rightSafe) 
                                    || (leftSafe && !centerSafe && !rightSafe) 
                                    || (!leftSafe && centerSafe && rightSafe) 
                                    || (leftSafe && centerSafe && !rightSafe));

                if (nextSafeTiles[j])
                    safeCount++;
            }

            lastSafeTiles = nextSafeTiles;
            nextSafeTiles = new bool[nextSafeTiles.Length];
        }
        
        return safeCount.ToString();
    }
}
