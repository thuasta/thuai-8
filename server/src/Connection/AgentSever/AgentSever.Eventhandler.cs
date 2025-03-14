using System.Security.Cryptography.X509Certificates;

namespace Thuai.Server.Connection;

public partial class AgentServer
{
    public void HandleAfterGameTickEvent(object? sender, GameLogic.Game.AfterGameTickEventArgs e)
    {
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
            List<Wall> walls = [];
            List<Bullet> bullets = [];
            foreach (GameLogic.MapGenerator.Wall wall in e.Game.RunningBattle.Map.Walls)
            {
                walls.Add(
                    new Wall()
                    {
                        Position = new()
                        {
                            X = wall.X,
                            Y = wall.Y,
                            Angle = wall.Angle,
                        }
                    }
                );
            }
            foreach (GameLogic.IBullet bullet in e.Game.RunningBattle.Bullets)
            {
                bullets.Add(
                    new Bullet()
                    {
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
            Publish(
                new EnvironmentInfoMessage()
                {
                    Walls = [..walls],
                    Fences = [],                    // TODO: Implement Fences
                    Bullets = [..bullets],
                    MapSize = e.Game.RunningBattle.Map.Height
                }
            );
            foreach (GameLogic.Player receiver in e.Game.AllPlayers)
            {
                List<Player> players = [];
                foreach(GameLogic.Player player in e.Game.AllPlayers)
                {
                    List<Skill> skills = [];
                    foreach (GameLogic.Skill skill in player.PlayerSkills)
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
                        new Player()
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
                                Knife = player.PlayerArmor.Knife.ToString(),
                                DodgeRate = player.PlayerArmor.DodgeRate,
                            },
                            Skills = [..skills],
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
                    new AllPlayerInfoMessage()
                    {
                        Players = [..players]
                    },
                    receiver.Token
                );
            }
        }
        else if (e.Game.RunningBattle.Stage == GameLogic.Battle.BattleStage.ChoosingAward)
        {
            List<string> buffNames = [];
            foreach (GameLogic.Buff.Buff buff in e.Game.AvilableBuffsAfterCurrentBattle)
            {
                buffNames.Add(buff.ToString());
            } 
            Publish(
                new AvailableBuffsMessage()
                {
                    AvailableBuffs = [..buffNames],
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
