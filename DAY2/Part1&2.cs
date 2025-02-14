using DAY2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day02
{
    class Part2
    {
        static void Main()
        {
            string inputData = PuzzleInput.Data;


            List<List<int>> reports = inputData.Trim()
                                               .Split('\n')
                                               .Select(line => line.Trim()
                                                                   .Split()
                                                                   .Select(int.Parse)
                                                                   .ToList())
                                               .ToList();

            // Count the number of safe reports Part 2
            int safeCount = reports.Count(IsSafeWithDampener);

            
            Console.WriteLine($"Safe Reports Count with Dampener: {safeCount}");
        }

     
        static bool IsSafe(List<int> report)
        {
            bool increasing = true, decreasing = true;

            for (int i = 1; i < report.Count; i++)
            {
                int diff = report[i] - report[i - 1];

                if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
                    return false;

                if (diff > 0) decreasing = false;
                if (diff < 0) increasing = false;
            }

            return increasing || decreasing;
        }

        // Function to check if a report can be safe with one level removed
        static bool IsSafeWithDampener(List<int> report)
        {
            // If it's already safe, return true
            if (IsSafe(report)) return true;

            // Try removing each element and check if it becomes safe
            for (int i = 0; i < report.Count; i++)
            {
                List<int> modifiedReport = new List<int>(report);
                modifiedReport.RemoveAt(i);

                if (IsSafe(modifiedReport))
                    return true;
            }

            return false;
        }
    }
}