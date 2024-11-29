namespace Thuai.Server.GameController;

/// <summary>
/// Represents a player in the game.
/// </summary>
public class Player
{
    /// <summary>
    /// Character controlled by the player.
    /// </summary>
    public GameLogic.Tank? Tank { get; private set; } = null;

    public bool Ready()
    {
        return Tank != null;
    }

    /// <summary>
    /// Binds a character to the player.
    /// </summary>
    /// <param name="tank">The character to be controlled.</param>
    public void Bind(GameLogic.Tank tank)
    {
        Tank = tank;
    }

    /// <summary>
    /// Unbinds the character from the player.
    /// </summary>
    public void Unbind()
    {
        Tank = null;
    }
}
