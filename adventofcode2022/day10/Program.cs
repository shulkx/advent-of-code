// See https://aka.ms/new-console-template for more information

using System.Text;

// Parse the input program to create a list of instructions and their associated values
// E.g., 
// noop => ("noop", 0)
// addx 9 => ("addx", 9)
string input = File.ReadAllText("input.txt");
List<(string instruction, int value)> program = input.Split(Environment.NewLine)
                                                    .Select(line => line.Split(' '))
                                                    .Select(parts => parts.Length == 2 ? (parts[0], int.Parse(parts[1])) : (parts[0], 0))
                                                    .ToList();

int x = 1;          // Register X Value
int cycle = 1;      // Current cycle number

int sum = 0;        // [Puzzle 1]: Signal Strength Sum
int position = 1;   // [Puzzle 2]: Sprite Position

StringBuilder sbCRT = new StringBuilder();  // [Puzzle 2]: CRT display

List<int> targetCycles = new List<int>() { 20, 60, 100, 140, 180, 220 };

// Iterate over the instructions in the program and execute each instruction
foreach ((string instruction, int value) in program)
{
    // [Puzzle 2]: Draw CRT
    sbCRT.Append(position >= x && position <= x + 2 ? "#" : ".");

    if (cycle % 40 == 0)
    {
        sbCRT.Append(Environment.NewLine);
        position = 0;
    }

    if (instruction == "noop")
    {
        cycle++;        // Increment the cycle number
        position++;
    }
    else if (instruction == "addx")
    {

        cycle++;        // Increment the number for 1st cycle
        position++;

        // [Puzzle 2]: Draw CRT
        sbCRT.Append(position >= x && position <= x + 2 ? "#" : ".");

        if (cycle % 40 == 0)
        {
            sbCRT.Append(Environment.NewLine);
            position = 0;
        }

        // [Puzzle 1]: Check if the current cycle is one of the specified cycles (20, 60, 100, 140, 180, or 220)
        if (targetCycles.Any(x => cycle == x))
        {
            //Console.WriteLine($"At cycle {cycle}, X = {x}");
            sum += cycle * x;
        }

        x += value;     // Increment the number for 2nd cycle
        cycle++;
        position++;
    }

    // [Puzzle 1]: Check if the current cycle is one of the specified cycles (20, 60, 100, 140, 180, or 220)
    if (targetCycles.Any(x => cycle == x))
    {
        //Console.WriteLine($"At cycle {cycle}, X = {x}");
        sum += cycle * x;
    }
}

Console.WriteLine($"Day 10 Part 1: {sum}");
Console.WriteLine($"Day 10 Part 2:\n{sbCRT.ToString()}");