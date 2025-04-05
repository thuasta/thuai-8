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

    private int _playerId = 0;

    #region Methods

    /// <summary>
    /// Find a player by token.
    /// </summary>
    /// <param name="token">Token of the player.</param>
    /// <returns>Player with given token. Returns null if no player owns that token.</returns>
    public Player? FindPlayer(string token)
    {
        return AllPlayers.Find(player => player.Token == token);
    }

    /// <summary>
    /// Add player in the game.
    /// </summary>
    /// <param name="playerId">The player to be added.</param>
    /// <returns>If the adding succeeds.</returns>
    public bool AddPlayer(string token)
    {
        if (FindPlayer(token) != null)
        {
            _logger.Error(
                $"Cannot add player: Player with token {Utility.Tools.LogHandler.Truncate(token, 8)} already exists."
            );
            return false;
        }
        if (Stage != GameStage.Waiting)
        {
            _logger.Error("Cannot add player: The game is already started.");
            return false;
        }

        try
        {
            lock (_lock)
            {
                Player player = new(token, _playerId);
                AllPlayers.Add(player);
                Scoreboard.Add(player, 0);

                _logger.Information($"Player {player.TruncatedToken} joined with id {player.ID}.");

                ++_playerId;

                return true;
            }
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot add player:");
            Utility.Tools.LogHandler.LogException(_logger, e);
            return false;
        }
    }

    public void AddPlayer(string[] tokens)
    {
        foreach (string token in tokens)
        {
            AddPlayer(token);
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
            _logger.Error($"Cannot remove player:");
            Utility.Tools.LogHandler.LogException(_logger, e);
        }
    }

    public void AddScore(Player player, int score)
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
            _logger.Error($"Failed to add score to player {player.ID}.");
            Utility.Tools.LogHandler.LogException(_logger, e);
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
            throw new Exception($"Unexpected count of players with highest scores: {highScoreCount}.");
        }
    }
    #endregion
}
