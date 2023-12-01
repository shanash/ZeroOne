using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyDuck.Util
{
    public class Dispatcher : MonoBehaviour, IDispatcher
    {
        List<Action> pending = new List<Action>();
        private static Dispatcher _instance;
        public static Dispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = FindObjectOfType<Dispatcher>();
                    if (obj != null)
                    {
                        _instance = obj;
                    }
                    else
                    {
                        var new_obj = new GameObject();
                        _instance = new_obj.AddComponent<Dispatcher>();
                        new_obj.name = _instance.GetType().Name;
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Schedule code for execution in the main-thread
        /// </summary>
        /// <param name="fn"></param>
        public void AddAction(Action fn)
        {
            if (fn == null)
            {
                return;
            }
            lock (pending)
            {
                pending.Add(fn);
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

        /// <summary>
        /// Execute pending actions
        /// </summary>
        public void InvokePending()
        {
            lock (pending)
            {
                if (pending.Count > 0)
                {
                    for (int i = 0; i < pending.Count; i++)
                    {
                        var action = pending[i];
                        action();
                    }
                    pending.Clear();
                }
            }
        }

        void Update()
        {
            InvokePending();
        }
    }

}
