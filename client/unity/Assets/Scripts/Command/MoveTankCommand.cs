using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class MoveTankCommand : AbstractCommand
    {
        private readonly int _tankId;

        public MoveTankCommand(int tankId)
        {
            _tankId = tankId;
        }

        protected override void OnExecute()
        {
            TankModel player = Tank.GetTanks()[_tankId];
            player.TankObject.GetComponent<Animator>().SetBool("IsMoving", true);
        }
    }
}

