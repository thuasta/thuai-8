
using Thuai.Server.GameController;

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
            // Update the players.
        }
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
        // TODO: implement.
    }

    /// <summary>
    /// Get the count of the alive players.
    /// </summary>
    /// <returns>Count of alive players.</returns>
    private int AlivePlayers()
    {
        // TODO: implement.
        return 0;
    }

    /// <summary>
    /// Get the player with the highest HP.
    /// </summary>
    /// <returns>The player, null if more than one players have the highest 
    /// HP.</returns>
    /// <remarks>
    /// Returns null if more than one players have the highest HP.
    /// </remarks>
    private Player? PlayerWithHighestHP()
    {
        // TODO: implement
        return null;
    }

    #endregion

}