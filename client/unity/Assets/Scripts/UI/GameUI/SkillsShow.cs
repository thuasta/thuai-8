using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCity
{
    public class SkillsShow : AbstractModel
    {
        public Dictionary<int, Dictionary<int, Image>> skills_cd = new();

        protected override void OnInit()
        {
            Image Skill_Mask1 = GameObject.Find("Canvas/Skills_1/Img_1/Mask_1").GetComponent<Image>();
            Image Skill_Mask2 = GameObject.Find("Canvas/Skills_2/Img_1/Mask_1").GetComponent<Image>();
            skills_cd[1][1] = Skill_Mask1;
            skills_cd[2][1] = Skill_Mask2;
        }
    }
}