using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day04 : Solution
    {
        public override string Part1(string input)
        {
            string[] entries = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            if (entries.Length == 1)
                //unix line endings
                entries = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

            string[][] includedFields = entries
                .Select(entry => entry.Split(new[] {' ', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries))
                .Select(fieldValues => fieldValues.Select(fieldValue => fieldValue.Split(':')[0]).ToArray()).ToArray();

            int validCount = 0;

            foreach (string[] fields in includedFields)
            {
                if (fields.Length > 8 || fields.Length < 7)
                    continue;
                
                int numFields = 0;

                foreach (string field in fields)
                {
                    if (field == "cid")
                        continue;

                    numFields += field switch
                    {
                        "byr" => 1,
                        "iyr" => 1,
                        "eyr" => 1,
                        "hgt" => 1,
                        "hcl" => 1,
                        "ecl" => 1,
                        "pid" => 1,
                        _ => -20
                    };
                }

                if (numFields == 7)
                    validCount++;
            }


            return (validCount).ToString();
        }

        public override string Part2(string input)
        {
            string[] entries = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            if (entries.Length == 1)
                //unix line endings
                entries = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

            (string, string)[][] includedFields = entries
                .Select(entry => entry.Split(new[] {' ', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries))
                .Select(fieldValues =>
                    fieldValues.Select(fieldValue => (fieldValue.Split(':')[0], fieldValue.Split(':')[1])).ToArray())
                .ToArray();

            int validCount = 0;

            foreach ((string, string)[] fields in includedFields)
            {
                if (fields.Length > 8 || fields.Length < 7)
                    continue;
                
                int numFields = 0;
                foreach ((string key, string val) in fields)
                {
                    if (key == "cid")
                        continue;
                    if (key == "byr" && (int.Parse(val) < 1920 || int.Parse(val) > 2002)) break;
                    if (key == "iyr" && (int.Parse(val) < 2010 || int.Parse(val) > 2020)) break;
                    if (key == "eyr" && (int.Parse(val) < 2020 || int.Parse(val) > 2030)) break;
                    if (key == "hgt")
                    {
                        bool goodUnits = val.Substring(val.Length - 2) == "cm";
                        int numberPart = int.Parse(val.Substring(0, val.Length - 2));
                        if (goodUnits && (numberPart < 150 || numberPart > 193) ||
                            !goodUnits && (numberPart < 59 || numberPart > 76))
                            break;
                    }

                    if (key == "hcl" && (val[0] != '#' ||
                                              val.Substring(1).Any(c => (c < '0' || c > '9') && (c < 'a' || c > 'f'))))
                        break;
                    if (key == "ecl" && !new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(val))
                        break;
                    if (key == "pid" && (val.Length != 9 || val.Any(c => c < '0' || c > '9')))
                        break;

                    numFields++;
                }

                if (numFields == 7)
                    validCount++;
            }


            return (validCount).ToString();
        }
    }
}
