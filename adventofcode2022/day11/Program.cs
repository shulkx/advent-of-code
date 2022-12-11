//// See https://aka.ms/new-console-template for more information

string input = File.ReadAllText("input.txt");
string[] sections = input.Split($"{Environment.NewLine}{Environment.NewLine}");

// Puzzle 1
List<Monkey> monkeys = sections.Select(x => ParseMonkey(x)).ToList();
Console.WriteLine($"Day 11 Part 1: {Run(monkeys, 20, val => val / 3)}");

// Puzzle 2
monkeys = sections.Select(x => ParseMonkey(x)).ToList();
long commonMultiple = monkeys.Aggregate(1, (val, monkey) => val * monkey.Test);
Console.WriteLine($"Day 11 Part 2: {Run(monkeys, 10000, val => val % commonMultiple)}");

static long Run(List<Monkey> monkeys, int roundCnt, Func<long, long> worryLevelsManager)
{
    for (int round = 0; round < roundCnt; round++)
    {
        for (int i = 0; i < monkeys.Count; i++)
        {
            // Get the current monkey
            Monkey monkey = monkeys[i];

            // Iterate through the items in the monkey's list
            for (int j = 0; j < monkey.StartingItems.Count; j++)
            {
                // Get the current item
                long item = monkey.StartingItems[j];
                monkey.InspectTimes++;

                // Apply the operation to the item value to get a new item value
                long newItem = monkey.Operation(item);

                long finalItem = worryLevelsManager(newItem);

                // Test whether the new item value is divisible by the test value
                if (finalItem % monkey.Test == 0)
                {
                    // If true, throw the item to the monkey specified in the "If true" field
                    monkeys[monkey.IfTrue].StartingItems.Add(finalItem);
                }
                else
                {
                    // If false, throw the item to the monkey specified in the "If false" field
                    monkeys[monkey.IfFalse].StartingItems.Add(finalItem);
                }
            }

            // Clear the current monkey's list of items
            monkey.StartingItems.Clear();
        }
    }

    //monkeys.ForEach(x => Console.WriteLine(x.InspectTimes));

    var orderedMonkeys = monkeys.OrderByDescending(x => x.InspectTimes).ToList();
    return orderedMonkeys[0].InspectTimes * orderedMonkeys[1].InspectTimes;
}



static Monkey ParseMonkey(string section)
{
    string[] lines = section.Split($"{Environment.NewLine}");
    Monkey monkey = new Monkey();
    
    foreach (var line in lines)
    {
        string cleanLine = line.Trim();

        if (cleanLine.StartsWith("Monkey"))
        {
            monkey.Id = int.Parse(cleanLine.Split(' ', ':')[1]);
        }
        else if (cleanLine.StartsWith("Starting items"))
        {
            monkey.StartingItems = cleanLine.Split(':')[1].Split(',').Select(x => long.Parse(x.Trim())).ToList();
        }
        else if (cleanLine.StartsWith("Operation"))
        {
            string[] delimiters = { "Operation:", " " };
            string[] operationVars = cleanLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            if (int.TryParse(operationVars.Last(), out int num))
            {
                if (operationVars[operationVars.Length - 2] == "+")
                {
                    monkey.Operation = val => val + num;
                }
                else if (operationVars[operationVars.Length - 2] == "*")
                {
                    monkey.Operation = val => val * num;
                }
            }
            else
            {
                if (operationVars[operationVars.Length - 2] == "+")
                {
                    monkey.Operation = val => val + val;
                }
                else if (operationVars[operationVars.Length - 2] == "*")
                {
                    monkey.Operation = val => val * val;
                }
            }

            if (monkey.Operation == null)
            {
                throw new Exception("Input Operation cannot be analyzed.");
            }
        }
        else if (cleanLine.StartsWith("Test"))
        {
            monkey.Test = int.Parse(cleanLine.Split(' ').Last());
        }
        else if (cleanLine.StartsWith("If true"))
        {
            monkey.IfTrue = int.Parse(cleanLine.Split(' ').Last());
        }
        else if (cleanLine.StartsWith("If false"))
        {
            monkey.IfFalse = int.Parse(cleanLine.Split(' ').Last());
        }
    }

    return monkey;
}

class Monkey
{
    public int Id { get; set; }
    public List<long> StartingItems { get; set; } = new List<long>();
    public Func<long, long> Operation { get; set; }
    public int Test { get; set; }
    public int IfTrue { get; set; }
    public int IfFalse { get; set; }

    public long InspectTimes { get; set; } = 0;
}