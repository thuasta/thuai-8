using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class SkillsAddCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly string _skill;
        public SkillsAddCommand(int tankId, string skill)
        {
            _tankId = tankId;
            _skill = skill;
        }

        protected override void OnExecute()
        {
            var Skills_Image = this.GetModel<SkillsShow>().skills_image[_tankId];
            var Skills_List = this.GetModel<SkillsShow>().skills_list[_tankId];
            Skills_List.Add(_skill);
            Skills_Image[Skills_List.Count - 1].sprite = Resources.Load<Sprite>(_skill); 
        }
        
    }
}