using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class CountdownText : AbstractModel
    {
        public Text Ticks;

        protected override void OnInit()
        {
            Text TicksText = GameObject.Find("Canvas/Countdown").GetComponent<Text>();
            Ticks = TicksText;
        }
    }
}


