using System.Collections.Generic;
using UnityEngine;

namespace FluffyDuck.Util
{
    public class CustomUpdateManager : MonoSingleton<CustomUpdateManager>
    {
        protected override bool Is_DontDestroyOnLoad { get => true; }

        List<IUpdateComponent> Update_Component_List;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void CallInstance()
        {
            _ = Instance;
        }

        protected override void Initialize()
        {
            Update_Component_List = new List<IUpdateComponent>();
        }

        void Update()
        {
            float dt = Time.deltaTime;
            for (int i = 0; i < Update_Component_List.Count; i++)
            {
                Update_Component_List[i].OnUpdate(dt);
            }
        }

        public void RegistCustomUpdateComponent(GameObject obj)
        {
            IUpdateComponent comp = obj.GetComponent<IUpdateComponent>();
            RegistCustomUpdateComponent(comp);
        }

        public void DeregistCustomUpdateComponent(GameObject obj)
        {
            IUpdateComponent comp = obj.GetComponent<IUpdateComponent>();
            DeregistCustomUpdateComponent(comp);
        }

        public void RegistCustomUpdateComponent(IUpdateComponent comp)
        {
            if (comp != null)
            {
                if (!Update_Component_List.Contains(comp))
                {
                    Update_Component_List.Add(comp);
                }
            }
        }

        public void DeregistCustomUpdateComponent(IUpdateComponent comp)
        {
            if (comp != null)
            {
                if (Update_Component_List.Contains(comp))
                {
                    Update_Component_List.Remove(comp);
                }
            }
        }
    }
}
