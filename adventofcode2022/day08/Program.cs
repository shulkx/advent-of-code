// See https://aka.ms/new-console-template for more information
using (StreamReader sr = new StreamReader("input.txt"))
{
    int[][] grid = File.ReadAllLines("input.txt").Select(line => line.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();

    List<int> scenicScores = new List<int>();
    // Loop through each tree in the grid
    int visibleTreeCount = 0;
    for (int row = 0; row < grid.Length; row++)
    {
        for (int col = 0; col < grid[row].Length; col++)
        {
            // Check if the current tree is visible and get scenic score
            (bool isTreeVisible, int scenicScore) = GetTreeVisibleAndScenicScore(grid, row, col);
            
            if (isTreeVisible)
            {
                visibleTreeCount++;
            }

            scenicScores.Add(scenicScore);
        }
    }

    // Print the result
    Console.WriteLine($"Day 08 Part 1: {visibleTreeCount}");
    Console.WriteLine($"Day 08 Part 2: {scenicScores.Max()}");
}

static (bool, int) GetTreeVisibleAndScenicScore(int[][] grid, int row, int col)
{
    // Get the height of the current tree
    int height = grid[row][col];

    // 4 visibility variables for 4 directions
    bool isUpVisible = true;
    bool isDownVisible = true;
    bool isLeftVisible = true;
    bool isRightVisible = true;

    //Up direction
    int upCount = 0;
    for (int i = row - 1; i >= 0; i--)
    {
        upCount++;
        
        // Stop if we reach a tree that is the same height or taller
        if (grid[i][col] >= height)
        {
            isUpVisible = false;
            break;
        }
    }

    // Down direction
    int downCount = 0;
    for (int i = row + 1; i < grid.Length; i++)
    {
        downCount++;

        // Stop if we reach a tree that is the same height or taller
        if (grid[i][col] >= height)
        {
            isDownVisible = false;
            break;
        }
    }

    // Left direction
    int leftCount = 0;
    for (int i = col - 1; i >= 0; i--)
    {
        leftCount++;

        if (grid[row][i] >= height)
        {
            isLeftVisible = false;
            break;
        }
    }

    // Right direction
    int rightCount = 0;
    for (int i = col + 1; i < grid[row].Length; i++)
    {
        rightCount++;
        
        if (grid[row][i] >= height)
        {
            isRightVisible = false;
            break;
        }
    }

    // Return if current tree is visible and ScenicScore
    bool isTreeVisible = isUpVisible || isDownVisible || isLeftVisible || isRightVisible;
    int scenicScore = upCount * downCount * leftCount *  rightCount;

    return (isTreeVisible, scenicScore);
}
