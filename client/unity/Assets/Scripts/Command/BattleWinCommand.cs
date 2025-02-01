using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class BattleWinCommand : AbstractCommand
    {
        private readonly int _tankId;

        public BattleWinCommand(int tankId)
        {
            _tankId = tankId;
        }

        protected override void OnExecute()
        {
            TankModel player = Tank.GetTanks()[_tankId];
            player.TankObject.GetComponent<Animator>().SetTrigger("BattleEnd");
        }
    }
}

