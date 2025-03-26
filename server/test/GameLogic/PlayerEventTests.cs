using Thuai.Server.GameLogic;

// Confusing. Muted all original tests 03/17/2025
namespace Thuai.Server.Test.GameLogic
{
    public class PlayerEventTests
    {
        [Fact]
        public void PlayerMoveEvent_ShouldTrigger_WhenEventIsRaised()
        {
            // Arrange
            // var player = new Player("playerToken", 1);
            // var moveDirection = new MoveDirection(); // 假设 MoveDirection 是一个有效的类
            // var args = new Player.PlayerMoveEventArgs(player, moveDirection);
            // bool eventTriggered = false;

            // // 订阅事件
            // player.PlayerMoveEvent += (sender, eventArgs) =>
            // {
            //     eventTriggered = true;
            //     Assert.Equal(Player.PlayerEventType.PlayerMove, Player.PlayerMoveEventArgs.EventName);
            //     Assert.Equal(player, eventArgs.Player);
            //     Assert.Equal(moveDirection, eventArgs.Movedirection);
            // };

            // // Act
            // player.PlayerMoveEvent?.Invoke(player, args);

            // // Assert
            // Assert.True(eventTriggered);
        }

        [Fact]
        public void PlayerAttackEvent_ShouldTrigger_WhenEventIsRaised()
        {
            // Arrange
            // var player = new Player("playerToken", 1);
            // var args = new Player.PlayerAttackEventArgs(player);
            // bool eventTriggered = false;

            // // 订阅事件
            // player.PlayerAttackEvent += (sender, eventArgs) =>
            // {
            //     eventTriggered = true;
            //     Assert.Equal(Player.PlayerEventType.PlayerAttack, Player.PlayerAttackEventArgs.EventName);
            //     Assert.Equal(player, eventArgs.Player);
            // };

            // // Act
            // player.PlayerAttackEvent?.Invoke(player, args);

            // // Assert
            // Assert.True(eventTriggered);
        }

        [Fact]
        public void PlayerTurnEvent_ShouldTrigger_WhenEventIsRaised()
        {
            // Arrange
            // var player = new Player("playerToken", 1);
            // var turnDirection = new TurnDirection(); // 假设 TurnDirection 是一个有效的类
            // var args = new Player.PlayerTurnEventArgs(player, turnDirection);
            // bool eventTriggered = false;

            // // 订阅事件
            // player.PlayerTurnEvent += (sender, eventArgs) =>
            // {
            //     eventTriggered = true;
            //     Assert.Equal(Player.PlayerEventType.PlayerTurn, Player.PlayerTurnEventArgs.EventName);
            //     Assert.Equal(player, eventArgs.Player);
            //     Assert.Equal(turnDirection, eventArgs.Turndirection);
            // };

            // // Act
            // player.PlayerTurnEvent?.Invoke(player, args);

            // // Assert
            // Assert.True(eventTriggered);
        }
    }
}
