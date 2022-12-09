// Print the result
Console.WriteLine($"Day 09 Part 1: {GetTailVisitedPositions(2)}");
Console.WriteLine($"Day 09 Part 2: {GetTailVisitedPositions(10)}");


static int GetTailVisitedPositions (int knotCnt)
{
    int[] knotsX = new int[knotCnt];
    int[] knotsY = new int[knotCnt];

    var visited = new HashSet<(int, int)>();
    string[] instructions = File.ReadAllLines("input.txt");
    
    foreach (var instruction in instructions)
    {
        string[] input = instruction.Split();
        string direction = input[0];
        int steps = int.Parse(input[1]);

        for (int i = 1; i <= steps; i++)
        {
            // Update the position of the head based on the motion
            switch (direction)
            {
                case "U":
                    knotsY[0] += 1;
                    break;
                case "D":
                    knotsY[0] -= 1;
                    break;
                case "L":
                    knotsX[0] -= 1;
                    break;
                case "R":
                    knotsX[0] += 1;
                    break;
            }

            for (int j = 1; j < knotCnt; j++)
            {

                // If the head and tail are not in the same row and column and are not touching,
                // update the position of the tail to be one step diagonally behind the head
                if ((Math.Abs(knotsX[j - 1] - knotsX[j]) > 1) && (Math.Abs(knotsY[j - 1] - knotsY[j]) > 1))
                {
                    knotsX[j] = knotsX[j - 1] - Math.Sign(knotsX[j - 1] - knotsX[j]);
                    knotsY[j] = knotsY[j - 1] - Math.Sign(knotsY[j - 1] - knotsY[j]);
                }
                // If the head is more than one step away from the tail in the x or y direction,
                // update the position of the tail to be one step behind the head in the same direction
                // Besides, make sure the other direction has the same value (in the same row or column)
                else if (Math.Abs(knotsX[j - 1] - knotsX[j]) > 1)
                {
                    knotsX[j] = knotsX[j - 1] - Math.Sign(knotsX[j - 1] - knotsX[j]);

                    if (knotsY[j] != knotsY[j - 1])
                    {
                        knotsY[j] = knotsY[j - 1];
                    }
                }
                else if (Math.Abs(knotsY[j - 1] - knotsY[j]) > 1)
                {
                    knotsY[j] = knotsY[j - 1] - Math.Sign(knotsY[j - 1] - knotsY[j]);

                    if (knotsX[j] != knotsX[j - 1])
                    {
                        knotsX[j] = knotsX[j - 1];
                    }
                }
            }

            visited.Add((knotsX[knotCnt - 1], knotsY[knotCnt - 1]));
        }
    }

    return visited.Count;
}