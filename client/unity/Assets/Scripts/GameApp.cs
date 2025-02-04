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
            // 获取当前活动场景 
            Scene currentScene = SceneManager.GetActiveScene();
            // 获取场景名称 
            string sceneName = currentScene.name;

            
            // this.RegisterModel(new AmmoText());
            // this.RegisterSystem(new ScoreSystem());
            // this.RegisterModel(new CountdownText());
            if (sceneName == "Game")
            {
                this.RegisterModel(new HealthShow());
                this.RegisterModel(new ArmorShow());
                this.RegisterModel(new BuffShow());
                this.RegisterModel(new ScoresShow());
                this.RegisterModel(new RoundsShow());
                this.RegisterModel(new SkillsShow());
            }
                
            if (sceneName == "test_Game")
            {
                this.RegisterModel(new Tanks());
                this.RegisterModel(new Bullets());
                this.RegisterModel(new Map());
                this.RegisterModel(new RecordInfo());
            }
                
            if(sceneName == "End")
            {
                this.RegisterModel(new EndInfo());
            }
                

        }
        
    }
}

