using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class DAY5
{
    static void Main()
    {
        // Read input file
        string[] inputLines = File.ReadAllLines("puzzleInput.txt");

        List<string> orderRules = new List<string>();
        List<List<int>> printUpdates = new List<List<int>>();
        bool readingUpdates = false;

        // Read and separate ordering rules from updates
        foreach (var line in inputLines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                readingUpdates = true;
                continue;
            }

            if (readingUpdates)
                printUpdates.Add(line.Split(',').Select(int.Parse).ToList());
            else
                orderRules.Add(line);
        }

        // Dictionary to store dependencies
        Dictionary<int, List<int>> dependencyMap = new Dictionary<int, List<int>>();

        foreach (var rule in orderRules)
        {
            var parts = rule.Split('|').Select(int.Parse).ToArray();
            int firstPage = parts[0], secondPage = parts[1];

            if (!dependencyMap.ContainsKey(firstPage))
                dependencyMap[firstPage] = new List<int>();

            dependencyMap[firstPage].Add(secondPage);
        }

        List<List<int>> fixedUpdates = new List<List<int>>();

        foreach (var update in printUpdates)
        {
            if (!IsOrderCorrect(update, dependencyMap))
            {
                fixedUpdates.Add(ReorderPages(update, dependencyMap));
            }
        }

        // Sum up the middle pages after fixing incorrect updates
        int totalMiddlePagesSum = fixedUpdates.Select(GetMiddlePage).Sum();
        Console.WriteLine($"Sum of middle pages after correction: {totalMiddlePagesSum}");
    }

    // Check if the update follows the ordering rules
    static bool IsOrderCorrect(List<int> pages, Dictionary<int, List<int>> dependencyMap)
    {
        HashSet<int> seenPages = new HashSet<int>();

        foreach (var page in pages)
        {
            seenPages.Add(page);

            if (dependencyMap.ContainsKey(page))
            {
                foreach (int mustComeAfter in dependencyMap[page])
                {
                    if (seenPages.Contains(mustComeAfter))
                        return false;
                }
            }
        }
        return true;
    }

    // Function to reorder pages correctly using Topological Sorting
    static List<int> ReorderPages(List<int> pages, Dictionary<int, List<int>> dependencyMap)
    {
        Dictionary<int, int> localDependencies = new Dictionary<int, int>();

        foreach (var page in pages)
        {
            localDependencies[page] = 0;
        }

        foreach (var page in pages)
        {
            if (dependencyMap.ContainsKey(page))
            {
                foreach (int dependentPage in dependencyMap[page])
                {
                    if (pages.Contains(dependentPage))
                    {
                        localDependencies[dependentPage]++;
                    }
                }
            }
        }

        // Pages with no dependencies are ready to be printed first
        Queue<int> readyToPrint = new Queue<int>();
        foreach (var page in pages)
        {
            if (localDependencies[page] == 0)
            {
                readyToPrint.Enqueue(page);
            }
        }

        List<int> sortedPages = new List<int>();

        while (readyToPrint.Count > 0)
        {
            int page = readyToPrint.Dequeue();
            sortedPages.Add(page);

            if (dependencyMap.ContainsKey(page))
            {
                foreach (int dependentPage in dependencyMap[page])
                {
                    if (pages.Contains(dependentPage))
                    {
                        localDependencies[dependentPage]--;
                        if (localDependencies[dependentPage] == 0)
                        {
                            readyToPrint.Enqueue(dependentPage);
                        }
                    }
                }
            }
        }

        return sortedPages;
    }

    // Get the middle page from an ordered list
    static int GetMiddlePage(List<int> update)
    {
        if (update.Count == 0) return 0;
        return update[update.Count / 2];
    }
}