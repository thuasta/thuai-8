using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class AmmoChangeCommand : AbstractCommand
    {
        private readonly int _tankId;

        private readonly Sprite _ammo_type;
        private readonly int _ammo_number;

        public AmmoChangeCommand(int tankId, int ammo_type, int ammo_number)
        {
            _tankId = tankId;
            _ammo_number = ammo_number;
            switch(ammo_type)
            {
                case 0:
                    _ammo_type = Resources.Load<Sprite>("bullet");
                    break;
                case 1:
                    _ammo_type = Resources.Load<Sprite>("laser");
                    break;
                case 2:
                    _ammo_type = Resources.Load<Sprite>("arrow");
                    break;
                default:
                break;
            }
        }

        protected override void OnExecute()
        {

            var ammoNumber = this.GetModel<AmmoText>().mAmmoNumber[_tankId];
            var ammoType = this.GetModel<AmmoText>().mAmmoType[_tankId];
            ammoNumber.text = $"{_ammo_number}";
            ammoType.sprite = _ammo_type;
        }
        
    }
}

