using Thuai.GameServer.MapGenerator;

namespace Thuai.Server.GameLogic;

public partial class Battle
{
    private Map? Map = null;

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

    private double CalculateDistance(Position p1, Position p2)
    {
        double dx = p2.Xpos - p1.Xpos;
        double dy = p2.Ypos - p1.Ypos;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private Position? GetFinalPos(Position startPos, Position endPos)
    {
        if (Map != null)
        {
            Position? finalEndPos = endPos;
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
                        continue; // 平行或共线
                    }

                    double ua = ((endWall.Xpos - startWall.Xpos) * (startPos.Ypos - startWall.Ypos) - (endWall.Ypos - startWall.Ypos) * (startPos.Xpos - startWall.Xpos)) / denom;
                    double ub = ((endPos.Xpos - startPos.Xpos) * (startPos.Ypos - startWall.Ypos) - (endPos.Ypos - startPos.Ypos) * (startPos.Xpos - startWall.Xpos)) / denom;

                    if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                    {
                        // 计算交点坐标
                        double X = startPos.Xpos + ua * (endPos.Xpos - startPos.Xpos);
                        double Y = startPos.Ypos + ua * (endPos.Ypos - startPos.Ypos);
                        X -= Math.Cos(startPos.Angle) / Math.Abs(Math.Cos(startPos.Angle)) * Constants.WALL_THICK;
                        Y -= Math.Sin(startPos.Angle) / Math.Abs(Math.Sin(startPos.Angle)) * Constants.WALL_THICK;
                        Position tempEndPosition = new Position(X, Y);
                        double tempDistance = CalculateDistance(startPos, tempEndPosition);
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