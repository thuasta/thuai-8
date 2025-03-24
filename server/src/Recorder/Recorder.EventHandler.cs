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
            && e.Game.RunningBattle.Stage == GameLogic.Battle.BattleStage.InBattle)
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
                        Token = (player.ID + 1).ToString(),   // Because client actually reads ID and it starts from 1 ...
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
                            Angle = player.PlayerPosition.Angle
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
                        Position = new()
                        {
                            // Relavant to the wall length
                            X = bullet.BulletPosition.Xpos / GameLogic.Constants.WALL_LENGTH,
                            Y = bullet.BulletPosition.Ypos / GameLogic.Constants.WALL_LENGTH,
                            Angle = bullet.BulletPosition.Angle
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
            foreach (GameLogic.MapGenerator.Wall wall in e.Game.RunningBattle?.Map?.Walls ?? [])
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

            Protocol.Scheme.MapUpdateEvent mapUpdate = new()
            {
                Walls = [.. walls],
                // TODO: Add other map elements
                Fences = [],
                Traps = [],
                Laser = []
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
        else if (currentStage == Protocol.Scheme.Stage.REST)
        {
            // TODO: Add reward choosing information
            Record(stageInfo);
        }
        else
        {
            Record(stageInfo);
        }
    }
}
