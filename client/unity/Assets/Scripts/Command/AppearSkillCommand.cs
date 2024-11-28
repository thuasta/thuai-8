using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class AppearSkillCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly string _skillId;

        public AppearSkillCommand(int tankId, string skillId)
        {
            _tankId = tankId;
            _skillId = skillId;
        }

        protected override void OnExecute()
        {
            TankModel player = Tank.GetTanks()[_tankId];
            player.TankObject.GetComponent<Animator>().Play(_skillId + "Active");
        }
    }
}

