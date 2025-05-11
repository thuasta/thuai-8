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
            // 确保内部字典被初始化
            skills_cd[1] = new Dictionary<int, Image>();
            skills_cd[2] = new Dictionary<int, Image>();
            skills_image[1] = new Dictionary<int, Image>();
            skills_image[2] = new Dictionary<int, Image>();
            skills_list[1] = new List<string>();
            skills_list[2] = new List<string>();

            for(int i = 1; i <= 8; i++)
            {
                Image Skill_1 = GameObject.Find($"Canvas/Skills_1/Img_{i}").GetComponent<Image>();
                Image Skill_2 = GameObject.Find($"Canvas/Skills_2/Img_{i}").GetComponent<Image>();
                Image CD_1 = GameObject.Find($"Canvas/Skills_1/Img_{i}/Mask_1").GetComponent<Image>();
                Image CD_2 = GameObject.Find($"Canvas/Skills_2/Img_{i}/Mask_1").GetComponent<Image>();
                
                // 这里将每个Image存储到对应的字典中
                skills_image[1][i] = Skill_1;
                skills_image[2][i] = Skill_2;
                skills_cd[1][i] = CD_1;
                skills_cd[2][i] = CD_2;
            }
        }
    }
}