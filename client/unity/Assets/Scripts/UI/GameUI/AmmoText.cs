using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace BattleCity
{
    public class AmmoText : AbstractModel
    {
        public Dictionary<int, Dictionary<int, Image>> mAmmoType = new();
        public Dictionary<int, Dictionary<int, TMP_Text>> mAmmoNumber = new();

        protected override void OnInit()
        {
            // 初始化嵌套字典
            mAmmoType[1] = new Dictionary<int, Image>();
            mAmmoType[2] = new Dictionary<int, Image>();
            mAmmoNumber[1] = new Dictionary<int, TMP_Text>();
            mAmmoNumber[2] = new Dictionary<int, TMP_Text>();

            // 查找和存储弹药类型的图像
            Image AmmoType_1_1 = GameObject.Find("Canvas/Ammo_1/Image_bullet").GetComponent<Image>();
            Image AmmoType_1_2 = GameObject.Find("Canvas/Ammo_1/Image_arrow").GetComponent<Image>();
            Image AmmoType_1_3 = GameObject.Find("Canvas/Ammo_1/Image_laser").GetComponent<Image>();
            Image AmmoType_2_1 = GameObject.Find("Canvas/Ammo_2/Image_bullet").GetComponent<Image>();
            Image AmmoType_2_2 = GameObject.Find("Canvas/Ammo_2/Image_arrow").GetComponent<Image>();
            Image AmmoType_2_3 = GameObject.Find("Canvas/Ammo_2/Image_laser").GetComponent<Image>();

            // 查找和存储弹药数量的文本
            TMP_Text AmmoNumber_1_1 = GameObject.Find("Canvas/Ammo_1/Bullet_Number").GetComponent<TMP_Text>();
            TMP_Text AmmoNumber_1_2 = GameObject.Find("Canvas/Ammo_1/Arrow_Number").GetComponent<TMP_Text>();
            TMP_Text AmmoNumber_1_3 = GameObject.Find("Canvas/Ammo_1/Laser_Number").GetComponent<TMP_Text>();
            TMP_Text AmmoNumber_2_1 = GameObject.Find("Canvas/Ammo_2/Bullet_Number").GetComponent<TMP_Text>();
            TMP_Text AmmoNumber_2_2 = GameObject.Find("Canvas/Ammo_2/Arrow_Number").GetComponent<TMP_Text>();
            TMP_Text AmmoNumber_2_3 = GameObject.Find("Canvas/Ammo_2/Laser_Number").GetComponent<TMP_Text>();

            // 存储弹药类型到字典
            mAmmoType[1][1] = AmmoType_1_1;
            mAmmoType[1][2] = AmmoType_1_2;
            mAmmoType[1][3] = AmmoType_1_3;
            mAmmoType[2][1] = AmmoType_2_1;
            mAmmoType[2][2] = AmmoType_2_2;
            mAmmoType[2][3] = AmmoType_2_3;

            // 存储弹药数量到字典
            mAmmoNumber[1][1] = AmmoNumber_1_1;
            mAmmoNumber[1][2] = AmmoNumber_1_2;
            mAmmoNumber[1][3] = AmmoNumber_1_3;
            mAmmoNumber[2][1] = AmmoNumber_2_1;
            mAmmoNumber[2][2] = AmmoNumber_2_2;
            mAmmoNumber[2][3] = AmmoNumber_2_3;

        }
    }
}
