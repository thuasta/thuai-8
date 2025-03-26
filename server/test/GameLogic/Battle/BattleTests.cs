using Thuai.Server.GameLogic;
using Serilog;

namespace Thuai.Server.Test.GameLogic;

public class BattleTests
{
    [Fact]
    public void ResultClass_DefaultValues_ReturnsCorrect()
    {
        // Arrange
        Player player = new Player("", 0);
        var result = new Battle.Result(player, true);

        // Act
        // No need to act.
        // try to modify the Result object will cause a compile error.
        // result.Winner = null;
        // result.Valid = false;

        // Assert
        Assert.Same(player, result.Winner);
        Assert.True(result.Valid);
    }

    [Fact]
    public void ResultClass_NullPlayer_ReturnsCorrect()
    {
        // Arrange
        var result = new Battle.Result(null, false);

        // Act
        // No need to act.

        // Assert
        Assert.Null(result.Winner);
        Assert.False(result.Valid);
    }

    [Fact]
    public void Properties_DefaultValues_ReturnsCorrect()
    {
        // Arrange.
        Battle battle = new(new(), []);

        // Act.
        // No need to act.

        // Assert.
        Assert.Equal(0, battle.CurrentTick);
        Assert.Equal(0, (int)battle.Stage);
        Assert.NotNull(battle.GameSettings);
    }

    [Fact]
    public void GetResult_NotFinishedOrChoosingAward_ReturnsInvalidResult()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        var battle = new Battle(new() { MaxBattleTicks = 0 }, players);

        // Act
        var result = battle.GetResult();

        // Assert
        Assert.Null(result.Winner);
        Assert.False(result.Valid);

        // same test when stage is InBattle
        battle.Tick();
        result = battle.GetResult();
        Assert.Null(result.Winner);
        Assert.False(result.Valid);
    }

    [Fact]
    public void GetResult_WhenStageIsFinished_ReturnsValidResult()
    {
        // Arrange
        var player = new Player("Player1", 1);
        var players = new List<Player> { player };
        var battle = new Battle(new(), players);
        // Todo: Set the stage to Finished

        // Act
        var result = battle.GetResult();

        // Assert

    }

    [Fact]
    public void GetResult_WhenStageIsChoosingAward_ReturnsValidResult()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new() { MaxBattleTicks = 0 }, players);

        // Act
        battle.Tick();
        battle.Tick();
        var result = battle.GetResult();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Valid);
    }

    [Fact(Skip = "Not implemented yet")]
    public void Initialize_GenerateMapSucceeds_ReturnsTrue()
    {
        // Arrange
        Battle battle = new Battle(new(), []);
        // Todo: Mock GenerateMap to return true

        // Act
        var result = battle.Initialize();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Initialize_GenerateMapfails_ReturnsFalse()
    {
        // Arrange
        Battle battle = new Battle(new(), []);
        // Todo: Mock GenerateMap to return false

        // Act
        var result = battle.Initialize();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Tick_InBattle_UpdateAndAddTick()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new(), players);

        // Act
        battle.Tick();
        battle.Tick();

        // Assert
        Assert.Equal(1, battle.CurrentTick);
    }

    [Fact]
    public void Tick_NotInBattle_DoesNotUpdatePlayersBulletsMapOrIncrementTick()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new(), players);

        // Act
        battle.Tick();

        // Assert
        Assert.Equal(0, battle.CurrentTick);
    }

    [Fact]
    public void IsBattleOver_OverMaxTicks_ReturnsTrue()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new() { MaxBattleTicks = 0 }, players);

        // Act
        battle.Tick();
        battle.Tick();

        // Assert
        Assert.True(battle.IsBattleOver());
    }

    [Fact]
    public void IsBattleOver_PlayerLessThanTwo_ReturnsTrue()
    {
        // Arrange
        Battle battle = new Battle(new(), []);

        // Act
        // No need to act.

        // Assert
        Assert.True(battle.IsBattleOver());
    }

    [Fact(Skip = "Not implemented yet")]
    public void IsBattleOver_PlayerMoreThanOne_ReturnsFalse()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new(), players);

        // Act
        // No need to act.

        // Assert
        Assert.False(battle.IsBattleOver());
    }

    [Fact]
    public void StageControl_WaitingStageWithTwoPlayers_ChangesToInBattle()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new(), players);

        // Act
        battle.Tick();

        // Assert
        Assert.Equal(1, (int)battle.Stage);
    }

    [Fact]
    public void StageControl_WaitingStageWithNoPlayer_NoChanges()
    {
        // Arrange
        Battle battle = new Battle(new(), []);

        // Act
        battle.Tick();

        // Assert
        Assert.Equal(0, (int)battle.Stage);
    }

    [Fact]
    public void StageControl_InBattleStageWithEnd_ChangesToChoosingAward()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new() { MaxBattleTicks = 0 }, players);

        // Act
        battle.Tick();
        battle.Tick();

        // Assert
        Assert.Equal(2, (int)battle.Stage);
    }

    [Fact(Skip = "Not implemented yet")]
    public void StageControl_InBattleStageWithNotEnd_NoChanges()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new(), players);

        // Act
        battle.Tick();
        battle.Tick();

        // Assert
        Assert.Equal(0, (int)battle.Stage);
    }

    [Fact]
    public void StageControl_ChoosingAwardStage_NoChanges()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new() { MaxBattleTicks = 0 }, players);

        // Act
        battle.Tick();
        battle.Tick();
        battle.Tick();

        // Assert
        Assert.Equal(2, (int)battle.Stage);
    }

    [Fact(Skip = "Not implemented yet")]
    public void StageControl_FinishedStage_NoChanges()
    {
        // Arrange
        var player1 = new Player("Player1", 1);
        var player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new() { MaxBattleTicks = 0 }, players);

        // Act
        battle.Tick();
        battle.Tick();
        battle.Tick();
        battle.Tick();

        // Assert
        Assert.Equal(3, (int)battle.Stage);
    }
}