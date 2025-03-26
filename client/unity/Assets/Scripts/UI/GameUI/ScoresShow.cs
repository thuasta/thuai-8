using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class ScoresShow : AbstractModel
    {
        public Text scores;

        protected override void OnInit()
        {
            Text Scores_Text = null;//GameObject.Find("Canvas/Scores").GetComponent<Text>();
            scores = Scores_Text;
        }
    }
}