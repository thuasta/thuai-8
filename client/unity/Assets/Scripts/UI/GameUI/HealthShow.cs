using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class HealthShow : AbstractModel
    {
        public Dictionary<int, Slider> health = new();

        protected override void OnInit()
        {
            Slider Health_1 = GameObject.Find("Canvas/Health_1").GetComponent<Slider>();
            Slider Health_2 = GameObject.Find("Canvas/Health_2").GetComponent<Slider>();
            health[1] = Health_1;
            health[2] = Health_2;
        }
    }
}