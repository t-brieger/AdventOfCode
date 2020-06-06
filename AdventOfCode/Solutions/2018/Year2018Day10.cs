using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2018Day10 : Solution
    {
        public override string Part1(string input)
        {
            ((int x, int y) velocity, (int x, int y) position)[] points = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(line => ((Int32.Parse(line.Substring(36, 2)), Int32.Parse(line.Substring(40, 2))), (Int32.Parse(line.Substring(10, 6)), Int32.Parse(line.Substring(18, 6))))).ToArray();

            bool shouldLoop = true;
            ((int start, int end) x, (int start, int end) y) boundingBox = ((50001, -50001), (50001, -50001));
            bool isFirstIter = true;
            while (true)
            {
                if (!isFirstIter)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        if (points[i].position.x + points[i].velocity.x <= boundingBox.x.end &&
                            points[i].position.x + points[i].velocity.x >= boundingBox.x.start &&
                            points[i].position.y + points[i].velocity.y <= boundingBox.y.end &&
                            points[i].position.y + points[i].velocity.y >= boundingBox.y.start) continue;
                        shouldLoop = false;
                        break;
                    }
                }

                isFirstIter = false;

                if (!shouldLoop)
                    break;

                boundingBox = ((0, 0), (0, 0));

                for (int i = 0; i < points.Length; i++)
                {
                    points[i].position.x += points[i].velocity.x;
                    points[i].position.y += points[i].velocity.y;


                    if (points[i].position.x > boundingBox.x.end)
                        boundingBox.x.end = points[i].position.x;
                    else if (points[i].position.y > boundingBox.y.end)
                        boundingBox.y.end = points[i].position.y;


                    if (points[i].position.x < boundingBox.x.start)
                        boundingBox.x.start = points[i].position.x;
                    else if (points[i].position.y < boundingBox.y.start)
                        boundingBox.y.start = points[i].position.y;
                }
            }

            bool[,] result = new bool[boundingBox.x.end - boundingBox.x.start + 1, boundingBox.y.end - boundingBox.y.start + 1];
            for (int i = 0; i < points.Length; i++)
            {
                int y = points[i].position.y - boundingBox.y.start;
                int x = points[i].position.x - boundingBox.x.start;
                result[x, y] = true;
            }
            for (int x = 0; x < result.GetLength(0); x += 1)
            {
                for (int y = 0; y < result.GetLength(1); y += 1)
                {
                    Console.Write(result[x, y] ? '#' : ' ');
                }

                Console.Write('\n');
            }

            return "uncomment Writes in code - spam warning";
            //return "saved as output.png";

        }

        public override string Part2(string input)
        {
            ((int x, int y) velocity, (int x, int y) position)[] points = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(line => ((Int32.Parse(line.Substring(36, 2)), Int32.Parse(line.Substring(40, 2))), (Int32.Parse(line.Substring(10, 6)), Int32.Parse(line.Substring(18, 6))))).ToArray();


            bool shouldLoop = true;
            ((int start, int end) x, (int start, int end) y) boundingBox = ((50001, -50001), (50001, -50001));
            bool isFirstIter = true;
            int secs = -1; //I dunno why but it always seems to return one too much
            while (true)
            {
                secs++;
                if (!isFirstIter)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        if (points[i].position.x + points[i].velocity.x <= boundingBox.x.end &&
                            points[i].position.x + points[i].velocity.x >= boundingBox.x.start &&
                            points[i].position.y + points[i].velocity.y <= boundingBox.y.end &&
                            points[i].position.y + points[i].velocity.y >= boundingBox.y.start) continue;
                        shouldLoop = false;
                        break;
                    }
                }

                isFirstIter = false;

                if (!shouldLoop)
                    break;

                boundingBox = ((0, 0), (0, 0));

                for (int i = 0; i < points.Length; i++)
                {
                    points[i].position.x += points[i].velocity.x;
                    points[i].position.y += points[i].velocity.y;


                    if (points[i].position.x > boundingBox.x.end)
                        boundingBox.x.end = points[i].position.x;
                    else if (points[i].position.y > boundingBox.y.end)
                        boundingBox.y.end = points[i].position.y;


                    if (points[i].position.x < boundingBox.x.start)
                        boundingBox.x.start = points[i].position.x;
                    else if (points[i].position.y < boundingBox.y.start)
                        boundingBox.y.start = points[i].position.y;
                }
            }

            return secs.ToString();
        }
    }
}