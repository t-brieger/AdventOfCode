using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AdventOfCode;
using AngleSharp;
using AngleSharp.Dom;

namespace GeneralRunner;

public static class GeneralRunner
{
    private const string UserAgent = "https://github.com/t-brieger/AdventofCode by <t-brieger at gmx.de>";

    private static async Task<int> Main(string[] args)
    {
        bool all = false, pause = false, showdesc = false, test = false;
        int y = -1, d = -1, progressWidth = 12;

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
                    if (!int.TryParse(args[i + 1], out y))
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
                    if (!int.TryParse(args[i + 1], out d))
                        ShowUsage("--day needs an integer argument");
                    if (d is < 1 or > 31)
                        ShowUsage("--day needs an argument between 1 and 31");
                    i++;
                    break;
                case "--bar-width":
                    if (!int.TryParse(args[++i], out progressWidth) || progressWidth <= 0)
                        ShowUsage("--bar-width's argument needs to be a positive integer");
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
                             (Solution)Activator.CreateInstance(t)!))
            {
                //gets called on an instance of all children of Solution
                string[] yearday = s.GetType().Name.Replace("Year", "").Split("Day", 2);
                byte day = byte.Parse(yearday[1]);
                ushort year = ushort.Parse(yearday[0]);

                if (y != -1 && year != y)
                    continue;

                if (showdesc)
                    await DisplayText(day, year);
                Console.WriteLine();

                await RunSolution(s, day, year, test, progressWidth);

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
                s = (Solution)Activator.CreateInstance(Assembly.GetAssembly(typeof(Solution))!
                    .GetType($"AdventOfCode.Solutions.Year{y}Day{d:00}", true)!)!;
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
                
            await RunSolution(s, (byte)d, (ushort)y, test, progressWidth);
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
            "-d,--day      specify the day to run solutions from, not compatible with -a");
        Environment.Exit(1);
    }

    private static async Task RunSolution(Solution s, byte d, ushort y, bool test, int progressWidth) {
        string input = await GetInput(d, y, test);
        s!.rawInput = input;
        input = input.Trim();

        Console.WriteLine($"Running Day {y}/{d:00}...");
        
        string p1 = null;
        try
        {
            (Action p1Inc, Action<int> p1Res, Action<int> p1Set, Func<int> p1Get) = MakeProgressBar(progressWidth, $"{y} / {d,2} Part 1");
            s.IncreaseBar = p1Inc;
            s.RescaleBar = p1Res;
            s.SetBar = p1Set;
            s.GetBarScale = p1Get;
            p1 = s.Part1(input);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        Console.WriteLine("  " + p1);

        string p2 = null;
        try
        {
            (Action p2Inc, Action<int> p2Res, Action<int> p2Set, Func<int> p2Get) = MakeProgressBar(progressWidth, $"{y} / {d,2} Part 2");
            s.IncreaseBar = p2Inc;
            s.RescaleBar = p2Res;
            s.SetBar = p2Set;
            s.GetBarScale = p2Get;
            p2 = s.Part2(input);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        Console.WriteLine("  " + p2);
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
            await DownloadInput(GetSession(), day, year);
            return await GetInput(day, year, test);
        }
        catch (FileNotFoundException)
        {
            await DownloadInput(GetSession(), day, year);
            return await GetInput(day, year, test);
        }
    }

    private static async Task DownloadInput(string session, byte day, ushort year)
    {
        CookieContainer cookieContainer = new();

        using HttpClient client = new(
            new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
        HttpRequestMessage hrm = new HttpRequestMessage();
        hrm.Headers.UserAgent.TryParseAdd(UserAgent);
        hrm.Method = HttpMethod.Get;
        hrm.RequestUri = new Uri($"https://adventofcode.com/{year}/day/{day}/input");
        cookieContainer.Add(new Uri("https://adventofcode.com"), new Cookie("session", session));

        HttpResponseMessage response = await client.SendAsync(hrm);

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
        HttpRequestMessage hrm = new HttpRequestMessage();
        hrm.Headers.UserAgent.TryParseAdd(UserAgent);
        hrm.Method = HttpMethod.Get;
        hrm.RequestUri = new Uri($"https://adventofcode.com/{year}/day/{day}");
        cookieContainer.Add(new Uri("https://adventofcode.com"), new Cookie("session", session));

        HttpResponseMessage response = await client.SendAsync(hrm);
        
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

    private static (Action inc, Action<int> rescale, Action<int> set, Func<int> get) MakeProgressBar(int terminalWidth, string label)
    {
        int done = -1;
        int max = 1;

        Console.WriteLine();

        int lastFilledCount = -1, lastPercentage = -1;
        
        Action inc = () =>
        {
            done++;
            int percentage = (done * 100) / max;

            // +0.5 to round instead of truncating
            int filledCount = (int) ((float)done / max * terminalWidth + 0.5);

            if (filledCount == lastFilledCount && percentage == lastPercentage)
                return;
            
            Console.Write($"\r{label}: [{new string('#', filledCount)}{new string(' ', terminalWidth - filledCount)}] ({percentage,3}%)");
        };

        Action<int> rescale = x =>
        {
            max = x;
            done--;
            // redraw
            inc();
        };

        Action<int> set = x =>
        {
            done = x;
            done--;
            inc();
        };
        
        Func<int> get = () => max;

        inc();
        
        return (inc, rescale, set, get);
    }
}
