namespace AdventOfCode.Solutions
{
    class Year2015Day01 : Solution
    {
        public override string Part1(string input)
        {
            int floor = 0;
            foreach (char c in input)
                switch (c)
                {
                    case '(':
                        floor++;
                        break;
                    case ')':
                        floor--;
                        break;
                }

            return floor.ToString();
        }

        public override string Part2(string input)
        {
            int floor = 0;
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '(':
                        floor++;
                        break;
                    case ')':
                        floor--;
                        break;
                }

                if (floor < 0)
                    return (i + 1).ToString();
            }

            return "-1";
        }
    }
}