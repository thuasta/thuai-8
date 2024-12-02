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
        public Dictionary<int, Dictionary<int, Image>> skills_image = new();
        public Dictionary<int, List<string>> skills_list = new();

        protected override void OnInit()
        {
            for(int i = 1; i <= 8; i++)
            {
                Image Skill_1 = GameObject.Find($"Canvas/Skills_1/Img_{i}").GetComponent<Image>();
                Image Skill_2 = GameObject.Find($"Canvas/Skills_2/Img_{i}").GetComponent<Image>();
                Image CD_1 = GameObject.Find($"Canvas/Skills_1/Img_{i}/Mask_{i}").GetComponent<Image>();
                Image CD_2 = GameObject.Find($"Canvas/Skills_2/Img_{i}/Mask_{i}").GetComponent<Image>();
                skills_image[1][i] = Skill_1;
                skills_image[2][i] = Skill_2;
                skills_cd[1][i] = CD_1;
                skills_cd[2][i] = CD_2;
            }
        }
    }
}