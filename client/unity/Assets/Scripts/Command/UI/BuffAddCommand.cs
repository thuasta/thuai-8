using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class BuffAddCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly string _buff;
        private readonly int _episodes;

        public BuffAddCommand(int tankId, int episodes, string buff)
        {
            // Debug.Log($"Current episode: {episodes}");
            _tankId = tankId - 1;
            _buff = buff;
            _episodes = episodes;
        }

        protected override void OnExecute()
        {
            var buff_list = this.GetModel<BuffShow>().buffList[_tankId];
            buff_list[_episodes] = _buff;
        }
        
    }
}

