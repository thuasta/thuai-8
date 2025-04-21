using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BattleCity
{
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> executionQueue = new Queue<Action>();
        private static UnityMainThreadDispatcher instance = null;

        public static UnityMainThreadDispatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<UnityMainThreadDispatcher>();
                    if (instance == null)
                    {
                        var obj = new GameObject("MainThreadDispatcher");
                        instance = obj.AddComponent<UnityMainThreadDispatcher>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return instance;
            }
        }

        public void Enqueue(Action action)
        {
            lock (executionQueue)
            {
                executionQueue.Enqueue(action);
            }
        }

        void Update()
        {
            lock (executionQueue)
            {
                while (executionQueue.Count > 0)
                {
                    executionQueue.Dequeue().Invoke();
                }
            }
        }
    }
}

