namespace AdventOfCode.Solutions;

public class Year2021Day12 : Solution
{
    private static int FindPaths(Dictionary<string, HashSet<string>> connections, List<string> path, bool p2 = false,
        bool hasDoubleSmallCave = false)
    {
        /*if (p2)
            Console.WriteLine(new string('\t', path.Count - 1) + "starting processing of path " +
                          string.Join("->", path));*/

        string current = path.Last();
        if (current == "end")
            return 1;

        int repetitionsOfCurrent = path.Count(e => e == current);

        if (p2 && !hasDoubleSmallCave && current[0] is >= 'a' and <= 'z' && repetitionsOfCurrent == 2 &&
            current is not ("start" or "end"))
            hasDoubleSmallCave = true;
        else if (repetitionsOfCurrent != 1 && current[0] is >= 'a' and <= 'z')
            return 0;

        int pathsFound = 0;

        foreach (string c in connections[current])
        {
            List<string> newpath = new(path) { c };
            pathsFound += FindPaths(connections, newpath, p2, hasDoubleSmallCave);
        }

        /*Console.WriteLine(new string('\t', path.Count - 1) + "Done with processing nested path " +
                          string.Join("->", path));*/

        return pathsFound;
    }

    public override string Part1(string input)
    {
        Dictionary<string, HashSet<string>> connections = new();
        foreach (string conn in input.Split('\n'))
        {
            (string a, string b) = conn.Split('-');
            if (!connections.ContainsKey(a))
                connections[a] = new HashSet<string>();
            if (!connections.ContainsKey(b))
                connections[b] = new HashSet<string>();

            connections[a].Add(b);
            connections[b].Add(a);
        }

        return FindPaths(connections, new List<string>(new[] { "start" })).ToString();
    }

    public override string Part2(string input)
    {
        Dictionary<string, HashSet<string>> connections = new();
        foreach (string conn in input.Split('\n'))
        {
            (string a, string b) = conn.Split('-');
            if (!connections.ContainsKey(a))
                connections[a] = new HashSet<string>();
            if (!connections.ContainsKey(b))
                connections[b] = new HashSet<string>();

            connections[a].Add(b);
            connections[b].Add(a);
        }

        return FindPaths(connections, new List<string>(new[] { "start" }), true).ToString();
    }
}