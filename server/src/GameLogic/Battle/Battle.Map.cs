namespace Thuai.Server.GameLogic;

public partial class Battle
{
    public MapGeneration.Map? Map { get; private set; }

    private MapGeneration.MapGenerator MapGenerator = new();

    public void AddWall(MapGeneration.Wall wall)
    {
        if (wall.Angle == MapGeneration.WallDirection.HORIZONTAL)
        {
            wall.Bind(
                _env.CreateBody(
                    Physics.Environment.Categories.Wall,
                    new(wall.X * Constants.WALL_LENGTH, wall.Y * Constants.WALL_LENGTH),
                    (float)Math.PI
                )
            );
        }
        else
        {
            wall.Bind(
                _env.CreateBody(
                    Physics.Environment.Categories.Wall,
                    new(wall.X * Constants.WALL_LENGTH, wall.Y * Constants.WALL_LENGTH),
                    (float)(-Math.PI / 2)
                )
            );
        }
    }

    public void AddWall(List<MapGeneration.Wall> walls)
    {
        foreach (var wall in walls)
        {
            AddWall(wall);
        }
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
        if (Map is null)
        {
            _logger.Error("Failed to update map: Map is null.");
            return;
        }

        List<MapGeneration.Wall> toDelete = [];
        foreach (MapGeneration.Wall wall in Map.Walls)
        {
            if (wall.IsBroken == true)
            {
                toDelete.Add(wall);
            }
        }
        RemoveWall(toDelete);
        _logger.Debug("Map updated.");
    }

    private void RemoveWall(List<MapGeneration.Wall> walls)
    {
        foreach (MapGeneration.Wall wall in walls)
        {
            RemoveWall(wall);
        }
    }

    private void RemoveWall(MapGeneration.Wall wall)
    {
        try
        {
            Map?.Walls.Remove(wall);
            _logger.Debug($"Removed wall at ({wall.X}, {wall.Y}) with angle {wall.Angle}");
        }
        catch (Exception e)
        {
            _logger.Error("Failed to remove wall:");
            Utility.Tools.LogHandler.LogException(_logger, e);
        }
    }
}