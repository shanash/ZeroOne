using System;
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;

namespace FluffyDuck.Util
{
    public class MainThreadDispatcher : MonoSingleton<MainThreadDispatcher>, IDispatcher
    {
        ConcurrentQueue<Action> Pending;

        protected override bool Is_DontDestroyOnLoad => true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void CallInstance()
        {
            _ = Instance;
        }

        protected override void Initialize()
        {
            Pending = new ConcurrentQueue<Action>();
        }

        /// <summary>
        /// Schedule code for execution in the main-thread
        /// </summary>
        /// <param name="fn"></param>
        public void AddAction(Action fn)
        {
            if (fn != null)
            {
                Pending.Enqueue(fn);
            }
        }

        public void AddAction(Action fn, float delay)
        {
            StartCoroutine(StartDelayAddAction(fn, delay));
        }

        IEnumerator StartDelayAddAction(Action fn, float delay)
        {
            yield return new WaitForSeconds(delay);
            AddAction(fn);
        }

        void InvokePending()
        {
            while (Pending.TryDequeue(out Action action))
            {
                action();
            }
        }

        void Update()
        {
            InvokePending();
        }
    }
}
