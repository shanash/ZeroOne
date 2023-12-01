using System.Collections.Generic;
using UnityEngine;

namespace FluffyDuck.Util
{
    public class CustomUpdateManager : MonoBehaviour
    {

        private static CustomUpdateManager _instance = null;
        public static CustomUpdateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = FindObjectOfType<CustomUpdateManager>();
                    if (obj != null)
                    {
                        _instance = obj;
                    }
                    else
                    {
                        var new_obj = new GameObject();
                        _instance = new_obj.AddComponent<CustomUpdateManager>();
                        new_obj.name = _instance.GetType().Name;
                    }
                }
                return _instance;
            }
        }

        List<IUpdateComponent> Update_Component_List = new List<IUpdateComponent>();

        // Update is called once per frame
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
