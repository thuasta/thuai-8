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
        public const float FrameTime = 0.05f;
        public PlayState NowPlayState = PlayState.Pause;
        public int NowTick = 0;
        /// <summary>
        /// Now record serial number
        /// </summary>
        public int NowRecordNum = 0;
        /// <summary>
        /// The speed of the record which can be negative
        /// </summary>
        public float RecordSpeed = 1f;
        public const float MinSpeed = -5f;
        public const float MaxSpeed = 5f;

        /// <summary>
        /// Contains all the item in the game
        /// </summary>
        public double NowFrameTime
        {
            get
            {
                return FrameTime / RecordSpeed;
            }
        }
        /// <summary>
        /// If NowDeltaTime is larger than NowFrameTime, then play the next frame
        /// </summary>
        public float NowDeltaTime = 0;
        public long NowTime = 0;
        /// <summary>
        /// The target tick to jump
        /// </summary>
        public int JumpTargetTick = int.MaxValue;
        /// <summary>
        /// Current max tick
        /// </summary>
        public int MaxTick;
        public void Reset()
        {
            // RecordSpeed = 1f;
            NowTick = 0;
            NowRecordNum = 0;
            JumpTargetTick = int.MaxValue;
        }

        protected override void OnInit()
        {
        }
    }
}
