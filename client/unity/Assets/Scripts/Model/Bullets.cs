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
                DelBulletEffect(bullet.BulletObject);
                BulletsList.Remove(bullet);
                BulletsId.Remove(bullet.Id);
                bullet.SelfDestruct();
                return true; // 成功删除
            }

            return false; // 没有找到该ID的子弹模型
        }

        public void DelBulletEffect(GameObject bullet)
        {
            GameObject wallController = GameObject.Find("WallController");
            GameObject effectPrefab = null;

            // 加载特效预制件
            effectPrefab = Resources.Load<GameObject>($"Effects/HURT");

            if (effectPrefab != null)
            {
                // 实例化特效并将其放置在 player's TankObject 上
                GameObject effectInstance = GameObject.Instantiate(effectPrefab, bullet.transform.position, Quaternion.identity, wallController.transform);

                // 可选：设置特效实例的生命周期，假设特效在3秒后销毁
                //GameObject.Destroy(effectInstance, 3f);
            }
            else
            {
                Debug.LogWarning($"特效 HURT 未找到!");
            }
        }

        public void DelAllBullets()
        {
            List<BulletModel> BulletsToDel = new List<BulletModel>(BulletsList);
            foreach (BulletModel bullet in BulletsToDel)
            {
                DelBulletModel(bullet.Id) ;
            }
            BulletsList.Clear();
        }

    }
}

