using Thuai.Server.GameLogic;

namespace Thuai.Server.GameController;

/// <summary>
/// Runs and manages the game.
/// </summary>
public class GameRunner(Utility.Config.GameSettings gameSettings)
{
    public Utility.Config.GameSettings GameSettings = gameSettings;
    /// <summary>
    /// Runs a new game and return the result.
    /// </summary>
    /// <param name="game">The game to be run.</param>
    public void Run()
    {
        Game game = new(GameSettings);
        
        throw new NotImplementedException();
    }

    // TODO: Implement
}
