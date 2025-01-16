using UnityEngine;


namespace BattleCity
{
    public class Weapon
    {
        public float AttackSpeed { get; set; }
        public float BulletSpeed { get; set; }
        public bool IsLaser { get; set; }
        public bool AntiArmor { get; set; }
        public float Damage { get; set; }
        public int MaxBullets { get; set; }
        public int CurrentBullets { get; set; }

        public Weapon(float attackSpeed = 0, float bulletSpeed = 0, bool isLaser = false, bool antiArmor = false, float damage = 0, int maxBullets = 0, int currentBullets = 0)
        {
            AttackSpeed = attackSpeed;
            BulletSpeed = bulletSpeed;
            IsLaser = isLaser;
            AntiArmor = antiArmor;
            Damage = damage;
            MaxBullets = maxBullets;
            CurrentBullets = currentBullets;
        }

        public void UpdateWeapon(float attackSpeed, float bulletSpeed, bool isLaser, bool antiArmor, float damage, int maxBullets, int currentBullets)
        {
            AttackSpeed = attackSpeed;
            BulletSpeed = bulletSpeed;
            IsLaser = isLaser;
            AntiArmor = antiArmor;
            Damage = damage;
            MaxBullets = maxBullets;
            CurrentBullets = currentBullets;
        }
    }
}