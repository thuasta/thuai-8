using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Common;

using Serilog;

namespace Thuai.Server.Physics;

public partial class Environment
{
    public const int VELOCITY_ITERATIONS = 100;
    public const int POSITION_ITERATIONS = 100;
    public const int TOI_VELOCITY_ITERATIONS = 100;
    public const int TOI_POSITION_ITERATIONS = 100;
    public const float DEFAULT_DENSITY = 1f;
    public const float RAYCAST_FIX_DELTA = 0.001f;    // To fix the bug of stuck in wall.
    public const int SIMULATION_PER_STEP = 60; // Number of simulation steps per tick.

    // A tick in game is equivalent to a second in physics simulation.
    public const float TIME_STEP = 1f;

    private static SolverIterations _solverIterations = new()
    {
        VelocityIterations = VELOCITY_ITERATIONS,
        PositionIterations = POSITION_ITERATIONS,
        TOIVelocityIterations = TOI_VELOCITY_ITERATIONS,
        TOIPositionIterations = TOI_POSITION_ITERATIONS
    };

    private readonly World _world = new(Vector2.Zero);

    private readonly ILogger _logger = Utility.Tools.LogHandler.CreateLogger("Environment");
    private readonly object _lock = new();

    public Environment()
    {
        _logger.Debug("Environment settings:");
        _logger.Debug($"Velocity iterations: {VELOCITY_ITERATIONS}");
        _logger.Debug($"Position iterations: {POSITION_ITERATIONS}");
        _logger.Debug($"TOI velocity iterations: {TOI_VELOCITY_ITERATIONS}");
        _logger.Debug($"TOI position iterations: {TOI_POSITION_ITERATIONS}");
    }

    /// <summary>
    /// Generate a grid of bodies in the world.
    /// </summary>
    /// <param name="width">Width of the grid.</param>
    /// <param name="height">Height of the grid.</param>
    public void GenerateGrid(int width, int height)
    {
        // Create horizontal walls
        for (int i = 1; i <= width; ++i)
        {
            for (int j = 0; j <= height; ++j)
            {
                Body body = _world.CreateBody(
                    new(i * GameLogic.Constants.WALL_LENGTH, j * GameLogic.Constants.WALL_LENGTH),
                    (float)Math.PI,
                    BodyType.Static
                );
                Fixture fixture = body.CreateEdge(new(0, 0), new(GameLogic.Constants.WALL_LENGTH, 0));
                fixture.CollisionCategories = Categories.Grid;
                fixture.CollidesWith = CollisionList.GridCollidesWith;
                fixture.IsSensor = true;

                body.Tag = new Tag() { Owner = new() };
                Tag tag = (Tag)body.Tag;
                tag.AttachedData[Key.CorrespondingWallPosition] = new Protocol.Scheme.PositionInt()
                {
                    X = i,
                    Y = j,
                    Angle = GameLogic.MapGeneration.WallDirection.HORIZONTAL
                };
            }
        }
        // Create vertical walls
        for (int i = 0; i <= width; ++i)
        {
            for (int j = 1; j <= height; ++j)
            {
                Body body = _world.CreateBody(
                    new(i * GameLogic.Constants.WALL_LENGTH, j * GameLogic.Constants.WALL_LENGTH),
                    (float)(-Math.PI / 2),
                    BodyType.Static
                );
                Fixture fixture = body.CreateEdge(new(0, 0), new(GameLogic.Constants.WALL_LENGTH, 0));
                fixture.CollisionCategories = Categories.Grid;
                fixture.CollidesWith = CollisionList.GridCollidesWith;
                fixture.IsSensor = true;

                body.Tag = new Tag() { Owner = new() };
                Tag tag = (Tag)body.Tag;
                tag.AttachedData[Key.CorrespondingWallPosition] = new Protocol.Scheme.PositionInt()
                {
                    X = i,
                    Y = j,
                    Angle = GameLogic.MapGeneration.WallDirection.VERTICAL
                };
            }
        }
    }

    /// <summary>
    /// Run the physics simulation for one step.
    /// </summary>
    public void Step()
    {
        lock (_lock)
        {
            try
            {
                for (int i = 0; i < SIMULATION_PER_STEP; ++i)
                {
                    _world.Step(TIME_STEP / SIMULATION_PER_STEP, ref _solverIterations);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error during physics step:");
                Utility.Tools.LogHandler.LogException(_logger, ex);
            }
        }
    }

    /// <summary>
    /// Remove a body from the world.
    /// </summary>
    /// <param name="body">Body to remove.</param>
    public void RemoveBody(Body body)
    {
        lock (_lock)
        {
            _world.Remove(body);
        }
    }
}
