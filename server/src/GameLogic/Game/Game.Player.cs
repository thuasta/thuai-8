
using Thuai.Server.GameController;

namespace Thuai.Server.GameLogic;

public partial class Game
{

    #region Fields and properties
    public List<Player> AllPlayers { get; private set; } = [];

    public int PlayerCount => AllPlayers.Count;

    /// <summary>
    /// Record the scores of the players.
    /// </summary>
    public Dictionary<Player, int> Scoreboard { get; private set; } = [];

    #endregion

    #region Methods

    /// <summary>
    /// Add player in the game.
    /// </summary>
    /// <param name="playerId">The player to be added.</param>
    /// <returns>If the adding succeeds.</returns>
    public bool AddPlayer(string token, int playerId)
    {
        if (Stage != GameStage.Waiting)
        {
            _logger.Error("Cannot add player: The game is already started.");
            return false;
        }

        try
        {
            lock (_lock)
            {
                Player player = new(token, playerId)
                {
                    ID = playerId
                };
                AllPlayers.Add(player);
                Scoreboard.Add(player, 0);
                // SubscribePlayerEvents(player);
                return true;
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot add player: {e.Message}");
            _logger.Debug($"{e}");
            return false;
        }
    }

    /// <summary>
    /// Remove a player.
    /// </summary>
    /// <param name="player">The player to be removed.</param>
    public void RemovePlayer(Player player)
    {
        try
        {
            lock (_lock)
            {
                Scoreboard.Remove(player);
                AllPlayers.Remove(player);
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot remove player: {e.Message}");
            _logger.Debug($"{e}");
        }
    }

    public void addScore(Player player, int score)
    {
        try
        {
            lock (_lock)
            {
                Scoreboard[player] += score;
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot : {e.Message}");
            _logger.Debug($"{e}");
        }
    }

    /// <summary>
    /// Get the player with the highest score. Null if more than one players
    /// have the highest score.
    /// </summary>
    /// <returns>The reference of player with the highest score.</returns>
    /// <exception cref="Exception">Never thrown, unless some error occurs.</exception>
    public Player? GetHighScorePlayer()
    {
        int highScore = -1;
        Player? highScorePlayer = null;
        foreach (Player player in AllPlayers)
        {
            if (Scoreboard[player] > highScore)
            {
                highScore = Scoreboard[player];
                highScorePlayer = player;
            }
        }
        int highScoreCount = 0;
        foreach (Player player in AllPlayers)
        {
            if (Scoreboard[player] == highScore)
            {
                ++highScoreCount;
            }
        }
        if (highScoreCount == 1)
        {
            return highScorePlayer;
        }
        else if (highScoreCount > 1)
        {
            return null;
        }
        else
        {
            throw new Exception("This should NOT be thrown!");
        }
    }
    #endregion
}
