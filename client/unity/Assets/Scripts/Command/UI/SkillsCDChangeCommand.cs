using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class SkillsCDChangeCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly string _skill;
        private readonly float _skill_cd;

        public SkillsCDChangeCommand(int tankId, string skill, float cd)
        {
            _tankId = tankId;
            _skill = skill;
            _skill_cd = cd;
        }

        protected override void OnExecute()
        {
            var Skills_CD = this.GetModel<SkillsShow>().skills_cd[_tankId];
            var Skills_List = this.GetModel<SkillsShow>().skills_list[_tankId];
            var number = Skills_List.IndexOf(_skill);
            Skills_CD[number].fillAmount = _skill_cd;
        }
        
    }
}