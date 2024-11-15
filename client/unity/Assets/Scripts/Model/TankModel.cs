using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCity
{
    public class TankModel
    {
        public int Id { get; set; }

        public float Speed { get; set; }

        public float Health { get; set; }

        public int Ammo { get; set; }

        public Vector3 Position { get; set; }

        public GameObject TankObject { get; set; }

        public TankModel(int id, float speed, float health, int ammo, Vector3 position)
        {
            Id = id;
            Speed = speed;
            Health = health;
            Ammo = ammo;
            Position = position;
            GameObject prefab = Resources.Load<GameObject>($"Model/Tank/{Id}");
            if (prefab != null)
            {
                TankObject = GameObject.Instantiate(prefab, position, Quaternion.identity);
            }
            else
            {
                Debug.LogError($"Tank model {Id} not found in Resources/Model/Tank");
            }
        }
    }
}

