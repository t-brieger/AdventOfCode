using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace AdventOfCode
{
    // ReSharper disable once InconsistentNaming
    static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            bool all = false, pause = false, showdesc = false, test = false, time = true;
            int y = -1, d = -1;

            for (int i = 0; i < args.Length; i++)
                switch (args[i])
                {
                    case "-h":
                    case "--help":
                        ShowUsage();
                        break;
                    case "-a":
                    case "--all":
                        if (d != -1)
                            ShowUsage("--all can't be used with --day");
                        all = true;
                        break;
                    case "-p":
                    case "-s":
                    case "--pause":
                    case "--slow":
                        pause = true;
                        break;
                    case "--no-timing":
                        time = false;
                        break;
                    case "--showdesc":
                    case "--problems":
                        showdesc = true;
                        break;
                    case "-t":
                    case "--test":
                    case "--samples":
                        test = true;
                        break;
                    case "-y":
                    case "--year":
                        if (i >= args.Length - 1)
                            ShowUsage("--year needs an argument");
                        if (!Int32.TryParse(args[i + 1], out y))
                            ShowUsage("--year needs an integer argument");
                        if (y < 2015)
                            ShowUsage("--year needs an argument above 2014");
                        i++;
                        break;
                    case "-d":
                    case "--day":
                        if (all)
                            ShowUsage("--day can't be used with --all");
                        if (i >= args.Length - 1)
                            ShowUsage("--day needs an argument");
                        if (!Int32.TryParse(args[i + 1], out d))
                            ShowUsage("--day needs an integer argument");
                        if (d is < 1 or > 31)
                            ShowUsage("--day needs an argument between 1 and 31");
                        i++;
                        break;
                    default:
                        ShowUsage(args[i] + " was not recognized as a valid argument.");
                        break;
                }

            if (pause && !all)
                ShowUsage("--pause can't be used without --all");
            if (!all && (d == -1 || y == -1))
                ShowUsage("if --all is not specified, both --day and --year have to be given");

            if (all)
            {
                foreach (Solution s in Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => typeof(Solution).IsAssignableFrom(t) && t != typeof(Solution)).Select(t =>
                        (Solution)Activator.CreateInstance(t)))
                {
                    //gets called on an instance of all children of Solution
                    string[] yearday = s?.GetType().Name.Replace("Year", "").Split("Day", 2);
                    byte day = Byte.Parse(yearday?[1]!);
                    ushort year = UInt16.Parse(yearday?[0]!);

                    if (y != -1 && year != y)
                        continue;

                    if (showdesc)
                        await DisplayText(day, year);
                    Console.WriteLine();

                    await RunSolution(s, time, day, year, test);

                    if (!pause) continue;
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            else
            {
                //must have specified day and year
                Solution s = null;
                try
                {
                    s = (Solution)Activator.CreateInstance(Assembly.GetExecutingAssembly()
                        .GetType($"AdventOfCode.Solutions.Year{y}Day{d:00}", true)!);
                }
                catch (ArgumentException)
                {
                    ShowUsage("the specified day and/or year are too long");
                }
                catch (TypeLoadException)
                {
                    ShowUsage("That solution does not exist");
                }

                if (showdesc)
                    await DisplayText((byte)d, (ushort)y);
                Console.WriteLine();
                
                await RunSolution(s, time, (byte)d, (ushort)y, test);
            }

            return 0;
        }

        private static void ShowUsage(string problem = null)
        {
            if (problem != null)
                Console.WriteLine(problem);
            Console.WriteLine(
                "Usage:\n" +
                "--help,-h     show this usage help\n" +
                "--all,-a      run all solutions, optionally only in a given year, not compatible with -d\n" +
                "-p,--pause,\n" +
                "-s,--slow     wait for a keypress after running each solution, needs -a\n" +
                "-v,--visual   generates visualizations for some solutions\n" +
                "--visual-path where to store files for visualizations (default: ./AoCVisuals) - temporary files will be in a sub-folder called 'temp'." +
                "--showdesc,\n" +
                "--problems    show the problem description(s)\n" +
                "--test,-t,\n" +
                "--samples     run test inputs\n" +
                "-y,--year     specify the year to run solutions from\n" +
                "-d,--day      specify the day to run solutions from, not compatible with -a\n" +
                "--no-timing   disable timing the solutions");
            Environment.Exit(1);
        }

        private static async Task RunSolution(Solution s, bool time, byte d, ushort y, bool test) {
            string input = await GetInput(d, y, test);
            s.rawInput = input;
            input = input.Trim();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string p1 = null;
            try
            {
                p1 = s.Part1(input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            sw.Stop();
            long elapsed1 = sw.ElapsedMilliseconds;
            sw.Restart();
            string p2 = null;
            try
            {
                p2 = s.Part2(input);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }sw.Stop();
            long elapsed2 = sw.ElapsedMilliseconds;
            Console.WriteLine($"Running Day {y}/{d:00}...");
            Console.WriteLine($"  Part 1: {p1}");
            if (time)
                Console.WriteLine($"    Took {elapsed1} ms.");
            Console.WriteLine($"  Part 2: {p2}");
            if (time)
                Console.WriteLine($"    Took {elapsed2} ms.");
            if (time)
                Console.WriteLine($"  Took {elapsed1 + elapsed2} ms in total.");
        }

        private static string GetSession()
        {
            return File.ReadAllText("session");
        }

        private static async Task<string> GetInput(byte day, ushort year, bool test)
        {
            try
            {
                return (await File.ReadAllTextAsync(
                        $"Input/{(test ? "test/" : "")}{year}/Day{day.ToString().PadLeft(2, '0')}.in"))
                    .Replace("\r\n", "\n");
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory($"Input/{(test ? "test/" : "")}{year}");
                await DownloadSolution(GetSession(), day, year);
                return await GetInput(day, year, test);
            }
            catch (FileNotFoundException)
            {
                await DownloadSolution(GetSession(), day, year);
                return await GetInput(day, year, test);
            }
        }

        private static async Task DownloadSolution(string session, byte day, ushort year)
        {
            CookieContainer cookieContainer = new();

            using HttpClient client = new(
                new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                });
            cookieContainer.Add(new Uri("https://adventofcode.com"), new Cookie("session", session));

            HttpResponseMessage response = await client.GetAsync($"https://adventofcode.com/{year}/day/{day}/input");

            await File.WriteAllTextAsync($"Input/{year}/Day{day:00}.in", await response.Content.ReadAsStringAsync());
        }

        private static async Task<string> DownloadText(string session, byte day, ushort year)
        {
            CookieContainer cookieContainer = new();

            using HttpClient client = new(
                new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                });
            cookieContainer.Add(new Uri("https://adventofcode.com"), new Cookie("session", session));

            HttpResponseMessage response = await client.GetAsync($"https://adventofcode.com/{year}/day/{day}");

            return await response.Content.ReadAsStringAsync();
        }

        private static async Task<string> TryGetBufferedText(byte day, ushort year)
        {
            try
            {
                return await File.ReadAllTextAsync($"problems/{year}/{day:00}.html");
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static async Task<string> BufferText(string content, byte day, ushort year)
        {
            Directory.CreateDirectory($"problems/{year}");
            await File.WriteAllTextAsync($"problems/{year}/{day:00}.html", content);
            return content;
        }

        private static async Task<string> GetText(byte day, ushort year)
        {
            return await TryGetBufferedText(day, year) ??
                   await BufferText(await DownloadText(GetSession(), day, year), day, year);
        }

        private static async Task RenderNode(INode n, bool inPre = false)
        {
            //not using foreach loops here because We're modifying the child node list
            ConsoleColor oldfColor = Console.ForegroundColor;
            ConsoleColor oldbColor = Console.BackgroundColor;
            switch (n.NodeName.ToLowerInvariant())
            {
                case "script":
                case "style":
                    break;
                case "#text":
                    Console.Write(n.NodeValue.Replace("\n", inPre ? "\n" : ""));
                    break;
                case "code":
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    // ReSharper disable once ForCanBeConvertedToForeach
                    for (int i = 0; i < n.ChildNodes.Length; i++)
                        await RenderNode(n.ChildNodes[i], inPre);
                    break;
                case "h2":
                    Console.ForegroundColor = ConsoleColor.White;
                    // ReSharper disable once ForCanBeConvertedToForeach
                    for (int i = 0; i < n.ChildNodes.Length; i++)
                        await RenderNode(n.ChildNodes[i], inPre);
                    Console.WriteLine();
                    break;
                case "li":
                    Console.Write("  - ");
                    //this is dumb, but C# doesnt want me to just have it fallthrough
                    goto case "p";
                case "ul":
                    Console.WriteLine();
                    goto case "p";
                case "p":
                    if (((IElement)n).ClassList.Contains("day-success"))
                    {
                        while (n.NextSibling != null) n.NextSibling.RemoveFromParent();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    // ReSharper disable once ForCanBeConvertedToForeach
                    for (int i = 0; i < n.ChildNodes.Length; i++)
                        await RenderNode(n.ChildNodes[i], inPre);
                    Console.WriteLine();
                    Console.WriteLine();
                    break;
                case "pre":
                    // ReSharper disable once ForCanBeConvertedToForeach
                    for (int i = 0; i < n.ChildNodes.Length; i++)
                        await RenderNode(n.ChildNodes[i], true);
                    break;
                case "em":
                    Console.ForegroundColor = ((IElement)n).ClassList.Contains("star")
                        ? ConsoleColor.Yellow
                        : ConsoleColor.White;
                    // ReSharper disable once ForCanBeConvertedToForeach
                    for (int i = 0; i < n.ChildNodes.Length; i++)
                        await RenderNode(n.ChildNodes[i], inPre);
                    break;
                default:
                    // ReSharper disable once ForCanBeConvertedToForeach
                    for (int i = 0; i < n.ChildNodes.Length; i++)
                        await RenderNode(n.ChildNodes[i], inPre);
                    break;
            }

            Console.ForegroundColor = oldfColor;
            Console.BackgroundColor = oldbColor;
        }

        private static async Task DisplayText(byte day, ushort year)
        {
            string html = await GetText(day, year);

            IDocument doc = await BrowsingContext.New(Configuration.Default).OpenAsync(r => r.Content(html));

            INode main = doc.Body.ChildNodes.First(n => n.NodeName.ToLower() == "main");

            await RenderNode(main);
        }
    }
}