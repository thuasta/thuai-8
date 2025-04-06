using Thuai.Server.Protocol.Scheme;

namespace Thuai.Server.GameLogic;

public partial class Player
{
    public void OnSkillActivation(object? sender, ISkill.OnActivationEventArgs e)
    {
        try
        {
            switch (e.Name)
            {
                case SkillName.SPEED_UP:
                    if (Body is null)
                    {
                        _logger.Error("Cannot activate SpeedUp skill: body is null.");
                        return;
                    }
                    Physics.Tag tag = (Physics.Tag)Body.Tag;
                    tag.AttachedData[Physics.Key.SpeedUpFactor] = Constants.SkillEffect.SPPED_UP_FACTOR;
                    break;

                case SkillName.FLASH:
                    if (Body is null)
                    {
                        _logger.Error("Cannot activate Flash skill: body is null.");
                        return;
                    }
                    Position newPosition = new(
                        PlayerPosition.Xpos + Constants.SkillEffect.FLASH_DISTANCE * Orientation.X,
                        PlayerPosition.Ypos + Constants.SkillEffect.FLASH_DISTANCE * Orientation.Y,
                        PlayerPosition.Angle
                    );
                    PlayerPosition = newPosition;
                    break;

                case SkillName.RECOVER:
                    Recover();
                    break;

                case SkillName.KAMUI:
                    Kamui = true;
                    _stunCounter.Clear();   // Remove stun effects
                    break;

                case SkillName.BLACK_OUT:
                case SkillName.DESTROY:
                case SkillName.CONSTRUCT:
                case SkillName.TRAP:
                    // Don't have effect on player itself
                    break;

                default:
                    throw new ArgumentException($"Invalid skill name {e.Name}.");
            }

            SkillActivationEvent?.Invoke(this, new(this, e.Name));
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
                    // Don't have effect on player itself
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

                case SkillName.KAMUI:
                    Kamui = false;
                    break;

                // Instant skills do not have a deactivation event
                case SkillName.FLASH:
                case SkillName.DESTROY:
                case SkillName.CONSTRUCT:
                case SkillName.TRAP:
                case SkillName.RECOVER:
                    break;

                default:
                    throw new ArgumentException($"Invalid skill name {e.Name}.");
            }

            SkillDeactivationEvent?.Invoke(this, new(this, e.Name));
        }
        catch (Exception ex)
        {
            _logger.Error($"Error deactivating skill {e.Name}:");
            Utility.Tools.LogHandler.LogException(_logger, ex);
        }
    }
}
