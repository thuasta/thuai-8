using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class Tank : AbstractModel
    {
        private static readonly Dictionary<int, TankModel> _tankDict = new();

        protected override void OnInit()
        {
        }
        public static Dictionary<int, TankModel> GetTanks()
        {
            return _tankDict;
        }
        public static void AddTankModel()
        {
            //TODO
        }

    }
}

