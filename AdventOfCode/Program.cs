using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class Program
    {
        private static async Task<int> Main(string[] args)
        {
            //TODO showdesc
            bool all = false, pause = false, showdesc = false, test = false;
            int y = -1, d = -1;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-h":
                    case "--help":
                        showUsage();
                        break;
                    case "-a":
                    case "--all":
                        if (d != -1)
                            showUsage("--all can't be used with --day");
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
                            showUsage("--year needs an argument");
                        if (!Int32.TryParse(args[i + 1], out y))
                            showUsage("--year needs an integer argument");
                        if (y < 2015)
                            showUsage("--year needs an argument above 2014");
                        break;
                    case "-d":
                    case "--day":
                        if (all)
                            showUsage("--day can't be used with --all");
                        if (i >= args.Length - 1)
                            showUsage("--day needs an argument");
                        if (!Int32.TryParse(args[i + 1], out d))
                            showUsage("--day needs an integer argument");
                        if (d < 1 || d > 31)
                            showUsage("--day needs an argument between 1 and 31");
                        break;
                    default:
                        showUsage(args[i] + " was not recognized as a valid argument.");
                        break;
                }
            }

            if (pause && !all)
                showUsage("--pause can't be used without --all");
            if (!all && (d == -1 || y == -1))
                showUsage("if --all is not specified, both --day and --year have to be given");

            if (all)
            {
                foreach (Solution s in Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => typeof(Solution).IsAssignableFrom(t) && t != typeof(Solution)).Select(t =>
                        (Solution) Activator.CreateInstance(t)))
                {
                    //gets called on an instance of all children of Solution
                    string[] yearday = s.GetType().Name.Replace("Year", "").Split("Day", 2);
                    byte day = Byte.Parse(yearday[1]);
                    ushort year = UInt16.Parse(yearday[0]);

                    if (y != -1 && year != y)
                        continue;

                    string input = await GetInput(day, year, test);
                    Console.WriteLine($"{year}/{day:00}/1: {s.Part1(input)}");
                    Console.WriteLine($"{year}/{day:00}/2: {s.Part2(input)}");

                    if (pause)
                        Console.ReadKey();
                }
            }
            else
            {
                //must have specified day and year
                Solution s = null;
                try
                {
                    s = (Solution) Activator.CreateInstance(Assembly.GetExecutingAssembly()
                        .GetType($"AdventOfCode.Solutions.Year{y}Day{d:00}", true));
                }
                catch (ArgumentException)
                {
                    showUsage("the specified day and/or year are too long");
                }
                catch (TypeLoadException)
                {
                    showUsage("That solution does not exist");
                }

                string input = await GetInput((byte) d, (ushort) y, test);
                Console.WriteLine($"{y}/{d:00}/1: {s.Part1(input)}");
                Console.WriteLine($"{y}/{d:00}/2: {s.Part2(input)}");
            }
            return 0;
        }

        private static void showUsage(string problem = null)
        {
            if (problem != null)
                Console.WriteLine(problem);
            Console.WriteLine(
                "Usage:\n" +
                "--help,-h     show this usage help\n" +
                "--all,-a      run all solutions, optionally only in a given year, not compatible with -d\n" +
                "-p,--pause,\n" +
                "-s,--slow     wait for a keypress after running each solution, needs -a\n" +
                "--showdesc,\n" +
                "--problems    show the problem description(s)\n" +
                "--test,-t,\n" +
                "--samples     run test inputs\n" +
                "-y,--year     specify the year to run solutions from\n" +
                "-d,--day      specify the day to run solutions from, not compatible with -a");
            Environment.Exit(1);
        }

        private static string GetSession()
        {
            return File.ReadAllText("session");
        }
        
        private static async Task<string> GetInput(byte day, ushort year, bool test)
        {
            try
            {
                return File.ReadAllText($"Input/{(test ? "test/" : "")}{year}/Day{day.ToString().PadLeft(2, '0')}.in");
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
            CookieContainer cookieContainer = new CookieContainer();

            using HttpClient client = new HttpClient(
                new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                });
            cookieContainer.Add(new Uri("https://adventofcode.com"), new Cookie("session", session));

            HttpResponseMessage response = await client.GetAsync($"https://adventofcode.com/{year}/day/{day}/input");

            await File.WriteAllTextAsync($"Input/{year}/Day{day:00}.in", await response.Content.ReadAsStringAsync());
        }
    }
}