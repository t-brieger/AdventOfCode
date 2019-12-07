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
        static int Main(string[] args)
        {
            Match inputRegex =
                new Regex(@"^(-d,(?<day>\d\d),-y,(?<year>\d\d\d\d)|(?<all>-a?))(?<test>,-t)?$").Match(
                    string.Join(',', args));
            if (!inputRegex.Success)
            {
                Console.Error.WriteLine("USAGE: \"(-d <day> -y <year>) | -a[ -t]\"");
                Console.ReadKey();
                return 1;
            }

            if (!inputRegex.Groups["all"].Success)
            {
                byte day = byte.Parse(inputRegex.Groups["day"].Value);
                ushort year = ushort.Parse(inputRegex.Groups["year"].Value);
                Console.WriteLine(day + " - " + year);

                try
                {
                    executeSolution(day, year, inputRegex.Groups["test"].Success);
                }
                catch (FileNotFoundException)
                {
                    Console.Error.WriteLine("Solution not found, exiting");
                    Console.ReadKey();
                    return 1;
                }
            }
            else
            {
                foreach (Solution s in Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => typeof(Solution).IsAssignableFrom(t) && t != typeof(Solution)).Select(t =>
                        (Solution) Activator.CreateInstance(t)))
                {
                    string[] splitted = s.GetType().Name.Split("Day", 2);
                    ushort year = ushort.Parse(splitted[0].Replace("Year", ""));
                    byte day = byte.Parse(splitted[1]);
                    /*string input = getInput(day, year);
                    string output;
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    output = s.Part1(input);
                    sw.Stop();
                    Console.WriteLine($"{day:00}/{year}/1: {output} completed in (very roughly) {sw.ElapsedMilliseconds}");
                    sw = new Stopwatch();
                    sw.Start();
                    output = s.Part2(input);
                    sw.Stop();
                    Console.WriteLine($"{day:00}/{year}/2: {output} completed in (very roughly) {sw.ElapsedMilliseconds}");*/
                    try
                    {
                        executeSolution(day, year, inputRegex.Groups["test"].Success);
                        if (inputRegex.Groups["slow"].Success)
                            Console.ReadKey();
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine($"Solution {day:00}/{year} missing");
                    }
                }
            }

            Console.ReadKey();
            return 0;
        }

        private static void executeSolution(byte day, ushort year, bool test)
        {
            Solution solution = getSolution(day, year);
            string input = "";
            try
            {
                input = getInput(day, year, test);
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException) && !(e is DirectoryNotFoundException))
                    throw;

                if (test)
                {
                    Console.WriteLine($"no test input defined for {day:00}/{year}");
                    return;
                }

                if (!File.Exists("session"))
                    throw new Exception("session file not found");
                
                downloadSolution(File.ReadAllText("session"), day, year).Wait();
                input = getInput(day, year, test);
            }

            string output;
            if (!test)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                output = solution.Part1(input);
                sw.Stop();
                Console.WriteLine($"{day:00}/{year}/1: {output} completed in (very roughly) {sw.ElapsedMilliseconds}");

                sw = new Stopwatch();
                sw.Start();
                output = solution.Part2(input);
                sw.Stop();
                Console.WriteLine($"{day:00}/{year}/2: {output} completed in (very roughly) {sw.ElapsedMilliseconds}");
            }
            else
            {
                string[] split = input.Replace("\r", "").Split("\n\n\n\n\n");
                for (int i = 0; i < split.Length; i += 3)
                {
                    //Console.WriteLine($"input: {split[i]}");

                    string output1 = solution.Part1(split[i]);
                    string output2 = solution.Part2(split[i]);
                    Console.WriteLine(
                        $"{day:00}/{year}/1/test: {(output1 == split[i + 1] ? ("Successfully passed test input #" + (i / 3)) : ($"failed test input #{i / 3}, expected output: {split[i + 1]}"))} (output: {output1})");
                    Console.WriteLine(
                        $"{day:00}/{year}/2/test: {(output2 == split[i + 2] ? ("Successfully passed test input #" + (i / 3)) : ($"failed test input #{i / 3}, expected output: {split[i + 2]}"))} (output: {output2})");
                }
            }
        }

        private static Solution getSolution(byte day, ushort year)
        {
            IEnumerable<Type> solutions = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(Solution).IsAssignableFrom(t) && t != typeof(Solution));
            Type solutionType = solutions.FirstOrDefault(t =>
            {
                string[] splitted = t.Name.Split("Day", 2);
                ushort typeYear = ushort.Parse(splitted[0].Replace("Year", ""));
                byte typeDay = byte.Parse(splitted[1]);
                return typeYear == year && typeDay == day;
            });
            if (solutionType == null)
                throw new Exception("Solution for day" + day + ", year" + year + " not found.");
            return (Solution) Activator.CreateInstance(solutionType);
        }

        private static string getInput(byte day, ushort year, bool test)
        {
            return File.ReadAllText($"Input/{(test ? "test/" : "")}{year}/Day{day.ToString().PadLeft(2, '0')}.in");
        }

        private static async Task downloadSolution(string session, byte day, ushort year)
        {
            var cookieContainer = new CookieContainer();

            using var client = new HttpClient(
                new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                });
            cookieContainer.Add(new Uri("https://adventofcode.com"), new Cookie("session", session));

            var response = await client.GetAsync($"https://adventofcode.com/{year}/day/{day}/input");

            Directory.CreateDirectory($"Input/{year}");

            await File.WriteAllTextAsync($"Input/{year}/Day{day:00}.in", await response.Content.ReadAsStringAsync());
        }

    }
}
