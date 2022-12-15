// See https://aka.ms/new-console-template for more information


using System.Drawing;
string input = File.ReadAllText("input.txt");
Tunnel tunnel = new Tunnel(input);
Console.WriteLine(tunnel._targetX.Count);



public class Tunnel
{

    public HashSet<int> _targetX = new HashSet<int>();


    public Tunnel(string input)
    {

        var infos = input.Split(Environment.NewLine);
        HashSet<int> beaconXs = new HashSet<int>();

        Dictionary<Point, int> sensorsWithDistance = new Dictionary<Point, int>();
        HashSet<Point> surroundPoints = new HashSet<Point>();

        foreach (var info in infos)
        {
            string[] delimiters = { "Sensor at x=", ", y=", ": closest beacon is at x=" };
            int[] axisVal = info.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();

            Point sensor = new Point(axisVal[0], axisVal[1]);
            Point beacon = new Point(axisVal[2], axisVal[3]);

            if (beacon.Y == 2000000)
            {
                beaconXs.Add(beacon.X);
            }

            GetSurroundPoints(sensor, beacon, surroundPoints);

            LabelBeaconArea(sensor, beacon, 2000000);

            sensorsWithDistance.Add(sensor, Utility.GetManhattanDistance(sensor, beacon));
        }

        _targetX = _targetX.Except(beaconXs).ToHashSet();


        foreach (var sensor in sensorsWithDistance.Keys)
        {
            int distance = sensorsWithDistance[sensor];
            surroundPoints = surroundPoints.Where(x => Utility.GetManhattanDistance(x, sensor) > distance).ToHashSet();
        }

    }

    private void LabelBeaconArea(Point sensor, Point beacon, int targetY)
    {
        int distance = Utility.GetManhattanDistance(sensor, beacon);
        int minY = sensor.Y - distance;
        int maxY = sensor.Y + distance;

        if (minY <= targetY && targetY <= maxY)
        {
            int diff = Math.Abs(distance - Math.Abs(targetY - sensor.Y));

            for (int j = -1 * diff; j <= diff; j++)
            {
                _targetX.Add(sensor.X + j);

            }
        }

    }


    private void GetSurroundPoints(Point sensor, Point beacon, HashSet<Point> surroundPoints)
    {

        int distancePlusOne = Utility.GetManhattanDistance(sensor, beacon) + 1;
        int minY = sensor.Y - distancePlusOne;
        int maxY = sensor.Y + distancePlusOne;

        for (int y = minY; y <= maxY; y++)
        {
            int diff = Math.Abs(distancePlusOne - Math.Abs(y - sensor.Y));
            int minX = sensor.X - diff;
            int maxX = sensor.X + diff;

            Point minP = new Point(minX, y);
            Point maxP = new Point(maxX, y);

            if (IsWithinRange(minP))
            {
                surroundPoints.Add(minP);
            }

            if (IsWithinRange(maxP))
            {
                surroundPoints.Add(maxP);
            }
        }
    }

    private int _rangeStart = 0;
    private int _rangeEnd = 4000000;

    private bool IsWithinRange(Point p)
    {
        return (p.X >= _rangeStart && p.X <= _rangeEnd) &&
            (p.Y >= _rangeStart && p.Y <= _rangeEnd);
    }

}

public static class Utility
{
    public static int GetManhattanDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

    public static long GetTunningFreq(Point p) => p.X * 4000000 + p.Y;
}
