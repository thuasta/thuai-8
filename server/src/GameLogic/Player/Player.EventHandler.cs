namespace Thuai.Server.GameLogic;

public partial class Player
{
    public void OnSkillActivation(object? sender, ISkill.OnActivationEventArgs e)
    {
        try
        {
            switch (e.Name)
            {
                case SkillName.BLACK_OUT:
                    SkillActivationEvent?.Invoke(this, new(this, e.Name));
                    break;

                case SkillName.SPEED_UP:
                    if (Body is null)
                    {
                        _logger.Error("Cannot activate SpeedUp skill: body is null.");
                        return;
                    }
                    Physics.Tag tag = (Physics.Tag)Body.Tag;
                    tag.AttachedData[Physics.Key.SpeedUpFactor] = Constants.SkillEffect.SPPED_UP_FACTOR;
                    break;

                default:
                    throw new ArgumentException($"Invalid skill name {e.Name}.");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Error activating skill {e.Name}:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }

    public void OnSkillDeactivation(object? sender, ISkill.OnDeactivationEventArgs e)
    {
        try
        {
            switch (e.Name)
            {
                case SkillName.BLACK_OUT:
                    SkillDeactivationEvent?.Invoke(this, new(this, e.Name));
                    break;

                case SkillName.SPEED_UP:
                    if (Body is null)
                    {
                        _logger.Error("Cannot deactivate SpeedUp skill: body is null.");
                        return;
                    }
                    Physics.Tag tag = (Physics.Tag)Body.Tag;
                    tag.AttachedData[Physics.Key.SpeedUpFactor] = 1f;
                    break;

                default:
                    throw new ArgumentException($"Invalid skill name {e.Name}.");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Error deactivating skill {e.Name}:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }
}
