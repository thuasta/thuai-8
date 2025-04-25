using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace BattleCity
{
    public class TankModel: IController
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
            GameObject prefab = Resources.Load<GameObject>($"Model/Tank/{Id}");
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
            TankObject.AddComponent<Movement>();
        }
        public TankModel(int id)
        {
            GameObject wallController = GameObject.Find("WallController");
            Id = id;
            TankWeapon = new Weapon();
            TankArmor = new Armor();
            TankSkills = new Skills();
            TankPosition = new Position();
            GameObject prefab = Resources.Load<GameObject>($"Model/Tank/{Id}");
            if (prefab != null)
            {
                Vector3 position = new Vector3(
                    (float)(TankPosition.X + Constants.GENERAL_XBIAS), (float)TankPosition.Y, (float)(TankPosition.Z + Constants.GENERAL_ZBIAS)
                );

                Quaternion rotation = Quaternion.Euler(0, -(float)TankPosition.Angle, 0); // Server's Clockwise is negative

                TankObject = Object.Instantiate(prefab, wallController.transform);
                TankObject.transform.localPosition = position;
                TankObject.transform.localRotation = rotation;
            }
            else
            {
                Debug.LogError($"Tank model {Id} not found in Resources/Model/Tank");
            }
            TankObject.AddComponent<Movement>();
        }

        public void UpdateTankPosition( Position tankPosition)
        {
            TankPosition = tankPosition;
            RecordInfo _recordInfo = this.GetModel<RecordInfo>();

            if (TankObject != null)
            {
                Vector3 newPosition = new Vector3(
                    (float)(tankPosition.X + Constants.GENERAL_XBIAS), (float)tankPosition.Y, (float)(tankPosition.Z + Constants.GENERAL_ZBIAS)
                );

                TankObject.GetComponent<Movement>().MoveTo(newPosition, _recordInfo.FrameTime);

                Quaternion newRotation = Quaternion.Euler(0, -(float)tankPosition.Angle, 0); // Server's Clockwise is negative
                TankObject.transform.localRotation = Quaternion.RotateTowards(TankObject.transform.localRotation, newRotation, 10 * Time.deltaTime);
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

        public void DestroyTank()
        {
            if (TankObject != null)
            {
                Object.Destroy(TankArmor.GravityInstance);
                TankArmor.GravityInstance = null;
                Object.Destroy(TankArmor.Knife_AC_Instance);
                TankArmor.Knife_AC_Instance = null;
                Object.Destroy(TankArmor.Knife_AV_Instance);
                TankArmor.Knife_AV_Instance = null;
                Object.Destroy(TankArmor.Reflect_AV_Instance );
                TankArmor.Reflect_AV_Instance= null;
                // 销毁游戏对象
                Object.Destroy(TankObject);
                TankObject = null;  // 清空引用避免野指针
                                
            }
            else
            {
                Debug.LogWarning($"TankObject already destroyed for tank ID: {Id}");
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }
    }
}

