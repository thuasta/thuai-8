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
            trapPos = pos;
            Vector3 position = new Vector3((float)(pos.X), (float)(pos.Y + Constants.Y_BIAS), (float)(pos.Z + Constants.WALL_ZBIAS));
            string prefabPath = "Prefabs/Trap/";
            GameObject TrapPrefab = Resources.Load<GameObject>(prefabPath);
            createdTrapObject = Object.Instantiate(TrapPrefab, position, Quaternion.identity);
            this.isActive = isActive;
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
