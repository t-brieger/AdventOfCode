namespace AdventOfCode;

// ReSharper disable once InconsistentNaming
public static class Extensions
{
    public static void Deconstruct<T>(this T[] x, out T t0, out T t1)
    {
        t0 = x[0];
        t1 = x[1];
    }
}