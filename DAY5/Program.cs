using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string[] inputLines = File.ReadAllLines("puzzleInput.txt");

        List<string> orderingRules = new List<string>();
        List<List<int>> updatesToPrint = new List<List<int>>();
        bool isReadingUpdates = false;

        // Read input and split into rules and updates
        foreach (var line in inputLines)
        {
            if (string.IsNullOrWhiteSpace(line)) // Blank line separates rules from updates
            {
                isReadingUpdates = true;
                continue;
            }

            if (isReadingUpdates)
                updatesToPrint.Add(line.Split(',').Select(int.Parse).ToList());
            else
                orderingRules.Add(line);
        }

        // Build a dependency graph from ordering rules
        Dictionary<int, List<int>> orderGraph = new Dictionary<int, List<int>>();
        Dictionary<int, int> dependencyCount = new Dictionary<int, int>();

        foreach (var rule in orderingRules)
        {
            var parts = rule.Split('|').Select(int.Parse).ToArray();
            int beforePage = parts[0], afterPage = parts[1];

            if (!orderGraph.ContainsKey(beforePage))
                orderGraph[beforePage] = new List<int>();
            if (!dependencyCount.ContainsKey(beforePage))
                dependencyCount[beforePage] = 0;
            if (!dependencyCount.ContainsKey(afterPage))
                dependencyCount[afterPage] = 0;

            orderGraph[beforePage].Add(afterPage);
            dependencyCount[afterPage]++;
        }

        // Identify incorrect updates and fix them
        List<List<int>> incorrectUpdates = new List<List<int>>();
        List<List<int>> correctedUpdates = new List<List<int>>();

        foreach (var update in updatesToPrint)
        {
            if (IsOrderCorrect(update, orderGraph))
            {
                // If update is already correct, no need to fix it
                continue;
            }
            else
            {
                // Reorder the incorrect update properly
                correctedUpdates.Add(ReorderPages(update, orderGraph, dependencyCount));
            }
        }

        // Calculate the sum of the middle pages from the corrected updates
        int sumOfMiddlePages = correctedUpdates.Select(FindMiddlePage).Sum();

        Console.WriteLine($"Sum of middle pages after correction: {sumOfMiddlePages}");
    }

    // Check if the update follows the given order constraints
    static bool IsOrderCorrect(List<int> pages, Dictionary<int, List<int>> orderGraph)
    {
        HashSet<int> pagesSeen = new HashSet<int>();

        foreach (var page in pages)
        {
            pagesSeen.Add(page);

            if (orderGraph.ContainsKey(page))
            {
                foreach (int mustComeAfter in orderGraph[page])
                {
                    if (pagesSeen.Contains(mustComeAfter))
                        return false; 
                }
            }
        }
        return true;
    }

    // Function to reorder pages correctly using topological sorting
    static List<int> ReorderPages(List<int> pages, Dictionary<int, List<int>> orderGraph, Dictionary<int, int> dependencyCount)
    {
        // Create a fresh dependency count only for pages in the update
        Dictionary<int, int> localDependencyCount = new Dictionary<int, int>();
        foreach (var page in pages)
            if (dependencyCount.ContainsKey(page))
                localDependencyCount[page] = dependencyCount[page];

        Queue<int> readyToPrint = new Queue<int>();
        foreach (var page in pages)
            if (!localDependencyCount.ContainsKey(page) || localDependencyCount[page] == 0)
                readyToPrint.Enqueue(page);

        List<int> orderedPages = new List<int>();

        while (readyToPrint.Count > 0)
        {
            int page = readyToPrint.Dequeue();
            orderedPages.Add(page);

            if (orderGraph.ContainsKey(page))
            {
                foreach (int dependentPage in orderGraph[page])
                {
                    if (localDependencyCount.ContainsKey(dependentPage))
                    {
                        localDependencyCount[dependentPage]--;
                        if (localDependencyCount[dependentPage] == 0)
                            readyToPrint.Enqueue(dependentPage);
                    }
                }
            }
        }

        return orderedPages;
    }

}
