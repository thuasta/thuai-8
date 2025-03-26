using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class ArmorValueChangeCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly int _armor_value;

        public ArmorValueChangeCommand(int tankId, int armor_value)
        {
            _tankId = tankId;
            _armor_value = armor_value;
        }

        protected override void OnExecute()
        {
            var Armor_Value = this.GetModel<ArmorShow>().armor_value[_tankId];
            Armor_Value.value = _armor_value;
        }
        
    }
}