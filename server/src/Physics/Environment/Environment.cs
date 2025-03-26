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

    private static SolverIterations _solverIterations = new()
    {
        VelocityIterations = VELOCITY_ITERATIONS,
        PositionIterations = POSITION_ITERATIONS,
        TOIVelocityIterations = TOI_VELOCITY_ITERATIONS,
        TOIPositionIterations = TOI_POSITION_ITERATIONS
    };

    public float TimeStep { get; init; }

    private readonly World _world = new(Vector2.Zero);

    private readonly ILogger _logger = Utility.Tools.LogHandler.CreateLogger("Environment");

    public Environment(int ticksPerSecond)
    {
        TimeStep = 1f / ticksPerSecond;

        _logger.Debug("Environment settings:");
        _logger.Debug($"Time step: {1000 * TimeStep:F2}ms");
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
        _world.Step(TimeStep, ref _solverIterations);
    }
}
