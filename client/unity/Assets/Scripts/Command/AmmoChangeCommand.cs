using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class AmmoChangeCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly int _ammo;

        public AmmoChangeCommand(int tankId, int ammo)
        {
            _tankId = tankId;
            _ammo = ammo;
        }

        protected override void OnExecute()
        {
            var ammoText = this.GetModel<AmmoText>().mAmmoText[_tankId];
            ammoText.text = $"Ammo_{_tankId}: {_ammo}";
        }
        
    }
}

