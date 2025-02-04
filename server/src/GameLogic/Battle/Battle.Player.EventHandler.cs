using Thuai.Server.GameController;

namespace Thuai.Server.GameLogic;

public partial class Battle
{
    public void SubscribePlayerEvents(Player player)
    {
        player.PlayerMoveEvent += OnPlayerMove;
        player.PlayerTurnEvent += OnPlayerTurn;
        player.PlayerAttackEvent += OnPlayerAttack;
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
                double delta_x = e.Player.Speed * Math.Cos(e.Player.PlayerPosition.Angle);
                double delta_y = e.Player.Speed * Math.Sin(e.Player.PlayerPosition.Angle);
                if (e.Movedirection == MoveDirection.BACK)
                {
                    double endXpos = e.Player.PlayerPosition.Xpos - delta_x;
                    double endYpos = e.Player.PlayerPosition.Ypos - delta_y;
                    Position startPos = new(e.Player.PlayerPosition.Xpos, e.Player.PlayerPosition.Ypos, e.Player.PlayerPosition.Angle - Math.PI);
                    Position endPos = new(endXpos, endYpos, e.Player.PlayerPosition.Angle);
                    Position? finalPos = GetPlayerFinalPos(startPos, endPos);
                    if (finalPos != null)
                    {
                        e.Player.PlayerPosition = finalPos;
                    }
                }
                else if (e.Movedirection == MoveDirection.FORTH)
                {
                    double endXpos = e.Player.PlayerPosition.Xpos + delta_x;
                    double endYpos = e.Player.PlayerPosition.Ypos + delta_y;
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
            _logger.Error($"[Player {e.Player.ID}] Failed to move: {ex.Message}");
            _logger.Debug($"{ex}");
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
                double delta_angle = e.Player.TurnSpeed;
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
            _logger.Error($"[Player {e.Player.ID}] Failed to turn: {ex.Message}");
            _logger.Debug($"{ex}");
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
                if (e.Player.PlayerWeapon.currentBullets > 0)
                {
                    e.Player.PlayerWeapon.currentBullets -= 1;
                    double delta_x = Constants.WALL_THICK * Math.Cos(e.Player.PlayerPosition.Angle);
                    double delta_y = Constants.WALL_THICK * Math.Sin(e.Player.PlayerPosition.Angle);
                    Position bulletPosition = new(e.Player.PlayerPosition.Xpos + delta_x, e.Player.PlayerPosition.Ypos + delta_y, e.Player.PlayerPosition.Angle);
                    //The bullet will be spawned in front of the player!
                    if (e.Player.PlayerWeapon.isLaser == false)
                    {
                        Bullet bullet = new(bulletPosition, e.Player.PlayerWeapon.bulletSpeed, e.Player.PlayerWeapon.damage, e.Player.PlayerWeapon.antiArmor);
                        AddBullet(bullet);
                    }
                    else
                    {
                        LaserBullet laserBullet = new(bulletPosition, e.Player.PlayerWeapon.bulletSpeed, e.Player.PlayerWeapon.damage, e.Player.PlayerWeapon.antiArmor);
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
            _logger.Error($"[Player {e.Player.ID}] Failed to attack: {ex.Message}");
            _logger.Debug($"{ex}");
        }
    }

}