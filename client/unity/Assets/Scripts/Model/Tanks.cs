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
        
        public TankModel GetTank(int id)
        {
            TankDict.TryGetValue(id, out var tank);
            return tank;
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

        public bool DelTankModel(int id)
        {
            if (TankDict.ContainsKey(id))
            {
                TankDict.Remove(id);
                return true;
            }
                        
            return false;
        }

    }
}

