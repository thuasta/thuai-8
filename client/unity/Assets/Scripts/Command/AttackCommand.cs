using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class AttackCommand : AbstractCommand
    {
        private readonly int _tankId;

        public AttackCommand(int tankId)
        {
            _tankId = tankId;
        }

        protected override void OnExecute()
        {
            TankModel player = Tank.GetTanks()[_tankId];
            Bullet.AddBulletModel(Bullet.Count() + 1, player.Position, player.Angle, "Bullet");
            player.TankObject.GetComponent<Animator>().Play("TankAttack");
        }
    }
}

