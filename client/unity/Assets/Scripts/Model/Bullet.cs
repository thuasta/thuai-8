using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BattleCity
{
    public class Bullet : AbstractModel
    {
        private static readonly Dictionary<int, BulletModel> _bulletDict = new();

        protected override void OnInit()
        {
        }

        public static Dictionary<int, BulletModel> GetBullets()
        {
            return _bulletDict;
        }

        public void AddBulletModel(int id, Vector3 position, string bulletType)
        {
            //TODO
        }
    }
}

