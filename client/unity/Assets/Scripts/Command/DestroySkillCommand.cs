using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class DestroySkillCommand : AbstractCommand
    {
        private readonly string _skillId;
        TankModel player;



        public DestroySkillCommand(string skillId, TankModel player)
        {
            _skillId = skillId;
            this.player = player;
        }

        protected override void OnExecute()
        {
            player.TankObject.GetComponent<Animator>().Play(_skillId + "Destroy");
        }
    }
}

