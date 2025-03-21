using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleCity
{
    public class TankModel
    {
        public int Id { get; set; }

        public Weapon TankWeapon { get; set; }

        public Armor TankArmor { get; set; }

        public Skills TankSkills { get; set; }

        public Position TankPosition { get; set; }

        public GameObject TankObject { get; set; }

        public TankModel(int id, Weapon tankWeapon, Armor tankArmor, Skills tankSkills, Position tankPosition)
        {
            GameObject wallController = GameObject.Find("WallController");
            Id = id;
            TankWeapon = tankWeapon;
            TankArmor = tankArmor;
            TankSkills = tankSkills;
            TankPosition = tankPosition;
            GameObject prefab = Resources.Load<GameObject>($"Model/Tank/{1}");
            if (prefab != null)
            {
                Vector3 position = new Vector3((float)tankPosition.X, (float)tankPosition.Y, (float)tankPosition.Z);
                                
                Quaternion rotation = Quaternion.Euler(0, (float)tankPosition.Angle, 0); // 假设 Y 轴旋转
                
                TankObject = Object.Instantiate(prefab, wallController.transform);
                TankObject.transform.localPosition = position;
                TankObject.transform.localRotation = rotation;
            }
            else
            {
                Debug.LogError($"Tank model {Id} not found in Resources/Model/Tank");
            }
        }
        public TankModel(int id)
        {
            GameObject wallController = GameObject.Find("WallController");
            Id = id;
            TankWeapon = new Weapon();
            TankArmor = new Armor();
            TankSkills = new Skills();
            TankPosition = new Position();
            GameObject prefab = Resources.Load<GameObject>($"Model/Tank/{1}");
            if (prefab != null)
            {
                Vector3 position = new Vector3((float)TankPosition.X, (float)TankPosition.Y, (float)TankPosition.Z);

                Quaternion rotation = Quaternion.Euler(0, (float)TankPosition.Angle, 0); // 假设 Y 轴旋转

                TankObject = Object.Instantiate(prefab, wallController.transform);
                TankObject.transform.localPosition = position;
                TankObject.transform.localRotation = rotation;
            }
            else
            {
                Debug.LogError($"Tank model {1} not found in Resources/Model/Tank");
            }
        }

        public void UpdateTankPosition( Position tankPosition)
        {
            TankPosition = tankPosition;

            if (TankObject != null)
            {
                Vector3 newPosition = new Vector3((float)tankPosition.X, (float)tankPosition.Y, (float)tankPosition.Z);
                TankObject.transform.localPosition = newPosition;

                Quaternion newRotation = Quaternion.Euler(0, (float)tankPosition.Angle, 0); // 假设 Y 轴旋转
                TankObject.transform.localRotation = newRotation;
            }
            else
            {
                Debug.LogWarning($"tankObject is null for id {Id}. Cannot update position.");
            }

        }

        public void UpdateTankPosition(float x, float y, float angle)
        {
            Position position = new Position(x, y, angle);
            UpdateTankPosition (position);
        }
    }
}

