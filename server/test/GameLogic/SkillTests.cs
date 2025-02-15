using Thuai.Server.GameLogic;

namespace Thuai.Server.Test.GameLogic
{
    public class SkillTests
    {
        [Fact]
        public void Skill_Constructor_ShouldSetNameCorrectly()
        {
            // Arrange
            var skillName = SkillName.SPEED_UP;

            // Act
            var skill = new Skill(skillName);

            // Assert
            Assert.Equal(skillName, skill.name);
        }

        [Fact]
        public void Skill_UpdateCoolDown_ShouldDecreaseCooldown()
        {
            // Arrange
            var skill = new Skill(SkillName.BLACK_OUT)
            {
                maxCooldown = 5,
                currentCooldown = 3
            };

            // Act
            skill.UpdateCoolDown();

            // Assert
            Assert.Equal(2, skill.currentCooldown);  // Cooldown should decrease by 1
        }

        [Fact]
        public void Skill_UpdateCoolDown_ShouldNotGoBelowZero()
        {
            // Arrange
            var skill = new Skill(SkillName.FLASH)
            {
                maxCooldown = 5,
                currentCooldown = 0
            };

            // Act
            skill.UpdateCoolDown();

            // Assert
            Assert.Equal(0, skill.currentCooldown);  // Cooldown should stay at 0
        }

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
