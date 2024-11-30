using Thuai.Server.GameController;

namespace Thuai.Server.GameLogic;

public partial class Game
{
    public void SubscribePlayerEvents(Player player)
    {
        player.PlayerMoveEvent += OnPlayerMove;
        player.PlayerTurnEvent += OnPlayerTurn;
        player.PlayerAttackEvent += OnPlayerAttack;
    }


    private void OnPlayerMove(object? sender, Player.PlayerMoveEventArgs e)
    {
        if (Stage != GameStage.InBattle)
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
                throw new NotImplementedException();
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