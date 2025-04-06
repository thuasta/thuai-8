using nkast.Aether.Physics2D.Common;

namespace Thuai.Server.GameLogic;

public partial class Battle
{

    #region Fields and properties
    public List<Bullet> Bullets { get; } = [];
    public List<LaserBullet> ActivatedLasers { get; } = [];
    private readonly List<LaserBullet> _lasersToActivate = [];

    #endregion

    #region Methods

    private bool AddBullet(Bullet bullet)
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error("Cannot add bullet: The battle hasn't started or has ended.");
            return false;
        }
        try
        {
            Bullets.Add(bullet);

            _logger.Information($"Bullet {bullet.Id} has been added.");
            _logger.Debug("Speed: " + bullet.BulletSpeed);
            _logger.Debug("Damage: " + bullet.BulletDamage);
            _logger.Debug("AntiArmor: " + bullet.AntiArmor);

            return true;
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot add bullet:");
            Utility.Tools.LogHandler.LogException(_logger, e);
            return false;
        }
    }

    private void RemoveBullet(List<Bullet> bullets)
    {
        foreach (Bullet bullet in bullets)
        {
            RemoveBullet(bullet);
        }
    }

    private void RemoveBullet(Bullet bullet)
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

            if (bullet.Body is not null)
            {
                _env.RemoveBody(bullet.Body);
                bullet.Unbind();
            }
            Bullets.Remove(bullet);

            _logger.Information($"Bullet {bullet.Id} has been removed.");
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot remove bullet:");
            Utility.Tools.LogHandler.LogException(_logger, e);
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

        List<Bullet> toDelete = [];

        foreach (Bullet bullet in Bullets)
        {
            try
            {
                bullet.Update();
                if (bullet.IsDestroyed == true)
                {
                    toDelete.Add(bullet);
                }

            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to update bullet:");
                Utility.Tools.LogHandler.LogException(_logger, ex);
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
            _logger.Error($"Failed to apply laser:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
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
            List<Vector2> trace = _env.ActivateLaser(laser);
            laser.Trace = [.. trace];
            ActivatedLasers.Add(laser);

            _logger.Information($"A laser has been activated and reflected {trace.Count - 2} times.");
            _logger.Debug("Length: " + laser.Length);
            _logger.Debug("Damage: " + laser.BulletDamage);
            _logger.Debug("AntiArmor: " + laser.AntiArmor);
            _logger.Debug("Trace: " + string.Join(", ", trace.Select(v => v.ToString())));
        }
        catch (Exception ex)
        {
            _logger.Error($"Failed to activate laser:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }

    #endregion
}
