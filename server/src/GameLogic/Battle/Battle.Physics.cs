namespace Thuai.Server.GameLogic;

public partial class Battle
{
    public void UpdateGravityFieldCoverage()
    {
        // Reset the covered fields count for all players and bullets
        foreach (Player player in AllPlayers)
        {
            if (player.Body is null)
            {
                _logger.Error($"Player {player.ID} has no body.");
                continue;
            }

            Physics.Tag tag = (Physics.Tag)player.Body.Tag;
            tag.AttachedData[Physics.Key.CoveredFields] = 0;
        }
        foreach (IBullet bullet in Bullets)
        {
            if (bullet is Bullet b)
            {
                if (b.Body is null)
                {
                    _logger.Error($"Bullet {b.Id} has no body.");
                    continue;
                }

                Physics.Tag tag = (Physics.Tag)b.Body.Tag;
                tag.AttachedData[Physics.Key.CoveredFields] = 0;
            }
        }
        
        // TODO: Update the covered fields count for all players and bullets
    }
}
