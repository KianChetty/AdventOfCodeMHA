using System;
using System.Text.RegularExpressions;

namespace DAY3
{
    class Part1
    {
        static void Main()
        {
            string corruptedMemory = PuzzleInput.Data;

            string mulPattern = @"mul\((\d{1,3}),(\d{1,3})\)";
            string doPattern = @"do\(\)";
            string dontPattern = @"don't\(\)";

            // Find all occurrences of mul(X,Y), do(), and don't()
            MatchCollection mulMatches = Regex.Matches(corruptedMemory, mulPattern);
            MatchCollection doMatches = Regex.Matches(corruptedMemory, doPattern);
            MatchCollection dontMatches = Regex.Matches(corruptedMemory, dontPattern);

            bool isEnabled = true;
            int sum = 0;
            int lastIndex = 0;

            foreach (Match match in Regex.Matches(corruptedMemory, $"{mulPattern}|{doPattern}|{dontPattern}"))
            {
                // Update enable/disable state based on do() / don't()
                if (Regex.IsMatch(match.Value, doPattern))
                {
                    isEnabled = true;
                }
                else if (Regex.IsMatch(match.Value, dontPattern))
                {
                    isEnabled = false;
                }
                else if (isEnabled) 
                {
                    int x = int.Parse(match.Groups[1].Value);
                    int y = int.Parse(match.Groups[2].Value);
                    sum += x * y;
                }
            }

            // Output the final sum
            Console.WriteLine($"Total sum of valid multiplications with conditions: {sum}");
        }
    }
}

