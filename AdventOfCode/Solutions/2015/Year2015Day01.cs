namespace AdventOfCode.Solutions._2015
{
    class Year2015Day01 : Solution
    {
        public override string Part1(string input)
        {
            int floor = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                    floor++;
                else if (input[i] == ')')
                    floor--;
            }

            return floor.ToString();
        }

        public override string Part2(string input)
        {
            int floor = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                    floor++;
                else if (input[i] == ')')
                    floor--;
                if (floor < 0)
                    return (i + 1).ToString();
            }
            return "-1";
        }
    }
}
