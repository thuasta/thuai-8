using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Common;

namespace Thuai.Server.Physics;

public partial class Environment
{
    /// <summary>
    /// Create a body with specified category, position and rotation.
    /// </summary>
    /// <param name="category">Category of the body.</param>
    /// <param name="position">Initial position of the body.</param>
    /// <param name="rotation">Initial rotation of the body.</param>
    /// <returns>The body created.</returns>
    /// <exception cref="InvalidOperationException">Raises when creating a laser with this method.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Raises when the category is invalid.</exception>
    public Body CreateBody(Category category, Vector2 position, float rotation)
    {
        Body? result;
        Fixture? fixture;

        lock (_lock)
        {
            try
            {
                switch (category)
                {
                    case Categories.Player:
                        result = _world.CreateBody(position, rotation, BodyType.Dynamic);
                        fixture = result.CreateCircle(GameLogic.Constants.PLAYER_RADIUS, DEFAULT_DENSITY);
                        fixture.CollisionCategories = Categories.Player;
                        fixture.CollidesWith = CollisionList.PlayerCollidesWith;
                        fixture.Friction = 0f;
                        fixture.Restitution = 0f;
                        return result;

                    case Categories.Wall:
                        result = _world.CreateBody(position, rotation, BodyType.Static);
                        fixture = result.CreateEdge(new(0, 0), new(GameLogic.Constants.WALL_LENGTH, 0));
                        fixture.CollisionCategories = Categories.Wall;
                        fixture.CollidesWith = CollisionList.WallCollidesWith;
                        fixture.Friction = 0f;
                        return result;

                    case Categories.Bullet:
                        result = _world.CreateBody(position, rotation, BodyType.Dynamic);
                        fixture = result.CreateCircle(GameLogic.Constants.BULLET_RADIUS, DEFAULT_DENSITY);
                        fixture.CollisionCategories = Categories.Bullet;
                        fixture.CollidesWith = CollisionList.BulletCollidesWith;
                        fixture.Friction = 0f;
                        fixture.Restitution = 1f;
                        return result;

                    case Categories.Trap:
                        result = _world.CreateBody(position, rotation, BodyType.Static);
                        fixture = result.CreateCircle(GameLogic.Constants.PLAYER_RADIUS, DEFAULT_DENSITY);
                        fixture.CollisionCategories = Categories.Trap;
                        fixture.CollidesWith = CollisionList.TrapCollidesWith;
                        fixture.IsSensor = true;
                        return result;

                    case Categories.Laser:
                        throw new InvalidOperationException("Laser should be created with CreateLaser method.");

                    case Categories.GravityField:
                        throw new InvalidOperationException(
                            "Gravity field should be created with Player.AppendGravityField method."
                        );

                    case Categories.Grid:
                        throw new InvalidOperationException("Grid should be created with GenerateGrid method.");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(category), $"Invalid category.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to create body:");
                Utility.Tools.LogHandler.LogException(_logger, ex);
                throw;
            }
        }
    }

    public Body CreateLaser(Vector2 startPoint, Vector2 direction, float laserLength)
    {
        List<Vector2> vertices = CalculateLaserPath(startPoint, direction, laserLength);
        return CreateLaser(vertices);
    }

    public Body CreateLaser(List<Vector2> vertices)
    {
        Body result = _world.CreateBody(Vector2.Zero, 0f, BodyType.Static);
        Fixture fixture = result.CreateChainShape([.. vertices]);
        fixture.CollisionCategories = Categories.Laser;
        fixture.CollidesWith = CollisionList.LaserCollidesWith;
        fixture.IsSensor = true;
        return result;
    }

    public List<Vector2> CalculateLaserPath(Vector2 startPoint, Vector2 direction, float laserLength)
    {
        List<Vector2> laserPath = [startPoint];
        Vector2 currentStart = startPoint;
        Vector2 currentDirection = direction;

        float remainingLength = laserLength;

        while (remainingLength > 0f)
        {
            Vector2? hitPoint = null;
            Vector2? hitNormal = null;

            _world.RayCast((fixture, point, normal, fraction) =>
            {
                if (fixture.CollisionCategories != Categories.Wall)
                {
                    return -1f;
                }

                hitPoint = point;
                hitNormal = normal;
                return fraction;
            }, currentStart, currentStart + currentDirection * remainingLength);

            if (hitPoint.HasValue && hitNormal.HasValue)
            {
                laserPath.Add(hitPoint.Value);
                Vector2 hitPointValue = hitPoint.Value;
                Vector2.Distance(ref currentStart, ref hitPointValue, out float distance);
                remainingLength -= distance;

                currentDirection = Reflect(currentDirection, hitNormal.Value);
                currentStart = hitPoint.Value;
            }
            else
            {
                laserPath.Add(currentStart + currentDirection * remainingLength);
                remainingLength = 0f;
                break;
            }
        }

        return laserPath;
    }

    public GameLogic.MapGeneration.Wall? GetFacingEdge(Vector2 startPoint, Vector2 direction)
    {
        Protocol.Scheme.PositionInt? result = null;
        _world.RayCast((fixture, point, normal, fraction) =>
        {
            if (fixture.CollisionCategories != Categories.Grid)
            {
                return -1f;
            }

            Tag tag = (Tag)fixture.Body.Tag;
            if (
                tag.AttachedData.TryGetValue(
                    Key.CorrespondingWallPosition, out object? value
                ) && value is Protocol.Scheme.PositionInt position
            )
            {
                result = position;
            }
            return 0;
        }, startPoint, startPoint + direction * 2 * GameLogic.Constants.WALL_LENGTH);

        if (result is null)
        {
            _logger.Error("Failed to find the wall in front of the player.");
            return null;
        }
        return new(result.X, result.Y, result.Angle);
    }
}
