using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class BulletModel
    {
        public int Id { get; set; }

        public Position BulletPosition{ get; set; }

        private GameObject BulletObject { get; set; }

        public BulletModel(int id, Position bulletPosition, int speed, int damage, bool isMissile = false, bool isAntiArmor = false, float traveledDistance = 0)
        {
            Id = id;
            GameObject prefab;

            //TODO: different speed and damage
            if (isMissile)
            {
                prefab = Resources.Load<GameObject>($"Model/Bullet/");
            }
            else
            {
                prefab = Resources.Load<GameObject>($"Model/Bullet/");
            }
            if(isAntiArmor)
            {
                //TODO
            }
            

            if (prefab != null)
            {
                Vector3 position = new Vector3((float)bulletPosition.X, (float)bulletPosition.Y, (float)bulletPosition.Z);

                Quaternion rotation = Quaternion.Euler(0, (float)bulletPosition.Angle, 0); // 假设 Y 轴旋转

                BulletObject = Object.Instantiate(prefab, position, rotation);
            }
            else
            {
                Debug.LogError($"Bullet model not found in Resources/Model/Bullet");
            }
        }

        public void UpdateBulletPosition(Position bulletPosition)
        {                        
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
        public void UpdateBulletPosition(float x, float y, float angle)
        {
            Position position = new Position(x, y, angle);
            UpdateBulletPosition(position);
        }


    }
}

