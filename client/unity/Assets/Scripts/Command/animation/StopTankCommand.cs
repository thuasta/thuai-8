using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class StopTankCommand : AbstractCommand
    {
        private TankModel player;

        public StopTankCommand(TankModel tank)
        {
            player = tank;
        }

        protected override void OnExecute()
        {
            player.TankObject.GetComponent<Animator>().SetBool("IsMoving", false);
        }
    }
}

