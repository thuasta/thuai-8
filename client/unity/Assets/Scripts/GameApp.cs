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
            RegisterBattleModels();
            Debug.Log("GameApp Initialized");
            TypeEventSystem.Global.Register<BattleStageEvent>(e =>
            {
                Debug.Log($"GameApp {SceneData.GameStage}");
                RegisterBattleModels();
            });


            // this.RegisterModel(new AmmoText());
            // this.RegisterModel(new CountdownText());
            
                
            if(SceneData.GameStage == "End")
            {
                this.RegisterModel(new EndInfo());
            }                
        }

        private void RegisterBattleModels()
        {
            if (SceneData.GameStage == "Battle")
            {
                /*this.RegisterModel(new HealthShow());
                this.RegisterModel(new ArmorShow());
                this.RegisterModel(new BuffShow());
                this.RegisterModel(new ScoresShow());
                this.RegisterModel(new RoundsShow());
                this.RegisterModel(new SkillsShow());*/
                this.RegisterModel(new AmmoText());
                this.RegisterModel(new CountdownText());
                this.RegisterModel(new HealthShow());
                this.RegisterModel(new ArmorShow());
                this.RegisterModel(new BuffShow());
                this.RegisterModel(new ScoresShow());
                this.RegisterModel(new RoundsShow());
                this.RegisterModel(new SkillsShow());
                this.RegisterModel(new Tanks());
                this.RegisterModel(new Bullets());
                this.RegisterModel(new Map());
                this.RegisterModel(new RecordInfo());
                Debug.Log("Game Stage Change!");
            }
        }


    }
}

