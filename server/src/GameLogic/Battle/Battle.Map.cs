namespace Thuai.Server.GameLogic;

public partial class Battle
{
    public MapGeneration.Map? Map { get; private set; }

    private static MapGeneration.MapGenerator MapGenerator = new();

    public void AddWall(MapGeneration.Wall wall)
    {
        if (Map is null)
        {
            _logger.Error("Cannot add wall: Map is null.");
            return;
        }
        if (Map.Walls.Any(w => w == wall))
        {
            _logger.Error(
                "Cannot add wall:"
                + $" Wall at ({wall.X}, {wall.Y}) with angle {wall.Angle} already exists."
            );
            return;
        }

        Map.Walls.Add(wall);
        BindWall(wall);
        _logger.Information($"Added wall at ({wall.X}, {wall.Y}) with angle {wall.Angle}");
    }

    public void BindWall(MapGeneration.Wall wall)
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

    public void BindWall(List<MapGeneration.Wall> walls)
    {
        foreach (var wall in walls)
        {
            BindWall(wall);
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
            Map = MapGenerator.GenerateMaps(1, 10, 10)[0];
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

    /// <summary>
    /// Remove a wall from the map.
    /// </summary>
    /// <param name="target">The target. Only the position, not bound to a physical body.</param>
    private void RemoveWall(MapGeneration.Wall target)
    {
        try
        {
            if (Map is null)
            {
                _logger.Error("Failed to remove wall: Map is null.");
                return;
            }

            // Find corresponding wall in the map.
            MapGeneration.Wall? wall = Map.Walls.FirstOrDefault(w => w == target);
            if (wall is null)
            {
                _logger.Error(
                    $"Failed to remove wall: Wall at ({target.X}, {target.Y}) with angle {target.Angle} not found."
                );
                return;
            }

            if (wall.Body is not null)
            {
                _env.RemoveBody(wall.Body);
                wall.Unbind();
            }
            Map?.Walls.Remove(wall);
            _logger.Information($"Removed wall at ({wall.X}, {wall.Y}) with angle {wall.Angle}");
        }
        catch (Exception e)
        {
            _logger.Error("Failed to remove wall:");
            Utility.Tools.LogHandler.LogException(_logger, e);
        }
    }
}
