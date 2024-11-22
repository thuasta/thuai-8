using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using QFramework;
using UnityEngine;


namespace BattleCity
{
    public class GameApp : Architecture<GameApp>
    {
        protected override void Init()
        {
            this.RegisterModel(new Tank());
            this.RegisterModel(new Bullet());
            this.RegisterModel(new AmmoText());
            this.RegisterSystem(new ScoreSystem());
            this.RegisterModel(new CountdownText());
            this.RegisterModel(new HealthShow());
            this.RegisterModel(new ArmorShow());
            this.RegisterModel(new BuffShow());
        }
        
    }
}

