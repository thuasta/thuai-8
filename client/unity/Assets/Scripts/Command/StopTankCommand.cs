using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class StopTankCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly int _speed;
        private readonly float _angle;

        public StopTankCommand(int tankId, int speed, float angle)
        {
            _tankId = tankId;
            _speed = speed;
            _angle = angle;
        }

        protected override void OnExecute()
        {
            TankModel player = Tank.GetTanks()[_tankId];
            player.TankObject.GetComponent<Animator>().SetBool("IsMoving", false);
        }
    }
}

