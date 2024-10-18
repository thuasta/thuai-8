using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class BulletModel
    {
        public int Id { get; set; }

        public Vector3 Position { get; set; }

        public string BulletType { get; set; }

        private GameObject BulletObject { get; set; }

        public BulletModel(int id, Vector3 position, string bulletType)
        {
            Id = id;
            Position = position;
            BulletType = bulletType;

            GameObject prefab = Resources.Load<GameObject>($"Model/Bullet/{BulletType}");
            if (prefab != null)
            {
                BulletObject = GameObject.Instantiate(prefab, position, Quaternion.identity);
            }
            else
            {
                Debug.LogError($"Tank model {BulletType} not found in Resources/Model/Bullet");
            }
        }


    }
}

