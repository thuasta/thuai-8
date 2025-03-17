namespace Thuai.Server.Recorder;

public partial class Recorder
{
    public void HandleAfterGameTickEvent(object? sender, GameLogic.Game.AfterGameTickEventArgs e)
    {
        StageChange stageChange = new()
        {
            targetStage = e.Game.Stage.ToString(),
        };
        Record(stageChange);

        if (e.Game.Stage == GameLogic.Game.GameStage.InBattle && e.Game.RunningBattle != null)
        {
            List<playerType> players = [];
            foreach (GameLogic.Player player in e.Game.AllPlayers)
            {
                List<playerType.skillType> skills = [];
                foreach (GameLogic.Skill skill in player.PlayerSkills)
                {
                    skills.Add(
                        new playerType.skillType()
                        {
                            name = skill.Name.ToString(),
                            maxCoolDown = skill.MaxCooldown,
                            currentCoolDown = skill.CurrentCooldown,
                            isActive = skill.IsActive
                        }
                    );
                }
                players.Add(
                    new playerType()
                    {
                        token = player.Token,
                        weapon = new()
                        {
                            attackSpeed = player.PlayerWeapon.AttackSpeed,
                            bulletSpeed = player.PlayerWeapon.BulletSpeed,
                            isLaser = player.PlayerWeapon.IsLaser,
                            antiArmor = player.PlayerWeapon.AntiArmor,
                            damage = player.PlayerWeapon.Damage,
                            maxBullets = player.PlayerWeapon.MaxBullets,
                            currentBullets = player.PlayerWeapon.CurrentBullets
                        },
                        armor = new()
                        {
                            canReflect = player.PlayerArmor.CanReflect,
                            armorValue = player.PlayerArmor.ArmorValue,
                            health = player.PlayerArmor.Health,
                            gravityField = player.PlayerArmor.GravityField,
                            knife = player.PlayerArmor.Knife.ToString(),
                            dodgeRate = player.PlayerArmor.DodgeRate
                        },
                        skills = [.. skills],
                        position = new()
                        {
                            x = player.PlayerPosition.Xpos,
                            y = player.PlayerPosition.Ypos,
                            angle = player.PlayerPosition.Angle
                        }
                    }
                );
            }
            BattleUpdate battleUpdate = new()
            {
                currentTicks = e.Game.RunningBattle?.CurrentTick ?? 0,
                Players = [.. players],
                Events = []             // TODO: Add events
            };

            Record(battleUpdate);
        }
    }
}
