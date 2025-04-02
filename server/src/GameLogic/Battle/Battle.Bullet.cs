namespace Thuai.Server.GameLogic;

public partial class Battle
{

    #region Fields and properties
    public List<IBullet> Bullets { get; } = [];
    public List<LaserBullet> ActivatedLasers { get; } = [];
    private readonly List<LaserBullet> _lasersToActivate = [];

    #endregion

    #region Methods

    private bool AddBullet(IBullet bullet)
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error("Cannot add bullet: The battle hasn't started or has ended.");
            return false;
        }
        try
        {
            Bullets.Add(bullet);

            _logger.Debug(
                $"A bullet has been added at ({bullet.BulletPosition.Xpos:F2}, {bullet.BulletPosition.Ypos:F2})"
                + $" with angle {bullet.BulletPosition.Angle:F2}"
            );
            _logger.Verbose("Type: " + bullet.Type.ToString());
            _logger.Verbose("Speed: " + bullet.BulletSpeed);
            _logger.Verbose("Damage: " + bullet.BulletDamage);
            _logger.Verbose("AntiArmor: " + bullet.AntiArmor);

            return true;
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot add bullet: {e.Message}");
            _logger.Debug($"{e}");
            return false;
        }
    }

    private void RemoveBullet(List<IBullet> bullets)
    {
        foreach (IBullet bullet in bullets)
        {
            RemoveBullet(bullet);
        }
    }

    private void RemoveBullet(IBullet bullet)
    {
        try
        {
            if (bullet.Owner.CurrentBullets >= bullet.Owner.MaxBullets)
            {
                _logger.Error("Unexpected bullet deletion. Please contact the developer.");
            }
            else
            {
                ++bullet.Owner.CurrentBullets;
            }

            if (bullet is Bullet b && b.Body is not null)
            {
                _env.RemoveBody(b.Body);
                b.Unbind();
            }
            Bullets.Remove(bullet);

            _logger.Debug($"A bullet at ({bullet.BulletPosition.Xpos:F2}, {bullet.BulletPosition.Ypos:F2}) has been removed.");
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot remove bullet: {e.Message}");
            _logger.Debug($"{e}");
        }
    }

    private void UpdateBullets()
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error(
                $"Bullet Cannot be updated when the battle is at stage {Stage}."
            );
            return;
        }

        List<IBullet> toDelete = [];

        foreach (IBullet bullet in Bullets)
        {
            try
            {
                if (bullet is Bullet b)
                {
                    b.Update();
                    if (b.IsDestroyed == true)
                    {
                        toDelete.Add(b);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Bullet Failed to be updated: {ex.Message}");
                _logger.Debug($"{ex}");
            }
        }

        RemoveBullet(toDelete);

        _logger.Debug($"Bullets updated.");
    }

    private void ApplyLaser(LaserBullet laserBullet)
    {
        try
        {
            _lasersToActivate.Add(laserBullet);
        }
        catch (Exception ex)
        {
            _logger.Error($"Laser failed to take damage: {ex.Message}");
            _logger.Debug($"{ex}");
        }
    }

    private void ActivateLasers(List<LaserBullet> lasers)
    {
        foreach (LaserBullet laser in lasers)
        {
            ActivateLaser(laser);
        }
    }

    private void ActivateLaser(LaserBullet laser)
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error("Cannot activate laser: The battle hasn't started or has ended.");
            return;
        }
        try
        {
            // TODO: Implement laser activation logic
        }
        catch (Exception ex)
        {
            _logger.Error($"Laser failed to be activated: {ex.Message}");
            _logger.Debug($"{ex}");
        }
    }

    #endregion
}
