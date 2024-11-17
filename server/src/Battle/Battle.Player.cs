
using Thuai.Server.GameController;

namespace Thuai.Server.GameLogic;

public partial class Battle
{
    #region Fields and properties
    public List<Player> AllPlayers {get; private set;} = [];

    public int PlayerCount => AllPlayers.Count;

    #endregion

    #region Methods
        public bool AddPlayer(Player player)
        {
            if (Stage != BattleStage.Waiting)
            {
                _logger.Error("Cannot add player: The battle is already started");
                return false;
            }
            try
            {
                lock(_lock)
                {
                    AllPlayers.Add(player);
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

        public void RemovePlayer(Player player)
        {
            try
            {
                lock(_lock)
                {
                    AllPlayers.Remove(player);
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Cannot remove player: {e.Message}");
                _logger.Debug($"{e}");
            }
        }

        public void UpdatePlayers()
        {
            foreach (Player player in AllPlayers)
            {
                // Update the players.
            }
        }
    #endregion

}