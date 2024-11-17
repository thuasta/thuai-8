using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class CountdownChangeCommand : AbstractCommand
    {
        private readonly int _ticks;

        public CountdownChangeCommand(int ticks)
        {
            _ticks = ticks;
        }

        protected override void OnExecute()
        {
            var TicksText = this.GetModel<CountdownText>().Ticks;
            TicksText.text = $"{_ticks}";
        }
        
    }
}