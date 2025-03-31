namespace Thuai.Server.Connection;

public partial class AgentServer
{
    public void HandleAfterGameTickEvent(object? sender, GameLogic.Game.AfterGameTickEventArgs e)
    {
        Protocol.Scheme.Stage currentStage;
        if (e.Game.Stage == GameLogic.Game.GameStage.Finished)
        {
            currentStage = Protocol.Scheme.Stage.END;
        }
        else if (e.Game.Stage == GameLogic.Game.GameStage.InBattle
            && e.Game.RunningBattle != null
            && e.Game.RunningBattle.Stage == GameLogic.Battle.BattleStage.InBattle)
        {
            currentStage = Protocol.Scheme.Stage.BATTLE;
        }
        else
        {
            currentStage = Protocol.Scheme.Stage.REST;
        }

        Publish(
            new Protocol.Messages.GameStatisticsMessage()
            {
                CurrentStage = currentStage.ToString(),
                CountDown = 0,  // TODO: Implement countdown
                Ticks = e.Game.CurrentTick,
                Scores = []
            }
        );

        if (e.Game.Stage != GameLogic.Game.GameStage.InBattle)
        {
            _logger.Debug("Game stage is not InBattle, skipping.");
            return;
        }
        if (e.Game.RunningBattle == null)
        {
            _logger.Debug("RunningBattle is null, skipping.");
            return;
        }

        if (e.Game.RunningBattle.Stage == GameLogic.Battle.BattleStage.InBattle)
        {
            if (e.Game.RunningBattle.Map == null)
            {
                _logger.Error("Cannot get map from RunningBattle, skipping.");
                return;
            }
            List<Protocol.Scheme.Wall> walls = [];
            List<Protocol.Scheme.Fence> fences = [];
            List<Protocol.Scheme.Bullet> bullets = [];
            foreach (GameLogic.MapGeneration.Wall wall in e.Game.RunningBattle.Map.Walls)
            {
                if (wall.Breakable == true)
                {
                    fences.Add(
                        new Protocol.Scheme.Fence()
                        {
                            Position = new()
                            {
                                X = wall.X,
                                Y = wall.Y,
                                Angle = wall.Angle,
                            },
                            Health = wall.WallDurability
                        }
                    );
                    continue;
                }
                else
                {
                    walls.Add(
                        new Protocol.Scheme.Wall()
                        {
                            X = wall.X,
                            Y = wall.Y,
                            Angle = wall.Angle,
                        }
                    );
                }
            }
            foreach (GameLogic.IBullet bullet in e.Game.RunningBattle.Bullets)
            {
                bullets.Add(
                    new Protocol.Scheme.Bullet()
                    {
                        No = bullet.Id,
                        IsMissile = bullet.IsMissile,
                        IsAntiArmor = bullet.AntiArmor,
                        Position = new()
                        {
                            X = bullet.BulletPosition.Xpos,
                            Y = bullet.BulletPosition.Ypos,
                            Angle = bullet.BulletPosition.Angle,
                        },
                        Speed = bullet.BulletSpeed,
                        Damage = bullet.BulletDamage,

                        // TODO: Implement TravelledDistance in IBullet
                        TraveledDistance = 0
                    }
                );
            }
            foreach (GameLogic.Player receiver in e.Game.AllPlayers)
            {
                if (receiver.IsBlinded == true)
                {
                    _logger.Debug($"Player {receiver.ID} is blinded, skipping.");
                    continue;
                }

                Publish(
                    new Protocol.Messages.EnvironmentInfoMessage()
                    {
                        Walls = [.. walls],
                        Fences = [.. fences],
                        Bullets = [.. bullets],
                        MapSize = e.Game.RunningBattle.Map.Height
                    },
                    receiver.Token
                );
                List<Protocol.Scheme.Player> players = [];
                foreach (GameLogic.Player player in e.Game.AllPlayers)
                {
                    List<Protocol.Scheme.Skill> skills = [];
                    foreach (GameLogic.ISkill skill in player.PlayerSkills)
                    {
                        skills.Add(
                            new()
                            {
                                Name = skill.Name.ToString(),
                                MaxCooldown = skill.MaxCooldown,
                                CurrentCooldown = skill.CurrentCooldown,
                                IsActive = skill.IsActive
                            }
                        );
                    }
                    players.Add(
                        new Protocol.Scheme.Player()
                        {
                            Token = (player.Token == receiver.Token) ? player.Token : "",
                            Weapon = new()
                            {
                                AttackSpeed = player.PlayerWeapon.AttackSpeed,
                                BulletSpeed = player.PlayerWeapon.BulletSpeed,
                                IsLaser = player.PlayerWeapon.IsLaser,
                                AntiArmor = player.PlayerWeapon.AntiArmor,
                                Damage = player.PlayerWeapon.Damage,
                                MaxBullets = player.PlayerWeapon.MaxBullets,
                                CurrentBullets = player.PlayerWeapon.CurrentBullets,
                            },
                            Armor = new()
                            {
                                CanReflect = player.PlayerArmor.CanReflect,
                                ArmorValue = player.PlayerArmor.ArmorValue,
                                Health = player.PlayerArmor.Health,
                                GravityField = player.PlayerArmor.GravityField,
                                Knife = player.PlayerArmor.Knife.State.ToString(),
                                DodgeRate = player.PlayerArmor.DodgeRate,
                            },
                            Skills = [.. skills],
                            Position = new()
                            {
                                X = player.PlayerPosition.Xpos,
                                Y = player.PlayerPosition.Ypos,
                                Angle = player.PlayerPosition.Angle,
                            }
                        }
                    );
                }
                Publish(
                    new Protocol.Messages.AllPlayerInfoMessage()
                    {
                        Players = [.. players]
                    },
                    receiver.Token
                );
            }
        }
        else if (e.Game.RunningBattle.Stage == GameLogic.Battle.BattleStage.ChoosingAward)
        {
            List<string> buffNames = [];
            foreach (GameLogic.Buff.Buff buff in e.Game.AvailableBuffsAfterCurrentBattle)
            {
                buffNames.Add(buff.ToString());
            }
            Publish(
                new Protocol.Messages.AvailableBuffsMessage()
                {
                    AvailableBuffs = [.. buffNames],
                }
            );
        }
    }

    public void HandleAfterPlayerConnectEvent(object? sender, GameController.GameRunner.AfterPlayerConnectEventArgs e)
    {
        // Remove all items whose value is e.Token
        List<Guid> keys = [];
        foreach (KeyValuePair<Guid, string> pair in _socketTokens)
        {
            if (pair.Value == e.Token)
            {
                keys.Add(pair.Key);
            }
        }
        foreach (Guid key in keys)
        {
            _socketTokens.TryRemove(key, out _);
        }

        _socketTokens.AddOrUpdate(e.SocketId, e.Token, (key, oldValue) => e.Token);
    }
}
