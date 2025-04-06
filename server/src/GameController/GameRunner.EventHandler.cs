namespace Thuai.Server.GameController;

public partial class GameRunner
{
    public void HandleAfterMessageReceiveEvent(object? sender, Connection.AgentServer.AfterMessageReceiveEventArgs e)
    {
        try
        {
            if (e.Message is not Protocol.Messages.PerformMessage message)
            {
                _logger.Error(
                    $"message \"{Utility.Tools.LogHandler.Truncate(e.Message.MessageType, 32)}\" shouldn't come from a player."
                );
                return;
            }

            Protocol.Messages.PerformMessage performMessage = message;
            _logger.Debug(
                $"Received message \"{Utility.Tools.LogHandler.Truncate(performMessage.MessageType, 32)}\" "
                + $"from player {Utility.Tools.LogHandler.Truncate(performMessage.Token, 8)}."
            );

            GameLogic.Player? player = Game.FindPlayer(performMessage.Token);
            if (player == null)
            {
                _logger.Error(
                    $"Player with token {Utility.Tools.LogHandler.Truncate(performMessage.Token, 8)} not found."
                );
                return;
            }

            AfterPlayerConnectEvent?.Invoke(this, new AfterPlayerConnectEventArgs(player.Token, e.SocketId));

            switch (performMessage)
            {
                case Protocol.Messages.PerformMoveMessage moveMessage:
                    GameLogic.MoveDirection moveDirection = moveMessage.Direction switch
                    {
                        "FORTH" => GameLogic.MoveDirection.FORTH,
                        "BACK" => GameLogic.MoveDirection.BACK,
                        _ => GameLogic.MoveDirection.NONE
                    };
                    player.MoveDirection = moveDirection;

                    if (double.IsNaN(moveMessage.Distance) || double.IsInfinity(moveMessage.Distance))
                    {
                        _logger.Error($"[Player {player.ID}] Invalid speed: {moveMessage.Distance}.");
                        player.Speed = 0;
                        return;
                    }
                    player.Speed = Math.Clamp((float)moveMessage.Distance, 0, player.MaxSpeed);

                    _logger.Debug($"[Player {player.ID}] Move direction set to {moveDirection}.");
                    _logger.Debug($"[Player {player.ID}] Speed set to {player.Speed}.");

                    break;

                case Protocol.Messages.PerformTurnMessage turnMessage:
                    GameLogic.TurnDirection turnDirection = turnMessage.Direction switch
                    {
                        "CLOCKWISE" => GameLogic.TurnDirection.CLOCKWISE,
                        "COUNTER_CLOCKWISE" => GameLogic.TurnDirection.COUNTER_CLOCKWISE,
                        _ => GameLogic.TurnDirection.NONE
                    };
                    player.TurnDirection = turnDirection;

                    double angle = turnMessage.Angle * Math.PI / 180.0; // Convert to radians
                    if (double.IsNaN(angle) || double.IsInfinity(angle))
                    {
                        _logger.Error($"[Player {player.ID}] Invalid angular speed: {angle}.");
                        player.TurnSpeed = 0;
                        return;
                    }
                    player.TurnSpeed = Math.Clamp((float)angle, 0, player.MaxTurnSpeed);

                    _logger.Debug($"[Player {player.ID}] Turn direction set to {turnDirection}.");
                    _logger.Debug($"[Player {player.ID}] Turn speed set to {player.TurnSpeed}.");

                    break;

                case Protocol.Messages.PerformAttackMessage attackMessage:
                    if (Game.Stage != GameLogic.Game.GameStage.InBattle
                        || Game.RunningBattle == null
                        || Game.RunningBattle.Stage != GameLogic.Battle.BattleStage.InBattle)
                    {
                        _logger.Debug($"[Player {player.ID}] Cannot attack when not in battle.");
                        return;
                    }
                    player.PlayerAttack();
                    break;

                case Protocol.Messages.PerformSkillMessage skillMessage:
                    if (Game.Stage != GameLogic.Game.GameStage.InBattle
                        || Game.RunningBattle == null
                        || Game.RunningBattle.Stage != GameLogic.Battle.BattleStage.InBattle)
                    {
                        _logger.Debug($"[Player {player.ID}] Cannot use skill when not in battle.");
                        return;
                    }
                    player.PlayerPerformSkill(GameLogic.ISkill.SkillNameFromString(skillMessage.SkillName));
                    break;

                case Protocol.Messages.PerformSelectMessage selectMessage:
                    if (Game.Stage != GameLogic.Game.GameStage.InBattle
                        || Game.RunningBattle == null
                        || Game.RunningBattle.Stage != GameLogic.Battle.BattleStage.ChoosingAward)
                    {
                        _logger.Debug(
                            $"[Player {player.ID}] Cannot select award when not in battle or battle stage is not ChoosingAward."
                        );
                        return;
                    }
                    if (player.HasChosenAward)
                    {
                        _logger.Error($"[Player {player.ID}] An award is already chosen.");
                        return;
                    }
                    if (!Game.AvailableBuffsAfterCurrentBattle.Any(buff => buff.ToString() == selectMessage.BuffName))
                    {
                        _logger.Error($"[Player {player.ID}] Cannot choose a buff that is not in given awards.");
                        return;
                    }

                    int awardId = 0;
                    foreach (GameLogic.Buff.Buff award in Game.AvailableBuffsAfterCurrentBattle)
                    {
                        awardId++;
                        if (award.ToString() == selectMessage.BuffName)
                        {
                            Game.BuffSelector.SelectBuff(player, awardId);
                            break;
                        }
                    }

                    break;

                case Protocol.Messages.GetPlayerinfoMessage getPlayerinfoMessage:
                    _logger.Debug(
                        $"[Player {player.ID}] Requested player info."
                    );
                    break;

                default:
                    _logger.Warning(
                        $"Message \"{Utility.Tools.LogHandler.Truncate(performMessage.MessageType, 32)}\" is not supported."
                    );
                    _logger.Warning(
                        $"Still, the token will be used to identify the player."
                    );
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to handle received message:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }
}
