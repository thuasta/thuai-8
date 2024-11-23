using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class MoveTankCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly int _speed;
        private readonly float _angle;

        public MoveTankCommand(int tankId, int speed, float angle)
        {
            _tankId = tankId;
            _speed = speed;
            _angle = angle;
        }

        protected override void OnExecute()
        {
            TankModel player = Tank.GetTanks()[_tankId];
            player.TankObject.GetComponent<Animator>().Play("Move");
            //TankObject.transform.rotation = Vector3.Lerp(transform.position, targetPosition.position, angle * Time.deltaTime);
            //TankObject.transform.position = Vector3.Lerp(transform.position, targetPosition.position, speed * Time.deltaTime);
        }
    }
}

