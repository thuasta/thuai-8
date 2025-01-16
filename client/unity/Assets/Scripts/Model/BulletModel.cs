using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class BulletModel
    {
        public int Id { get; set; }

        public Position BulletPosition{ get; set; }

        public string BulletType { get; set; }

        private GameObject BulletObject { get; set; }

        public BulletModel(int id, Position bulletPosition, string bulletType)
        {
            Id = id;
            BulletType = bulletType;

            GameObject prefab = Resources.Load<GameObject>($"Model/Bullet/{BulletType}");
            if (prefab != null)
            {
                Vector3 position = new Vector3((float)bulletPosition.X, (float)bulletPosition.Y, (float)bulletPosition.Z);

                Quaternion rotation = Quaternion.Euler(0, (float)bulletPosition.Angle, 0); // 假设 Y 轴旋转

                BulletObject = Object.Instantiate(prefab, position, rotation);
            }
            else
            {
                Debug.LogError($"Bullet model {BulletType} not found in Resources/Model/Bullet");
            }
        }

        public void UpdateBulletPosition(int id, Position bulletPosition)
        {
            if (id != Id)
            {
                Debug.LogWarning($"Trying to update bullet position for a bullet with id {id}, but this is bullet {Id}.");
                return;
            }
                        
            BulletPosition = bulletPosition;

            if (BulletObject != null)
            {
                Vector3 newPosition = new Vector3((float)bulletPosition.X, (float)bulletPosition.Y, (float)bulletPosition.Z);
                BulletObject.transform.position = newPosition;

                Quaternion newRotation = Quaternion.Euler(0, (float)bulletPosition.Angle, 0); // 假设 Y 轴旋转
                BulletObject.transform.rotation = newRotation;
            }
            else
            {
                Debug.LogWarning($"BulletObject is null for bullet with id {Id}. Cannot update position.");
            }

        }


    }
}

