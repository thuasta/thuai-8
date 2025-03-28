using nkast.Aether.Physics2D.Common;

namespace Thuai.Server.GameLogic;

public partial class Battle
{
    public void UpdateGravityFieldCoverage()
    {
        _logger.Debug("Updating gravity field coverage.");

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

        // Update the covered fields count for all players and bullets
        foreach (Player player in AllPlayers)
        {
            if (player.Body is null)
            {
                _logger.Error($"Player {player.ID} has no body. Ignoring.");
                continue;
            }

            if (player.PlayerArmor.GravityField == true)
            {
                foreach (Player target in AllPlayers)
                {
                    if (target.ID == player.ID)
                    {
                        _logger.Debug("Gravity field won't affect the player itself.");
                        continue;
                    }

                    if (target.Body is null)
                    {
                        _logger.Error($"Player {target.ID} has no body.");
                        continue;
                    }

                    Physics.Tag tag = (Physics.Tag)target.Body.Tag;

                    Vector2 playerPosition = player.Body.Position;
                    Vector2 targetPosition = target.Body.Position;

                    Vector2.Distance(
                        ref playerPosition, ref targetPosition, out float distance
                    );
                    if (distance < Constants.PLAYER_RADIUS + Constants.GRAVITY_FIELD_RADIUS)
                    {
                        tag.AttachedData[Physics.Key.CoveredFields] =
                            (int)tag.AttachedData[Physics.Key.CoveredFields] + 1;
                        _logger.Debug($"Player {target.ID} is covered by player {player.ID}'s gravity field.");
                    }
                }

                foreach (IBullet bullet in Bullets)
                {
                    if (bullet is Bullet target)
                    {
                        if (target.Body is null)
                        {
                            _logger.Error($"Bullet {target.Id} has no body.");
                            continue;
                        }

                        Physics.Tag tag = (Physics.Tag)target.Body.Tag;

                        Vector2 playerPosition = player.Body.Position;
                        Vector2 targetPosition = target.Body.Position;

                        Vector2.Distance(
                            ref playerPosition, ref targetPosition, out float distance
                        );
                        if (distance < Constants.BULLET_RADIUS + Constants.GRAVITY_FIELD_RADIUS)
                        {
                            tag.AttachedData[Physics.Key.CoveredFields] =
                                (int)tag.AttachedData[Physics.Key.CoveredFields] + 1;
                            _logger.Debug($"Bullet {target.Id} is covered by player {player.ID}'s gravity field.");
                        }
                    }
                }
            }
        }
    }
}
