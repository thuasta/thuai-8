using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class BuffShowCommand : AbstractCommand
    {
        private readonly int _tankId;
        private readonly int _episodes;

        public BuffShowCommand(int tankId, int episodes)
        {
            _tankId = tankId - 1;
            _episodes = episodes;
        }

        protected override void OnExecute()
        {
            var buff_list = this.GetModel<BuffShow>().buffList[_tankId];
            var buff_image = this.GetModel<BuffShow>().buffImage[_tankId];
            for(int i = 0; i < buff_list.Count; i++)
            {
                if(i <= _episodes)
                {
                    string buff = buff_list[i];
                    if(buff != "")
                    {
                        buff_image[i].sprite = Resources.Load<Sprite>($"UI/Icons/{buff}");
                        Color new_color = buff_image[i].color;
                        new_color.a = 1;
                        buff_image[i].color = new_color;
                    }
                }
                else
                {
                    Color new_color = buff_image[i].color;
                    new_color.a = 0;
                    buff_image[i].color = new_color;
                }
            }
        }
        
    }
}

