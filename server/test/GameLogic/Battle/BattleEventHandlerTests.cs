using Thuai.Server.GameLogic;


namespace Thuai.Server.Test.GameLogic;


//Checked original tests 03/17/2025 (except those with loggers and laser)
public class BattleEventHandlerTests
{
    [Fact]
    public void OnPlayerMove_StageIsNotInBattle_LogError()
    {

        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        player1.PlayerMove(MoveDirection.FORTH);

        // Assert
        // No need to assert.
    }

    [Theory]
    [InlineData(1, 1, 0, 1.1, 1, 0)]
    [InlineData(1, 1, Math.PI / 2, 1, 1.1, Math.PI / 2)]
    [InlineData(1, 1, Math.PI / 4, 1.0707107, 1.0707107, Math.PI / 4)]
    public void OnPlayerMove_MoveDirectionIsForth_PlayerPositionIsUpdated(
        double startX, double startY, double startAngle, double expectedX, double expectedY, double expectedAngle)
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Initialize();
        player1.PlayerPosition = new Position(startX, startY, startAngle);
        player1.Speed = 0.1;
        battle.Tick();
        player1.PlayerMove(MoveDirection.FORTH);
        battle.Tick();

        // Assert
        Assert.Equal(expectedX, player1.PlayerPosition.Xpos, 1e-5);
        Assert.Equal(expectedY, player1.PlayerPosition.Ypos, 1e-5);
        Assert.Equal(expectedAngle, player1.PlayerPosition.Angle, 1e-5);
    }

    [Theory]
    [InlineData(1, 1, 0, 0.9, 1, 0)]
    [InlineData(1, 1, Math.PI / 2, 1, 0.9, Math.PI / 2)]
    [InlineData(1, 1, Math.PI / 4, 0.9292893, 0.9292893, Math.PI / 4)]
    public void OnPlayerMove_MoveDirectionIsBack_PlayerPositionIsUpdated(
        double startX, double startY, double startAngle, double expectedX, double expectedY, double expectedAngle)
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Initialize();
        player1.PlayerPosition = new Position(startX, startY, startAngle);
        player1.Speed = 0.1;
        battle.Tick();
        player1.PlayerMove(MoveDirection.BACK);

        // Assert
        Assert.Equal(expectedX, player1.PlayerPosition.Xpos, 1e-5);
        Assert.Equal(expectedY, player1.PlayerPosition.Ypos, 1e-5);
        Assert.Equal(expectedAngle, player1.PlayerPosition.Angle, 1e-5);
    }

    //Confusing
    [Fact]
    public void OnPlayerMove_FinalPosIsInvalid_Nochanges()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        player1.PlayerPosition = new Position(1, 1, 0);
        player1.Speed = 0.1;
        battle.Tick();
        player1.PlayerMove(MoveDirection.FORTH);

        // Assert
        Assert.Equal(1, player1.PlayerPosition.Xpos);
        Assert.Equal(1, player1.PlayerPosition.Ypos);
        Assert.Equal(0, player1.PlayerPosition.Angle);

        // Move back is the same as move forth
        player1.PlayerMove(MoveDirection.BACK);
        Assert.Equal(1, player1.PlayerPosition.Xpos);
        Assert.Equal(1, player1.PlayerPosition.Ypos);
        Assert.Equal(0, player1.PlayerPosition.Angle);
    }

    [Fact]
    public void OnPlayerMove_MoveDirectionIsInvalid_LogError()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Tick();
        player1.PlayerMove((MoveDirection)3);

        // Assert
        // No need to assert.
    }

    [Fact]
    public void OnPlayerMove_ExceptionThrown_LogError()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Initialize();
        player1.PlayerPosition = null!;
        battle.Tick();
        player1.PlayerMove(MoveDirection.FORTH);

        // Assert
        // No need to assert.
    }

    [Fact]
    public void OnPlayerTurn_StageIsNotInBattle_LogError()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        player1.PlayerTurn(TurnDirection.CLOCKWISE);

        // Assert
        // No need to assert.
    }

    [Fact]
    public void OnPlayerTurn_TurnDirectionIsClockwise_PlayerPositionIsUpdated()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Tick();
        player1.PlayerPosition = new Position(1, 1, 0);
        player1.TurnSpeed = 0.1;
        player1.PlayerTurn(TurnDirection.CLOCKWISE);

        // Assert
        Assert.Equal(-0.1, player1.PlayerPosition.Angle);
        Assert.Equal(1, player1.PlayerPosition.Xpos);
        Assert.Equal(1, player1.PlayerPosition.Ypos);
    }

    [Fact]
    public void OnPlayerTurn_TurnDirectionIsCounterClockwise_PlayerPositionIsUpdated()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Tick();
        player1.PlayerPosition = new Position(1, 1, 0);
        player1.TurnSpeed = 0.1;
        player1.PlayerTurn(TurnDirection.COUNTER_CLOCKWISE);

        // Assert
        Assert.Equal(0.1, player1.PlayerPosition.Angle);
        Assert.Equal(1, player1.PlayerPosition.Xpos);
        Assert.Equal(1, player1.PlayerPosition.Ypos);
    }

    [Fact]
    public void OnPlayerTurn_TurnDirectionIsInvalid_LogError()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Tick();
        player1.PlayerTurn((TurnDirection)2);

        // Assert
        // No need to assert.
    }

    [Fact]
    public void OnPlayerTurn_ExceptionThrown_LogError()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new(), players);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Initialize();
        player1.PlayerPosition = null!;
        battle.Tick();
        player1.PlayerTurn(TurnDirection.CLOCKWISE);

        // Assert
        // No need to assert.
    }

    [Fact]
    public void OnPlayerAttack_StageIsNotInBattle_LogError()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new(), players);
        battle.SubscribePlayerEvents(player1);

        // Act
        player1.PlayerAttack();

        // Assert
        // No need to assert.
    }

    [Theory]
    [InlineData(0, 1.11, 1)]
    [InlineData(Math.PI / 2, 1, 1.11)]
    [InlineData(Math.PI / 4, 1.077781, 1.077781)]

    public void OnPlayerAttack_IsBullet_AddBullet(
        double startAngle, double expectedX, double expectedY)
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new(), players);
        battle.SubscribePlayerEvents(player1);
        Bullet expectedBullet = new Bullet(new Position(expectedX, expectedY, startAngle), 3, 1, false);

        // Act
        battle.Tick();
        player1.PlayerPosition = new Position(1, 1, startAngle);
        player1.PlayerAttack();
        Bullet bullet = (Bullet)battle.Bullets[0];

        // Assert
        Assert.Equal(expectedBullet.BulletPosition.Xpos, bullet.BulletPosition.Xpos, 1e-5);
        Assert.Equal(expectedBullet.BulletPosition.Ypos, bullet.BulletPosition.Ypos, 1e-5);
        Assert.Equal(expectedBullet.BulletPosition.Angle, bullet.BulletPosition.Angle, 1e-5);
        //Assert.Equal(Constants.MAX_BULLETS - 1, player1.PlayerWeapon.currentBullets);
    }

    [Fact]
    public void OnPlayerAttack_IsLaser_AddBullet()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        player1.PlayerWeapon.IsLaser = true;
        var players = new List<Player> { player1, player2 };
        Battle battle = new Battle(new(), players);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Tick();
        player1.PlayerPosition = new Position(1, 1, 0);
        player1.PlayerWeapon.IsLaser = true;
        player1.PlayerAttack();

        // Assert
        // Todo : Need to assert the laser is applied.
        //Assert.Equal(Constants.MAX_BULLETS - 1, player1.PlayerWeapon.currentBullets);
    }

    [Fact]
    public void OnPlayerAttack_NoBullet_LogError()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        player1.PlayerWeapon.CurrentBullets = 0;
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Tick();
        player1.PlayerPosition = new Position(1, 1, 0);
        player1.PlayerAttack();

        // Assert
        // No need to assert.
    }

    [Fact]
    public void OnPlayerAttack_ExceptionThrown_LogError()
    {
        // Arrange
        Player player1 = new Player("Player1", 1);
        Player player2 = new Player("Player2", 2);
        Battle battle = new Battle(new(), [player1, player2]);
        battle.SubscribePlayerEvents(player1);

        // Act
        battle.Initialize();
        player1.PlayerPosition = null!;
        battle.Tick();
        player1.PlayerAttack();

        // Assert
        // No need to assert.
    }

}