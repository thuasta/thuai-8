using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class BuffShow : AbstractModel
    {
        public Dictionary<int, Dictionary<int, Image>> buffImage = new();

        public Dictionary<int, List<string>> buffList = new(); 

        protected override void OnInit()
        {
            for(int i = 1; i <= 8; i++)
            {
                Image Buff_1 = GameObject.Find($"Canvas/Buff_1/buff_{i}").GetComponent<Image>();
                Image Buff_2 = GameObject.Find($"Canvas/Buff_2/buff_{i}").GetComponent<Image>();
                buffImage[1][i] = Buff_1;
                buffImage[2][i] = Buff_2;
            }
        }
    }
}