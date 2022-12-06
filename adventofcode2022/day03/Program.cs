namespace day03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                int prioritiesSum1 = 0;
                int prioritiesSum2 = 0;
                string content = sr.ReadToEnd();
                var contentLines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                List<List<char>> threeLines = new List<List<char>>();
                foreach (var line in contentLines)
                {
                    // Puzzle 1
                    List<char> input = line.ToCharArray().ToList();
                    var input1 = input.Take(input.Count / 2);
                    var input2 = input.TakeLast(input.Count / 2);
                    var intersect = input1.Intersect(input2).First();
                    prioritiesSum1 += Letter2Priority(intersect);

                    // Puzzle 2
                    threeLines.Add(input);

                    if (threeLines.Count == 3)
                    {
                        var threeLinesIntersect = threeLines[0].Intersect(threeLines[1].Intersect(threeLines[2])).First();
                        prioritiesSum2 += Letter2Priority(threeLinesIntersect);
                        threeLines.Clear();
                    }

                }

                Console.WriteLine($"Day 03 Part 1: {prioritiesSum1}");
                Console.WriteLine($"Day 03 Part 2: {prioritiesSum2}");
            }
        }

        static int Letter2Priority(char c) => char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 1 + 26;
    }
}