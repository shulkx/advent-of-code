namespace day01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                List<int> clories = new List<int>();
                int sum = 0;
                while (!sr.EndOfStream)
                {
                    string? line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        clories.Add(sum);
                        sum = 0;
                    }
                    else
                    {
                        sum += int.Parse(line);
                    }
                }

                var ordered = clories.OrderByDescending(x => x).ToList();
                Console.WriteLine($"Day 01 Part 1: {ordered.Max()}");
                Console.WriteLine($"Day 01 Part 2: {ordered[0] + ordered[1] + ordered[2]}");
            }
        }
    }
}