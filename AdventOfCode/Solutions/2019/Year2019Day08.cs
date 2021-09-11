using System;
using System.Text;

namespace AdventOfCode.Solutions
{
    public class Year2019Day08 : Solution
    {
        public override string Part1(string input)
        {
            const int layerSize = 25 * 6;

            int fewestZeroes = Int32.MaxValue;
            int onesTimesTwoes = Int32.MinValue;

            for (int i = 0; i < input.Length / layerSize; i++)
            {
                string layer = input.Substring(i * layerSize, layerSize);

                int zeros = 0;
                int ones = 0;
                int twos = 0;

                foreach (char t in layer)
                    switch (t)
                    {
                        case '0':
                            zeros++;
                            break;
                        case '1':
                            ones++;
                            break;
                        case '2':
                            twos++;
                            break;
                    }


                if (zeros >= fewestZeroes) continue;
                fewestZeroes = zeros;
                onesTimesTwoes = ones * twos;
            }

            return onesTimesTwoes.ToString();
        }

        public override string Part2(string input)
        {
            const int layerWidth = 25;
            const int layerHeight = 6;

            const int layerSize = layerHeight * layerWidth;
            int layerCount = input.Length / layerSize;

            //NULL: transparent (so far?)/2, false: white/1, true: black/0
            bool?[] image = new bool?[layerSize];

            Array.Fill(image, null);

            for (int i = 0; i < layerCount; i++)
            {
                string layer = input.Substring(i * layerSize, layerSize);

                for (int j = 0; j < layerSize; j++)
                {
                    if (image[j] != null)
                        continue;
                    image[j] = layer[j] switch
                    {
                        '0' => true,
                        '1' => false,
                        _ => image[j]
                    };
                }
            }

            //                             data    + newlines    + buffer? 
            StringBuilder sb = new("\n", layerSize + layerHeight + 5);

            for (int i = 0; i < layerHeight; i++)
            {
                for (int j = 0; j < layerWidth; j++) sb.Append(image[i * layerWidth + j] != true ? '#' : ' ');

                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}