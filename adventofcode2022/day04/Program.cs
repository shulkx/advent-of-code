// See https://aka.ms/new-console-template for more information
using System.Drawing;

List<(Point first, Point second)> pairs = new List<(Point first, Point second)>();
using (StreamReader sr = new("input.txt"))
{
    string content = sr.ReadToEnd();
    string[] lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    foreach (var line in lines)
    {
        string[] elements = line.Split('-', ',');
        
        if (elements.Length != 4)
        {
            throw new Exception("Input Error.");
        }

        pairs.Add((PointGenerator(elements[0], elements[1]), PointGenerator(elements[2], elements[3])));
    }

    int puzzle1 = pairs.Select(x => IsFullyContained(x.first, x.second)).Sum();
    int puzzle2 = pairs.Select(x => IsOverlapped(x.first, x.second)).Sum();

    Console.WriteLine($"Day 04 Part 1: {puzzle1}");
    Console.WriteLine($"Day 04 Part 2: {puzzle2}");

}

static int IsFullyContained(Point first, Point second) 
    => (first.X >= second.X && first.Y <= second.Y) || (first.X <= second.X && first.Y >= second.Y) ? 1 : 0;

static int IsOverlapped(Point first, Point second)
    => first.X > second.Y || first.Y < second.X ? 0 : 1;

static Point PointGenerator(string element1, string element2) => new(int.Parse(element1), int.Parse(element2));