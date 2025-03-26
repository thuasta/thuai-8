using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class RoundsShow : AbstractModel
    {
        public TMP_Text Rounds;

        protected override void OnInit()
        {
            TMP_Text RoundsText = GameObject.Find("Canvas/Rounds").GetComponent<TMP_Text>();
            Rounds = RoundsText;
        }
    }
}