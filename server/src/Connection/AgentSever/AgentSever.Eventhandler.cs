using GameServer.GameController;
using GameServer.GameLogic;

namespace GameServer.Connection;

public partial class AgentServer
{
    public void HandleAfterGameTickEvent(object? sender, AfterGameTickEventArgs e)
    {
        // Add schemes in EnvironmentInfo
        // Add walls and fences
        List<EnvironmentInfoMessage.Wall> walls = new();
        List<EnvironmentInfoMessage.Fence> fences= new();
        foreach (Wall ArgsWall in e.EnvironmentInfo.Walls)
        {
            walls.Add(
                new EnvironmentInfoMessage.Wall
                {
                    Position = new EnvironmentInfoMessage.position
                    {
                        X = ArgsWall.position.x,
                        Y = ArgsWall.position.y,
                        Angle = ArgsWall.position.angle
                    }
                }
            );
        }

        foreach (Fence ArgsFence in e.EnvironmentInfo.Fences)
        {
            fences.Add(
                new EnvironmentInfoMessage.Fence
                {
                    Position = new EnvironmentInfoMessage.position
                    {
                        X = ArgsFence.position.x,
                        Y = ArgsFence.position.Y,
                        Angle = ArgsFence.position.angle
                    },
                    Health = ArgsFence.health
                }
            );
        }

        // Add bullets
        List<EnvironmentInfoMessage.Bullet> bullets = new();
        foreach (Bullet ArgsBullet in e.EnvironmentInfo.Bullets)
        {
            bullets.Add(
                new EnvironmentInfoMessage.Bullet
                {
                    Position = new EnvironmentInfoMessage.position
                    {
                        X = ArgsBullet.position.x,
                        Y = ArgsBullet.position.y,
                        Angle = ArgsBullet.Position.angle
                    },
                    Speed = ArgsBullet.speed,
                    Damage = ArgsBullet.damage,
                    TraveledDistance = ArgsBullet.traveledDistance
                }
            );
        }

        // Add player positions
        List<EnvironmentInfoMessage.playerPositions> playerPositions = new();
        foreach (playerPositions ArgsPlayerPosition in e.EnvironmentInfo.playerPositions)
        {
            playerPositions.Add(
                new EnvironmentInfoMessage.playerPosition
                {
                    Position = new EnvironmentInfoMessage.position
                    {
                        X = ArgsPlayerPosition.position.x,
                        Y = ArgsPlayerPosition.position.y,
                        Angle = ArgsPlayerPosition.position.angle
                    },
                    Token = ArgsPlayerPosition.token
                }
            );
        }

        //add player info
        List<PlayerInfoMessage> playerInfos = new();
        foreach (PlayerInfo ArgsPlayerInfo in e.PlayerInfo) {
            //add weapon 
            PlayerInfoMessage.weapon weapon = new()
            {
                AttackSpeed = ArgsPlayerInfo.weapon.attackSpeed,
                BulletSpeed = ArgsPlayerInfo.weapon.bulletSpeed,
                IsLaser = ArgsPlayerInfo.weapon.isLaser,
                AntiArmor = ArgsPlayerInfo.weapon.antiArmor,
                Damage = ArgsPlayerInfo.weapon.damage,
                MaxBullets = ArgsPlayerInfo.weapon.maxBullets,
                CurrentBullets = ArgsPlayerInfo.weapon.currentBullets
            };

            //add armor
            PlayerInfoMessage.armor armor = new()
            {
                CanReflect = ArgsPlayerInfo.armor.canReflect,
                ArmorValue = ArgsPlayerInfo.armor.armorValue,
                Health = ArgsPlayerInfo.armor.health,
                GravityField = ArgsPlayerInfo.armor.gravityField,
                Knife = ArgsPlayerInfo.armor.knife,
                DodgeRate = ArgsPlayerInfo.armor.dodgeRate
            };

            //add skills
            List<PlayerInfoMessage.skill> skills = new();
            foreach (skill ArgsSkill in ArgsPlayerInfo.skills) {
                skills.Add(
                    new PlayerInfoMessage.skill
                    {
                        Name = ArgsSkill.name,
                        MaxCooldown = ArgsSkill.maxcooldown,
                        CurrentCooldown = ArgsSkill.currentCooldown,
                        IsActive = ArgsSkill.isActive
                    }
                );
            }

            PlayerInfoMessage.Position = new PlayerInfoMessage.position
            {
                X = ArgsPlayerInfo.position.x,
                Y = ArgsPlayerInfo.position.y,
                Angle = ArgsPlayerInfo.position.angle
            };

            playerInfos.Add(
                new PlayerInfoMessage
                {
                    Token = ArgsPlayerInfo.token,
                    Weapon = weapon,
                    Armor = armor,
                    Skills = skills,
                    Position = position
                }
            );
        }

        //publish the message
        //temporarily used last yera's code as template
        Publish (
            new EnvironmentInfoMessage
            {
                MessageType = "ENVIRONMENT_INFO",
                Position = new EnvironmentInfoMessage.position
                {
                    X = e.EnvironmentInfo.Position.x,
                    Y = e.EnvironmentInfo.Position.y,
                    Angle = e.EnvironmentInfo.Position.angle
                },
                Walls = walls,
                Fences = fences,
                Bullets = bullets,
                PlayerPositions = playerPositions
            }
        );

        foreach (PlayerInfoMessage playerInfomessage in playerInfos)
        {
            Publish(playerInfomessage);
        }
    }

    public void HandleAfterPlayerConnectEvent(object? sender, AfterPlayerConnect e)
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
