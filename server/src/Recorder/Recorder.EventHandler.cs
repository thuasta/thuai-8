namespace Thuai.Server.Recorder;

public partial class Recorder
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
            && (
                e.Game.RunningBattle.Stage == GameLogic.Battle.BattleStage.InBattle
                || e.Game.RunningBattle.Stage == GameLogic.Battle.BattleStage.ChoosingAward
            )   //ChoosingAward is added to record last tick of the battle
        )
        {
            currentStage = Protocol.Scheme.Stage.BATTLE;
        }
        else
        {
            currentStage = Protocol.Scheme.Stage.REST;
        }
        Protocol.Messages.StageInfoMessage stageInfo = new()
        {
            CurrentStage = currentStage,
            TotalTicks = e.Game.CurrentTick
        };

        if (currentStage == Protocol.Scheme.Stage.BATTLE)
        {
            List<Protocol.Scheme.Player> players = [];
            foreach (GameLogic.Player player in e.Game.AllPlayers)
            {
                List<Protocol.Scheme.Skill> skills = [];
                foreach (GameLogic.ISkill skill in player.PlayerSkills)

                {
                    skills.Add(
                        new Protocol.Scheme.Skill()
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
                        Token = player.RecordToken,
                        Weapon = new()
                        {
                            AttackSpeed = player.PlayerWeapon.AttackSpeed,
                            BulletSpeed = player.PlayerWeapon.BulletSpeed,
                            IsLaser = player.PlayerWeapon.IsLaser,
                            AntiArmor = player.PlayerWeapon.AntiArmor,
                            Damage = player.PlayerWeapon.Damage,
                            MaxBullets = player.PlayerWeapon.MaxBullets,
                            CurrentBullets = player.PlayerWeapon.CurrentBullets
                        },
                        Armor = new()
                        {
                            CanReflect = player.PlayerArmor.CanReflect,
                            ArmorValue = player.PlayerArmor.ArmorValue,
                            Health = player.PlayerArmor.Health,
                            GravityField = player.PlayerArmor.GravityField,
                            Knife = player.PlayerArmor.Knife.State.ToString(),
                            DodgeRate = player.PlayerArmor.DodgeRate
                        },
                        Skills = [.. skills],
                        Position = new()
                        {
                            // Relavant to the wall length
                            X = player.PlayerPosition.Xpos / GameLogic.Constants.WALL_LENGTH,
                            Y = player.PlayerPosition.Ypos / GameLogic.Constants.WALL_LENGTH,
                            Angle = player.PlayerPosition.Angle * 180 / Math.PI // Convert to degree
                        }
                    }
                );
            }

            Protocol.Scheme.PlayerUpdateEvent playerUpdate = new()
            {
                Players = [.. players]
            };

            List<Protocol.Scheme.Bullet> bullets = [];
            foreach (GameLogic.IBullet bullet in e.Game.RunningBattle?.Bullets ?? [])
            {
                bullets.Add(
                    new Protocol.Scheme.Bullet()
                    {
                        No = bullet.Id,
                        IsMissile = bullet.IsMissile,
                        IsAntiArmor = bullet.AntiArmor,
                        Position = new()
                        {
                            // Relavant to the wall length
                            X = bullet.BulletPosition.Xpos / GameLogic.Constants.WALL_LENGTH,
                            Y = bullet.BulletPosition.Ypos / GameLogic.Constants.WALL_LENGTH,
                            Angle = bullet.BulletPosition.Angle * 180 / Math.PI // Convert to degree
                        },
                        Speed = bullet.BulletSpeed,
                        Damage = bullet.BulletDamage,
                        TraveledDistance = 0
                    }
                );
            }

            Protocol.Scheme.BulletsUpdateEvent bulletsUpdate = new()
            {
                Bullets = [.. bullets]
            };

            List<Protocol.Scheme.Wall> walls = [];
            List<Protocol.Scheme.Fence> fences = [];
            foreach (GameLogic.MapGeneration.Wall wall in e.Game.RunningBattle?.Map?.Walls ?? [])
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
                                Angle = wall.Angle
                            },
                            Health = wall.WallDurability
                        }
                    );
                }
                else
                {
                    walls.Add(
                        new Protocol.Scheme.Wall()
                        {
                            X = wall.X,
                            Y = wall.Y,
                            Angle = wall.Angle
                        }
                    );
                }
            }

            List<Protocol.Scheme.Trap> traps = [];
            foreach (GameLogic.Trap trap in e.Game.RunningBattle?.Traps ?? [])
            {
                traps.Add(
                    new Protocol.Scheme.Trap()
                    {
                        Position = new()
                        {
                            // Relavant to the wall length
                            X = trap.TrapPosition.Xpos / GameLogic.Constants.WALL_LENGTH,
                            Y = trap.TrapPosition.Ypos / GameLogic.Constants.WALL_LENGTH,
                            Angle = trap.TrapPosition.Angle * 180 / Math.PI // Convert to degree
                        },
                        IsActive = !trap.IsDestroyed
                    }
                );
            }

            List<Protocol.Scheme.Laser> lasers = [];
            foreach (GameLogic.LaserBullet laser in e.Game.RunningBattle?.ActivatedLasers ?? [])
            {
                for (int i = 0; i < laser.Trace.Count - 1; ++i)
                {
                    lasers.Add(
                        new()
                        {
                            // Position is relavent to wall length
                            Start = new()
                            {
                                X = laser.Trace[i].X / GameLogic.Constants.WALL_LENGTH,
                                Y = laser.Trace[i].Y / GameLogic.Constants.WALL_LENGTH
                            },
                            End = new()
                            {
                                X = laser.Trace[i + 1].X / GameLogic.Constants.WALL_LENGTH,
                                Y = laser.Trace[i + 1].Y / GameLogic.Constants.WALL_LENGTH
                            }
                        }
                    );
                }
            }

            Protocol.Scheme.MapUpdateEvent mapUpdate = new()
            {
                Walls = [.. walls],
                Fences = [.. fences],
                Traps = [.. traps],
                Laser = [.. lasers]
            };

            List<Protocol.Scheme.BattleUpdateEvent> battleUpdateEvent = [];
            battleUpdateEvent.Add(playerUpdate);
            battleUpdateEvent.Add(bulletsUpdate);
            battleUpdateEvent.Add(mapUpdate);

            Protocol.Messages.BattleUpdateMessage battleUpdate = new()
            {
                BattleTicks = e.Game.RunningBattle?.CurrentTick ?? 0,
                Events = [.. battleUpdateEvent]
            };

            Record(stageInfo, battleUpdate);
        }
        else if (currentStage == Protocol.Scheme.Stage.REST
            && e.Game.RunningBattle?.Stage == GameLogic.Battle.BattleStage.Waiting
            && e.Game.HasAwardBeforeBattle == true)
        {
            List<Protocol.Messages.Detail> buffDetails = [];
            foreach (GameLogic.Player player in e.Game.AllPlayers)
            {
                buffDetails.Add(
                    new()
                    {
                        Token = player.RecordToken,
                        Buff = player.LastChosenBuff?.ToString() ?? ""
                    }
                );
            }
            Protocol.Messages.BuffSelectMessage buffSelect = new()
            {
                Details = [.. buffDetails]
            };
            Record(stageInfo, buffSelect);
        }
        else
        {
            Record(stageInfo);
        }
    }
}
