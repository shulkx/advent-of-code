// See https://aka.ms/new-console-template for more information
using System.Drawing;
string input = File.ReadAllText("input.txt");

Cave cave = new Cave(input, false);
Console.WriteLine($"Day 14 Part 1: {cave.LabelSands(new Point(500, 0))}");

cave = new Cave(input, true);
Console.WriteLine($"Day 14 Part 2: {cave.LabelSands(new Point(500, 0))}");

public class Cave
{
	private int _maxY;
	private bool _hasFloor;

    public Dictionary<Point, char> Map { get; set; } = new Dictionary<Point, char>();

	public Cave(string input, bool hasFloor)
	{
		_hasFloor = hasFloor;

        var scans = input.Split(Environment.NewLine);

		foreach (var scan in scans)
		{
            List<Point> rockPoints = new List<Point>();

            var coordinates = scan.Split(" -> ");

			foreach (var coordinate in coordinates)
			{
				var axisValues = coordinate.Split(',');
				
				rockPoints.Add(new Point(
					int.Parse(axisValues[0]), 
					int.Parse(axisValues[1])
					));
            }

			for (int i = 1; i < rockPoints.Count; i++)
			{
				LabelRocks(rockPoints[i - 1], rockPoints[i]);
            }
		}

		_maxY = Map.Max(x => x.Key.Y);
	}

    public void LabelRocks(Point start, Point end)
    {
		int xDelta = Math.Sign(end.X - start.X);
		int yDelta = Math.Sign(end.Y - start.Y);

		while (start != end)
		{
            Map[start] = '#';
			start.Offset(xDelta, yDelta);
		}

		if (!Map.ContainsKey(end))
        {
            Map.Add(end, '#');
        }
    }

	public int LabelSands(Point sandStart)
	{
		while (true)
		{
			var sandLoc = SimulateSandUnit(sandStart);

            if (Map.ContainsKey(sandLoc))
            {
                break;
            }

            // If has floor, cancel the condition "sandLoc.Y == _maxY + 1"
            if (!_hasFloor && sandLoc.Y == _maxY + 1)
			{
				break;
			}

			Map[sandLoc] = 'o';
        }

        return Map.Values.Count(x => x == 'o');
    }

	public Point SimulateSandUnit(Point sand)
	{
		Point down = new Point(0, 1); ;
		Point leftDown = new Point(-1, 1);
		Point rightDown = new Point(1, 1);

		while (sand.Y <= _maxY)
		{

            if (!Map.ContainsKey(MovePoint(sand, down)))
			{
				sand = MovePoint(sand, down);
            }
			else if (!Map.ContainsKey(MovePoint(sand, leftDown)))
			{
                sand = MovePoint(sand, leftDown);
            }
			else if (!Map.ContainsKey(MovePoint(sand, rightDown)))
			{
                sand = MovePoint(sand, rightDown);
            }
			else
			{
				break;
			}
		}
        return sand;
    }

	private Point MovePoint(Point point, Point offset) => new Point(point.X + offset.X, point.Y + offset.Y);
}