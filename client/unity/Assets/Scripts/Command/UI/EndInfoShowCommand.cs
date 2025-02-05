using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class EndInfoShowCommand : AbstractCommand
    {
        public Dictionary<int, Dictionary<int, Dictionary<int, string>>> _mBuff = new();
        public Dictionary<int, Dictionary<int, int>> _scores = new();


        public EndInfoShowCommand(Dictionary<int, Dictionary<int, Dictionary<int, string>>> mBuff, Dictionary<int, Dictionary<int, int>> scores)
        {
            for(int i = 1; i <= 9; i++)
            {
                for(int j = 1; j <= 8; j++)
                {
                    _mBuff[i][1][j] = mBuff[i][1][j];
                    _mBuff[i][2][j] = mBuff[i][2][j];
                }
                _scores[i][1] = scores[i][1];
                _scores[i][2] = scores[i][2];
            }
        }

        protected override void OnExecute()
        {
            var AmmoBuff = this.GetModel<EndInfo>().mBuff;
            var AmmoScores = this.GetModel<EndInfo>().scores;
            for(int i = 1; i <= 9; i++)
            {
                for(int j = 1; j <= 8; j++)
                {
                    AmmoBuff[i][1][j].sprite = Resources.Load<Sprite>(_mBuff[i][1][j]);
                    AmmoBuff[i][2][j].sprite = Resources.Load<Sprite>(_mBuff[i][2][j]);
                }
                AmmoScores[i].text = $"{_scores[i][1]}:{_scores[i][2]}";
            }
        }
        
    }
}