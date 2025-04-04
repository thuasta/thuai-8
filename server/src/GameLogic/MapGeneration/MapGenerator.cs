namespace Thuai.Server.GameLogic.MapGeneration;

using System;
using System.Collections.Generic;

public class Map
{
    public Map(int width, int height)
    {
        // width is the column number of the map
        // height is the row number of the map
        Width = width;
        Height = height;
        GenerateRandomWalls();
    }

    public const int PathIteration = 2;

    public int Width { get; }
    public int Height { get; }
    public List<Wall> Walls { get; } = [];
    private static readonly Random random = new();

    public bool IsInsideMap(float x, float y)
    {
        return x >= 0 && x <= Width * Constants.WALL_LENGTH
            && y >= 0 && y <= Height * Constants.WALL_LENGTH;
    }

    private void GenerateRandomWalls()
    {
        // lines on the path, should be maintained
        List<Line> lines = [];
        for (int i = 0; i < PathIteration; i++)
        {
            CreateRandomPath(ref lines);
        }

        List<Wall> allPossibleWalls = GetAllPossibleWallsWithoutBoarder();
        foreach (var line in lines)
        {
            Wall wall = line.GetCorrespondingWall();
            allPossibleWalls.Remove(wall);
        }
        Walls.AddRange(allPossibleWalls);
        Walls.AddRange(GetBoarders());
    }

    private List<Wall> GetAllPossibleWallsWithoutBoarder()
    {
        List<Wall> walls = [];
        for (int i = 1; i < Width; i++)
        {
            for (int j = 1; j <= Height; j++)
            {
                walls.Add(new Wall(i, j, 90));
            }
        }
        for (int j = 1; j < Height; j++)
        {
            for (int i = 1; i <= Width; i++)
            {
                walls.Add(new Wall(i, j, 0));
            }
        }
        return walls;
    }

    private void CreateRandomPath(ref List<Line> lines)
    {
        int startX = random.Next(Width);
        int startY = random.Next(Height);
        Point startPoint = new(startX, startY);
        // points that have been visited, should be maintained
        List<Point> closedPoints = [];
        // points that are on the path and have not been visited
        Stack<Point> openPoints = new();
        openPoints.Push(startPoint);
        closedPoints.Add(startPoint);

        while (openPoints.Count > 0)
        {
            Point currentPoint = openPoints.Pop();
            Point nextPoint = ChooseNeighbor(currentPoint, ref closedPoints, ref lines);
            while (nextPoint.X != -1)
            {
                openPoints.Push(nextPoint);
                currentPoint = nextPoint;
                nextPoint = ChooseNeighbor(currentPoint, ref closedPoints, ref lines);
            }
        }
    }

    private Point ChooseNeighbor(Point currentPoint, ref List<Point> closedPoints, ref List<Line> lines)
    {
        // random select a valid direction
        List<Point> validDirections = [];
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
        int index = random.Next(validDirections.Count);
        Point newPoint = validDirections[index];
        closedPoints.Add(newPoint);  // add the new point to the closedPoints

        if (newPoint.X > currentPoint.X)
        {
            // Go right
            lines.Add(new Line(currentPoint.X + 1, currentPoint.Y + 1, currentPoint.X + 1, currentPoint.Y));
        }
        else if (newPoint.X < currentPoint.X)
        {
            // Go left
            lines.Add(new Line(currentPoint.X, currentPoint.Y + 1, currentPoint.X, currentPoint.Y));
        }
        else if (newPoint.Y > currentPoint.Y)
        {
            // Go up
            lines.Add(new Line(currentPoint.X, currentPoint.Y + 1, currentPoint.X + 1, currentPoint.Y + 1));
        }
        else
        {
            // Go down
            lines.Add(new Line(currentPoint.X, currentPoint.Y, currentPoint.X + 1, currentPoint.Y));
        }

        return newPoint;
    }

    private List<Wall> GetBoarders()
    {
        List<Wall> walls = [];
        for (int i = 1; i <= Width; i++)
        {
            walls.Add(new Wall(i, 0, WallDirection.HORIZONTAL));
            walls.Add(new Wall(i, Height, WallDirection.HORIZONTAL));
        }
        for (int j = 1; j <= Height; j++)
        {
            walls.Add(new Wall(0, j, WallDirection.VERTICAL));
            walls.Add(new Wall(Width, j, WallDirection.VERTICAL));
        }
        return walls;
    }
}

public class MapGenerator
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

struct Line(int x1, int y1, int x2, int y2)
{
    public Wall GetCorrespondingWall()
    {
        int angle = 0;
        int x = 0;
        int y = 0;
        if (X1 == X2)
        {
            angle = WallDirection.VERTICAL;
            x = X1;
            y = Math.Max(Y1, Y2);
        }
        else if (Y1 == Y2)
        {
            angle = WallDirection.HORIZONTAL;
            x = Math.Max(X1, X2);
            y = Y1;
        }
        return new Wall(x, y, angle);
    }
    public int X1 { get; } = x1;
    public int Y1 { get; } = y1;
    public int X2 { get; } = x2;
    public int Y2 { get; } = y2;
}

struct Point(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
}
