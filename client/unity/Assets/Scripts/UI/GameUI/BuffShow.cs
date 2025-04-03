using QFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BattleCity
{
    public class BuffShow : AbstractModel
    {
        public List<List<Image>> buffImage = new List<List<Image>>();
        public List<List<string>> buffList = new List<List<string>>();

        protected override void OnInit()
        {
            InitializeListStructure();
            // Initialize inner dictionaries for buffImage

            for (int i = 1; i <= 8; i++)
            {
                Image Buff_1 = GameObject.Find($"Canvas/Buff_1/buff_{i}")?.GetComponent<Image>();
                Image Buff_2 = GameObject.Find($"Canvas/Buff_2/buff_{i}")?.GetComponent<Image>();

                if (Buff_1 != null)
                {
                    buffImage[0].Add(Buff_1);
                    buffList[0].Add("");
                }
                else
                {
                    Debug.LogWarning($"Buff_1/buff_{i} not found.");
                }

                if (Buff_2 != null)
                {
                    buffImage[1].Add(Buff_2);
                    buffList[1].Add("");
                }
                else
                {
                    Debug.LogWarning($"Buff_2/buff_{i} not found.");
                }
            }
        }

        private void InitializeListStructure()
        {
            // 初始化buffImage（2x8）
            for (int i = 0; i < 2; i++)
            {
                buffImage.Add(new List<Image>());
            }
            // 初始化buffList（2x8）
            for (int i = 0; i < 2; i++)
            {
                buffList.Add(new List<string>());
            }
        }
        
    }
}