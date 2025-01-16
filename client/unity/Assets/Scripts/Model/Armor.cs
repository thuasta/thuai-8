using System;
using UnityEngine;


namespace BattleCity
{
    public class Armor
    {
        public enum KNIFE
        {
            NOT_OWNED,
            AVAILABLE,
            ACTIVE,
            BROKEN
        }
        public bool CanReflect { get; set; }
        public int ArmorValue { get; set; }
        public float Health { get; set; }
        public bool GravityField { get; set; }
        public KNIFE Knife { get; set; }
        public float DodgeRate { get; set; }


        public Armor(bool canReflect = false, int armorValue = 0, float health = 0, bool gravityField = false, KNIFE knife = KNIFE.NOT_OWNED, float dodgeRate = 0)
        {
            CanReflect = canReflect;
            ArmorValue = armorValue;
            Health = health;
            GravityField = gravityField;
            Knife = knife;
            DodgeRate = dodgeRate;
        }

        public void UpdateArmor(bool canReflect, int armorValue, float health, bool gravityField, KNIFE knife, float dodgeRate)
        {
            CanReflect = canReflect;
            ArmorValue = armorValue;
            Health = health;
            GravityField = gravityField;
            Knife = knife;
            DodgeRate = dodgeRate;
        }

        public bool SetKnife(string knifeString)
        {
            if (Enum.TryParse(knifeString, out KNIFE knife))
            {
                Knife = knife;
                return true;
            }
            else
            {
                Knife = KNIFE.NOT_OWNED; // 如果解析失败，设置为默认值
                return false;
            }
        }
    }
}