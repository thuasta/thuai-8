namespace Thuai.Server.GameLogic;

public partial class Battle
{

    #region Fields and properties
    private List<IBullet> Bullets = [];

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
            return true;
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot add bullet: {e.Message}");
            _logger.Debug($"{e}");
            return false;
        }
    }

    public void RemoveBullet(IBullet bullet)
    {
        try
        {
            Bullets.Remove(bullet);
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot remove bullet: {e.Message}");
            _logger.Debug($"{e}");
        }
    }

    private double ProjectLength(Position playerPos, Position line)
    {
        double dx = playerPos.Xpos - line.Xpos;
        double dy = playerPos.Ypos - line.Ypos;

        double angleInRadians = (double)(line.Angle * Math.PI / 180.0);
        double lineDirX = (double)Math.Cos(angleInRadians);
        double lineDirY = (double)Math.Sin(angleInRadians);

        double projectionLength = dx * lineDirX + dy * lineDirY;

        return projectionLength;
    }

    private Player? TakeDamage(Position startPos, Position endPos)
    {
        List<Player> tempPlayers = [];
        double min_proj = double.MaxValue;
        Player? finalPlayer = null;
        double line_len = PointDistance(startPos, endPos);
        foreach (Player player in AllPlayers)
        {
            if (LineDistance(startPos, player.PlayerPosition) < Constants.PLAYER_RADIO)
            {
                tempPlayers.Add(player);
            }
        }
        foreach (Player player in tempPlayers)
        {
            double tempProj = ProjectLength(player.PlayerPosition, startPos);
            if (tempProj > -Constants.PLAYER_RADIO && tempProj <= line_len)
            {
                if (min_proj > tempProj)
                {
                    min_proj = tempProj;
                    finalPlayer = player;
                }
            }
        }
        return finalPlayer;
    }

    private void UpdateBullets()
    {
        foreach (IBullet bullet in Bullets)
        {
            if (Stage != BattleStage.InBattle)
            {
                _logger.Error(
                    $"Bullet Cannot be updated when the battle is at stage {Stage}."
                );
                return;
            }
            try
            {
                lock (_lock)
                {
                    double delta_x = bullet.BulletSpeed * Math.Cos(bullet.BulletPosition.Angle);
                    double delta_y = bullet.BulletSpeed * Math.Sin(bullet.BulletPosition.Angle);
                    Position endPos = new(delta_x, delta_y);
                    Position? interPos = null;
                    Position? finalPos = GetBulletFinalPos(bullet.BulletPosition, endPos, out interPos);
                    if (finalPos != null)
                    {
                        Player? finalPlayer = null;
                        if (interPos != null)
                        {
                            finalPlayer = TakeDamage(bullet.BulletPosition, interPos);
                            if (finalPlayer != null)
                            {
                                finalPlayer.Injured(bullet.BulletDamage);
                                RemoveBullet(bullet);
                                continue;
                            }
                            finalPlayer = TakeDamage(interPos, finalPos);
                        }
                        else
                        {
                            finalPlayer = TakeDamage(bullet.BulletPosition, finalPos);
                        }
                        if (finalPlayer != null)
                        {
                            finalPlayer.Injured(bullet.BulletDamage);
                            RemoveBullet(bullet);
                            continue;
                        }

                        bullet.BulletPosition = finalPos;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Bullet Failed to be uodated: {ex.Message}");
                _logger.Debug($"{ex}");
            }
        }
    }
    private void Apply_Laser(LaserBullet laserBullet)
    {
        try
        {
            lock (_lock)
            {
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Laser failed to take damage: {ex.Message}");
            _logger.Debug($"{ex}");
        }

    }

    #endregion
}
