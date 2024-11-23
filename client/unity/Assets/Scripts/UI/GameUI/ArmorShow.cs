using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class ArmorShow : AbstractModel
    {
        public Dictionary<int, Slider> armor_value = new();
        public Dictionary<int, Color> armor_type = new();


        protected override void OnInit()
        {
            Slider Armor_1 = GameObject.Find("Canvas/Armor_1").GetComponent<Slider>();
            Slider Armor_2 = GameObject.Find("Canvas/Armor_2").GetComponent<Slider>();
            Image Type_1 = GameObject.Find("Canvas/Armor_1/Fill Area/Fill").GetComponent<Image>();
            Image Type_2 = GameObject.Find("Canvas/Armor_2/Fill Area/Fill").GetComponent<Image>();
            armor_value[1] = Armor_1;
            armor_type[1] = Type_1.color;
            armor_value[2] = Armor_2;
            armor_type[2] = Type_2.color;
        }
    }
}