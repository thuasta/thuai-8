namespace Thuai.Server.GameLogic;

public partial class Battle
{
    #region Fields and properties
    public List<Trap> Traps { get; } = [];

    #endregion

    #region Methods

    private bool AddTrap(Trap trap)
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error("Cannot add Trap: The battle hasn't started or has ended.");
            return false;
        }
        try
        {
            Traps.Add(trap);

            _logger.Information("A trap has been added.");

            return true;
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot add Trap: {e.Message}");
            Utility.Tools.LogHandler.LogException(_logger, e);
            return false;
        }
    }

    private void RemoveTrap(List<Trap> Traps)
    {
        foreach (Trap trap in Traps)
        {
            RemoveTrap(trap);
        }
    }

    private void RemoveTrap(Trap trap)
    {
        try
        {
            if (trap.Body is not null)
            {
                _env.RemoveBody(trap.Body);
                trap.Unbind();
            }
            else
            {
                _logger.Warning($"The trap to be removed has no body. Please contact the developer.");
            }

            Traps.Remove(trap);

            _logger.Information("A trap has been removed.");
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot remove Trap: {e.Message}");
            Utility.Tools.LogHandler.LogException(_logger, e);
        }
    }

    private void UpdateTraps()
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error(
                $"Trap Cannot be updated when the battle is at stage {Stage}."
            );
            return;
        }

        List<Trap> toDelete = [];

        foreach (Trap trap in Traps)
        {
            try
            {
                trap.Update();
                if (trap.IsDestroyed == true)
                {
                    toDelete.Add(trap);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to update trap: {ex.Message}");
                Utility.Tools.LogHandler.LogException(_logger, ex);
            }
        }

        RemoveTrap(toDelete);

        _logger.Debug($"Traps updated.");
    }

    #endregion
}
