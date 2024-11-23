using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class BuffChangeCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly string _buff;

        public BuffChangeCommand(int tankId, string buff)
        {
            _tankId = tankId;
            _buff = buff;
        }

        protected override void OnExecute()
        {
            var buffText = this.GetModel<BuffShow>().buffText[_tankId];
            buffText.text = _buff;
        }
        
    }
}

