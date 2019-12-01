using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2017
{
    public class Year2017Day20 : Solution
    {
        private class Particle
        {
            public int x, y, z, xv, yv, zv, xa, ya, za, id;

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
            public Particle() { }
        }

        /// <summary>
        /// from 0
        /// </summary>
        private static int manhattanDistance(int x, int y, int z)
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
                var things = line.Substring(0, line.Length - 1).Split(new[] {',', '<'});
                return manhattanDistance(int.Parse(things[9]), int.Parse(things[10]), int.Parse(things[11]));
            }).First().Split(' ')[0];
        }

        public override string Part2(string input)
        {
            int id = 0;

            HashSet<Particle> particles = new HashSet<Particle>(input.Split(new []{ '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(
                line =>
                {
                    var parts = line.Replace(">", "").Split('<', ',').ToList();
                    return new Particle(int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]),
                        int.Parse(parts[5]), int.Parse(parts[6]), int.Parse(parts[7]), int.Parse(parts[9]),
                        int.Parse(parts[10]), int.Parse(parts[11]), id++);
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

                foreach (var particle in toRemove)
                {
                    particles.Remove(particle);
                }
            }

            return particles.Count.ToString();
        }
    }
}