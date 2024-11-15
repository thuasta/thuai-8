namespace Thuai.Server.GameController;

/// <summary>
/// Runs and manages the game.
/// </summary>
public class GameRunner
{
    /// <summary>
    /// The game controlled by the game runner.
    /// </summary>
    public GameLogic.Game CreateGame(Utility.Config.GameSettings gameSettings)
    {
        return new(gameSettings);
    }

    /// <summary>
    /// Runs an existing game.
    /// </summary>
    /// <param name="game">The game to be run.</param>
    public void Run(GameLogic.Game game)
    {
        throw new NotImplementedException();
    }

    // TODO: Implement
}
