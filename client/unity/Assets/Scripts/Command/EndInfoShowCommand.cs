using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class EndInfoShowCommand : AbstractCommand
    {
        public Dictionary<int, Dictionary<int, Sprite>> _mAmmoImage = new();
        public Dictionary<int, Dictionary<int, int>> _mAmmoNumber = new();
        public Dictionary<int, Dictionary<int, string>> _mBuff = new();
        public Dictionary<int, Dictionary<int, int>> _scores = new();


        public EndInfoShowCommand(Dictionary<int, Dictionary<int, Sprite>> mAmmoImage, Dictionary<int, Dictionary<int, int>> mAmmoNumber,
         Dictionary<int, Dictionary<int, string>> mBuff, Dictionary<int, Dictionary<int, int>> scores)
        {
            for(int i = 1; i <= 9; i++)
            {
                _mAmmoImage[i][1] = mAmmoImage[i][1];
                _mAmmoImage[i][2] = mAmmoImage[i][2];
                _mAmmoNumber[i][1] = mAmmoNumber[i][1];
                _mAmmoNumber[i][2] = mAmmoNumber[i][2];
                _mBuff[i][1] = mBuff[i][1];
                _mBuff[i][2] = mBuff[i][2];
                _scores[i][1] = scores[i][1];
                _scores[i][2] = scores[i][2];
            }
        }

        protected override void OnExecute()
        {
            var AmmoImage = this.GetModel<EndInfo>().mAmmoImage;
            var AmmoNumber = this.GetModel<EndInfo>().mAmmoNumber;
            var AmmoBuff = this.GetModel<EndInfo>().mBuff;
            var AmmoScores = this.GetModel<EndInfo>().scores;
            for(int i = 1; i <= 9; i++)
            {
                AmmoImage[i][1].sprite = _mAmmoImage[i][1];
                AmmoImage[i][2].sprite = _mAmmoImage[i][2];
                AmmoNumber[i][1].text = $"{_mAmmoNumber[i][1]}";
                AmmoNumber[i][2].text = $"{_mAmmoNumber[i][2]}";
                AmmoBuff[i][1].text = _mBuff[i][1];
                AmmoBuff[i][2].text = _mBuff[i][2];
                AmmoScores[i].text = $"{_scores[i][1]}:{_scores[i][2]}";
            }
        }
        
    }
}