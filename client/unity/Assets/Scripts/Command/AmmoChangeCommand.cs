using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace BattleCity
{
    public class AmmoChangeCommand : AbstractCommand
    {
        private readonly int _tankId;

        private readonly int _ammo_type;
        private readonly int _ammo_number;

        public AmmoChangeCommand(int tankId, int ammo_type, int ammo_number)
        {
            _tankId = tankId;
            _ammo_type = ammo_type;
            var ammoNumber = this.GetModel<AmmoText>().mAmmoNumber[tankId][ammo_type];
            _ammo_number = int.Parse(ammoNumber.text) + ammo_number;
        }

        protected override void OnExecute()
        {
            var ammoNumber = this.GetModel<AmmoText>().mAmmoNumber[_tankId][_ammo_type];
            ammoNumber.text = $"{_ammo_number}";
        }
        
    }
}

