using Thuai.Server.GameLogic.MapGenerator;
using Thuai.Server.GameLogic;

namespace Thuai.Server.Test.GameLogic;

//Checked original tests 03/17/2025 (except those with map updating)
public class BattleMapTests
{
    [Fact]
    public void GenerateMap_WhenCalled_ShouldGenerateMap()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.Initialize();

        // Assert
        Assert.NotNull(battle.Map);
    }

    [Fact]
    public void UpdateMap_WhenCalled_ShouldUpdateMap()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.Initialize();
        battle.Tick();
        battle.Tick();

        // Assert
        Assert.Equal(1, battle.CurrentTick);
        // Todo : need to implement the map update test
    }

    [Fact]
    public void PointDistance_WhenCalled_ShouldStopEarly()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player1);
        // generate map
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new Position(1, 11, 0);
        player1.Speed = 100000;
        player1.PlayerMove(MoveDirection.FORTH);

        // Assert
        Assert.NotEqual(100001, player1.PlayerPosition.Xpos);
    }

    [Fact] // Invalid angle of wall doesn't exist!
    public void GetPlayerFinalPos_MapIsNull_LogError()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Tick();
        player1.PlayerPosition = new Position(1, 11, 0);
        player1.Speed = 100000;
        player1.PlayerMove(MoveDirection.FORTH);

        // Assert
        Assert.Equal(1, player1.PlayerPosition.Xpos);
    }

    [Theory]
    [InlineData(1, 1, 0, 0.1, 1.1, 1, 0)]
    [InlineData(1, 1, Math.PI / 2, 0.1, 1, 1.1, Math.PI / 2)]
    public void GetPlayerFinalPos_WithoutWall_ShouldReturnFinalPosition(
        double startX, double startY, double startAngle, double speed, double endX, double endY, double endAngle)
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player1);
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new Position(startX, startY, startAngle);
        player1.Speed = speed;
        player1.PlayerMove(MoveDirection.FORTH);

        // Assert
        Assert.Equal(endX, player1.PlayerPosition.Xpos, 1e-5);
        Assert.Equal(endY, player1.PlayerPosition.Ypos, 1e-5);
        Assert.Equal(endAngle, player1.PlayerPosition.Angle, 1e-5);
    }

    // [Fact]
    // public void GetPlayerFinalPos_AngleEquals0_ShouldReturnFinalPosition()
    // {
    //     // Arrange
    //     Player player1 = new("player1", 1);
    //     Player player2 = new("player2", 2);
    //     Battle battle = new(new(), [player1, player2]);
    //     double endX = 1;

    //     // Act
    //     battle.SubscribePlayerEvents(player1);
    //     battle.Initialize();
    //     battle.Tick();
    //     player1.PlayerPosition = new Position(1, 11, 0);
    //     player1.Speed = 100000;
    //     player1.PlayerMove(MoveDirection.FORTH);
    //     // calculate the final x position
    //     Map? map = battle.Map;
    //     if (map != null)
    //     {
    //         foreach (var wall in map.Walls)
    //         {
    //             if (wall.X * Constants.WALL_LENGTH > 1 && wall.Y == 1 && wall.Angle == 90)
    //             {
    //                 endX = wall.X * Constants.WALL_LENGTH - Constants.WALL_THICK - Constants.PLAYER_RADIO;
    //                 break;
    //             }
    //         }
    //     }

    //     // Assert
    //     Assert.Equal(endX, player1.PlayerPosition.Xpos, 1e-5);
    //     Assert.Equal(11, player1.PlayerPosition.Ypos, 1e-5);
    //     Assert.Equal(0, player1.PlayerPosition.Angle, 1e-5);
    // }

    // [Fact]
    // public void GetPlayerFinalPos_AngleEquals90_ShouldReturnFinalPosition()
    // {
    //     // Arrange
    //     Player player1 = new("player1", 1);
    //     Player player2 = new("player2", 2);
    //     Battle battle = new(new(), [player1, player2]);
    //     double endY = 1;

    //     // Act
    //     battle.SubscribePlayerEvents(player1);
    //     battle.Initialize();
    //     battle.Tick();
    //     player1.PlayerPosition = new Position(11, 1, Math.PI / 2);
    //     player1.Speed = 100000;
    //     player1.PlayerMove(MoveDirection.FORTH);
    //     // calculate the final y position
    //     Map? map = battle.Map;
    //     if (map != null)
    //     {
    //         foreach (var wall in map.Walls)
    //         {
    //             if (wall.Y * Constants.WALL_LENGTH > 1 && wall.X == 1 && wall.Angle == 0)
    //             {
    //                 endY = wall.Y * Constants.WALL_LENGTH - Constants.WALL_THICK - Constants.PLAYER_RADIO;
    //                 break;
    //             }
    //         }
    //     }

    //     // Assert
    //     Assert.Equal(10.8, player1.PlayerPosition.Xpos, 1e-5);
    //     Assert.Equal(endY, player1.PlayerPosition.Ypos, 1e-5);
    //     Assert.Equal(Math.PI / 2, player1.PlayerPosition.Angle, 1e-5);
    // }

    // [Fact]
    // public void GetPlayerFinalPos_AngleEquals45_ShouldReturnFinalPosition()
    // {
    //     // Arrange
    //     Player player1 = new("player1", 1);
    //     Player player2 = new("player2", 2);
    //     Battle battle = new(new(), [player1, player2]);
    //     double endX = 1;
    //     double endY = 1;

    //     // Act
    //     battle.SubscribePlayerEvents(player1);
    //     battle.Initialize();
    //     battle.Tick();
    //     player1.PlayerPosition = new Position(1, 1, Math.PI / 4);
    //     player1.Speed = 100000;
    //     player1.PlayerMove(MoveDirection.FORTH);
    //     // calculate the final y position
    //     Map? map = battle.Map;
    //     if (map != null)
    //     {
    //         foreach (var wall in map.Walls)
    //         {
    //             if (wall.X == wall.Y)
    //             {
    //                 endX = wall.X * Constants.WALL_LENGTH - Constants.WALL_THICK - Constants.PLAYER_RADIO;
    //                 endY = wall.Y * Constants.WALL_LENGTH - Constants.WALL_THICK - Constants.PLAYER_RADIO;
    //                 break;
    //             }
    //             else if (wall.X == wall.Y + 1 && wall.Angle == 90)
    //             {
    //                 endX = wall.X * Constants.WALL_LENGTH - Constants.WALL_THICK - Constants.PLAYER_RADIO;
    //                 endY = wall.X * Constants.WALL_LENGTH - Constants.WALL_THICK - Constants.PLAYER_RADIO;
    //                 break;
    //             }
    //             else if (wall.X == wall.Y - 1 && wall.Angle == 0)
    //             {
    //                 endX = wall.Y * Constants.WALL_LENGTH - Constants.WALL_THICK - Constants.PLAYER_RADIO;
    //                 endY = wall.Y * Constants.WALL_LENGTH - Constants.WALL_THICK - Constants.PLAYER_RADIO;
    //                 break;
    //             }
    //         }
    //     }

    //     // Assert
    //     Assert.Equal(endX, player1.PlayerPosition.Xpos, 1e-5);
    //     Assert.Equal(endY, player1.PlayerPosition.Ypos, 1e-5);
    //     Assert.Equal(Math.PI / 4, player1.PlayerPosition.Angle, 1e-5);
    // }

    [Fact]
    public void LineDistance_LessThanRadio_ShouldTakeDamage()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player2);
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new Position(3, 1, 0);
        player2.PlayerPosition = new Position(1, 1, 0);
        player2.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal([], battle.Bullets);
        Assert.Equal(0, player1.PlayerArmor.Health);
    }

    [Fact]
    public void LineDistance_MoreThanRadio_ShouldNotTakeDamage()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);
        Bullet bullet = new(new(4.1, 1, 0), 3, 1, false);

        // Act
        battle.SubscribePlayerEvents(player2);
        battle.Initialize();
        battle.Tick();
        player1.PlayerPosition = new Position(6, 1, 0);
        player2.PlayerPosition = new Position(1, 1, 0);
        player2.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.NotEqual([], battle.Bullets);
        Assert.Equal(1, player1.PlayerArmor.Health);
    }

    [Fact]
    public void GetBulletFinalPos_NoMap_LogError()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);

        // Act
        battle.SubscribePlayerEvents(player2);
        battle.Tick();
        player2.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.NotEqual([], battle.Bullets);
    }

    [Theory]
    [InlineData(0, 4.11, 1, 0)]
    [InlineData(Math.PI / 2, 1, 4.11, Math.PI / 2)]
    [InlineData(Math.PI / 4, 3.199102, 3.199102, Math.PI / 4)]
    public void GetBulletFinalPos_WithoutWall_ShouldReturnFinalPosition(
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
        player1.PlayerPosition = new Position(1, 1, startAngle);
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(endX, battle.Bullets[0].BulletPosition.Xpos, 1e-5);
        Assert.Equal(endY, battle.Bullets[0].BulletPosition.Ypos, 1e-5);
        Assert.Equal(endAngle, battle.Bullets[0].BulletPosition.Angle);
    }

    [Fact]
    public void GetBulletFinalPos_Hit90WallVertically_ShouldReturnFinalPosition()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);
        double wallX = 1;
        double endX;

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
                if (wall.X > 0 && wall.Y == 1 && wall.Angle == 90)
                {
                    wallX = wall.X * Constants.WALL_LENGTH;
                    break;
                }
            }
        }
        player1.PlayerPosition = new Position(wallX - 2, 11, 0);
        endX = wallX - 1.11;
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(endX, battle.Bullets[0].BulletPosition.Xpos, 1e-5);
        Assert.Equal(11, battle.Bullets[0].BulletPosition.Ypos, 1e-5);
        Assert.Equal(Math.PI, battle.Bullets[0].BulletPosition.Angle, 1e-5);
        // No need to test the intersection point
    }

    [Fact]
    public void GetBulletFinalPos_Hit0WallVertically_ShouldReturnFinalPosition()
    {
        // Arrange
        Player player1 = new("player1", 1);
        Player player2 = new("player2", 2);
        Battle battle = new(new(), [player1, player2]);
        double wallY = 1;
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
                if (wall.Y > 0 && wall.X == 1 && wall.Angle == 0)
                {
                    wallY = wall.Y * Constants.WALL_LENGTH;
                    break;
                }
            }
        }
        player1.PlayerPosition = new Position(11, wallY - 2, Math.PI / 2);
        endY = wallY - 1.11;
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(11, battle.Bullets[0].BulletPosition.Xpos, 1e-5);
        Assert.Equal(endY, battle.Bullets[0].BulletPosition.Ypos, 1e-5);
        Assert.Equal(-Math.PI / 2, battle.Bullets[0].BulletPosition.Angle, 1e-5);
    }

    [Fact]
    public void GetBulletFinalPos_Hit90WallNotVertically_ShouldReturnFinalPosition()
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
        player1.PlayerPosition = new Position(wallX - 2, wallY, Math.PI / 4);
        endX = wallX - 0.199102;
        endY = wallY + 2.199102;
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(endX, battle.Bullets[0].BulletPosition.Xpos, 1e-5);
        Assert.Equal(endY, battle.Bullets[0].BulletPosition.Ypos, 1e-5);
        Assert.Equal(3 * Math.PI / 4, battle.Bullets[0].BulletPosition.Angle, 1e-5);
    }

    [Fact]
    public void GetBulletFinalPos_Hit0WallNotVertically_ShouldReturnFinalPosition()
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
                if (wall.X > 0 && wall.Y > 0 && wall.Angle == 0)
                {
                    wallX = wall.X * Constants.WALL_LENGTH;
                    wallY = wall.Y * Constants.WALL_LENGTH;
                    break;
                }
            }
        }
        player1.PlayerPosition = new Position(wallX, wallY - 2, Math.PI / 4);
        endX = wallX + 2.199102;
        endY = wallY - 0.199102;
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(endX, battle.Bullets[0].BulletPosition.Xpos, 1e-5);
        Assert.Equal(endY, battle.Bullets[0].BulletPosition.Ypos, 1e-5);
        Assert.Equal(-Math.PI / 4, battle.Bullets[0].BulletPosition.Angle, 1e-5);
    }

    [Fact(Skip = "Need to implement the test")]
    public void GetBulletFinalPos_HitWallTwice_ShouldReturnFinalPosition()
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
            for (int i = 0; i < map.Walls.Count - 1; i++)
            {
                var currentWall = map.Walls[i];
                var nextWall = map.Walls[i + 1];

                if (currentWall.X == nextWall.X && currentWall.Y == nextWall.Y)
                {
                    wallX = currentWall.X * Constants.WALL_LENGTH;
                    wallY = currentWall.Y * Constants.WALL_LENGTH;
                    break;
                }
            }
        }
        player1.PlayerPosition = new Position(wallX + Math.Sqrt(2) / 2, wallY + Math.Sqrt(2), -3 * Math.PI / 4);
        endX = wallX - 0.192031;
        endY = wallY - 0.192031;
        player1.PlayerAttack();
        battle.Tick();

        // Assert
        Assert.Equal(endX, battle.Bullets[0].BulletPosition.Xpos, 1e-5);
        Assert.Equal(endY, battle.Bullets[0].BulletPosition.Ypos, 1e-5);
        Assert.Equal(Math.PI / 4, battle.Bullets[0].BulletPosition.Angle, 1e-5);
    }
}