namespace Thuai.GameServer.MapGenerator;
using System;
using System.Collections.Generic;

class Map
{
    public Map(int width, int height)
    {
        // width is the column number of the map
        // height is the row number of the map
        Width = width;
        Height = height;
        GenerateRandomWalls();
    }
    private int Width { get; }
    private int Height { get; }
    private List<Wall> Walls { get; } = new List<Wall>();
    private void GenerateRandomWalls()
    {
        // lines on the path, should be maintained
        List<Line> lines = new List<Line>();
        // points that have been visited, should be maintained
        List<Point> closedPoints = new List<Point>();
        // points that are on the path and have not been visited
        Stack<Point> openPoints = new Stack<Point>();
        // random select a path from the top-left point
        Point startPoint = new Point(0, 0);
        openPoints.Push(startPoint);
        while (openPoints.Count > 0)
        {
            Point currentPoint = openPoints.Pop();
            Point nextPoint = TakeOneStep(currentPoint, closedPoints, lines);
            if (nextPoint.X != -1)
            {
                openPoints.Push(nextPoint);
                lines.Add(new Line(currentPoint.X, currentPoint.Y, nextPoint.X, nextPoint.Y));
            }
        }
        List<Walls> allPossibleWalls = GetAllPossibleWalls();
        foreach (var line in lines)
        {
            Wall wall = line.GetCorrespondingWall();
            allPossibleWalls.Remove(wall);
        }
        Walls.AddRange(allPossibleWalls);
    }
    private Point TakeOneStep(Point currentPoint, List<Point>& closedPoints, List<Line>& lines)
    {
        // random select a valid direction
        List<Point> validDirections = new List<Point>();
        if (currentPoint.X > 0 && !closedPoints.Contains(new Point(currentPoint.X - 1, currentPoint.Y)))
        {
            validDirections.Add(new Point(currentPoint.X - 1, currentPoint.Y));
        }
        if (currentPoint.X < Width - 1 && !closedPoints.Contains(new Point(currentPoint.X + 1, currentPoint.Y)))
        {
            validDirections.Add(new Point(currentPoint.X + 1, currentPoint.Y));
        }
        if (currentPoint.Y > 0 && !closedPoints.Contains(new Point(currentPoint.X, currentPoint.Y - 1)))
        {
            validDirections.Add(new Point(currentPoint.X, currentPoint.Y - 1));
        }
        if (currentPoint.Y < Height - 1 && !closedPoints.Contains(new Point(currentPoint.X, currentPoint.Y + 1)))
        {
            validDirections.Add(new Point(currentPoint.X, currentPoint.Y + 1));
        }
        if (validDirections.Count == 0)  // if no valid direction, return Point(-1, -1)
        {
            return new Point(-1, -1);
        }
        // random select a direction
        Random random = new Random();
        int index = random.Next(validDirections.Count);
        Point newPoint = validDirections[index];
        closedPoints.Add(newPoint);  // add the new point to the closedPoints
        lines.Add(new Line(currentPoint.X, currentPoint.Y, newPoint.X, newPoint.Y));  // add the line to the lines
        return newPoint;
    }
    private List<Wall> GetAllPossibleWalls()
    {
        List<Wall> walls = new List<Wall>();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (i == 0 && j != 0)
                {
                    walls.Add(new Wall(i, j, 90));
                }
                else if (j == 0 && i != 0)
                {
                    walls.Add(new Wall(i, j, 0));
                }
                else if (i != 0 && j != 0)
                {
                    walls.Add(new Wall(i, j, 0));
                    walls.Add(new Wall(i, j, 90));
                }
            }
        }
    }
}

class MapGenerator
{
    public List<Map> GenerateMaps(int count, int width, int height)
    {
        var maps = new List<Map>();
        for (var i = 0; i < count; i++)
        {
            var map = new Map(width, height);
            maps.Add(map);
        }
        return maps;
    }
}

struct Line 
{
    public Line(int x1, int y1, int x2, int y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }
    public Wall GetCorrespondingWall()
    {
        int angle = 0;
        int x = 0;
        int y = 0;
        if (X1 == X2)
        {
            angle = 90; // vertical
            x = X1;
            y = Math.Max(Y1, Y2);
        }
        else if (Y1 == Y2)
        {
            angle = 0; // horizontal
            x = Math.Max(X1, X2);
            y = Y1;
        }
        return new Wall(x, y, angle);
    }
    private int X1 { get; }
    private int Y1 { get; }
    private int X2 { get; }
    private int Y2 { get; }
}

struct Point 
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    private int X { get; }
    private int Y { get; }
}

// test
class Program
{
    static void Main()
    {
        MapGenerator mapGenerator = new MapGenerator();
        List<Map> maps = mapGenerator.GenerateMaps(10, 10, 10);
        foreach (var map in maps)
        {
            Console.WriteLine($"Map: width={map.Width}, height={map.Height}");
            foreach (var wall in map.Walls)
            {
                Console.WriteLine($"Wall: x={wall.X}, y={wall.Y}, angle={wall.Angle}");
            }
        }
    }
}