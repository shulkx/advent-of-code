namespace day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<(int oppo, int num)> inputs = new List<(int oppo, int num)>();
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string? line = sr.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        char[] chars = line.ToCharArray();
                        inputs.Add((Letter2Num(chars[0]), Letter2Num(chars[2])));
                    }
                }
            }

            // Puzzle 1
            // Lose: 0, Even: 3, Win: 6
            int puzzle1Score = inputs.Select(x => x.num + JudgeResult(x.oppo, x.num)).Sum();
            Console.WriteLine($"Day 02 Part 1: {puzzle1Score}");

            // Puzzle 2
            int puzzle2Score = inputs.Select(x => GetPuzzle2Score(x.oppo, x.num)).Sum();
            Console.WriteLine($"Day 02 Part 2: {puzzle2Score}");

        }


        /// <summary>
        /// Rock (A) (X): 1
        /// Paper (B) (Y): 2
        /// Scissors (C) (Z): 3
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static int Letter2Num(char c)
        {
            switch (c)
            {
                case 'A':
                case 'X':
                    return 1;
                case 'B':
                case 'Y':
                    return 2;
                case 'C':
                case 'Z':
                    return 3;
                default:
                    throw new Exception("input error");
            }
        }

        static int JudgeResult(int oppo, int me)
        {
            int diff = me - oppo;
            if (diff == 0)
            {
                return 3;
            }
            else if (Math.Abs(diff) == 1)
            {
                if (diff > 0)
                {
                    return 6;
                }
                else
                {
                    return 0;
                }
            }
            else if (Math.Abs(diff) == 2)
            {
                if (diff > 0)
                {
                    return 0;
                }
                else
                {
                    return 6;
                }
            }

            throw new Exception("input error");
        }

        static int GetPuzzle2Score(int oppo, int result)
        {
            switch (result)
            {
                case 1: // Lose
                    return oppo - 1 == 0 ? 3 + 0 : oppo - 1 + 0;
                case 2: // Even
                    return oppo + 3;
                case 3: // Win
                    return oppo + 1 == 4 ? 1 + 6 : oppo + 1 + 6;
                default:
                    throw new Exception("input error");
            }
        }

    }


}