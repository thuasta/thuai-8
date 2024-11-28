using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class AmmoText : AbstractModel
    {
        public Dictionary<int, Image> mAmmoType = new();
        public Dictionary<int, Text> mAmmoNumber = new();

        protected override void OnInit()
        {
            Image AmmoType_1 = GameObject.Find("Canvas/Ammo_1/Image/Ammo_Type").GetComponent<Image>();
            Image AmmoType_2 = GameObject.Find("Canvas/Ammo_2/Image/Ammo_Type").GetComponent<Image>();
            Text AmmoNumber_1 = GameObject.Find("Canvas/Ammo_1/Ammo_Number").GetComponent<Text>();
            Text AmmoNumber_2 = GameObject.Find("Canvas/Ammo_2/Ammo_Number").GetComponent<Text>();
            mAmmoType[1] = AmmoType_1;
            mAmmoType[2] = AmmoType_2;
            mAmmoNumber[1] = AmmoNumber_1;
            mAmmoNumber[2] = AmmoNumber_2;
        }
    }
}

