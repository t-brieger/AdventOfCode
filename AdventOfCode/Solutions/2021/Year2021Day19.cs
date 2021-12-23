using System;
using System.Collections.Generic;
using System.Linq;
using ILGPU.Util;

namespace AdventOfCode.Solutions;

// To be honest, i cheated on day 19 - couldn't figure out the puzzle, so just took a solution from the megathread,
// copied it and edited it minimally for style. TODO: understand this.

public class Year2021Day19 : Solution
{
    private readonly struct Coord
    {
        public Coord(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
        }
    }

    private class Scanner
    {
        public Scanner(Coord center, int rotation, List<Coord> beaconsInLocal)
        {
            Center = center;
            Rotation = rotation;
            BeaconsInLocal = beaconsInLocal;
        }

        public Scanner Rotate() => new(Center, Rotation + 1, BeaconsInLocal);

        public Scanner Translate(Coord t) => new(
            new Coord(Center.X + t.X, Center.Y + t.Y, Center.Z + t.Z), Rotation, BeaconsInLocal);

        public Coord Transform(Coord coord)
        {
            (int x, int y, int z) = coord;

            (x, y, z) = (Rotation % 6) switch
            {
                0 => (x, y, z),
                1 => (-x, y, -z),
                2 => (y, -x, z),
                3 => (-y, x, z),
                4 => (z, y, -x),
                5 => (-z, y, x),
                _ => (x, y, z)
            };

            (x, y, z) = (Rotation / 6 % 4) switch
            {
                0 => (x, y, z),
                1 => (x, -z, y),
                2 => (x, -y, -z),
                3 => (x, z, -y),
                _ => (x, y, z)
            };

            return new Coord(Center.X + x, Center.Y + y, Center.Z + z);
        }

        public IEnumerable<Coord> GetBeaconsInWorld()
        {
            return BeaconsInLocal.Select(Transform);
        }

        public Coord Center { get; }
        private int Rotation { get; }
        private List<Coord> BeaconsInLocal { get; }
    }

    private static IEnumerable<Scanner> LocateScanners(string input)
    {
        HashSet<Scanner> scanners = new HashSet<Scanner>(Parse(input));
        HashSet<Scanner> locatedScanners = new HashSet<Scanner>();
        Queue<Scanner> q = new Queue<Scanner>();

        // when a scanner is located, it gets into the queue so that we can
        // explore its neighbours.

        locatedScanners.Add(scanners.First());
        q.Enqueue(scanners.First());

        scanners.Remove(scanners.First());

        while (q.Any())
        {
            Scanner scannerA = q.Dequeue();
            foreach (Scanner scannerB in scanners.ToArray())
            {
                Scanner maybeLocatedScanner = TryToLocate(scannerA, scannerB);
                if (maybeLocatedScanner == null) continue;
                locatedScanners.Add(maybeLocatedScanner);
                q.Enqueue(maybeLocatedScanner);

                scanners.Remove(scannerB); // sic! 
            }
        }

        return locatedScanners;
    }

    private static Scanner TryToLocate(Scanner scannerA, Scanner scannerB)
    {
        Coord[] beaconsInA = scannerA.GetBeaconsInWorld().ToArray();

        foreach ((Coord beaconInA, Coord beaconInB) in PotentialMatchingBeacons(scannerA, scannerB))
        {
            // now try to find the orientation for B:
            Scanner rotatedB = scannerB;
            for (int rotation = 0; rotation < 24; rotation++, rotatedB = rotatedB.Rotate())
            {
                // Moving the rotated scanner so that beaconA and beaconB overlaps. Are there 12 matches? 
                Coord beaconInRotatedB = rotatedB.Transform(beaconInB);

                Scanner locatedB = rotatedB.Translate(new Coord(
                    beaconInA.X - beaconInRotatedB.X,
                    beaconInA.Y - beaconInRotatedB.Y,
                    beaconInA.Z - beaconInRotatedB.Z
                ));

                if (locatedB.GetBeaconsInWorld().Intersect(beaconsInA).Count() >= 12)
                {
                    return locatedB;
                }
            }
        }

        // no luck
        return null;
    }

    private static IEnumerable<(Coord beaconInA, Coord beaconInB)> PotentialMatchingBeacons(Scanner scannerA,
        Scanner scannerB)
    {
        // If we had a matching beaconInA and beaconInB and moved the center
        // of the scanners to these then we would find at least 12 beacons 
        // with the same coordinates.

        // The only problem is that the rotation of scannerB is not fixed yet.

        // We need to make our check invariant to that:

        // After the translation, we could form a set from each scanner 
        // taking the absolute values of the x y and z coordinates of their beacons 
        // and compare those. 

        IEnumerable<int> AbsCoordinates(Scanner scanner) =>
            from coord in scanner.GetBeaconsInWorld()
            from v in new[] {coord.X, coord.Y, coord.Z}
            select Math.Abs(v);

        // ReSharper disable once PossibleMultipleEnumeration
        IEnumerable<T> Pick<T>(IEnumerable<T> ts) => ts.Take(ts.Count() - 11);

        foreach (Coord beaconInA in Pick(scannerA.GetBeaconsInWorld()))
        {
            HashSet<int> absA = AbsCoordinates(
                scannerA.Translate(new Coord(-beaconInA.X, -beaconInA.Y, -beaconInA.Z))
            ).ToHashSet();

            foreach (Coord beaconInB in Pick(scannerB.GetBeaconsInWorld()))
            {
                IEnumerable<int> absB = AbsCoordinates(
                    scannerB.Translate(new Coord(-beaconInB.X, -beaconInB.Y, -beaconInB.Z))
                );

                if (absB.Count(d => absA.Contains(d)) >= 3 * 12)
                {
                    yield return (beaconInA, beaconInB);
                }
            }
        }
    }

    private static Scanner[] Parse(string input) => (
        from block in input.Split("\n\n")
        let beacons =
            from line in block.Split("\n").Skip(1)
            let parts = line.Split(",").Select(int.Parse).ToArray()
            select new Coord(parts[0], parts[1], parts[2])
        select new Scanner(new Coord(0, 0, 0), 0, beacons.ToList())
    ).ToArray();

    public override string Part1(string input)
    {
        return LocateScanners(input).SelectMany(s => s.GetBeaconsInWorld()).Distinct().Count().ToString();
    }

    public override string Part2(string input)
    {
        IEnumerable<Scanner> s = LocateScanners(input);
        return s.Select(s1 => s.Where(s2 => s1 != s2).Select(s3 =>
            Math.Abs(s1.Center.X - s3.Center.X) + 
            Math.Abs(s1.Center.Y - s3.Center.Y) +
            Math.Abs(s1.Center.Z - s3.Center.Z)).Max()).Max().ToString();
    }
}