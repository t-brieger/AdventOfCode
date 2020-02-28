using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2017
{
    class Year2017Day21 : Solution
    {
        /*private class SubGrid
        {
            public bool[] fields;

            public SubGrid(SubGrid other)
            {
                this.fields = new bool[other.fields.Length];
                Buffer.BlockCopy(other.fields, 0, this.fields, 0, other.fields.Length);
            }

            public SubGrid(params bool[] field)
            {
                this.fields = field;
            }

            public bool Equals(SubGrid sg)
            {
                for (int i = 0; i < fields.Length; i++)
                    if (fields[i] != sg.fields[i])
                        return false;
                return true;
            }

            public override bool Equals(object o)
            {
                return this.Equals(o as SubGrid);
            }

            public override int GetHashCode()
            {
                int hash = 0;
                for (int i = 0; i < fields.Length; i++)
                    //this makes no sense in terms of ordering, but who cares, it only needs to be unique-ish
                    hash |= fields[i] ? (1 << i) : 0;

                hash |= fields.Length << 24;

                return hash;
            }

            public string ToString()
            {
                int size;
                if (fields.Length == 4)
                    size = 2;
                else if (fields.Length == 9)
                    size = 3;
                else
                    size = 4;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        sb.Append(fields[i * size + j] ? "#" : ".");
                    }
                    sb.Append("\n");
                }
                return sb.ToString();
            }
        }

        private static void transpose(bool[] arr)
        {
            int size;
            if (arr.Length == 4)
                size = 2;
            else if (arr.Length == 9)
                size = 3;
            else
                size = 4;

            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++) {
                    bool tmp = arr[size * i + j];
                    arr[size * i + j] = arr[size * j + i];
                    arr[size * j + i] = tmp;
                }
            }
        }

        private static void reverse_rows(bool[] arr)
        {
            int size;
            if (arr.Length == 4)
                size = 2;
            else if (arr.Length == 9)
                size = 3;
            else
                size = 4;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size / 2; j++)
                {
                    bool tmp = arr[i * size + j];
                    arr[i * size + j] = arr[(i + 1) * size - 1 - j];
                    arr[(i + 1) * size - 1 - j] = tmp;
                }
            }
        }

        private static bool[] flatStringToBoolArray(string s)
        {
            //ie: "##/##" -> indexOf returns 2
            int size = s.IndexOf('/');
            bool[,] ret = new bool[size, size];

            int row = 0;
            int col;
            foreach (string line in s.Split('/'))
            {
                col = 0;
                foreach (char c in line)
                {
                    ret[col++, row] = c == '#';
                }
                row++;
            }

            bool[] ret2 = new bool[ret.Length];
            Buffer.BlockCopy(ret, 0, ret2, 0, ret.Length);

            return ret2;
        }

        public override string Part1(string input)
        {
            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<SubGrid, SubGrid> transformations = new Dictionary<SubGrid, SubGrid>();

            foreach (string s in lines)
            {
                string[] parts = s.Split(" => ");
                SubGrid original = new SubGrid(flatStringToBoolArray(parts[0]));
                SubGrid outcome = new SubGrid(flatStringToBoolArray(parts[1]));
                transformations.Add(original, outcome);
                for (int i = 0; i < 3; i++)
                {
                    transpose(original.fields);
                    if (!transformations.ContainsKey(original))
                        transformations.Add(new SubGrid(original), outcome);
                    reverse_rows(original.fields);
                    if (!transformations.ContainsKey(original))
                        transformations.Add(new SubGrid(original), outcome);
                }
            }

            bool[] grid = new[] { false, true, false, false, false, true, true, true, true };

            //apply them
            for (int _ = 0; _ < 5; _++)
            {
                int size = grid.Length == 4 ? 2 : grid.Length == 9 ? 3 : grid.Length == 16 ? 4 : grid.Length == 36 ? 6 : grid.Length == 64 ? 8 : grid.Length == 81 ? 9 : grid.Length == 100 ? 10 : grid.Length == 144 ? 12 : grid.Length == 324 ? 18 : -1;

                int new_size = size % 2 == 0 ? (size + size / 2) : (size + size / 3);

                bool[] new_grid = new bool[new_size * new_size];

                for (int i = 0; i < size;)
                {
                    for (int j = 0; j < size;)
                    {
                        if (size % 2 == 0)
                        {
                            SubGrid replacement = transformations[new SubGrid(grid[i * size + j], grid[i * size + j + 1], grid[(i + 1) * size + j], grid[(i + 1) * size + j + 1])];

                            new_grid[(i + i / 2 + 0) * new_size + (j + j / 2) + 0] = replacement.fields[0];
                            new_grid[(i + i / 2 + 0) * new_size + (j + j / 2) + 1] = replacement.fields[1];
                            new_grid[(i + i / 2 + 0) * new_size + (j + j / 2) + 2] = replacement.fields[2];

                            new_grid[(i + i / 2 + 1) * new_size + (j + j / 2) + 0] = replacement.fields[3];
                            new_grid[(i + i / 2 + 1) * new_size + (j + j / 2) + 1] = replacement.fields[4];
                            new_grid[(i + i / 2 + 1) * new_size + (j + j / 2) + 2] = replacement.fields[5];

                            new_grid[(i + i / 2 + 2) * new_size + (j + j / 2) + 0] = replacement.fields[6];
                            new_grid[(i + i / 2 + 2) * new_size + (j + j / 2) + 1] = replacement.fields[7];
                            new_grid[(i + i / 2 + 2) * new_size + (j + j / 2) + 2] = replacement.fields[8];

                            i += 2;
                            j += 2;
                        } else
                        {
                            SubGrid replacement = transformations[new SubGrid(grid[i * size + j], grid[i * size + j + 1], grid[i * size + j + 2],
                                                                              grid[(i + 1) * size + j], grid[(i + 1) * size + j + 1], grid[(i + 1) * size + j + 2],
                                                                              grid[(i + 2) * size + j], grid[(i + 2) * size + j + 1], grid[(i + 2) * size + j + 2])];

                            new_grid[(i + i / 3 + 0) * new_size + (j + j / 3) + 0] = replacement.fields[0];
                            new_grid[(i + i / 3 + 0) * new_size + (j + j / 3) + 1] = replacement.fields[1];
                            new_grid[(i + i / 3 + 0) * new_size + (j + j / 3) + 2] = replacement.fields[2];
                            new_grid[(i + i / 3 + 0) * new_size + (j + j / 3) + 3] = replacement.fields[3];

                            new_grid[(i + i / 3 + 1) * new_size + (j + j / 3) + 0] = replacement.fields[4];
                            new_grid[(i + i / 3 + 1) * new_size + (j + j / 3) + 1] = replacement.fields[5];
                            new_grid[(i + i / 3 + 1) * new_size + (j + j / 3) + 2] = replacement.fields[6];
                            new_grid[(i + i / 3 + 1) * new_size + (j + j / 3) + 3] = replacement.fields[7];

                            new_grid[(i + i / 3 + 2) * new_size + (j + j / 3) + 0] = replacement.fields[8];
                            new_grid[(i + i / 3 + 2) * new_size + (j + j / 3) + 1] = replacement.fields[9];
                            new_grid[(i + i / 3 + 2) * new_size + (j + j / 3) + 2] = replacement.fields[10];
                            new_grid[(i + i / 3 + 2) * new_size + (j + j / 3) + 3] = replacement.fields[11];

                            new_grid[(i + i / 3 + 3) * new_size + (j + j / 3) + 0] = replacement.fields[12];
                            new_grid[(i + i / 3 + 3) * new_size + (j + j / 3) + 1] = replacement.fields[13];
                            new_grid[(i + i / 3 + 3) * new_size + (j + j / 3) + 2] = replacement.fields[14];
                            new_grid[(i + i / 3 + 3) * new_size + (j + j / 3) + 3] = replacement.fields[15];

                            i += 3;
                            j += 3;
                        }
                    }
                }

                grid = new_grid;
            }


            int cnt = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i])
                    cnt++;
            }
            return cnt.ToString();
        }

        public override string Part2(string input)
        {
            return "";
        }*/

        public string Part1()
        {
            //TODO !!
            return "Not Implemented - this is a huge mess";
        }

        public string Part2()
        {
            return "Not Implemented - this is a huge mess";
        }
    }
}
