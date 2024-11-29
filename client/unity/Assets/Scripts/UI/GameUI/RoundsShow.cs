using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class RoundsShow : AbstractModel
    {
        public Text Rounds;

        protected override void OnInit()
        {
            Text RoundsText = GameObject.Find("Canvas/Rounds").GetComponent<Text>();
            Rounds = RoundsText;
        }
    }
}