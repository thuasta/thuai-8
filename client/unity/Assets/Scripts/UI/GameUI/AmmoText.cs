using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class AmmoText : AbstractModel
    {
        public Dictionary<int, Text> mAmmoText = new();

        protected override void OnInit()
        {
            Text Ammo_1 = GameObject.Find("Canvas/Ammo_1").GetComponent<Text>();
            Text Ammo_2 = GameObject.Find("Canvas/Ammo_1").GetComponent<Text>();
            mAmmoText[1] = Ammo_1;
            mAmmoText[2] = Ammo_2;
        }
    }
}

