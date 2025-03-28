using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Collision.Shapes;
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
    /// Run the physics simulation for one step.
    /// </summary>
    public void Step()
    {
        lock (_lock)
        {
            try
            {
                _world.Step(TIME_STEP, ref _solverIterations);
            }
            catch (Exception ex)
            {
                _logger.Error("Error during physics step:");
                Utility.Tools.LogHandler.LogException(_logger, ex);
            }
        }
    }
}
