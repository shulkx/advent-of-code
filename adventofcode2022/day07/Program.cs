// See https://aka.ms/new-console-template for more information

using (StreamReader sr = new StreamReader("input.txt"))
{
    string content = sr.ReadToEnd();

    string[] commands = content.Split(Environment.NewLine,StringSplitOptions.RemoveEmptyEntries);

    Dir rootDir = new Dir("root", "root");

    Dictionary<string, Dir> dirPathDict = new Dictionary<string, Dir>();
    dirPathDict.Add(rootDir.FullPath, rootDir);     // Add Root Folder

    for (int i = 1; i < commands.Length; i++)       // Start from 1 to skip root folder
    {
        var commandParts = commands[i].Split(' ');

        if (commands[i].StartsWith("$ cd ") && !commandParts.Last().Equals(".."))       // Move in one level, e.g., $ cd a
        {
            rootDir = dirPathDict[Path.Combine(rootDir.FullPath, commandParts.Last())];
        }
        else if (commands[i].StartsWith("$ cd ") && commandParts.Last().Equals(".."))   // moves out one level, e.g., $ cd ..
        {
            string? parentPath = Path.GetDirectoryName(rootDir.FullPath);

            if (!string.IsNullOrEmpty(parentPath))
            {
                rootDir = dirPathDict[parentPath];
            }
            else
            {
                throw new Exception("Unable to find parent Dir");
            }
            
        }
        else if (int.TryParse(commandParts[0], out int size))       // File, e.g., 123 abc
        {
            rootDir.Files.Add(new File(commandParts.Last(), size));
        }
        else if (commands[i].StartsWith("dir "))                    // Directory, e.g., dir xyz
        {
            string childDirName = commandParts.Last();
            string childDirFullPath = Path.Combine(rootDir.FullPath, childDirName);
            Dir childDir = new Dir(childDirName, childDirFullPath);
            dirPathDict.Add(childDirFullPath, childDir);
        }

    }

    // Puzzle 1
    Dictionary<string, int> dirSizeDict = new Dictionary<string, int>();
    foreach (var dirFullPath in dirPathDict.Keys)
    {
        int totalSize = dirPathDict.Where(x => x.Key.Contains(dirFullPath)).Sum(x => x.Value.GetFileSize());
        dirSizeDict.Add(dirFullPath, totalSize);
    }

    var totalMatchedSize = dirSizeDict.Values.Where(x => x <= 100000).Sum();

    // Puzzle 2
    int unusedSpace = 70000000 - dirSizeDict["root"];
    int freeupSpace = 30000000 - unusedSpace;

    string smallestPath = string.Empty;
    int smallestSpace = int.MaxValue;
    foreach (var dirFullPath in dirPathDict.Keys)
    {
        int diff = dirSizeDict[dirFullPath] - freeupSpace;
        
        if (diff > 0 && diff < smallestSpace) 
        {
            smallestSpace = diff;
            smallestPath = dirFullPath; 
        }

    }

    Console.WriteLine($"Day 07 Part 1: {totalMatchedSize}");
    Console.WriteLine($"Day 07 Part 2: {dirSizeDict[smallestPath]}");

}



public class Dir
{
    public string Name { get; set; }

    public string FullPath { get; set; }

    public List<File> Files { get; set; } = new List<File>();

    public Dir(string name, string fullPath)
    {
        Name = name;
        FullPath = fullPath;
    }

    public int GetFileSize()
    {
        return Files.Sum(x => x.Size);
    }
}

public class File
{
    public string Name { get; set; }
    public int Size { get; set; }

    public File(string name, int size)
    {
        Name = name;
        Size = size;
    }
}
