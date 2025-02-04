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

        public BuffAddCommand(int tankId, string buff)
        {
            _tankId = tankId;
            _buff = buff;
        }

        protected override void OnExecute()
        {
            var buff_image = this.GetModel<BuffShow>().buffImage[_tankId];
            var buff_list = this.GetModel<BuffShow>().buffList[_tankId];
            buff_list.Add(_buff);
            buff_image[buff_list.Count - 1].sprite = Resources.Load<Sprite>(_buff); 
        }
        
    }
}

