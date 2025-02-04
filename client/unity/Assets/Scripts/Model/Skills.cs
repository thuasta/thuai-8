using System;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCity
{
    public class Skills
    {
        public enum SkillName
        {
            BLACK_OUT,
            SPEED_UP,
            FLASH,
            DESTROY,
            CONSTRUCT,
            TRAP,
            MISSILE,
            KAMUI
        }

        public class Skill
        {
            public SkillName name { get; set; }
            public int maxCooldown { get; set; }
            public int currentCooldown { get; set; }

            public bool isActive { get; set; }

            public Skill(SkillName name, int maxCooldown, int currentCooldown, bool isActive)
            {
                this.name = name;
                this.maxCooldown = maxCooldown;
                this.currentCooldown = currentCooldown;
                this.isActive = isActive;
            }
        }
        
        public List<Skill> skills {get; set;}

        public Skills()
        {
            skills = new List<Skill>();
        }

        public void UpdateSkill(string skillName, int maxCooldown, int currentCooldown, bool isActive)
        {
            if (!Enum.TryParse(typeof(SkillName), skillName, true, out var result))
            {
                throw new ArgumentException($"Invalid skill name: {skillName}");
            }
            SkillName skillEnum = (SkillName)result;
            Skill existingSkill = skills.Find(skill => skill.name == skillEnum);
            if (existingSkill != null)
            {
                // 找到技能，更新其属性
                existingSkill.maxCooldown = maxCooldown;
                existingSkill.currentCooldown = currentCooldown;
                existingSkill.isActive = isActive;
            }
            else
            {
                // 没有找到技能，添加一个新的技能
                Skill newSkill = new Skill(skillEnum, maxCooldown, currentCooldown, isActive);
                skills.Add(newSkill);
            }
        }

        
    }
}