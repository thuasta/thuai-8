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
                    Position endPos = new(endXpos, endYpos);
                    Position? finalPos = GetFinalPos(startPos, endPos);
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
                    Position? finalPos = GetFinalPos(startPos, endPos);
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
        throw new NotImplementedException();
    }

    private void OnPlayerAttack(object? sender, Player.PlayerAttackEventArgs e)
    {
        throw new NotImplementedException();
    }

}