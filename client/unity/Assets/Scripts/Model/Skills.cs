using System;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCity
{
    public class Skills
    {
        public enum Skill
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
        
        public List<Skill> skills {get; set;}

        public Skills()
        {
            skills = new List<Skill>();
        }

        public bool AddSkill(string skillName)
        {        
            if (Enum.TryParse(skillName.ToUpper(), out Skill skill))
            {          
                if (!skills.Contains(skill))
                {
                    skills.Add(skill);                                        
                }
                return true;
            }
            else
            {
                Debug.LogError($"{skillName} is invaild!");
                return false;
            }
        }
        
    }
}