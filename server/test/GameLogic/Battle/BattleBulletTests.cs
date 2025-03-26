using Thuai.Server.GameLogic;
using Thuai.Server.GameLogic.MapGenerator;

namespace Thuai.Server.Test.GameLogic;

//Checked original tests 03/17/2025 (except those with loggers)
public class BattleBulletTests
{
    [Theory]
    [InlineData(0, 0.10, 0, 0)]
    [InlineData(Math.PI / 2, 0, 0.10, Math.PI / 2)]
    [InlineData(Math.PI / 4, 0.07071067, 0.07071067, Math.PI / 4)]
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
        Assert.Equal(
            endX,
            battle.Bullets[0].BulletPosition.Xpos - player1.PlayerPosition.Xpos,
            1e-5
        );
        Assert.Equal(
            endY,
            battle.Bullets[0].BulletPosition.Ypos - player1.PlayerPosition.Ypos,
            1e-5
        );
        Assert.Equal(endAngle, battle.Bullets[0].BulletPosition.Angle, 1e-5);
    }


    [Theory]
    [InlineData(1, 1, 0, 1.1, 1, 0)]
    [InlineData(1, 1, 0, 0.2, 1, 0)]
    public void RemoveBullet_WhenCalled_ShouldRemoveCorrectly(
        double player1X, double player1Y, double player1Angle,
        double player2X, double player2Y, double player2Angle
    )
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        player1.PlayerPosition = new(player1X, player1Y, player1Angle);
        player2.PlayerPosition = new(player2X, player2Y, player2Angle);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        player1.PlayerAttack();
        battle.Tick();
        // battle.Tick();
        // NB: if another Tick() is used here, the assertion will be true

        // Assert
        Assert.Empty(battle.Bullets);
        // No need to assert the Remove Exception case.
    }

    [Theory]
    [InlineData(2, 1.05, 0)]
    [InlineData(2, 3, 1)]
    [InlineData(3, 0.95, 0)]
    [InlineData(2, 0.5, 1)]
    [InlineData(2, 1, 0)]
    [InlineData(0.9, 1, 1)]
    [InlineData(0.5, 1, 1)]
    [InlineData(5, 1, 1)]
    [InlineData(1.1, 1, 0)]
    public void TakeDamage_ShouldReturnCorrectly(
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
    [InlineData(0, 4.10, 1, 0)]
    [InlineData(Math.PI / 2, 1, 4.10, Math.PI / 2)]
    [InlineData(Math.PI / 4, 3.192031, 3.192031, Math.PI / 4)]
    public void UpdateBullets_NotHitWall_NotHitPlayer_ShouldReturnFinalPos(
        double startAngle, double endX, double endY, double endAngle)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);
        player1.PlayerPosition = new(1, 1, startAngle);
        player2.PlayerPosition = new(20, 20, 0);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
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
    public void UpdateBullets_NotHitWall_HitPlayer_ShouldRemove(
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
    [InlineData(0, 0.90, 0, Math.PI)]
    [InlineData(Math.PI / 4, 1.270943, 2.729057, 3 * Math.PI / 4)]
    public void UpdateBullets_HitWall_NotHitPlayer_ShouldReturnFinalPos(
        double startAngle, double endX, double endY, double endAngle)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);
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
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(
            endX + player1.PlayerPosition.Xpos,
            battle.Bullets[0].BulletPosition.Xpos,
            1e-5
        );
        Assert.Equal(
            endY + player1.PlayerPosition.Ypos,
            battle.Bullets[0].BulletPosition.Ypos,
            1e-5
        );
        Assert.Equal(endAngle, battle.Bullets[0].BulletPosition.Angle, 1e-5);
    }

    [Theory]
    [InlineData(Math.PI / 4, 1.250943, 2.749057, 0)]
    [InlineData(Math.PI / 4, 1.750943, 2.249057, 0)]
    public void UpdateBullets_HitWall_HitPlayer_ShouldRemove(
        double startAngle, double delta2X, double delta2Y, int health2)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);
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
        player2.PlayerPosition = new Position(wallX - 2 + delta2X, wallY + delta2Y, 0);
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(health2, player2.PlayerArmor.Health);
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