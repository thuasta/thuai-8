using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class MoveTankCommand : AbstractCommand
    {
        TankModel player;

        public MoveTankCommand(TankModel tank)
        {
            player = tank;
        }

        protected override void OnExecute()
        {
            player.TankObject.GetComponent<Animator>().SetBool("IsMoving", true);
        }
    }
}

