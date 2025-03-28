using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleCity
{
    public class Trap
    {
        public Position trapPos { get; set; }
        private GameObject createdTrapObject { get; set; }
        public bool isActive {  get; set; }
               
        public Trap(Position pos, bool isActive)
        {
            GameObject wallController = GameObject.Find("WallController");
            trapPos = pos;
            Vector3 position = new Vector3((float)(pos.X + Constants.WALL_XBIAS), (float)(pos.Y + Constants.Y_BIAS + 0.02f), (float)(pos.Z + Constants.WALL_ZFIX));
            string prefabPath = "Prefabs/Trap";
            GameObject TrapPrefab = Resources.Load<GameObject>(prefabPath);
            createdTrapObject = Object.Instantiate(TrapPrefab, wallController.transform);
            this.isActive = isActive;
            createdTrapObject.transform.localPosition = position;
            createdTrapObject.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            createdTrapObject.transform.localScale *= Constants.ZOOM;
        }     
               

        public void RemoveTrap()
        {
            if (createdTrapObject != null)
            {
                Object.Destroy(createdTrapObject); // 销毁墙体对象
                createdTrapObject = null; // 清空引用
            }
            else
            {
                Debug.LogWarning("No wall object to remove at this position.");
            }
        }

        
    }
}
