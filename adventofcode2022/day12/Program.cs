// See https://aka.ms/new-console-template for more information
string input = File.ReadAllText("input.txt");

// Parse the input to create a grid of squares.
(char[][] grid, (int row, int col) startLoc, (int row, int col) goalLoc, List<(int row, int col)> aLocs) = ParseInput(input);

// Store the number of rows and columns in the grid.
int rows = grid.Length;
int cols = grid[0].Length;

List<int> steps = new List<int>();

// Insert the 'S' as the first possible start point
aLocs.Insert(0, startLoc);

foreach (var aLocation in aLocs)
{
    Queue<(int row, int col)> queue = new Queue<(int row, int col)>();

    // Keep track of the squares that have been visited.
    bool[,] visited = new bool[rows, cols];

    // Keep track of the shortest path to each square.
    int[,] pathLengths = new int[rows, cols];

    // Start by adding the starting position to the queue.
    queue.Enqueue((aLocation.row, aLocation.col));

    while (queue.Count > 0)
    {
        (int row, int col) = queue.Dequeue();

        // If the square is the goal, record the number of steps required to reach it.
        if (row == goalLoc.row && col == goalLoc.col)
        {
            steps.Add(pathLengths[row, col]);
            break;
        }

        // If the square has not been visited, add its valid neighbors to the queue.
        if (!visited[row, col])
        {
            visited[row, col] = true;

            // Add the valid neighbors of the square to the queue.
            foreach (var neighbor in GetValidNeighbors(grid, row, col))
            {
                queue.Enqueue(neighbor);
                pathLengths[neighbor.row, neighbor.col] = pathLengths[row, col] + 1;
            }
        }
    }
}

Console.WriteLine(steps.First());
Console.WriteLine(steps.Min());


// Parses the input and returns a grid of squares.
static (char[][] grid, (int row, int col) startLoc, (int row, int col) goalLoc, List<(int, int)> aLocs) ParseInput(string input)
{
    // Split the input into lines.
    string[] lines = input.Split(Environment.NewLine);

    // Create a grid of squares.
    char[][] grid = new char[lines.Length][];

    (int row, int col) startLoc = (0, 0);
    (int row, int col) goalLoc = (0, 0);

    List<(int, int)> aLocations = new List<(int, int)>();

    for (int i = 0; i < lines.Length; i++)
    {
        // Store the characters in each line as a row in the grid.
        grid[i] = lines[i].ToCharArray();

        for (int j = 0; j < grid[i].Length; j++)
        {
            // Store the starting, goal positions and 'a' positions
            if (grid[i][j] == 'S')
            {
                grid[i][j] = 'a';       // Change the start position evlation to 'a'
                startLoc.row = i;
                startLoc.col = j;
            }
            else if (grid[i][j] == 'E')
            {
                grid[i][j] = 'z';       // Change the goal position evlation to 'z'
                goalLoc.row = i;
                goalLoc.col = j;
            }
            else if (grid[i][j] == 'a')
            {
                aLocations.Add((i, j));
            }
        }
    }

    return (grid, startLoc, goalLoc, aLocations);
}

// Returns the valid neighbors of the square at the given position.
static IEnumerable<(int row, int col)> GetValidNeighbors(char[][] grid, int row, int col)
{
    // Store the number of rows and columns in the grid.
    int rows = grid.Length, cols = grid[0].Length;

    // Check the four adjacent squares and return the ones that are valid.
    if (row > 0 && grid[row - 1][col] - grid[row][col] <= 1)
    {
        yield return (row - 1, col);
    }
    if (row < rows - 1 && grid[row + 1][col] - grid[row][col] <= 1)
    {
        yield return (row + 1, col);
    }
    if (col > 0 && grid[row][col - 1] - grid[row][col] <= 1)
    {
        yield return (row, col - 1);
    }
    if (col < cols - 1 && grid[row][col + 1] - grid[row][col] <= 1)
    {
        yield return (row, col + 1);
    }
}
