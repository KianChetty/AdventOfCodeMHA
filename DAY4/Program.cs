using DAY4;
using System;

namespace DAY4
{
    class Program
    {
        static void Main()
        {
            string[] wordSearch = PuzzleInput.Data[0].Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            int rows = wordSearch.Length;
            int cols = wordSearch[0].Length;
            char[,] grid = new char[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    grid[i, j] = wordSearch[i][j];

            // Find occurrences of X-MAS pattern
            int count = CountXMASPatterns(grid);
            Console.WriteLine($"Total occurrences of 'X-MAS': {count}");
        }

        static int CountXMASPatterns(char[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            int count = 0;

            // Loop through each cell in the grid (excluding edges to avoid out-of-bounds)
            for (int r = 1; r < rows - 1; r++)
            {
                for (int c = 1; c < cols - 1; c++)
                {
                    if (IsXMAS(grid, r, c))
                        count++;
                }
            }

            return count;
        }

        // Check if an X-MAS pattern exists at a given position
        static bool IsXMAS(char[,] grid, int r, int c)
        {
            if (grid[r, c] != 'A')
                return false; // Center must be 'A'

            // Get diagonal positions
            char topLeft = grid[r - 1, c - 1];
            char topRight = grid[r - 1, c + 1];
            char bottomLeft = grid[r + 1, c - 1];
            char bottomRight = grid[r + 1, c + 1];

            // Check all valid X-MAS configurations
            return (topLeft == 'M' && topRight == 'S' && bottomLeft == 'M' && bottomRight == 'S') ||
                   (topLeft == 'S' && topRight == 'M' && bottomLeft == 'S' && bottomRight == 'M') ||
                   (topLeft == 'M' && topRight == 'M' && bottomLeft == 'S' && bottomRight == 'S') ||
                   (topLeft == 'S' && topRight == 'S' && bottomLeft == 'M' && bottomRight == 'M');
        }
    }
}
