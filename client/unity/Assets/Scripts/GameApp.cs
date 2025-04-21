using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
            TypeEventSystem.Global.Register<LoadingEvent>(e =>
            {
                UnregisterAllModels(); // 清空所有已注册模型
                RegisterBattleModels();
            });
            TypeEventSystem.Global.Register<BattleStageEvent>(e =>
            {
                RegisterBattleModels();
            });
        }

        private void RegisterBattleModels()
        {         
            if (SceneData.GameStage == "Loading")
            {
                this.RegisterModel(new Tanks());
                this.RegisterModel(new Bullets());
                this.RegisterModel(new Map());
                this.RegisterModel(new RecordInfo());
                Debug.Log("Loading Models Registered!");
            }
            else if (SceneData.GameStage == "Battle")
            {
                this.RegisterModel(new AmmoText());
                this.RegisterModel(new CountdownText());
                this.RegisterModel(new HealthShow());
                this.RegisterModel(new ArmorShow());
                this.RegisterModel(new BuffShow());
                this.RegisterModel(new ScoresShow());
                this.RegisterModel(new RoundsShow());
                this.RegisterModel(new SkillsShow());
                Debug.Log("Battle Models Registered!");
            }
            else if (SceneData.GameStage == "End")
            {
                this.RegisterModel(new EndInfo());
                Debug.Log("End Model Registered!");
            }
        }

        private void UnregisterAllModels()
        {
            // 使用反射获取Architecture中的模型字典并清空
            var architectureType = typeof(Architecture<GameApp>);
            var modelsField = architectureType.GetField("mModels", BindingFlags.NonPublic | BindingFlags.Instance);

            if (modelsField != null)
            {
                var models = modelsField.GetValue(this) as Dictionary<Type, IModel>;
                models?.Clear();
                Debug.Log("All models unregistered.");
            }
           /* else
            {
                Debug.LogError("Failed to find mModels field in Architecture.");
            }*/
        }
    }
}