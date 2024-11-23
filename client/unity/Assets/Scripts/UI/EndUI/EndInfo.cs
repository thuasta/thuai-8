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
        public Dictionary<int, Dictionary<int, Image>> mAmmoImage = new();
        public Dictionary<int, Dictionary<int, Text>> mAmmoNumber = new();
        public Dictionary<int, Dictionary<int, Text>> mBuff = new();
        public Dictionary<int, Text> scores;
        protected override void OnInit()
        {
            for(int i = 1; i <= 9; i++)
            {
                Image Ammo_Image_1 = GameObject.Find($"Canvas/Round_{i}/Ammo_1/Image/Ammo_Type").GetComponent<Image>();
                Image Ammo_Image_2 = GameObject.Find($"Canvas/Round_{i}/Ammo_2/Image/Ammo_Type").GetComponent<Image>();
                Text Ammo_Number_1 = GameObject.Find($"Canvas/Round_{i}/Ammo_1/Ammo_Number").GetComponent<Text>();
                Text Ammo_Number_2 = GameObject.Find($"Canvas/Round_{i}/Ammo_2/Ammo_Number").GetComponent<Text>();
                Text Buff_1 = GameObject.Find($"Canvas/Round_{i}/Buff_1").GetComponent<Text>();
                Text Buff_2 = GameObject.Find($"Canvas/Round_{i}/Buff_2").GetComponent<Text>();
                Text Scores_Text = GameObject.Find($"Canvas/Round_{i}").GetComponent<Text>();
                mAmmoImage[i][1] = Ammo_Image_1;
                mAmmoImage[i][2] = Ammo_Image_2;
                mAmmoNumber[i][1] = Ammo_Number_1;
                mAmmoNumber[i][2] = Ammo_Number_2;
                mBuff[i][1] = Buff_1;
                mBuff[i][2] = Buff_2;
                scores[i] = Scores_Text;
            }
        }
    }
}