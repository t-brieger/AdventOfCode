using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day21 : Solution
    {
        private static bool PlayerWinsBattle(int pHp, int pAtk, int pDef, int bHp, int bAtk, int bDef)
        {
            pAtk -= bDef;
            bAtk -= pDef;
            if (pAtk <= 0)
                pAtk = 1;
            if (bAtk <= 0)
                bAtk = 1;

            while (pHp > 0 && bHp > 0)
            {
                bHp -= pAtk;
                if (bHp <= 0)
                    return true;
                pHp -= bAtk;
            }

            return false;
        }

        public override string Part1(string input)
        {
            int[] bossStats = input.Split('\n').Select(line => int.Parse(line.Split(": ")[1])).ToArray();

            (int cost, int atk)[] weapons = { (8, 4), (10, 5), (25, 6), (40, 7), (74, 8) };
            (int cost, int def)[] armours = { (0, 0), (13, 1), (31, 2), (53, 3), (75, 4), (102, 5) };
            (int cost, int atk, int def)[] rings =
                { (0, 0, 0), (0, 0, 0), (25, 1, 0), (50, 2, 0), (100, 3, 0), (20, 0, 1), (40, 0, 2), (80, 0, 3) };

            int bestPrice = int.MaxValue;

            foreach ((int cost, int atk) w in weapons)
            {
                foreach ((int cost, int def) a in armours)
                {
                    for (int k = 0; k < rings.Length; k++)
                    {
                        for (int l = k + 1; l < rings.Length; l++)
                        {
                            (int wcost, int watk) = w;
                            (int acost, int adef) = a;
                            (int r1Cost, int r1Atk, int r1Def) = rings[k];
                            (int r2Cost, int r2Atk, int r2Def) = rings[l];

                            if (!PlayerWinsBattle(100, watk + r1Atk + r2Atk, adef + r1Def + r2Def, bossStats[0],
                                bossStats[1], bossStats[2])) continue;
                            if (wcost + acost + r1Cost + r2Cost < bestPrice)
                                bestPrice = wcost + acost + r1Cost + r2Cost;
                        }
                    }
                }
            }


            return bestPrice.ToString();
        }

        public override string Part2(string input)
        {
            int[] bossStats = input.Split('\n').Select(line => int.Parse(line.Split(": ")[1])).ToArray();

            (int cost, int atk)[] weapons = { (8, 4), (10, 5), (25, 6), (40, 7), (74, 8) };
            (int cost, int def)[] armours = { (0, 0), (13, 1), (31, 2), (53, 3), (75, 4), (102, 5) };
            (int cost, int atk, int def)[] rings =
                { (0, 0, 0), (0, 0, 0), (25, 1, 0), (50, 2, 0), (100, 3, 0), (20, 0, 1), (40, 0, 2), (80, 0, 3) };

            int worstPrice = int.MinValue;

            foreach ((int cost, int atk) w in weapons)
            {
                foreach ((int cost, int def) a in armours)
                {
                    for (int k = 0; k < rings.Length; k++)
                    {
                        for (int l = k + 1; l < rings.Length; l++)
                        {
                            (int wcost, int watk) = w;
                            (int acost, int adef) = a;
                            (int r1Cost, int r1Atk, int r1Def) = rings[k];
                            (int r2Cost, int r2Atk, int r2Def) = rings[l];

                            if (PlayerWinsBattle(100, watk + r1Atk + r2Atk, adef + r1Def + r2Def, bossStats[0],
                                bossStats[1], bossStats[2])) continue;
                            if (wcost + acost + r1Cost + r2Cost > worstPrice)
                                worstPrice = wcost + acost + r1Cost + r2Cost;
                        }
                    }
                }
            }


            return worstPrice.ToString();
        }
    }
}