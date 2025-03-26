using Thuai.Server.GameLogic;

namespace Thuai.Server.Test.GameLogic;

//Checked original tests 03/17/2025 (except those with loggers)
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

    [Theory]
    [InlineData(1, 1, 100, false)]
    [InlineData(1, 1, 0, true)]
    [InlineData(0, 1, 100, true)]
    [InlineData(0, 0, 100, true)]
    public void AlivePlayers_Decide_BattleOverCorrectly(
        int health1, int health2, int ticks, bool expectedResult
    )
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        var battle = new Battle(new() { MaxBattleTicks = ticks }, [player1, player2]);

        // Act
        player1.PlayerArmor.Health = health1;
        player2.PlayerArmor.Health = health2;
        battle.Tick();
        battle.Tick();

        // Assert
        Assert.Equal(expectedResult, battle.IsBattleOver());
    }

    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 0)]
    public void DecideWinner_WhenSomeoneWins(
        int health1, int health2, int expectedWinner
    )
    {
        // Arrange
        List<Player> players = [new Player("Player1", 1), new Player("Player2", 2)];
        players[0].PlayerArmor.Health = health1;
        players[1].PlayerArmor.Health = health2;
        Battle battle = new Battle(new(), players);

        // Act
        battle.Tick();
        battle.Tick();
        var result = battle.GetResult();

        // Assert
        Assert.Equal(players[expectedWinner], result.Winner);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void DecideNoWinner_WhenPlayersAllAlive_OrDraw(
        int health1, int health2
    )
    {
        // Arrange
        List<Player> players = [new Player("Player1", 1), new Player("Player2", 2)];
        players[0].PlayerArmor.Health = health1;
        players[1].PlayerArmor.Health = health2;
        Battle battle = new Battle(new(), players);

        // Act
        battle.Tick();
        battle.Tick();
        var result = battle.GetResult();

        // Assert
        Assert.Null(result.Winner);
    }
}