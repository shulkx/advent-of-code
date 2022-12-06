// See https://aka.ms/new-console-template for more information
using (StreamReader sr = new StreamReader("input.txt"))
{
    string dataStream = sr.ReadToEnd();
    Console.WriteLine($"Day 06 Part 1: {GetFirstMarker(dataStream, 4)}");
    Console.WriteLine($"Day 06 Part 2: {GetFirstMarker(dataStream, 14)}");
}

static int GetFirstMarker(string dataStream, int distinctCharCnt)
{
    for (int i = 0; i < dataStream.Length - distinctCharCnt + 1; i++)
    {
        string data = dataStream.Substring(i, distinctCharCnt);
        var groups = data.GroupBy(c => c).Where(g => g.Count() > 1);
        
        if (!groups.Any())
        {
            return i + distinctCharCnt;
        }
    }

    throw new Exception("Cannot find answer.");
}
