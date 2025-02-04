using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class AddAttackCommand : AbstractCommand
    {
        private readonly int _tankId;

        public AddAttackCommand(int tankId)
        {
            _tankId = tankId;
        }

        protected override void OnExecute()
        {
            TankModel player = Tank.GetTanks()[_tankId];
            int t_IsAttacking = player.TankObject.GetComponent<Animator>().GetInteger("IsAttacking");
            player.TankObject.GetComponent<Animator>().SetInteger("IsAttacking", t_IsAttacking + 1);
        }
    }
}

