using Thuai.Server.GameLogic;

//Checked original tests 03/17/2025
namespace Thuai.Server.Test.GameLogic
{
    public class SkillTests
    {
        // [Fact]
        // public void Skill_Constructor_ShouldSetNameCorrectly()
        // {
        //     // Arrange
        //     var skillName = SkillName.SPEED_UP;

        //     // Act
        //     var skill = new ISkill(skillName);

        //     // Assert
        //     Assert.Equal(skillName, skill.Name);
        // }

        // [Fact]
        // public void Skill_UpdateCoolDown_ShouldDecreaseCooldown()
        // {
        //     // Arrange
        //     var skill = new ISkill(SkillName.BLACK_OUT)
        //     {
        //         CurrentCooldown = 3
        //     };

        //     // Act
        //     skill.UpdateCoolDown();

        //     // Assert
        //     Assert.Equal(2, skill.CurrentCooldown);  // Cooldown should decrease by 1
        // }

        // [Fact]
        // public void Skill_UpdateCoolDown_ShouldNotGoBelowZero()
        // {
        //     // Arrange
        //     var skill = new ISkill(SkillName.FLASH)
        //     {
        //         CurrentCooldown = 0
        //     };

        //     // Act
        //     skill.UpdateCoolDown();

        //     // Assert
        //     Assert.Equal(0, skill.CurrentCooldown);  // Cooldown should stay at 0
        // }

        [Fact]
        public void SkillName_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(0, (int)SkillName.BLACK_OUT);
            Assert.Equal(1, (int)SkillName.SPEED_UP);
            Assert.Equal(2, (int)SkillName.FLASH);
            Assert.Equal(3, (int)SkillName.DESTROY);
            Assert.Equal(4, (int)SkillName.CONSTRUCT);
            Assert.Equal(5, (int)SkillName.TRAP);
            Assert.Equal(6, (int)SkillName.MISSILE);
            Assert.Equal(7, (int)SkillName.KAMUI);
        }
    }
}
