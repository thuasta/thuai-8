using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BattleCity
{
    public class GameApp : Architecture<GameApp>
    {
        protected override void Init()
        {
            this.RegisterModel(new Tank());
            this.RegisterModel(new Bullet());
            this.RegisterModel(new AmmoText());
            // this.RegisterSystem(new ScoreSystem());
            this.RegisterModel(new CountdownText());
            this.RegisterModel(new HealthShow());
            this.RegisterModel(new ArmorShow());
            this.RegisterModel(new BuffShow());
            this.RegisterModel(new ScoresShow());
            this.RegisterModel(new RoundsShow());
            this.RegisterModel(new SkillsShow());
            this.RegisterModel(new Map());

            // 获取当前活动场景 
            Scene currentScene = SceneManager.GetActiveScene(); 
            // 获取场景名称 
            string sceneName = currentScene.name; 
            if(sceneName == "End")
                this.RegisterModel(new EndInfo());

        }
        
    }
}

