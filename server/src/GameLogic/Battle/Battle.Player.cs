
namespace Thuai.Server.GameLogic;

public partial class Battle
{
    #region Fields and properties
    public List<Player> AllPlayers { get; private set; } = players;

    public int PlayerCount => AllPlayers.Count;

    #endregion

    #region Methods

    /// <summary>
    /// Update the players. Called every tick.
    /// </summary>
    public void UpdatePlayers()
    {
        foreach (Player player in AllPlayers)
        {
            player.Update();
        }
        _logger.Debug("Players updated.");
    }

    public void UpdatePlayerSpeed()
    {
        foreach (Player player in AllPlayers)
        {
            player.UpdateSpeed();
        }
        _logger.Debug("Speed of players updated.");
    }

    /// <summary>
    /// Choose spawnpoint for all the players.
    /// </summary>
    /// <exception cref="Exception">Throws if map is null.</exception>
    public void ChooseSpawnpoint()
    {
        if (Map == null)
        {
            throw new Exception("No available map!");
        }

        foreach (Player player in AllPlayers)
        {
            float x = Constants.WALL_LENGTH * (_random.Next(0, Map.Width) + 0.5f);
            float y = Constants.WALL_LENGTH * (_random.Next(0, Map.Height) + 0.5f);
            float angle = _random.Next(
                0, (int)(2 * Math.PI / Constants.MAXIMUM_TURN_SPEED)
            ) * Constants.MAXIMUM_TURN_SPEED;
            player.PlayerPosition = new Position(x, y, angle);

            _logger.Information($"Player {player.ID} spawned at ({x:F2}, {y:F2}) with angle {angle:F2} rad.");
        }
    }

    /// <summary>
    /// Get the count of the alive players.
    /// </summary>
    /// <returns>Count of alive players.</returns>
    private int AlivePlayers()
    {
        int count = 0;
        foreach (Player player in AllPlayers)
        {
            if (player.IsAlive)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Get the player with the highest HP.
    /// </summary>
    /// <returns>The player, null if more than one players have the highest HP.</returns>
    /// <remarks>
    /// Returns null if more than one players have the highest HP.
    /// </remarks>
    private Player? PlayerWithHighestHP()
    {
        Player? player = null;
        int playerCount = 0;
        foreach (Player p in AllPlayers)
        {
            if (player == null || p.PlayerArmor.Health > player.PlayerArmor.Health)
            {
                player = p;
                playerCount = 1;
            }
            else if (p.PlayerArmor.Health == player.PlayerArmor.Health)
            {
                playerCount++;
            }
        }

        if (playerCount != 1)
        {
            return null;
        }
        return player;
    }

    #endregion

}