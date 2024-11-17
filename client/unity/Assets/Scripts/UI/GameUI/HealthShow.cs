using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class HealthShow : AbstractModel
    {
        public Dictionary<int, int> health = new();

        protected override void OnInit()
        {
            Slider Health_1 = GameObject.Find("Canvas/Health_1").GetComponent<Slider>();
            Slider Health_2 = GameObject.Find("Canvas/Health_2").GetComponent<Slider>();
            health[1] = (int)Health_1.value;
            health[2] = (int)Health_2.value;
        }
    }
}