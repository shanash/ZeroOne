using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyDuck.Util
{
    public enum UPDATE_EVENT_TYPE
    {
        NONE = 0,
    }

    public class UpdateEventDispatcher : MonoSingleton<UpdateEventDispatcher>
    {
        protected override bool Is_DontDestroyOnLoad => throw new System.NotImplementedException();

        Dictionary<UPDATE_EVENT_TYPE, List<System.Action<UPDATE_EVENT_TYPE>>> Event_Action_List = new Dictionary<UPDATE_EVENT_TYPE, List<System.Action<UPDATE_EVENT_TYPE>>>();
        List<UPDATE_EVENT_TYPE> Event_Queue = new List<UPDATE_EVENT_TYPE>();

        Dictionary<UPDATE_EVENT_TYPE, System.Action<UPDATE_EVENT_TYPE>> Event_Actions = new Dictionary<UPDATE_EVENT_TYPE, System.Action<UPDATE_EVENT_TYPE>>();

        bool Is_Use_Add_Action;

        protected override void Initialize()
        {
            Event_Actions = new Dictionary<UPDATE_EVENT_TYPE, System.Action<UPDATE_EVENT_TYPE>>();
            Is_Use_Add_Action = false;
        }

        public void AddEventCallback(UPDATE_EVENT_TYPE etype, System.Action<UPDATE_EVENT_TYPE> callback)
        {
            if (callback == null || etype == UPDATE_EVENT_TYPE.NONE)
            {
                return;
            }
            if (!Is_Use_Add_Action)
            {
                lock (Event_Action_List)
                {
                    if (Event_Action_List.ContainsKey(etype))
                    {
                        var actions = Event_Action_List[etype];
                        actions.Add(callback);
                    }
                    else
                    {
                        List<System.Action<UPDATE_EVENT_TYPE>> actions = new List<System.Action<UPDATE_EVENT_TYPE>>();
                        actions.Add(callback);
                        Event_Action_List.Add(etype, actions);
                    }
                }
            }
            else
            {
                lock (Event_Actions)
                {
                    if (Event_Actions.ContainsKey(etype))
                    {
                        var action = Event_Actions[etype];
                        action += callback;
                    }
                    else
                    {
                        Event_Actions.Add(etype, callback);
                    }
                }
            }
            

        }

        public void RemoveEventCallback(UPDATE_EVENT_TYPE etype)
        {
            if (etype == UPDATE_EVENT_TYPE.NONE)
            {
                return;
            }
            if (!Is_Use_Add_Action)
            {
                lock (Event_Action_List)
                {
                    if (Event_Action_List.ContainsKey(etype))
                    {
                        Event_Action_List.Remove(etype);
                    }
                }
            }
            else
            {
                lock (Event_Actions)
                {
                    if (Event_Actions.ContainsKey(etype))
                    {
                        Event_Actions.Remove(etype);
                    }
                }
            }
            
        }

        public void RemoveEventCallback(UPDATE_EVENT_TYPE etype, System.Action<UPDATE_EVENT_TYPE> cb)
        {
            if (etype == UPDATE_EVENT_TYPE.NONE)
            {
                return;
            }
            if (!Is_Use_Add_Action)
            {
                lock (Event_Action_List)
                {
                    if (Event_Action_List.ContainsKey(etype))
                    {
                        var actions = Event_Action_List[etype];
                        //var find_action = actions.Find(x => object.ReferenceEquals(x, cb));
                        var find_action = actions.Find(x => x == cb);
                        if (find_action != null)
                        {
                            actions.Remove(find_action);
                        }
                    }
                }
            }
            else
            {
                lock (Event_Actions)
                {
                    if (Event_Actions.ContainsKey(etype))
                    {
                        var action = Event_Actions[etype];
                        action -= cb;
                    }
                }
            }
            
        }

        public void AddEvent(UPDATE_EVENT_TYPE etype, bool is_dup_check = false)
        {
            if (etype == UPDATE_EVENT_TYPE.NONE)
            {
                return;
            }
            lock (Event_Queue)
            {
                if (is_dup_check)
                {
                    if (Event_Queue.Exists(x => x == etype))
                    {
                        return;
                    }
                }
                Event_Queue.Add(etype);
            }
        }

        public void RemoveEvent(UPDATE_EVENT_TYPE etype)
        {
            if (etype == UPDATE_EVENT_TYPE.NONE)
            {
                return;
            }
            lock (Event_Queue)
            {
                Event_Queue.RemoveAll(x => x == etype);
            }
        }

        void InvokeEventQueuePending()
        {
            lock (Event_Queue)
            {
                if (Event_Queue.Count > 0)
                {
                    int queue_count = Event_Queue.Count;
                    for (int q = 0; q < queue_count; q++)
                    {
                        UPDATE_EVENT_TYPE evt = Event_Queue[q];
                        if (!Is_Use_Add_Action)
                        {
                            if (!Event_Action_List.ContainsKey(evt))
                            {
                                continue;
                            }
                            List<System.Action<UPDATE_EVENT_TYPE>> acts = Event_Action_List[evt];
                            int cnt = acts.Count;
                            for (int i = 0; i < cnt; i++)
                            {
                                acts[i].Invoke(evt);
                            }
                        }
                        else
                        {
                            if (!Event_Actions.ContainsKey(evt))
                            {
                                continue;
                            }

                            var action = Event_Actions[evt];
                            action?.Invoke(evt);
                        }

                    }
                    Event_Queue.Clear();
                }
            }
        }

        private void Update()
        {
            InvokeEventQueuePending();
        }
    }

}

