using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class DAY6
{
    static void Main()
    {
        string[] map = File.ReadAllLines("puzzleInput.txt");

        // Find the guard's starting position and initial direction
        int guardX = -1, guardY = -1;
        char direction = '^'; // Default direction (up)

        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '^' || map[y][x] == 'v' || map[y][x] == '<' || map[y][x] == '>')
                {
                    guardX = x;
                    guardY = y;
                    direction = map[y][x];
                    break;
                }
            }
            if (guardX != -1) break;
        }

        // Define movement directions (up, right, down, left)
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { -1, 0, 1, 0 };
        char[] dirChars = { '^', '>', 'v', '<' };

        int dirIndex = Array.IndexOf(dirChars, direction);

        // Count the number of valid obstruction positions
        int validObstructionCount = 0;

        // Iterate over all positions on the map
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                // Skip the guard's starting position and non-empty positions
                if ((x == guardX && y == guardY) || map[y][x] != '.')
                {
                    continue;
                }

                char[][] modifiedMap = map.Select(row => row.ToCharArray()).ToArray();
                modifiedMap[y][x] = '#';

                if (CausesLoop(modifiedMap, guardX, guardY, dirIndex, dx, dy, dirChars))
                {
                    validObstructionCount++;
                }
            }
        }

        // Output the number of valid obstruction positions
        Console.WriteLine($"Valid obstruction positions: {validObstructionCount}");
    }

    // Check if adding an obstruction causes the guard to enter a loop
    static bool CausesLoop(char[][] map, int startX, int startY, int startDirIndex, int[] dx, int[] dy, char[] dirChars)
    {
        int guardX = startX, guardY = startY;
        int dirIndex = startDirIndex;

        HashSet<(int, int, int)> visited = new HashSet<(int, int, int)>();

        while (true)
        {
            // Check if the guard has entered a loop
            if (visited.Contains((guardX, guardY, dirIndex)))
            {
                return true; // Loop detected
            }

            // Mark the current state as visited
            visited.Add((guardX, guardY, dirIndex));

            // Calculate the next position
            int nextX = guardX + dx[dirIndex];
            int nextY = guardY + dy[dirIndex];

            // Check if the next position is outside the map
            if (nextX < 0 || nextX >= map[0].Length || nextY < 0 || nextY >= map.Length)
            {
                return false; // Guard leaves the map
            }

            // Check if there is an obstacle in front
            if (map[nextY][nextX] == '#')
            {
                // Turn right 90 degrees
                dirIndex = (dirIndex + 1) % 4;
            }
            else
            {
                // Move forward
                guardX = nextX;
                guardY = nextY;
            }
        }
    }
}