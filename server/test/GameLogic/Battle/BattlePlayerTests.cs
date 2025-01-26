using Thuai.Server.GameLogic;

namespace Thuai.Server.Test.GameLogic;

public class BattlePlayerTests
{
    [Fact]
    public void Properties_DefaultValues_ReturnsCorrect()
    {
        // Arrange
        var battle = new Battle(new(), []);

        // Act
        var allPlayers = battle.AllPlayers;
        var playerCount = battle.PlayerCount;

        // Assert
        Assert.Equal(0, playerCount);
        Assert.NotNull(allPlayers);
    }

    [Fact]
    public void UpdatePlayers_WhenCalled_ReturnsCorrect()
    {
        // Arrange
        var battle = new Battle(new(), []);

        // Act
        battle.UpdatePlayers();

        // Assert
    }

    [Fact]
    public void ChooseSpawnpoint_WhenMapIsNull_ThrowsException()
    {
        // Arrange
        var battle = new Battle(new(), []);

        // Act
        Action act = () => battle.ChooseSpawnpoint();

        // Assert
        var exception = Assert.Throws<Exception>(act);
        Assert.Equal("No available map!", exception.Message);
    }

    [Fact]
    public void ChooseSpawnpoint_WhenMapIsNotNull_ReturnsCorrect()
    {
        // Arrange
        var battle = new Battle(new(), []);

        // Act
        battle.Initialize();
        Action act = () => battle.ChooseSpawnpoint();

        // Assert
        var exception = Record.Exception(() => battle.ChooseSpawnpoint());
        Assert.Null(exception);
        // Todo : implement
    }

    [Fact(Skip = "Not implemented")]
    public void AlivePlayers_WhenCalled_ReturnsCorrect()
    {
        // Arrange
        var battle = new Battle(new(), []);

        // Act
        var result = battle.IsBattleOver();

        // Assert

    }

    [Fact(Skip = "Not implemented")]
    public void PlayerWithHighestHP_WhenCalled_ReturnsCorrect()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new(MBT: 0), players);

        // Act
        battle.Tick();
        battle.Tick();
        var result = battle.GetResult();

        // Assert

    }
}