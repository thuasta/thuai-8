using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class ArmorTypeChangeCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly Color _armor_type;

        public ArmorTypeChangeCommand(int tankId, bool canReflect)
        {
            _tankId = tankId;
            if(canReflect) _armor_type = new Color(138, 43, 226);
            else _armor_type = new Color(0, 240, 255);
        }

        protected override void OnExecute()
        {
            var Armor_Type = this.GetModel<ArmorShow>().armor_type[_tankId];
            Armor_Type = _armor_type;
        }
        
    }
}