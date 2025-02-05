using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class RoundsChangeCommand : AbstractCommand
    {
        private readonly int _total_rounds;
        private readonly int _current_rounds;

        public RoundsChangeCommand(int total_rounds, int current_rounds)
        {
            _current_rounds = current_rounds;
            _total_rounds = total_rounds;
        }

        protected override void OnExecute()
        {
            var RoundsText = this.GetModel<RoundsShow>().Rounds;
            RoundsText.text = $"{_current_rounds}/{_total_rounds}";
        }
        
    }
}