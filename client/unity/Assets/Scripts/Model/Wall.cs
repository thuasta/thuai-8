using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BattleCity
{
    public class Wall
    {
        public Position wallPos { get; set; }
        public GameObject vertWall { get; set; }
        public GameObject horiWall { get; set; }
        public GameObject vertFence { get; set; }
        public GameObject horiFence { get; set; }

        public GameObject createdWallObject; // 用于保存创建的墙体对象

        private static readonly string[] horiWallPrefabNames =
        {
            "horiWall00"
            /*"horiWall01",
            "horiWall02",
            "horiWall03",
            "horiWall04"*/
        };

        private static readonly string[] vertWallPrefabNames =
        {
            "vertWall00"
            /*"vertWall01",
            "vertWall02"*/
        };
        public Wall(Position wallpos)
        {
            wallPos = wallpos;
            AssignRandomHoriWall();
            AssignRandomVertWall();
            vertFence = Resources.Load<GameObject>("Prefabs/Wall/vertFence");
            horiFence = Resources.Load<GameObject>("Prefabs/Wall/vertFence");
        }

        public Wall(double X, double Y, double Angle)
        {
            Position position = new(X, Y, Angle);
            wallPos = position;
            AssignRandomHoriWall();
            AssignRandomVertWall();
            vertFence = Resources.Load<GameObject>("Prefabs/Wall/vertFence");
            horiFence = Resources.Load<GameObject>("Prefabs/Wall/vertFence");
        }

        public GameObject CreateWallObject()
        {
            GameObject wallController = GameObject.Find("WallController");
            if (wallPos.Angle == 90)
            {
                Vector3 position = new Vector3((float)(wallPos.X + Constants.WALL_XBIAS), (float)(wallPos.Y + Constants.Y_BIAS), (float)(wallPos.Z + Constants.WALL_ZFIX));
                createdWallObject = Object.Instantiate(vertWall, wallController.transform);
                createdWallObject.transform.localPosition = position;
                createdWallObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
                createdWallObject.transform.localScale *= Constants.ZOOM;
            }
            else if (wallPos.Angle == 0)
            {
                Vector3 position = new Vector3((float)(wallPos.X + Constants.WALL_XFIX), (float)(wallPos.Y + Constants.Y_BIAS), (float)(wallPos.Z + Constants.WALL_ZBIAS));
                createdWallObject = Object.Instantiate(horiWall, wallController.transform);
                createdWallObject.transform.localPosition = position;
                createdWallObject.transform.localRotation = Quaternion.identity;
                createdWallObject.transform.localScale *= Constants.ZOOM;
            }
            else
            {
                Debug.LogError("The angle of the wall is invalid!");
            }


            return createdWallObject;
        }

        public GameObject CreateFenceObject()
        {
            GameObject wallController = GameObject.Find("WallController");
            if (wallPos.Angle == 90)
            {
                Vector3 position = new Vector3((float)(wallPos.X + Constants.WALL_XBIAS), (float)(wallPos.Y + Constants.Y_BIAS), (float)(wallPos.Z + Constants.WALL_ZFIX));
                createdWallObject = Object.Instantiate(vertFence, wallController.transform);
                createdWallObject.transform.localPosition = position;
                createdWallObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
                createdWallObject.transform.localScale *= Constants.ZOOM;
            }
            else if (wallPos.Angle == 0)
            {
                Vector3 position = new Vector3((float)(wallPos.X + Constants.WALL_XFIX), (float)(wallPos.Y + Constants.Y_BIAS), (float)(wallPos.Z + Constants.WALL_ZBIAS));
                createdWallObject = Object.Instantiate(horiFence, wallController.transform);
                createdWallObject.transform.localPosition = position;
                createdWallObject.transform.localRotation = Quaternion.identity;
                createdWallObject.transform.localScale *= Constants.ZOOM;
            }
            else
            {
                Debug.LogError("The angle of the fence is invalid!");
            }

            return createdWallObject;
        }

        

        public void RemoveWall()
        {
            if (createdWallObject != null)
            {
                Object.Destroy(createdWallObject); // 销毁墙体对象
                createdWallObject = null; // 清空引用
            }
            else
            {
                Debug.LogWarning("No wall object to remove at this position.");
            }
        }

        private void AssignRandomHoriWall()
        {
            // 随机选择一个预制体索引
            int randomIndex = Random.Range(0, horiWallPrefabNames.Length);
            // 根据索引加载对应的预制体
            string prefabPath = "Prefabs/Wall/" + horiWallPrefabNames[randomIndex];
            horiWall = Resources.Load<GameObject>(prefabPath);

            if (horiWall == null)
            {
                Debug.LogError("Failed to load prefab: " + prefabPath);
            }
        }
        private void AssignRandomVertWall()
        {
            // 随机选择一个预制体索引
            int randomIndex = Random.Range(0, vertWallPrefabNames.Length);
            // 根据索引加载对应的预制体
            string prefabPath = "Prefabs/Wall/" + vertWallPrefabNames[randomIndex];
            vertWall = Resources.Load<GameObject>(prefabPath);

            if (vertWall == null)
            {
                Debug.LogError("Failed to load prefab: " + prefabPath);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Wall other)
            {
                return wallPos.Equals(other.wallPos);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return wallPos.GetHashCode();
        }
    }
}
