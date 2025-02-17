using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class DAY8
{
    static void Main()
    {
        string[] map = File.ReadAllLines("puzzleInput.txt");

        // Find the positions of all antennas and their frequencies
        List<(int x, int y, char frequency)> antennas = new List<(int, int, char)>();
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] != '.')
                {
                    antennas.Add((x, y, map[y][x]));
                }
            }
        }

        // Track unique antinode locations
        HashSet<(int x, int y)> antinodes = new HashSet<(int, int)>();

        // Iterate over all grid positions
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (IsAntinode(x, y, antennas))
                {
                    antinodes.Add((x, y));
                }
            }
        }

        Console.WriteLine($"Unique antinode locations: {antinodes.Count}");
    }

    // Check if a position is an antinode
    static bool IsAntinode(int x, int y, List<(int x, int y, char frequency)> antennas)
    {
        // Group antennas by frequency
        var frequencyGroups = antennas.GroupBy(a => a.frequency);

        foreach (var group in frequencyGroups)
        {
            // Get all antennas of this frequency
            var sameFrequencyAntennas = group.ToList();

            // Check if this position is in line with at least two antennas of the same frequency
            if (sameFrequencyAntennas.Count >= 2)
            {
                for (int i = 0; i < sameFrequencyAntennas.Count; i++)
                {
                    for (int j = i + 1; j < sameFrequencyAntennas.Count; j++)
                    {
                        if (AreCollinear(x, y, sameFrequencyAntennas[i], sameFrequencyAntennas[j]))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    // Check if a point (x, y) is collinear with two antennas
    static bool AreCollinear(int x, int y, (int x, int y, char frequency) a, (int x, int y, char frequency) b)
    {
        int crossProduct = (a.x - x) * (b.y - y) - (a.y - y) * (b.x - x);
        return crossProduct == 0;
    }
}