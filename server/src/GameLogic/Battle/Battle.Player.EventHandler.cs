using nkast.Aether.Physics2D.Common;

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
            _logger.Debug(
                $"[Player {e.Player.ID}] Cannot attack when the battle is at stage {Stage}."
            );
            return;
        }

        try
        {
            lock (_lock)
            {
                Vector2 delta = e.Player.Orientation * Constants.BULLET_GENERATE_DISTANCE;
                Position bulletPosition = new(
                    e.Player.PlayerPosition.Xpos + delta.X,
                    e.Player.PlayerPosition.Ypos + delta.Y,
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
                    LaserBullet laserBullet = new(
                        bulletPosition,
                        e.Player.PlayerWeapon.BulletSpeed,
                        e.Player.PlayerWeapon.Damage,
                        e.Player.PlayerWeapon.LaserLength,
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
            _logger.Debug(
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
                                _logger.Information($"[Player {player.ID}] Player is blinded.");
                            }
                        }
                        break;

                    case SkillName.SPEED_UP:
                        _logger.Information($"[Player {e.Player.ID}] Activated SpeedUp skill.");
                        break;

                    case SkillName.FLASH:
                        UpdateGravityFieldCoverage();
                        break;

                    case SkillName.DESTROY:
                        MapGeneration.Wall? target = _env.GetFacingEdge(
                            new(e.Player.PlayerPosition.Xpos, e.Player.PlayerPosition.Ypos),
                            e.Player.Orientation
                        );
                        if (target is null)
                        {
                            _logger.Error($"[Player {e.Player.ID}] No edge found to destroy.");
                        }
                        else
                        {
                            _logger.Information(
                                $"[Player {e.Player.ID}] Destroy skill"
                                + $" targeted edge at ({target.X}, {target.Y})"
                                + $" with angle {target.Angle}."
                            );
                            RemoveWall(target);
                        }
                        break;

                    case SkillName.CONSTRUCT:
                        MapGeneration.Wall? edge = _env.GetFacingEdge(
                            new(e.Player.PlayerPosition.Xpos, e.Player.PlayerPosition.Ypos),
                            e.Player.Orientation
                        );
                        if (edge is null)
                        {
                            _logger.Error($"[Player {e.Player.ID}] No edge found to construct.");
                        }
                        else
                        {
                            _logger.Information(
                                $"[Player {e.Player.ID}] Construct skill"
                                + $" targeted edge at ({edge.X}, {edge.Y})"
                                + $" with angle {edge.Angle}."
                            );

                            MapGeneration.Wall wall = new(edge.X, edge.Y, edge.Angle, true);
                            AddWall(wall);
                        }
                        break;

                    case SkillName.TRAP:
                        Trap trap = new() { Owner = e.Player };
                        Position trapPosition = new(
                            e.Player.PlayerPosition.Xpos,
                            e.Player.PlayerPosition.Ypos,
                            e.Player.PlayerPosition.Angle
                        );
                        trap.Bind(
                            _env.CreateBody(
                                Physics.Environment.Categories.Trap,
                                new(trapPosition.Xpos, trapPosition.Ypos),
                                trapPosition.Angle
                            )
                        );
                        AddTrap(trap);
                        break;

                    case SkillName.RECOVER:
                        _logger.Information($"[Player {e.Player.ID}] Activated Recover skill.");
                        break;

                    case SkillName.KAMUI:
                        _logger.Information($"[Player {e.Player.ID}] Activated Kamui skill.");
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
                                _logger.Information($"[Player {player.ID}] Recovered from blindness.");
                            }
                        }
                        break;

                    case SkillName.SPEED_UP:
                        _logger.Information($"[Player {e.Player.ID}] Effect of SpeedUp skill ends.");
                        break;

                    case SkillName.KAMUI:
                        _logger.Information($"[Player {e.Player.ID}] Effect of Kamui skill ends.");
                        break;

                    // Instant skills do not have deactivation effects.
                    case SkillName.FLASH:
                    case SkillName.DESTROY:
                    case SkillName.CONSTRUCT:
                    case SkillName.TRAP:
                    case SkillName.RECOVER:
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
