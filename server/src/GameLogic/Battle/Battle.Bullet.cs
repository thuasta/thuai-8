namespace Thuai.Server.GameLogic;

public partial class Battle {

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

    private void UpdateBullets()
    {
        foreach (IBullet bullet in Bullets)
        {
            // TODO: implement.
        }
    }

    #endregion
}
