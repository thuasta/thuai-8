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
                        fixture.Restitution = 0f;
                        return result;

                    case Categories.Bullet:
                        result = _world.CreateBody(position, rotation, BodyType.Dynamic);
                        result.IsBullet = true;
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
                        throw new InvalidOperationException("Laser should not be created with this method.");

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

    /// <summary>
    /// Activate the laser from the start point in the specified direction.
    /// </summary>
    /// <param name="laser">The laser bullet to be activated.</param>
    /// <returns>Vectices of the laser.</returns>
    public List<Vector2> ActivateLaser(GameLogic.LaserBullet laser)
    {
        Vector2 startPoint = new(laser.BulletPosition.Xpos, laser.BulletPosition.Ypos);
        Vector2 direction = new(
            (float)Math.Cos(laser.BulletPosition.Angle),
            (float)Math.Sin(laser.BulletPosition.Angle)
        );

        List<Vector2> laserPath = [startPoint];
        Vector2 currentStart = startPoint;
        Vector2 currentDirection = direction;

        float remainingLength = laser.Length;
        int reflectionCount = 0;

        while (remainingLength > 0f && reflectionCount <= GameLogic.Constants.MAXIMUM_LASER_REFLECTION)
        {
            Vector2? hitPoint = null;
            Vector2? hitNormal = null;

            _world.RayCast((fixture, point, normal, fraction) =>
            {
                // Conditions that will ignore the collision
                if (
                    fixture.CollisionCategories != Categories.Wall
                    && fixture.CollisionCategories != Categories.Player
                )
                {
                    return -1f;
                }
                if (fixture.CollisionCategories == Categories.Player)
                {
                    Tag tag = (Tag)fixture.Body.Tag;
                    if (tag.Owner is not GameLogic.Player)
                    {
                        _logger.Error(
                            "Collision category doesn't match the expected type. Please contact the developer."
                        );
                        return -1f;
                    }

                    GameLogic.Player player = (GameLogic.Player)tag.Owner;
                    Vector2.Distance(ref currentStart, ref point, out float distance);
                    int damage = GameLogic.LaserBullet.CauculateDamage(
                        laser.BulletDamage,
                        remainingLength - distance,
                        laser.Length
                    );
                    player.Injured(damage, laser.AntiArmor, out bool reflected);
                    if (reflected == false)
                    {
                        return -1f;
                    }
                }

                hitPoint = point;
                hitNormal = fixture.Body.GetWorldVector(normal);
                return fraction;
            }, currentStart, currentStart + currentDirection * remainingLength);

            if (hitPoint.HasValue && hitNormal.HasValue)
            {
                laserPath.Add(hitPoint.Value);
                Vector2 hitPointValue = hitPoint.Value;
                Vector2.Distance(ref currentStart, ref hitPointValue, out float distance);
                remainingLength -= distance;

                currentDirection = Reflect(currentDirection, hitNormal.Value);

                // Fix the start point to avoid the laser being stuck in the wall
                currentStart = hitPoint.Value + currentDirection * RAYCAST_FIX_DELTA;
                remainingLength -= RAYCAST_FIX_DELTA;

                reflectionCount++;
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
            if (fixture.Body.Tag is not Tag)
            {
                _logger.Error(
                    "The fixture doesn't have a tag. Please contact the developer."
                );
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
                return fraction;
            }

            _logger.Error(
                "Failed to get the wall position from the fixture tag. Please contact the developer."
            );
            return -1f;

        }, startPoint, startPoint + direction * 2 * GameLogic.Constants.WALL_LENGTH);

        if (result is null)
        {
            _logger.Error("Failed to find target wall.");
            return null;
        }
        return new(result.X, result.Y, result.Angle);
    }
}
