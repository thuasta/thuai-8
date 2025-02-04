using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BattleCity
{
    public class Bullets : AbstractModel
    {
        private List<BulletModel> BulletsList { get;set;}
        private List<int> BulletsId { get;set;}

        protected override void OnInit()
        {
            BulletsList = new();
            BulletsId = new List<int>();
        }

        public BulletModel GetBullet(int id)
        {
            BulletModel bullet = BulletsList.Find(Bullet => Bullet.Id == id);
            if (bullet!=null)
            {
                return bullet;
            }
            return null;           
        }

        public List<int> GetBulletIds()
        {
            return BulletsId;
        }

        public bool AddBulletModel(BulletModel bullet)
        {
            if (bullet == null || BulletsList.Exists(b => b.Id == bullet.Id))
            {
                // 如果子弹模型为null或ID已存在，则返回false
                return false;
            }

            BulletsList.Add(bullet);
            BulletsId.Add(bullet.Id);
            return true; // 成功添加
        }

        public bool DelBulletModel(int id)
        {
            BulletModel bullet = GetBullet(id);
            if (bullet != null)
            {
                BulletsList.Remove(bullet);
                BulletsId.Remove(bullet.Id);
                return true; // 成功删除
            }

            return false; // 没有找到该ID的子弹模型
        }

    }
}

