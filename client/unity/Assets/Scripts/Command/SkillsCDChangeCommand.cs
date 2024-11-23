using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class SkillsCDChangeCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly int _skills_number;
        private readonly float _skills_cd;

        public SkillsCDChangeCommand(int tankId, int skill_number, float cd)
        {
            _tankId = tankId;
            _skills_number = skill_number;
            _skills_cd = cd;
        }

        protected override void OnExecute()
        {
            var Skills_CD = this.GetModel<SkillsShow>().skills_cd[_tankId];
            Skills_CD[_skills_number].fillAmount = _skills_cd;
        }
        
    }
}