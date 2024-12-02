using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class DestroySkillCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly string _skillId;

        public DestroySkillCommand(int tankId, string skillId)
        {
            _tankId = tankId;
            _skillId = skillId;
        }

        protected override void OnExecute()
        {
            TankModel player = Tank.GetTanks()[_tankId];
            player.TankObject.GetComponent<Animator>().Play(_skillId + "Destroy");
        }
    }
}

