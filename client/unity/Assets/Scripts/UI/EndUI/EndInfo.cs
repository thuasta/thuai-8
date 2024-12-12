using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class EndInfo : AbstractModel
    {
        public Dictionary<int, Dictionary<int, Dictionary<int, Image>>> mBuff = new();
        public Dictionary<int, Text> scores = new(); // 初始化 scores 字典

        protected override void OnInit()
        {
            // 确保内部字典被初始化
            for (int i = 1; i <= 9; i++)
            {
                mBuff[i] = new Dictionary<int, Dictionary<int, Image>>();
                for (int k = 1; k <= 2; k++) // 由于 Buff 的数量是 2，故此处用 k 迭代
                {
                    mBuff[i][k] = new Dictionary<int, Image>();
                }

                for (int j = 1; j <= 8; j++)
                {
                    Image Buff_1 = GameObject.Find($"Canvas/Round_{i}/Buff_1/buff_{j}").GetComponent<Image>();
                    Image Buff_2 = GameObject.Find($"Canvas/Round_{i}/Buff_2/buff_{j}").GetComponent<Image>();

                    mBuff[i][1][j] = Buff_1; // 赋值 Buff_1
                    mBuff[i][2][j] = Buff_2; // 赋值 Buff_2
                }

                Text Scores_Text = GameObject.Find($"Canvas/Round_{i}").GetComponent<Text>();
                scores[i] = Scores_Text; // 赋值给 scores 字典
            }
        }
    }
}