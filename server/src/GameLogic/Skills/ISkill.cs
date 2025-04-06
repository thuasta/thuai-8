namespace Thuai.Server.GameLogic;

public partial interface ISkill
{
    public SkillName Name { get; }
    public int MaxCooldown { get; }
    public int CurrentCooldown { get; }
    public bool IsAvailable { get; }
    public bool IsActive { get; }

    public static SkillName SkillNameFromString(string skillName)
    {
        return (SkillName)Enum.Parse(typeof(SkillName), skillName);
    }

    public void Update();
    public void Reset();
    public void Recover();
    public void Activate();
    public void Deactivate();
}
