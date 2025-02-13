using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class HealthChangeCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly int _health;

        public HealthChangeCommand(int tankId, int health)
        {
            _tankId = tankId;
            _health = health;
        }

        protected override void OnExecute()
        {
            var Health = this.GetModel<HealthShow>().health[_tankId];
            Health.value = _health;        }
        
    }
}

