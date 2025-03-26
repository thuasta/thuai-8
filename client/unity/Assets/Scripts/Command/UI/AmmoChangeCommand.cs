using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class AmmoChangeCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly int _bulletNum;

        public AmmoChangeCommand(int tankId, int bullet)
        {
            _tankId = tankId;
            _bulletNum = bullet;
        }

        protected override void OnExecute()
        {
            var bulletNumText = this.GetModel<AmmoText>().mAmmoNumber[_tankId][1];
            bulletNumText.text = _bulletNum.ToString();
        }

    }
}

