using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace BattleCity
{
    public enum PlayState
    {
        Rest,
        Battle,
        Pause,
        End,
    }

    public class RecordInfo : AbstractModel
    {
        // 20 frame per second
        public float FrameTime = 0.05f;
        public PlayState NowPlayState = PlayState.Rest;
        public int NowTick = 0;
        public int BattleTick = 0;
        
        public int NowRecordNum = 0;
        public const float MinSpeed = -5f;
        public const float MaxSpeed = 5f;

        public int MaxTick;

        public int GameRounds = 0;
        public int CurrentBattle = 0;
        public void Reset()
        {
            NowTick = 0;
            NowRecordNum = 0;
            BattleTick = 0;
        }

        public void FastSpeed()
        {
            if (FrameTime > 0.0125f)
            {
                FrameTime /= 2;
            }
        }

        public void SlowSpeed()
        {
            if (FrameTime < 0.2f)
            {
                FrameTime *= 2;
            }
        }

        public void ResetSpeed()
        {
            FrameTime = 0.05f;
        }

        protected override void OnInit()
        {
        }
    }
}
