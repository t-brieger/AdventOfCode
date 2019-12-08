using System;
using System.Text;

namespace AdventOfCode.Solutions._2019
{
    public class Year2019Day08 : Solution
    {
        public override string Part1(string input)
        {
            int layerSize = 25 * 6;
            
            int fewestZeroes = int.MaxValue;
            int onesTimesTwoes = int.MinValue;
            
            for (int i = 0; i < input.Length / layerSize; i++)
            {
                string layer = input.Substring(i * layerSize, layerSize);

                int zeros = 0;
                int ones = 0;
                int twos = 0;
                
                for (int j = 0; j < layer.Length; j++)
                    if (layer[j] == '0')
                        zeros++;
                    else if (layer[j] == '1')
                        ones++;
                    else if (layer[j] == '2')
                        twos++;

                if (zeros < fewestZeroes)
                {
                    fewestZeroes = zeros;
                    onesTimesTwoes = ones * twos;
                }
            }

            return onesTimesTwoes.ToString();
        }

        public override string Part2(string input)
        {
            int layerWidth = 25;
            int layerHeight = 6;

            int layerSize = layerHeight * layerWidth;
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
                    if (layer[j] == '0')
                        image[j] = true;
                    else if (layer[j] == '1')
                        image[j] = false;
                }
            }

            //                                           data    + newlines    + buffer? 
            StringBuilder sb = new StringBuilder("\n", layerSize + layerHeight + 5);
            
            for (int i = 0; i < layerHeight; i++)
            {
                for (int j = 0; j < layerWidth; j++)
                {
                    if (image[i * layerWidth + j] == null || !(bool) image[i * layerWidth + j])
                        sb.Append('#');
                    else
                        sb.Append(' ');
                }

                sb.Append('\n');
            }
            
            return sb.ToString();
        }
    }
}