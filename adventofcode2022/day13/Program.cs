// See https://aka.ms/new-console-template for more information
using System.Text.Json.Nodes;

// 1. JsonNode.Parse() is helpful for this puzzle
// 2. List.Sort(Comparison comparison) => public delegate int Comparison<in T>(T x, T y);

string[] pairs = File.ReadAllText("input.txt").Split($"{Environment.NewLine}{Environment.NewLine}");
int indicesSum = 0;

List<string> allPackages = new List<string>();
for (int i = 0; i < pairs.Length; i++)
{
    string[] packages = pairs[i].Split(Environment.NewLine);

    // Record packets for Puzzle 2
    allPackages.Add(packages[0]);
    allPackages.Add(packages[1]);

    if (IsInRightOrder(packages[0], packages[1]))
    {
        indicesSum += (i + 1);
    }
}

Console.WriteLine($"Day 13 Part 1: {indicesSum}");

// Puzzle 2
// Add two additional divider packets
var divider0 = JsonNode.Parse("[[2]]");
var divider1 = JsonNode.Parse("[[6]]");

var allJsonNodes = allPackages.Select(x => JsonNode.Parse(x)).ToList();
allJsonNodes.Add(divider0);
allJsonNodes.Add(divider1);

allJsonNodes.Sort(Compare);

Console.WriteLine($"Day 13 Part 2: {(allJsonNodes.IndexOf(divider0) + 1) * (allJsonNodes.IndexOf(divider1) + 1)}");


bool IsInRightOrder(string left, string right)
{
    // Parse the left and right packets
    JsonNode? leftNode = JsonNode.Parse(left);
    JsonNode? rightNode = JsonNode.Parse(right);

    return Compare(leftNode, rightNode) < 0; 
}

int Compare(JsonNode? left, JsonNode? right)
{
    if (left is JsonValue && right is JsonValue)
    {
        return (int)left - (int)right;

    }
    else
    {
        var leftArray = left as JsonArray ?? new JsonArray((int?)left);
        var rightArray = right as JsonArray ?? new JsonArray((int?)right);

        var zipLeftRight = Enumerable.Zip(leftArray, rightArray);
        var compareResults = zipLeftRight.Select(a => Compare(a.First, a.Second)).Where(x => x != 0);

        if (!compareResults.Any())
        {
            return leftArray.Count - rightArray.Count;
        }
        else
        {
            return compareResults.First();
        }
    }
}