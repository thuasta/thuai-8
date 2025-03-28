namespace Thuai.Server.GameLogic;

public partial class Battle
{
    public void SubscribePlayerEvents(Player player)
    {
        player.PlayerAttackEvent += OnPlayerAttack;
        player.SkillActivationEvent += OnSkillActivation;
        player.SkillDeactivationEvent += OnSkillDeactivation;
    }
    public void UnsubscribePlayerEvents(Player player)
    {
        player.PlayerAttackEvent -= OnPlayerAttack;
        player.SkillActivationEvent -= OnSkillActivation;
        player.SkillDeactivationEvent -= OnSkillDeactivation;
    }

    private void OnPlayerAttack(object? sender, Player.PlayerAttackEventArgs e)
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error(
                $"[Player {e.Player.ID}] Cannot attack when the battle is at stage {Stage}."
            );
            return;
        }

        try
        {
            lock (_lock)
            {
                float delta_x = Constants.BULLET_GENERATE_DISTANCE * (float)Math.Cos(e.Player.PlayerPosition.Angle);
                float delta_y = Constants.BULLET_GENERATE_DISTANCE * (float)Math.Sin(e.Player.PlayerPosition.Angle);
                Position bulletPosition = new(
                    e.Player.PlayerPosition.Xpos + delta_x,
                    e.Player.PlayerPosition.Ypos + delta_y,
                    e.Player.PlayerPosition.Angle
                );

                if (e.Player.PlayerWeapon.IsLaser == false)
                {
                    Bullet bullet = new(
                        e.Player.PlayerWeapon.BulletSpeed,
                        e.Player.PlayerWeapon.Damage,
                        e.Player.PlayerWeapon.AntiArmor
                    )
                    {
                        Owner = e.Player.PlayerWeapon
                    };
                    bullet.Bind(
                        _env.CreateBody(
                            Physics.Environment.Categories.Bullet,
                            new(bulletPosition.Xpos, bulletPosition.Ypos),
                            bulletPosition.Angle
                        )
                    );
                    AddBullet(bullet);
                }
                else
                {
                    // TODO: Implement laser application
                    LaserBullet laserBullet = new(
                        bulletPosition,
                        e.Player.PlayerWeapon.BulletSpeed,
                        e.Player.PlayerWeapon.Damage,
                        e.Player.PlayerWeapon.AntiArmor
                    )
                    {
                        Owner = e.Player.PlayerWeapon
                    };

                    ApplyLaser(laserBullet);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"[Player {e.Player.ID}] Failed to attack:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }

    private void OnSkillActivation(object? sender, Player.SkillActivationEventArgs e)
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error(
                $"[Player {e.Player.ID}] Cannot activate skill when battle is at state {Stage}."
            );
            return;
        }
        try
        {
            lock (_lock)
            {
                switch (e.SkillName)
                {
                    case SkillName.BLACK_OUT:
                        foreach (Player player in AllPlayers)
                        {
                            if (player.ID != e.Player.ID && player.IsAlive == true && player.IsBlinded == false)
                            {
                                player.IsBlinded = true;
                                _logger.Information($"[Player {player.ID}] is blinded.");
                            }
                        }
                        break;

                    case SkillName.SPEED_UP:
                        _logger.Information($"[Player {e.Player.ID}] activated SpeedUp skill.");
                        break;

                    default:
                        _logger.Error($"[Player {e.Player.ID}] Invalid skill name: {e.SkillName}");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"[Player {e.Player.ID}] Failed to activate skill:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }

    private void OnSkillDeactivation(object? sender, Player.SkillDeactivationEventArgs e)
    {
        // Deactivation works in any stage.
        try
        {
            lock (_lock)
            {
                switch (e.SkillName)
                {
                    case SkillName.BLACK_OUT:
                        foreach (Player player in AllPlayers)
                        {
                            if (player.ID != e.Player.ID && player.IsBlinded == true)
                            {
                                player.IsBlinded = false;
                                _logger.Information($"[Player {player.ID}] recovered from blindness.");
                            }
                        }
                        break;

                    case SkillName.SPEED_UP:
                        _logger.Information($"[Player {e.Player.ID}] deactivated SpeedUp skill.");
                        break;

                    default:
                        _logger.Error($"[Player {e.Player.ID}] Invalid skill name: {e.SkillName}");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"[Player {e.Player.ID}] Failed to deactivate skill:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }
}
