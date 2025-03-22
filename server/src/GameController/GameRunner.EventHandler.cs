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

                    _logger.Information($"[Player {player.ID}] Move direction set to {moveDirection}.");

                    break;

                case Protocol.Messages.PerformTurnMessage turnMessage:
                    GameLogic.TurnDirection turnDirection = turnMessage.Direction switch
                    {
                        "CLOCKWISE" => GameLogic.TurnDirection.CLOCKWISE,
                        "COUNTER_CLOCKWISE" => GameLogic.TurnDirection.COUNTER_CLOCKWISE,
                        _ => GameLogic.TurnDirection.NONE
                    };
                    player.TurnDirection = turnDirection;

                    _logger.Information($"[Player {player.ID}] Turn direction set to {turnDirection}.");
                    

                    break;

                case Protocol.Messages.PerformAttackMessage attackMessage:
                    if (Game.Stage != GameLogic.Game.GameStage.InBattle
                        || Game.RunningBattle == null
                        || Game.RunningBattle.Stage != GameLogic.Battle.BattleStage.InBattle)
                    {
                        _logger.Error($"[Player {player.ID}] Cannot attack when not in battle.");
                        return;
                    }
                    player.PlayerAttack();
                    break;

                case Protocol.Messages.PerformSkillMessage skillMessage:
                    if (Game.Stage != GameLogic.Game.GameStage.InBattle
                        || Game.RunningBattle == null
                        || Game.RunningBattle.Stage != GameLogic.Battle.BattleStage.InBattle)
                    {
                        _logger.Error($"[Player {player.ID}] Cannot use skill when not in battle.");
                        return;
                    }
                    player.PlayerPerformSkill(GameLogic.ISkill.SkillNameFromString(skillMessage.SkillName));
                    break;

                case Protocol.Messages.PerformSelectMessage selectMessage:
                    if (Game.Stage != GameLogic.Game.GameStage.InBattle
                        || Game.RunningBattle == null
                        || Game.RunningBattle.Stage != GameLogic.Battle.BattleStage.ChoosingAward)
                    {
                        _logger.Error(
                            $"[Player {player.ID}] Cannot select when not in battle or battle stage is not ChoosingAward."
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
                            player.HasChosenAward = true;
                            break;
                        }
                    }

                    break;

                case Protocol.Messages.GetPlayerinfoMessage getPlayerinfoMessage:
                    _logger.Debug(
                        $"[Player {player.ID}] Requested player info."
                    );

                    List<Protocol.Scheme.Player> players = [];
                    foreach (GameLogic.Player p in Game.AllPlayers)
                    {
                        List<Protocol.Scheme.Skill> skills = [];
                        foreach (GameLogic.ISkill s in p.PlayerSkills)
                        {
                            skills.Add(new Protocol.Scheme.Skill
                            {
                                Name = s.Name.ToString(),
                                MaxCooldown = s.MaxCooldown,
                                CurrentCooldown = s.CurrentCooldown,
                                IsActive = s.IsActive
                            });
                        }
                        players.Add(new Protocol.Scheme.Player
                        {
                            Token = player.Token == p.Token ? p.Token : "",
                            Weapon = new()
                            {
                                AttackSpeed = p.PlayerWeapon.AttackSpeed,
                                BulletSpeed = p.PlayerWeapon.BulletSpeed,
                                IsLaser = p.PlayerWeapon.IsLaser,
                                AntiArmor = p.PlayerWeapon.AntiArmor,
                                Damage = p.PlayerWeapon.Damage,
                                MaxBullets = p.PlayerWeapon.MaxBullets,
                                CurrentBullets = p.PlayerWeapon.CurrentBullets
                            },
                            Armor = new()
                            {
                                CanReflect = p.PlayerArmor.CanReflect,
                                ArmorValue = p.PlayerArmor.ArmorValue,
                                Health = p.PlayerArmor.Health,
                                GravityField = p.PlayerArmor.GravityField,
                                Knife = p.PlayerArmor.Knife.ToString(),
                                DodgeRate = p.PlayerArmor.DodgeRate
                            },
                            Skills = [.. skills],
                            Position = new()
                            {
                                X = p.PlayerPosition.Xpos,
                                Y = p.PlayerPosition.Ypos,
                                Angle = p.PlayerPosition.Angle
                            },
                        });
                    }
                    Protocol.Messages.AllPlayerInfoMessage response = new()
                    {
                        Players = [.. players]
                    };
                    AfterPlayerRequestEvent?.Invoke(this, new AfterPlayerRequestEventArgs(player.Token, response));
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
