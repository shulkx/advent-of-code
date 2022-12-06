// See https://aka.ms/new-console-template for more information
using System.Text;

using (StreamReader sr = new StreamReader("input.txt"))
{
    string[] sections = sr.ReadToEnd().Split($"{Environment.NewLine}{Environment.NewLine}");

    Dictionary<int, Stack<string>> stackDict1 = new Dictionary<int, Stack<string>>();
    Dictionary<int, Stack<string>> stackDict2 = new Dictionary<int, Stack<string>>();

    string[] stackLines = sections[0].Split(Environment.NewLine);

    // Get Stach Total Number
    var stackNum = int.Parse(stackLines.Last().ToCharArray().Last(x => !char.IsWhiteSpace(x)).ToString());

    for (int i = stackLines.Length - 2; i >= 0 ; i--)
    {
        for (int j = 1; j <= stackNum; j++)
        {
            string cargo = stackLines[i].Substring((j - 1) * 4, 3);

            if (!cargo.All(x => char.IsWhiteSpace(x)))
            {
                string cargoLetter = cargo.Split('[', ']')[1];

                if (!stackDict1.ContainsKey(j))
                {
                    stackDict1.Add(j, new Stack<string>());
                    stackDict2.Add(j, new Stack<string>());
                }

                stackDict1[j].Push(cargoLetter);
                stackDict2[j].Push(cargoLetter);
            }
        }
    }

    
    // Instructions
    string[] instructionLines = sections[1].Split(Environment.NewLine);
    foreach (var instructionLine in instructionLines)
    {
        string[] delimiters = { "move ", " from ", " to " };
        string[] orderNums = instructionLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        int moveNum = int.Parse(orderNums[0]);
        int from = int.Parse(orderNums[1]);
        int to = int.Parse(orderNums[2]);

        // Intermediate Stack to convert first in first out => first in last out
        Stack<string> moveCargos = new Stack<string>();

        for (int i = 0; i < moveNum; i++)
        {
            stackDict1[to].Push(stackDict1[from].Pop());

            moveCargos.Push(stackDict2[from].Pop());
        }

        while (moveCargos.Count != 0)
        {
            stackDict2[to].Push(moveCargos.Pop());
        }
    }

    StringBuilder sb1 = new StringBuilder();
    StringBuilder sb2 = new StringBuilder();

    for (int i = 1; i <= stackNum; i++)
    {
        sb1.Append(stackDict1[i].Peek());
        sb2.Append(stackDict2[i].Peek());
    }

    Console.WriteLine($"Day 05 Part 1: {sb1}");
    Console.WriteLine($"Day 05 Part 2: {sb2}");
}
