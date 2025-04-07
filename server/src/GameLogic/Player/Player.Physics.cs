using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

namespace Thuai.Server.GameLogic;

public partial class Player : Physics.IPhysicalObject
{
    public Body? Body { get; private set; }
    public bool Enabled { get; set; } = true;

    public Vector2 Orientation => new(
        (float)Math.Cos(Body?.Rotation ?? 0),
        (float)Math.Sin(Body?.Rotation ?? 0)
    );

    public void Bind(Body body)
    {
        Body = body;
        Body.Tag = new Physics.Tag() { Owner = this };

        Physics.Tag tag = (Physics.Tag)Body!.Tag;
        tag.AttachedData[Physics.Key.CoveredFields] = 0;
        tag.AttachedData[Physics.Key.SpeedUpFactor] = 1f;

        Body.OnCollision += OnCollision;
        Body.OnSeparation += OnSeparation;
    }

    public void Unbind()
    {
        if (Body is null)
        {
            return;
        }

        Body.OnCollision -= OnCollision;
        Body.OnSeparation -= OnSeparation;
        Body.Tag = new();
        Body = null;
    }

    public void AppendGravityField()
    {
        if (PlayerArmor.GravityField == false)
        {
            return;
        }
        if (Body is null)
        {
            return;
        }
        foreach (Fixture f in Body.FixtureList)
        {
            if (f.CollisionCategories == Physics.Environment.Categories.GravityField)
            {
                return;
            }
        }

        Fixture fixture = Body.CreateCircle(Constants.GRAVITY_FIELD_RADIUS, Physics.Environment.DEFAULT_DENSITY);
        fixture.CollisionCategories = Physics.Environment.Categories.GravityField;
        fixture.CollidesWith = Physics.Environment.CollisionList.GravityFieldCollidesWith;
        fixture.IsSensor = true;
        fixture.Tag = new Physics.Tag() { Owner = this };
    }

    private bool OnCollision(Fixture a, Fixture b, Contact contact)
    {
        if (a.CollisionCategories == Physics.Environment.Categories.GravityField)
        {
            if (b.Body.Tag is not Physics.Tag)
            {
                _logger.Error("Gravity field collided with an object without a tag.");
                return true;
            }

            Physics.Tag tag = (Physics.Tag)b.Body.Tag;

            if (tag.AttachedData.ContainsKey(Physics.Key.CoveredFields) == false
                || tag.AttachedData[Physics.Key.CoveredFields] is not int)
            {
                tag.AttachedData[Physics.Key.CoveredFields] = 0;
            }

            tag.AttachedData[Physics.Key.CoveredFields] = (int)tag.AttachedData[Physics.Key.CoveredFields] + 1;

            if (tag.Owner is Player player && player.IsInvulnerable == true)
            {
                _logger.Debug($"Target player {player.ID} is invulnerable to gravity field.");
                return false;
            }
            if ((int)tag.AttachedData[Physics.Key.CoveredFields] == 1)
            {
                b.Body.LinearVelocity *= Constants.GRAVITY_FIELD_STRENGTH;
                b.Body.AngularVelocity *= Constants.GRAVITY_FIELD_STRENGTH;
            }

            return true;
        }

        // Below handles the case when the fixture is a player

        if (IsAlive == false)
        {
            _logger.Debug($"Player {ID} is dead. Collision will be ignored.");
            return false;
        }
        if (Kamui == true)
        {
            // Kamui is active, so the player won't be affected by anything
            _logger.Debug("Collision will be ignored because Kamui is active.");
            return false;
        }

        if (b.Body.Tag is Physics.Tag bodyTag)
        {
            switch (bodyTag.Owner)
            {
                case Bullet bullet:
                    if (bullet.IsDestroyed == true)
                    {
                        _logger.Debug($"Bullet {bullet.Id} is already destroyed.");
                        return false;
                    }

                    Injured(bullet.BulletDamage, bullet.AntiArmor, out bool reflected);
                    if (reflected == false)
                    {
                        b.Body.LinearVelocity = Vector2.Zero;
                        bullet.Enabled = false;
                    }
                    else
                    {
                        Vector2 relativeVelocity = b.Body.LinearVelocity - a.Body.LinearVelocity;
                        contact.GetWorldManifold(out Vector2 normal, out _);
                        relativeVelocity = Physics.Environment.Reflect(relativeVelocity, normal);
                        b.Body.LinearVelocity = relativeVelocity + a.Body.LinearVelocity;
                        // TODO: There exists a method to speed up a bullet to a extremely high speed.
                        // This should be fixed (or just make it an easter egg?).
                    }
                    return false;

                case LaserBullet:
                    _logger.Error("Unexpected collision with laser bullet. Please contact the developer.");
                    return false;

                case Trap trap:
                    if (trap.Owner.ID == ID)
                    {
                        _logger.Debug($"Player won't be affected by its own trap.");
                        return false;
                    }
                    if (trap.IsDestroyed == true)
                    {
                        _logger.Debug($"Trap is already destroyed.");
                        return false;
                    }

                    // Trap is activated even if the player is invulnerable
                    trap.Enabled = false;
                    if (IsInvulnerable == true)
                    {
                        _logger.Information($"Player {ID} is invulnerable to trap.");
                        return false;
                    }
                    _logger.Information($"Player {ID} is caught by a trap.");
                    _stunCounter.Reset();

                    // Set player's speed to zero
                    a.Body.LinearVelocity = Vector2.Zero;
                    a.Body.AngularVelocity = 0f;

                    return true;

                default:
                    return true;
            }
        }
        return true;
    }

    private void OnSeparation(Fixture a, Fixture b, Contact contact)
    {
        if (a.CollisionCategories == Physics.Environment.Categories.GravityField)
        {
            if (b.Body.Tag is not Physics.Tag)
            {
                _logger.Error("Gravity field seperated with an object without a tag.");
                return;
            }

            Physics.Tag tag = (Physics.Tag)b.Body.Tag;
            if (tag.AttachedData.ContainsKey(Physics.Key.CoveredFields) == false
                || tag.AttachedData[Physics.Key.CoveredFields] is not int)
            {
                _logger.Error("Gravity field seperated with an object without a \"coveredFields\" tag.");
                return;
            }
            if ((int)tag.AttachedData[Physics.Key.CoveredFields] < 0)
            {
                _logger.Error("Value of \"coveredFields\" tag if negative. Please contact the developer.");
                return;
            }
            if ((int)tag.AttachedData[Physics.Key.CoveredFields] == 0)
            {
                _logger.Debug("Target is no longer slowed down.");
                return;
            }

            tag.AttachedData[Physics.Key.CoveredFields] = (int)tag.AttachedData[Physics.Key.CoveredFields] - 1;

            if (tag.Owner is Player player && player.IsInvulnerable == true)
            {
                // Object was not affected by the gravity field before separation
                _logger.Debug($"Target player {player.ID} is invulnerable to gravity field.");
                return;
            }
            if ((int)tag.AttachedData[Physics.Key.CoveredFields] == 0)
            {
                // Object has been affected by the gravity field before separation
                b.Body.LinearVelocity /= Constants.GRAVITY_FIELD_STRENGTH;
                b.Body.AngularVelocity /= Constants.GRAVITY_FIELD_STRENGTH;
            }
        }
    }
}
