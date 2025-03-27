namespace Thuai.Server.GameLogic;

public partial class Battle
{
    public MapGeneration.Map? Map { get; private set; }

    private MapGeneration.MapGenerator MapGenerator = new();

    public static float PointDistance(Position p1, Position p2)
    {
        float dx = p2.Xpos - p1.Xpos;
        float dy = p2.Ypos - p1.Ypos;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// If map is null, generate a map.
    /// </summary>
    /// <returns>If the map available.</returns>
    private bool GenerateMap()
    {
        try
        {
            MapGeneration.MapGenerator mapGenerator = new();
            Map = mapGenerator.GenerateMaps(1, 10, 10)[0];
            _logger.Information($"Map generated successfully.");
            return true;
        }
        catch (Exception e)
        {
            _logger.Error($"Failed to generate the map:");
            Utility.Tools.LogHandler.LogException(_logger, e);
            return false;
        }
    }

    /// <summary>
    /// Update the map.
    /// </summary>
    private void UpdateMap()
    {
        // TODO: implement.
        _logger.Debug("Map updated.");
    }

    private float LineDistance(Position line, Position point)
    {
        // 计算直线的斜率
        float m = (float)Math.Tan(line.Angle);

        // 直线方程的截距b
        float b = line.Ypos - m * line.Xpos;

        // 直线 Ax + By + C = 0 的形式
        float A = m;
        float B = -1;
        float C = b;

        // 计算点到直线的距离
        float distance = (float)Math.Abs(A * point.Xpos + B * point.Ypos + C) / (float)Math.Sqrt(A * A + B * B);

        return distance;

    }

    private Position? GetBulletFinalPos(Position startPos, Position endPos, out Position? interPos)
    {
        if (Map != null)
        {
            Position? tempInterPos = null;
            int wall_Id = -1;
            float distance = float.MaxValue;
            int i = 0;
            foreach (var wall in Map.Walls)
            {
                Position? startWall = null;
                Position? endWall = null;

                if (wall.Angle == 0)
                {
                    startWall = new Position(wall.X * Constants.WALL_LENGTH, wall.Y * Constants.WALL_LENGTH);
                    endWall = new Position((wall.X + 1) * Constants.WALL_LENGTH, wall.Y * Constants.WALL_LENGTH);
                }
                else if (wall.Angle == 90)
                {
                    startWall = new Position(wall.X * Constants.WALL_LENGTH, wall.Y * Constants.WALL_LENGTH);
                    endWall = new Position(wall.X * Constants.WALL_LENGTH, (wall.Y + 1) * Constants.WALL_LENGTH);
                }
                else
                {
                    _logger.Error($"The angle of Wall ({wall.X},{wall.Y}) is invalid!");
                }

                if (startWall != null && endWall != null)
                {
                    float denom = (endWall.Ypos - startWall.Ypos) * (endPos.Xpos - startPos.Xpos) - (endWall.Xpos - startWall.Xpos) * (endPos.Ypos - startPos.Ypos);

                    if (denom == 0)
                    {
                        continue;
                    }

                    float ua = ((endWall.Xpos - startWall.Xpos) * (startPos.Ypos - startWall.Ypos) - (endWall.Ypos - startWall.Ypos) * (startPos.Xpos - startWall.Xpos)) / denom;
                    float ub = ((endPos.Xpos - startPos.Xpos) * (startPos.Ypos - startWall.Ypos) - (endPos.Ypos - startPos.Ypos) * (startPos.Xpos - startWall.Xpos)) / denom;

                    if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                    {
                        float X = startPos.Xpos + ua * (endPos.Xpos - startPos.Xpos);
                        float Y = startPos.Ypos + ua * (endPos.Ypos - startPos.Ypos);
                        X -= (float)(Math.Cos(startPos.Angle) / Math.Abs(Math.Cos(startPos.Angle)) * Constants.WALL_THICK);
                        Y -= (float)(Math.Sin(startPos.Angle) / Math.Abs(Math.Sin(startPos.Angle)) * Constants.WALL_THICK);
                        Position tempPos = new Position(X, Y);
                        float tempDistance = PointDistance(startPos, tempPos);
                        if (tempDistance < distance)
                        {
                            tempInterPos = tempPos;
                            wall_Id = i;
                            distance = tempDistance;
                        }
                    }
                }
                i++;
            }
            if (wall_Id == -1)
            {
                Position finalEndPos = new(endPos.Xpos, endPos.Ypos, startPos.Angle);
                interPos = null;
                return finalEndPos;
            }
            else
            {
                MapGeneration.Wall wall = Map.Walls[wall_Id];
                if (wall.Angle == MapGeneration.WallDirection.HORIZONTAL)
                {
                    float finalXpos = endPos.Xpos;
                    float finalYpos = endPos.Ypos - 2 * (endPos.Ypos - (wall.Y + 1) * Constants.WALL_LENGTH);
                    float angle = -startPos.Angle;
                    Position finalEndPos = new(finalXpos, finalYpos, angle);
                    interPos = tempInterPos;
                    return finalEndPos;
                }
                else if (wall.Angle == MapGeneration.WallDirection.VERTICAL)
                {
                    float finalXpos = endPos.Xpos - 2 * (endPos.Xpos - (wall.X + 1) * Constants.WALL_LENGTH);
                    float finalYpos = endPos.Ypos;
                    float angle = (float)(Math.PI - startPos.Angle);
                    Position finalEndPos = new(finalXpos, finalYpos, angle);
                    interPos = tempInterPos;
                    return finalEndPos;
                }
                else
                {
                    _logger.Error($"The angle of Wall ({wall.X},{wall.Y}) is invalid!");
                    interPos = null;
                    return null;
                }
            }

        }
        else
        {
            _logger.Error($"The map is null!");
            interPos = null;
            return null;
        }
    }

    private Position? GetPlayerFinalPos(Position startPos, Position endPos)
    {
        if (Map != null)
        {
            Position finalEndPos = endPos;
            float distance = float.MaxValue;
            foreach (var wall in Map.Walls)
            {
                Position? startWall = null;
                Position? endWall = null;

                if (wall.Angle == 0)
                {
                    startWall = new Position(wall.X * Constants.WALL_LENGTH, wall.Y * Constants.WALL_LENGTH);
                    endWall = new Position((wall.X + 1) * Constants.WALL_LENGTH, wall.Y * Constants.WALL_LENGTH);
                }
                else if (wall.Angle == 90)
                {
                    startWall = new Position(wall.X * Constants.WALL_LENGTH, wall.Y * Constants.WALL_LENGTH);
                    endWall = new Position(wall.X * Constants.WALL_LENGTH, (wall.Y + 1) * Constants.WALL_LENGTH);
                }
                else
                {
                    _logger.Error($"The angle of Wall ({wall.X},{wall.Y}) is invalid!");
                }

                if (startWall != null && endWall != null)
                {
                    float denom = (endWall.Ypos - startWall.Ypos) * (endPos.Xpos - startPos.Xpos) - (endWall.Xpos - startWall.Xpos) * (endPos.Ypos - startPos.Ypos);

                    if (denom == 0)
                    {
                        continue;
                    }

                    float ua = ((endWall.Xpos - startWall.Xpos) * (startPos.Ypos - startWall.Ypos) - (endWall.Ypos - startWall.Ypos) * (startPos.Xpos - startWall.Xpos)) / denom;
                    float ub = ((endPos.Xpos - startPos.Xpos) * (startPos.Ypos - startWall.Ypos) - (endPos.Ypos - startPos.Ypos) * (startPos.Xpos - startWall.Xpos)) / denom;

                    if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                    {
                        float X = startPos.Xpos + ua * (endPos.Xpos - startPos.Xpos);
                        float Y = startPos.Ypos + ua * (endPos.Ypos - startPos.Ypos);
                        X -= (float)Math.Cos(startPos.Angle) / (float)Math.Abs(Math.Cos(startPos.Angle)) * Constants.WALL_THICK;
                        Y -=(float) Math.Sin(startPos.Angle) / (float)Math.Abs(Math.Sin(startPos.Angle)) * Constants.WALL_THICK;
                        Position tempEndPosition = new(X, Y, endPos.Angle);
                        float tempDistance = PointDistance(startPos, tempEndPosition);
                        if (tempDistance < distance)
                        {
                            finalEndPos = tempEndPosition;
                            distance = tempDistance;
                        }
                    }
                }
            }
            return finalEndPos;
        }
        else
        {
            _logger.Error($"The map is null!");
            return null;
        }

    }

}