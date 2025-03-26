using QFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BattleCity
{
    public class BuffShow : AbstractModel
    {
        public Dictionary<int, Dictionary<int, Image>> buffImage = new Dictionary<int, Dictionary<int, Image>>();
        public Dictionary<int, List<string>> buffList = new Dictionary<int, List<string>>();

        protected override void OnInit()
        {
            // Initialize inner dictionaries for buffImage
            for (int i = 1; i <= 2; i++) // Assuming you have 2 buff slots, adjust as necessary
            {
                buffImage[i] = new Dictionary<int, Image>();
            }

            for (int i = 1; i <= 8; i++)
            {
                Image Buff_1 = GameObject.Find($"Canvas/Buff_1/buff_{i}")?.GetComponent<Image>();
                Image Buff_2 = GameObject.Find($"Canvas/Buff_2/buff_{i}")?.GetComponent<Image>();

                if (Buff_1 != null)
                {
                    buffImage[1][i] = Buff_1;
                }
                else
                {
                    Debug.LogWarning($"Buff_1/buff_{i} not found.");
                }

                if (Buff_2 != null)
                {
                    buffImage[2][i] = Buff_2;
                }
                else
                {
                    Debug.LogWarning($"Buff_2/buff_{i} not found.");
                }
            }
        }
    }
}