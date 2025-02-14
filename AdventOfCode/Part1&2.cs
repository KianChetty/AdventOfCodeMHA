using DAY1;
using System;
using System.Collections.Generic;
using System.Linq; 

class Program
{
    static void Main()
    {
        
        string inputData = PuzzleInput.Data;

        // Split input into lines and process each line
        List<int> leftList = new List<int>();
        List<int> rightList = new List<int>();

        string[] lines = inputData.Trim().Split('\n');
        foreach (var line in lines)
        {
            string[] parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                leftList.Add(int.Parse(parts[0]));
                rightList.Add(int.Parse(parts[1]));
            }
        }

        // Sort both lists
        leftList.Sort();
        rightList.Sort();

        // Compute total distance
        int totalDistance = leftList.Zip(rightList, (left, right) => Math.Abs(left - right)).Sum();

        // Create a dictionary to count occurrences of numbers in the right list
        Dictionary<int, int> rightCounts = new Dictionary<int, int>();

        foreach (int num in rightList)
        {
            if (rightCounts.ContainsKey(num))
                rightCounts[num]++;
            else
                rightCounts[num] = 1;
        }

        // Compute similarity score
        int similarityScore = 0;
        foreach (int num in leftList)
        {
            if (rightCounts.ContainsKey(num))
            {
                similarityScore += num * rightCounts[num];
            }
        }

        // Output the result
        Console.WriteLine($"Similarity Score: {similarityScore}");

        // Output result
        Console.WriteLine($"Total Distance: {totalDistance}");
    }
}
