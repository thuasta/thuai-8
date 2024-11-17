using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class BuffShow : AbstractModel
    {
        public Dictionary<int, Text> buffText = new();

        protected override void OnInit()
        {
            Text Buff_1 = GameObject.Find("Canvas/Buff_1").GetComponent<Text>();
            Text Buff_2 = GameObject.Find("Canvas/Buff_2").GetComponent<Text>();
            buffText[1] = Buff_1;
            buffText[2] = Buff_2;
        }
    }
}