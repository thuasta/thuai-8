namespace Thuai.Server.GameController;

/// <summary>
/// Represents a player in the game.
/// </summary>
public class Player
{
    /// <summary>
    /// Character controlled by the player.
    /// </summary>
    public GameLogic.Character? Character { get; private set; } = null;

    /// <summary>
    /// Binds a character to the player.
    /// </summary>
    /// <param name="character">The character to be controlled.</param>
    public void Bind(GameLogic.Character character)
    {
        Character = character;
    }

    /// <summary>
    /// Unbinds the character from the player.
    /// </summary>
    public void Unbind()
    {
        Character = null;
    }
}
