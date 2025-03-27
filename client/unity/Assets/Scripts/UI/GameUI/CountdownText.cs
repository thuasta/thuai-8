using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class CountdownText : AbstractModel
    {
        public TMP_Text Ticks;

        protected override void OnInit()
        {
            TMP_Text TicksText = GameObject.Find("Canvas/Countdown").GetComponent<TMP_Text>();
            Ticks = TicksText;
        }
    }
}


