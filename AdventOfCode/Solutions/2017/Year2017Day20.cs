using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2017Day20 : Solution
    {
        private class Particle
        {
            public int x, y, z, xv, yv, zv;
            public readonly int xa, ya, za, id;

            public Particle(int x, int y, int z, int xv, int yv, int zv, int xa, int ya, int za, int id)
            {
                this.id = id;
                this.x = x;
                this.y = y;
                this.z = z;
                this.xv = xv;
                this.yv = yv;
                this.zv = zv;
                this.xa = xa;
                this.ya = ya;
                this.za = za;
            }
        }

        /// <summary>
        /// from 0
        /// </summary>
        private static int ManhattanDistance(int x, int y, int z)
        {
            return Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
        }

        //sample: 0 <1    ,2   ,3    ,4  <5  ,6  ,7   ,8  <9 ,10,11
        //        p=<-1659,1444,-463>, v=<-14,-25,-77>, a=<10,-5,10>
        public override string Part1(string input)
        {
            int i = 0;

            return input.Split(new []{'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).Select(l => $"{i++} {l}").OrderBy(line =>
            {
                string[] things = line.Substring(0, line.Length - 1).Split(',', '<');
                return ManhattanDistance(Int32.Parse(things[9]), Int32.Parse(things[10]), Int32.Parse(things[11]));
            }).First().Split(' ')[0];
        }

        public override string Part2(string input)
        {
            int id = 0;

            HashSet<Particle> particles = new HashSet<Particle>(input.Split(new []{ '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(
                line =>
                {
                    string[] parts = line.Replace(">", "").Split('<', ',').ToArray();
                    return new Particle(Int32.Parse(parts[1]), Int32.Parse(parts[2]), Int32.Parse(parts[3]),
                        Int32.Parse(parts[5]), Int32.Parse(parts[6]), Int32.Parse(parts[7]), Int32.Parse(parts[9]),
                        Int32.Parse(parts[10]), Int32.Parse(parts[11]), id++);
                }));
            
            for (int i = 0; i < 300; i++)
            {
                foreach (Particle p in particles)
                {
                    p.xv += p.xa;
                    p.yv += p.ya;
                    p.zv += p.za;
                    p.x += p.xv;
                    p.y += p.yv;
                    p.z += p.zv;
                }

                HashSet<Particle> toRemove = new HashSet<Particle>();

                foreach (Particle p in particles)
                {
                    toRemove.UnionWith(particles.Where(p1 => p.x == p1.x && p.y == p1.y && p.z == p1.z && p.id != p1.id));
                }

                foreach (Particle particle in toRemove)
                {
                    particles.Remove(particle);
                }
            }

            return particles.Count.ToString();
        }
    }
}
