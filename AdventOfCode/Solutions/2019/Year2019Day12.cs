using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2019Day12 : Solution
    {
        public override string Part1(string input)
        {
            (int, int, int)[] positions = input.Split('\n')
                .Select(l => l[1..^1].Split(", ").Select(x => x[2..]).ToArray())
                .Select(arr => (int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2]))).ToArray();

            (int, int, int)[] velocities = new (int, int, int)[positions.Length];
            for (int i = 0; i < velocities.Length; i++)
                velocities[i] = (0, 0, 0);

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < positions.Length; j++)
                {
                    for (int k = 0; k < positions.Length; k++)
                    {
                        if (k == j)
                            continue;
                        (int x1, int y1, int z1) = positions[j];
                        (int x2, int y2, int z2) = positions[k];

                        (int velx, int vely, int velz) = velocities[j];

                        if (x1 > x2)
                            velx--;
                        if (x1 < x2)
                            velx++;

                        if (y1 > y2)
                            vely--;
                        if (y1 < y2)
                            vely++;

                        if (z1 > z2)
                            velz--;
                        if (z1 < z2)
                            velz++;

                        velocities[j] = (velx, vely, velz);
                    }
                }

                for (int j = 0; j < positions.Length; j++)
                {
                    (int x, int y, int z) = positions[j];
                    (int vx, int vy, int vz) = velocities[j];
                    positions[j] = (x + vx, y + vy, z + vz);
                }
            }

            int energy = 0;
            for (int i = 0; i < positions.Length; i++)
            {
                (int x, int y, int z) = positions[i];
                (int dx, int dy, int dz) = velocities[i];

                energy += (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) * (Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz));
            }

            return energy.ToString();
        }

        public override string Part2(string input)
        {
            (int, int, int)[] positions = input.Split('\n')
                .Select(l => l[1..^1].Split(", ").Select(x => x[2..]).ToArray())
                .Select(arr => (int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2]))).ToArray();

            (int, int, int)[] initialPositions = positions.Select(t => t).ToArray();

            (int, int, int)[] velocities = new (int, int, int)[positions.Length];
            for (int i = 0; i < velocities.Length; i++)
                velocities[i] = (0, 0, 0);

            long[] periods = { -1, -1, -1 };

            #region x

            for (long i = 0;; i++)
            {
                for (int j = 0; j < positions.Length; j++)
                {
                    for (int k = 0; k < positions.Length; k++)
                    {
                        if (j == k)
                            continue;

                        if (positions[j].Item1 > positions[k].Item1)
                            velocities[j] = (velocities[j].Item1 - 1, velocities[j].Item2, velocities[j].Item3);
                        if (positions[j].Item1 < positions[k].Item1)
                            velocities[j] = (velocities[j].Item1 + 1, velocities[j].Item2, velocities[j].Item3);
                    }
                }

                for (int j = 0; j < positions.Length; j++)
                {
                    positions[j] = (positions[j].Item1 + velocities[j].Item1, positions[j].Item2, positions[j].Item3);
                }

                if (velocities.All(t => t.Item1 == 0))
                {
                    bool allMatch = true;
                    for (int j = 0; j < positions.Length; j++)
                        if (positions[j].Item1 != initialPositions[j].Item1)
                        {
                            allMatch = false;
                            break;
                        }

                    if (allMatch)
                    {
                        periods[0] = i;
                        break;
                    }
                }
            }

            #endregion

            #region y

            for (long i = 0;; i++)
            {
                for (int j = 0; j < positions.Length; j++)
                {
                    for (int k = 0; k < positions.Length; k++)
                    {
                        if (j == k)
                            continue;

                        if (positions[j].Item2 > positions[k].Item2)
                            velocities[j] = (velocities[j].Item1, velocities[j].Item2 - 1, velocities[j].Item3);
                        if (positions[j].Item2 < positions[k].Item2)
                            velocities[j] = (velocities[j].Item1, velocities[j].Item2 + 1, velocities[j].Item3);
                    }
                }

                for (int j = 0; j < positions.Length; j++)
                {
                    positions[j] = (positions[j].Item1, positions[j].Item2 + velocities[j].Item2, positions[j].Item3);
                }

                if (velocities.All(t => t.Item2 == 0))
                {
                    bool allMatch = true;
                    for (int j = 0; j < positions.Length; j++)
                        if (positions[j].Item2 != initialPositions[j].Item2)
                        {
                            allMatch = false;
                            break;
                        }

                    if (allMatch)
                    {
                        periods[1] = i;
                        break;
                    }
                }
            }

            #endregion

            #region z

            for (long i = 0;; i++)
            {
                for (int j = 0; j < positions.Length; j++)
                {
                    for (int k = 0; k < positions.Length; k++)
                    {
                        if (j == k)
                            continue;

                        if (positions[j].Item3 > positions[k].Item3)
                            velocities[j] = (velocities[j].Item1, velocities[j].Item2, velocities[j].Item3 - 1);
                        if (positions[j].Item3 < positions[k].Item3)
                            velocities[j] = (velocities[j].Item1, velocities[j].Item2, velocities[j].Item3 + 1);
                    }
                }

                for (int j = 0; j < positions.Length; j++)
                {
                    positions[j] = (positions[j].Item1, positions[j].Item2, positions[j].Item3 + velocities[j].Item3);
                }

                if (velocities.All(t => t.Item3 == 0))
                {
                    bool allMatch = true;
                    for (int j = 0; j < positions.Length; j++)
                        if (positions[j].Item3 != initialPositions[j].Item3)
                        {
                            allMatch = false;
                            break;
                        }

                    if (allMatch)
                    {
                        periods[2] = i;
                        break;
                    }
                }
            }

            #endregion
 
            return Util.Lcm(periods.Select(l => l + 1).ToArray()).ToString();
        }
    }
}