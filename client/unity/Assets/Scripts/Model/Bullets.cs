using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BattleCity
{
    public class Bullets : AbstractModel
    {
        private Dictionary<int, BulletModel> BulletDict { get;set;}

        protected override void OnInit()
        {
            BulletDict = new();
        }

        public BulletModel GetBullet(int id)
        {
            BulletDict.TryGetValue(id, out var tank);
            return tank;
        }

        public bool AddBulletModel(BulletModel bullet)
        {
            if (bullet == null || BulletDict.ContainsKey(bullet.Id))
            {
                return false;
            }

            BulletDict[bullet.Id] = bullet;
            return true;
        }

        public bool DelBulletModel(int id)
        {
            if (BulletDict.ContainsKey(id))
            {
                BulletDict.Remove(id);
                return true;
            }

            return false;
        }

    }
}

