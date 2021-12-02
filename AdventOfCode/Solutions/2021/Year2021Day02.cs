using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2021Day02 : Solution
    {
        public override string Part1(string input)
        {
            int depth = 0, horizontal = 0;
            foreach ((char d, int num) in input.Split('\n').Select(l => l.Split(' '))
                .Select(l => (l[0][0], int.Parse(l[1]))))
            {
                switch (d)
                {
                    case 'u':
                        depth -= num;
                        break;
                    case 'd':
                        depth += num;
                        break;
                    default:
                        // forward
                        horizontal += num;
                        break;
                }
            }

            return (depth * horizontal).ToString();
        }

        public override string Part2(string input)
        {
            int depth = 0, horizontal = 0, aim = 0;
            foreach ((char d, int num) in input.Split('\n').Select(l => l.Split(' '))
                .Select(l => (l[0][0], int.Parse(l[1]))))
            {
                switch (d)
                {
                    case 'u':
                        aim -= num;
                        break;
                    case 'd':
                        aim += num;
                        break;
                    default:
                        // forward
                        horizontal += num;
                        depth += aim * num;
                        break;
                }
            }

            return (depth * horizontal).ToString();
        }
    }
}