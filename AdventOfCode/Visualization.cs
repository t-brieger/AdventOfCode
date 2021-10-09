using System.IO;

namespace AdventOfCode
{
    public abstract class Visualization
    {
        public abstract void Generate(StreamReader data, string outputFileName);
    }
}