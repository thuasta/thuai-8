namespace Thuai.Server.GameController;

/// <summary>
/// Represents a player in the game.
/// </summary>
public class Player
{
    /// <summary>
    /// Character controlled by the player.
    /// </summary>
    public List<GameLogic.Tank>? Tanks { get; private set; } = null;

    public bool Ready()
    {
        return Tanks != null;
    }

    /// <summary>
    /// Binds a character to the player.
    /// </summary>
    /// <param name="character">The character to be controlled.</param>
    public void Bind(GameLogic.Tank character)
    {
        // TODO
    }

    /// <summary>
    /// Unbinds the character from the player.
    /// </summary>
    public void Unbind()
    {
        // TODO
    }
}
