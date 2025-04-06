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
                // ����ӵ�ģ��Ϊnull��ID�Ѵ��ڣ��򷵻�false
                return false;
            }

            BulletsList.Add(bullet);
            BulletsId.Add(bullet.Id);
            return true; // �ɹ�����
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
                return true; // �ɹ�ɾ��
            }

            return false; // û���ҵ���ID���ӵ�ģ��
        }

        public void DelBulletEffect(GameObject bullet)
        {
            GameObject wallController = GameObject.Find("WallController");
            GameObject effectPrefab = null;

            // ������ЧԤ�Ƽ�
            effectPrefab = Resources.Load<GameObject>($"Effects/HURT");

            if (effectPrefab != null)
            {
                // ʵ������Ч����������� player's TankObject ��
                GameObject effectInstance = GameObject.Instantiate(effectPrefab, bullet.transform.position, Quaternion.identity, wallController.transform);

                // ��ѡ��������Чʵ�����������ڣ�������Ч��3�������
                //GameObject.Destroy(effectInstance, 3f);
            }
            else
            {
                Debug.LogWarning($"��Ч HURT δ�ҵ�!");
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

