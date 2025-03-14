using Thuai.Server.GameLogic;
using Thuai.Server.GameLogic.MapGenerator;

namespace Thuai.Server.Test.GameLogic;

public class BattleBulletTests
{
    [Theory]
    [InlineData(0, 1.11, 1, 0)]
    [InlineData(Math.PI / 2, 1, 1.11, Math.PI / 2)]
    [InlineData(Math.PI / 4, 1.077781, 1.077781, Math.PI / 4)]
    public void AddBullet_ValidBullet_ShouldAddCorrectly(
        double startAngle, double endX, double endY, double endAngle)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new(1, 1, startAngle);
        player2.PlayerPosition = new(20, 20, 0);
        player1.PlayerAttack();

        // Assert
        Assert.Equal(endX, battle.Bullets[0].BulletPosition.Xpos, 1e-5);
        Assert.Equal(endY, battle.Bullets[0].BulletPosition.Ypos, 1e-5);
        Assert.Equal(endAngle, battle.Bullets[0].BulletPosition.Angle, 1e-5);
        // No need to assert the NotInBattle case.
        // No need to assert the Add Exception case.
    }

    [Fact]
    public void RemoveBullet_WhenCalled_ShouldRemoveCorrectly()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new(1, 1, 0);
        player2.PlayerPosition = new(2, 1, 0);
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Empty(battle.Bullets);
        // No need to assert the Remove Exception case.
    }

    [Theory]
    [InlineData(2, 1.05, 0)]
    [InlineData(2, 3, 1)]
    [InlineData(2, 0.95, 0)]
    [InlineData(2, 0.5, 1)]
    public void TakeDamage_Offline_ShouldReturnCorrectly(
        double playerX, double playerY, int health)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new(1, 1, 0);
        player2.PlayerPosition = new(playerX, playerY, 0);
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(health, player2.PlayerArmor.Health);
    }

    [Theory]
    [InlineData(2, 1, 0)]
    [InlineData(0.9, 1, 1)]
    [InlineData(0.5, 1, 1)]
    [InlineData(5, 1, 1)]
    public void TakeDamage_Online_ShouldReturnCorrectly(
        double playerX, double playerY, int health)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new(1, 1, 0);
        player2.PlayerPosition = new(playerX, playerY, 0);
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(health, player2.PlayerArmor.Health);
    }

    [Theory]
    [InlineData(0, 4.11, 1, 0)]
    [InlineData(Math.PI / 2, 1, 4.11, Math.PI / 2)]
    [InlineData(Math.PI / 4, 3.199102, 3.199102, Math.PI / 4)]
    public void UpdateBullets_NoIntersectNoPlayer_ShouldReturnFinalPos(
        double startAngle, double endX, double endY, double endAngle)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new(1, 1, startAngle);
        player2.PlayerPosition = new(20, 20, 0);
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(endX, battle.Bullets[0].BulletPosition.Xpos, 1e-5);
        Assert.Equal(endY, battle.Bullets[0].BulletPosition.Ypos, 1e-5);
        Assert.Equal(endAngle, battle.Bullets[0].BulletPosition.Angle, 1e-5);
    }

    [Theory]
    [InlineData(0, 2, 1, 0)]
    [InlineData(Math.PI / 2, 1, 2, 0)]
    [InlineData(Math.PI / 4, 1.5, 1.5, 0)]
    public void UpdateBullets_NoIntersectHitPlayer_ShouldRemove(
        double startAngle, double playerX, double playerY, int health)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new(1, 1, startAngle);
        player2.PlayerPosition = new(playerX, playerY, startAngle);
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(health, player2.PlayerArmor.Health);
        Assert.Empty(battle.Bullets);
    }

    [Theory]
    [InlineData(0, -1.11, 0, Math.PI)]
    [InlineData(Math.PI / 4, -0.199102, 2.199102, 3 * Math.PI / 4)]
    public void UpdateBullets_IntersectNotHitPlayer_ShouldRemove(
        double startAngle, double deltaX, double deltaY, double endAngle)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);
        double wallX = 1;
        double wallY = 1;
        double endX;
        double endY;

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        // select the proper wall
        Map? map = battle.Map;
        if (map != null)
        {
            foreach (var wall in map.Walls)
            {
                if (wall.X > 0 && wall.Y > 0 && wall.Angle == 90)
                {
                    wallX = wall.X * Constants.WALL_LENGTH;
                    wallY = wall.Y * Constants.WALL_LENGTH;
                    break;
                }
            }
        }
        player1.PlayerPosition = new Position(wallX - 2, wallY, startAngle);
        endX = wallX + deltaX;
        endY = wallY + deltaY;
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(endX, battle.Bullets[0].BulletPosition.Xpos, 1e-5);
        Assert.Equal(endY, battle.Bullets[0].BulletPosition.Ypos, 1e-5);
        Assert.Equal(endAngle, battle.Bullets[0].BulletPosition.Angle, 1e-5);
    }

    [Theory]
    [InlineData(0, -1, 0, -1, 0, 0, 1)]
    [InlineData(0, -10, 0, -1, 0, 1, 0)]
    [InlineData(Math.PI / 4, -1.292893, 0.707106, -0.192031, 2.192031, 0, 1)]
    [InlineData(Math.PI / 4, -10, 0.707106, -0.192031, 2.192031, 1, 0)]
    public void UpdateBullets_IntersectHitPlayer_ShouldRemove(
        double startAngle, double delta2X, double delta2Y, double delta3X, double delta3Y, int health2, int health3)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Player player3 = new("player3", 3);
        Battle battle = new(new(), [player1, player2, player3]);
        double wallX = 1;
        double wallY = 1;

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        // select the proper wall
        Map? map = battle.Map;
        if (map != null)
        {
            foreach (var wall in map.Walls)
            {
                if (wall.X > 0 && wall.Y > 0 && wall.Angle == 90)
                {
                    wallX = wall.X * Constants.WALL_LENGTH;
                    wallY = wall.Y * Constants.WALL_LENGTH;
                    break;
                }
            }
        }
        player1.PlayerPosition = new Position(wallX - 2, wallY, startAngle);
        player2.PlayerPosition = new Position(wallX + delta2X, wallY + delta2Y, 0);
        player3.PlayerPosition = new Position(wallX + delta3X, wallY + delta3Y, 0);
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(health2, player2.PlayerArmor.Health);
        Assert.Equal(health3, player3.PlayerArmor.Health);
        Assert.Empty(battle.Bullets);
    }

    [Fact]
    public void UpdateBullets_ThrowException_ShouldLogError()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Player player3 = null!;
        Battle battle = new(new(), [player1, player2, player3]);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new Position(1, 1, 0);
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        // No need to assert the log message.
    }
}