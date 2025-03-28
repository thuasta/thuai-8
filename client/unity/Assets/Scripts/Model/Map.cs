using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BattleCity
{
    public class Map : AbstractModel
    {
        public int MapSize { get; set; }
        public List<Wall> CityWall { get; set; }
        public List<Wall> CityFence { get; set; }

        public List<Trap> Traps { get; set; }
        public List<GameObject> CityFloors { get; set; }
        protected override void OnInit()
        {
            CityWall = new List<Wall>();
            CityFence = new List<Wall>();
            CityFloors = new List<GameObject>();
            Traps = new List<Trap>();
        }

        public void setSize(int? mapSize)
        {
            if (mapSize != null)
            {
                MapSize = (int)mapSize;
            }
            else
            {
                MapSize = Constants.MAP_SIZE;
            }
        }
        
        //用于初始创建地图时增加wall
        public void AddWall(Position wallPos)
        {
            Wall wall = new(wallPos);
            CityWall.Add(wall);
        }

        public void AddWall(double x, double y,double angle) 
        {
            Position position = new(x, y, angle);
            AddWall(position);
        }

        //用于后续更新wall
        public void UpdateWall(Position wallPos)
        {
            Wall wall = new(wallPos);
            CityWall.Add(wall);
            wall.CreateWallObject();
        }
        public void UpdateWall(double x, double y, double angle)
        {
            Position position = new(x, y, angle);
            UpdateWall(position);
        }
        //用于后续增加fench
        public void UpdateFence(Position wallPos)
        {
            Wall wall = new(wallPos);
            CityFence.Add(wall);
            wall.CreateFenceObject();
        }
        public void UpdateFence(double x, double y, double angle)
        {
            Position position = new(x, y, angle);
            UpdateFence(position);
        }

        //用于移除墙体
        public void RemoveWall(Position wallPos) 
        {
            //Wall wall = new(wallPos);
            Wall foundWall = CityWall.Find(w => w.wallPos.Equals(wallPos));
            if (foundWall != null)
            {
                RemoveWallEffect(foundWall);
                foundWall.RemoveWall(); // 假设 RemoveWall 方法在 Wall 类中
                CityWall.Remove(foundWall); // 从 CityWall 中移除找到的墙
            }
            else
            {
                Debug.LogError("The wall is not found!");
            }
        }
        public void RemoveWall(double x, double y,double angle) 
        {
            Position position = new(x, y, angle);
            RemoveWall(position);
        }
        public void RemoveWall(Wall wall)
        {
            RemoveWall(wall.wallPos);
        }

        //用于移除fence
        public void RemoveFence(Position wallPos)
        {
            //Wall wall = new(wallPos);
            Wall foundFence = CityFence.Find(w => w.wallPos.Equals(wallPos));
            if (foundFence != null)
            {
                RemoveWallEffect(foundFence);
                foundFence.RemoveWall(); 
                CityFence.Remove(foundFence);
                
            }
            else
            {
                Debug.LogError("The fence is not found!");
            }
        }
        public void RemoveFence(double x, double y, double angle)
        {
            Position position = new(x, y, angle);
            RemoveFence(position);
        }

        public void RemoveFence(Wall wall)
        {
            RemoveFence(wall.wallPos);
        }

        public void RemoveWallEffect(Wall wall)
        {
            GameObject effectPrefab = null;
            GameObject wallController = GameObject.Find("WallController");
            // 加载特效预制件
            effectPrefab = Resources.Load<GameObject>($"Effects/REMOVE_WALL");

            if (effectPrefab != null)
            {
                // 实例化特效并将其放置在 player's TankObject 上
                GameObject effectInstance = GameObject.Instantiate(effectPrefab, wall.createdWallObject.transform.position + new Vector3(0,0.5f,0), Quaternion.identity, wallController.transform);

                // 可选：设置特效实例的生命周期，假设特效在3秒后销毁
                GameObject.Destroy(effectInstance, 3f);
            }
            else
            {
                Debug.LogWarning($"特效 CONSTRUCT 未找到!");
            }
        }

        public void AddTrap(Position trapPos, bool isActive = false)
        {
            Trap trap = new(trapPos,isActive);
            Traps.Add(trap);
        }

        public void UpdateTrap(Trap trap, bool isActive)
        {
            if (trap == null)
            {
                trap.isActive = isActive;
            }
        }
        public void RemoveTrap(Position position)
        {
            Trap trap = Traps.Find(w => w.trapPos == position);
            if (trap != null)
            {
                Traps.Remove(trap);
                trap.RemoveTrap();
            }
            else
            {
                Debug.LogError("The Trap is not found!");
            }
        }

        public void RemoveTrap(Trap trap)
        {
            RemoveTrap(trap.trapPos);
        }

        public void DeleteMap()
        {
            // 使用临时列表来避免在遍历时修改集合
            List<Trap> trapsToRemove = new List<Trap>(Traps);
            foreach (Trap trap in trapsToRemove)
            {
                RemoveTrap(trap);
            }

            List<Wall> wallsToRemove = new List<Wall>(CityWall);
            foreach (Wall wall in wallsToRemove)
            {
                RemoveWall(wall);
            }

            List<Wall> fencesToRemove = new List<Wall>(CityFence);
            foreach (Wall fence in fencesToRemove)
            {
                RemoveFence(fence);
            }

            foreach (GameObject floor in CityFloors)
            {
                Object.Destroy(floor);
            }

            // 清空列表
            Traps.Clear();
            CityWall.Clear();
            CityFence.Clear();
            CityFloors.Clear();
        }

    }
}
