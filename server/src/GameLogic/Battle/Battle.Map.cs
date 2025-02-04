using Thuai.GameServer.MapGenerator;

namespace Thuai.Server.GameLogic;

public partial class Battle
{
    public Map? Map { get; private set; }

    private MapGenerator MapGenerator = new();

    /// <summarYpos>
    /// If map is null, generate a map.
    /// </summarYpos>
    /// <returns>If the map available.</returns>
    private bool GenerateMap()
    {
        MapGenerator mapGenerator = new();
        Map = mapGenerator.GenerateMaps(1, 10, 10)[0];
        return false;
    }

    /// <summarYpos>
    /// Update the map.
    /// </summarYpos>
    private void UpdateMap()
    {
        // TODO: implement.
    }

    private double PointDistance(Position p1, Position p2)
    {
        double dx = p2.Xpos - p1.Xpos;
        double dy = p2.Ypos - p1.Ypos;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private double LineDistance(Position line, Position point)
    {
        // 计算直线的斜率
        double m = Math.Tan(line.Angle);

        // 直线方程的截距b
        double b = line.Ypos - m * line.Xpos;

        // 直线 Ax + By + C = 0 的形式
        double A = m;
        double B = -1;
        double C = b;

        // 计算点到直线的距离
        double distance = Math.Abs(A * point.Xpos + B * point.Ypos + C) / Math.Sqrt(A * A + B * B);

        return distance;

    }

    private Position? GetBulletFinalPos(Position startPos, Position endPos, out Position? interPos)
    {
        if (Map != null)
        {
            Position? tempInterPos = null;
            int wall_Id = -1;
            double distance = double.MaxValue;
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
                    double denom = (endWall.Ypos - startWall.Ypos) * (endPos.Xpos - startPos.Xpos) - (endWall.Xpos - startWall.Xpos) * (endPos.Ypos - startPos.Ypos);

                    if (denom == 0)
                    {
                        continue;
                    }

                    double ua = ((endWall.Xpos - startWall.Xpos) * (startPos.Ypos - startWall.Ypos) - (endWall.Ypos - startWall.Ypos) * (startPos.Xpos - startWall.Xpos)) / denom;
                    double ub = ((endPos.Xpos - startPos.Xpos) * (startPos.Ypos - startWall.Ypos) - (endPos.Ypos - startPos.Ypos) * (startPos.Xpos - startWall.Xpos)) / denom;

                    if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                    {
                        double X = startPos.Xpos + ua * (endPos.Xpos - startPos.Xpos);
                        double Y = startPos.Ypos + ua * (endPos.Ypos - startPos.Ypos);
                        X -= Math.Cos(startPos.Angle) / Math.Abs(Math.Cos(startPos.Angle)) * Constants.WALL_THICK;
                        Y -= Math.Sin(startPos.Angle) / Math.Abs(Math.Sin(startPos.Angle)) * Constants.WALL_THICK;
                        Position tempPos = new Position(X, Y);
                        double tempDistance = PointDistance(startPos, tempPos);
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
                Wall wall = Map.Walls[wall_Id];
                if (wall.Angle == 0)
                {
                    double finalXpos = endPos.Xpos;
                    double finalYpos = endPos.Ypos - 2 * (endPos.Ypos - (wall.Y + 1) * Constants.WALL_LENGTH);
                    double angle = -startPos.Angle;
                    Position finalEndPos = new(finalXpos, finalYpos, angle);
                    interPos = tempInterPos;
                    return finalEndPos;
                }
                else if (wall.Angle == 90)
                {
                    double finalXpos = endPos.Xpos - 2 * (endPos.Xpos - (wall.X + 1) * Constants.WALL_LENGTH);
                    double finalYpos = endPos.Ypos;
                    double angle = Math.PI - startPos.Angle;
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
            double distance = double.MaxValue;
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
                    double denom = (endWall.Ypos - startWall.Ypos) * (endPos.Xpos - startPos.Xpos) - (endWall.Xpos - startWall.Xpos) * (endPos.Ypos - startPos.Ypos);

                    if (denom == 0)
                    {
                        continue;
                    }

                    double ua = ((endWall.Xpos - startWall.Xpos) * (startPos.Ypos - startWall.Ypos) - (endWall.Ypos - startWall.Ypos) * (startPos.Xpos - startWall.Xpos)) / denom;
                    double ub = ((endPos.Xpos - startPos.Xpos) * (startPos.Ypos - startWall.Ypos) - (endPos.Ypos - startPos.Ypos) * (startPos.Xpos - startWall.Xpos)) / denom;

                    if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                    {
                        double X = startPos.Xpos + ua * (endPos.Xpos - startPos.Xpos);
                        double Y = startPos.Ypos + ua * (endPos.Ypos - startPos.Ypos);
                        X -= Math.Cos(startPos.Angle) / Math.Abs(Math.Cos(startPos.Angle)) * Constants.WALL_THICK;
                        Y -= Math.Sin(startPos.Angle) / Math.Abs(Math.Sin(startPos.Angle)) * Constants.WALL_THICK;
                        Position tempEndPosition = new Position(X, Y, endPos.Angle);
                        double tempDistance = PointDistance(startPos, tempEndPosition);
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