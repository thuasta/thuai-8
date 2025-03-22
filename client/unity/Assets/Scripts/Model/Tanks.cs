using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class Tanks : AbstractModel
    {
        private Dictionary<int, TankModel> TankDict { get; set; }

        protected override void OnInit()
        {
            TankDict = new();
        }

        public int GetPlayerNum()
        {
            return TankDict.Count;
        }
        
        public TankModel GetTank(int id)
        {
            TankDict.TryGetValue(id, out var tank);
            return tank;
        }

        public Dictionary<int, TankModel> GetTankDictCopy()
        {
            return TankDict != null
                ? new Dictionary<int, TankModel>(TankDict)
                : new Dictionary<int, TankModel>();
        }

        public bool AddTankModel(TankModel tank)
        {
            if (tank == null || TankDict.ContainsKey(tank.Id))
            {
                return false;
            }

            TankDict[tank.Id] = tank;
            return true;
        }

        public bool AddTankModel(int id)
        {
            TankModel tank = new TankModel(id);
            AddTankModel(tank);
            return true;
        }

        public bool DelTankModel(int id)
        {
            if (TankDict.ContainsKey(id))
            {
                TankDict[id].DestroyTank();
                TankDict.Remove(id);
                return true;
            }
                        
            return false;
        }

        public void DelAllTanks()
        {
            Dictionary<int, TankModel> TankToDelete = new Dictionary<int,TankModel>(TankDict);
            foreach (var tank in TankToDelete.Values)
            {
                DelTankModel(tank.Id);
            }
            TankDict.Clear();
        }

    }
}

