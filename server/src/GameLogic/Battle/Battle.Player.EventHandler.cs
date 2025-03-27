namespace Thuai.Server.GameLogic;

public partial class Battle
{
    public void SubscribePlayerEvents(Player player)
    {
        player.PlayerMoveEvent += OnPlayerMove;
        player.PlayerTurnEvent += OnPlayerTurn;
        player.PlayerAttackEvent += OnPlayerAttack;
        player.SkillActivationEvent += OnSkillActivation;
        player.SkillDeactivationEvent += OnSkillDeactivation;
    }
    public void UnsubscribePlayerEvents(Player player)
    {
        player.PlayerMoveEvent -= OnPlayerMove;
        player.PlayerTurnEvent -= OnPlayerTurn;
        player.PlayerAttackEvent -= OnPlayerAttack;
        player.SkillActivationEvent -= OnSkillActivation;
        player.SkillDeactivationEvent -= OnSkillDeactivation;
    }

    private void OnPlayerMove(object? sender, Player.PlayerMoveEventArgs e)
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error(
                $"[Player {e.Player.ID}] Cannot move when the battle is at stage {Stage}."
            );
            return;
        }
        try
        {
            lock (_lock)
            {
                if (e.Movedirection == MoveDirection.NONE)
                {
                    return;
                }

                float speed = e.Player.Speed;

                foreach (Player player in AllPlayers)
                {
                    if (
                        player.ID != e.Player.ID
                        && player.IsAlive == true
                        && player.PlayerArmor.GravityField == true
                        && PointDistance(player.PlayerPosition, e.Player.PlayerPosition) <= Constants.GRAVITY_FIELD_RADIUS
                    )
                    {
                        speed *= Constants.GRAVITY_FIELD_STRENGTH;
                        break;  // Gravity field only affects the player once.
                    }
                }

                float delta_x = speed * (float)Math.Cos(e.Player.PlayerPosition.Angle);
                float delta_y = speed * (float)Math.Sin(e.Player.PlayerPosition.Angle);
                if (e.Movedirection == MoveDirection.BACK)
                {
                    float endXpos = e.Player.PlayerPosition.Xpos - delta_x;
                    float endYpos = e.Player.PlayerPosition.Ypos - delta_y;
                    Position startPos = new(
                        e.Player.PlayerPosition.Xpos,
                        e.Player.PlayerPosition.Ypos,
                        e.Player.PlayerPosition.Angle - (float)Math.PI
                    );
                    Position endPos = new(endXpos, endYpos, e.Player.PlayerPosition.Angle);
                    Position? finalPos = GetPlayerFinalPos(startPos, endPos);
                    if (finalPos != null)
                    {
                        e.Player.PlayerPosition = finalPos;
                    }
                }
                else if (e.Movedirection == MoveDirection.FORTH)
                {
                    float endXpos = e.Player.PlayerPosition.Xpos + delta_x;
                    float endYpos = e.Player.PlayerPosition.Ypos + delta_y;
                    Position startPos = e.Player.PlayerPosition;
                    Position endPos = new(endXpos, endYpos);
                    Position? finalPos = GetPlayerFinalPos(startPos, endPos);
                    if (finalPos != null)
                    {
                        e.Player.PlayerPosition = finalPos;
                    }
                }
                else
                {
                    _logger.Error($"[Player {e.Player.ID}] Failed to move: MoveDirection is invalid!");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"[Player {e.Player.ID}] Failed to move:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }

    private void OnPlayerTurn(object? sender, Player.PlayerTurnEventArgs e)
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error(
                $"[Player {e.Player.ID}] Cannot turn when the battle is at stage {Stage}."
            );
            return;
        }
        try
        {
            lock (_lock)
            {
                if (e.Turndirection == TurnDirection.NONE)
                {
                    return;
                }

                float delta_angle = e.Player.TurnSpeed;
                if (e.Turndirection == TurnDirection.CLOCKWISE)
                {
                    e.Player.PlayerPosition.Angle -= delta_angle;
                }
                else if (e.Turndirection == TurnDirection.COUNTER_CLOCKWISE)
                {
                    e.Player.PlayerPosition.Angle += delta_angle;
                }
                else
                {
                    _logger.Error($"[Player {e.Player.ID}] Failed to turn: TurnDirection is invalid!");
                }

            }
        }
        catch (Exception ex)
        {
            _logger.Error($"[Player {e.Player.ID}] Failed to turn:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
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
                if (e.Player.PlayerWeapon.CurrentBullets > 0)
                {
                    e.Player.PlayerWeapon.CurrentBullets -= 1;
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
                            bulletPosition,
                            e.Player.PlayerWeapon.BulletSpeed,
                            e.Player.PlayerWeapon.Damage,
                            e.Player.PlayerWeapon.AntiArmor
                        );
                        AddBullet(bullet);
                    }
                    else
                    {
                        LaserBullet laserBullet = new(
                            bulletPosition,
                            e.Player.PlayerWeapon.BulletSpeed,
                            e.Player.PlayerWeapon.Damage,
                            e.Player.PlayerWeapon.AntiArmor
                        );
                        Apply_Laser(laserBullet);
                    }
                }
                else
                {
                    _logger.Error($"[Player {e.Player.ID}] Failed to attack: no bullets!");
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
