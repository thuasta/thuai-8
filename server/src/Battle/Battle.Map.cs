namespace Thuai.Server.GameLogic;

public partial class Battle
{
    public Map? Map { get; private set; } = null;

    /// <summary>
    /// If map is null, generate a map.
    /// </summary>
    /// <returns>If the map available.</returns>
    private bool GenerateMap()
    {
        // TODO: implement. Maybe wait until generating map finished.
        return false;
    }

    /// <summary>
    /// Update the map.
    /// </summary>
    public void UpdateMap()
    {

    }
}