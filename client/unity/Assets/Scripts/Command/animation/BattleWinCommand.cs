using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class BattleWinCommand : AbstractCommand
    {
        private TankModel player;

        public BattleWinCommand(TankModel tank)
        {
            player = tank;
        }

        protected override void OnExecute()
        {
            player.TankObject.GetComponent<Animator>().SetTrigger("BattleEnd");
        }
    }
}

