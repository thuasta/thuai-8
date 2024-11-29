using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class ScoresChangeCommand : AbstractCommand
    {
        private readonly Dictionary<int, int> _score;

        public ScoresChangeCommand(int tankId, int score)
        {
            _score[tankId] = score;
        }

        protected override void OnExecute()
        {
            var Score = this.GetModel<ScoresShow>().scores;
            Score.text = $"{_score[1]}:{_score[2]}";
        }
        
    }
}