using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2018Day08 : Solution
    {
        public static int Part1RecursiveMethod(int[] tree, ref int len)
        {
            int sum = 0;

            int children = tree[len++];
            int dataCount = tree[len++];

            for (int i = 0; i < children; i++)
            {
                sum += Part1RecursiveMethod(tree, ref len);
            }

            for (int i = 0; i < dataCount; i++)
            {
                sum += tree[len++];
            }

            return sum;
        }

        public static int Part2RecursiveMethod(int[] tree, ref int len)
        {
            int sum = 0;

            int children = tree[len++];
            int dataCount = tree[len++];

            if (children == 0)
            {
                for (int i = 0; i < dataCount; i++)
                {
                    sum += tree[len++];
                }
            }
            else
            {
                int[] childrenValues = new int[children];

                for (int i = 0; i < children; i++)
                {
                    childrenValues[i] = Part2RecursiveMethod(tree, ref len);
                }

                for (int i = 0; i < dataCount; i++)
                {
                    if (tree[len] > childrenValues.Length || tree[len] <= 0)
                    {
                        len++;
                        continue;
                    }

                    sum += childrenValues[tree[len++] - 1];
                }

            }

            return sum;
        }

        public override string Part1(string input)
        {
            /*
            input = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
            //*/

            int i = 0;
            return Part1RecursiveMethod(input.Split(' ').Select(x => int.Parse(x)).ToArray(), ref i).ToString();
        }

        public override string Part2(string input)
        {
            /*
            input = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
            //*/

            int i = 0;
            return Part2RecursiveMethod(input.Split(' ').Select(x => int.Parse(x)).ToArray(), ref i).ToString();
        }
    }
}