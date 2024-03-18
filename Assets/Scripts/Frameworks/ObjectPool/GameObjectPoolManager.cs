using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace FluffyDuck.Util
{
    /// <summary>
    /// Code by ForestJ (https://forestj.tistory.com)
    /// 미리 GameObject를 생성해둘 필요는 없으며,
    /// 외부에서 Instance를 호출하면 Instance 생성 여부에 따라 자동으로 GameObjectPoolMananger를 Sigleton으로 생성해 준다.
    /// 필요한 경우 각 씬마다 1개씩 생성하여 사용이 가능
    /// DontDestory 모드가 아니기 때문에 씬 변경시 모두 삭제됨
    /// </summary>
    public class GameObjectPoolManager : MonoSingleton<GameObjectPoolManager>
    {
        protected override bool Is_DontDestroyOnLoad => false;

        /// <summary>
        /// 회수된 오브젝트를 보관 할 컨테이너
        /// </summary>
        Transform Recycled_Object_Container;

        /// <summary>
        /// Object Pool 리스트
        /// </summary>
        Dictionary<string, PoolManager> Pool_List;

        List<string> Preload_Prefab_Path_List;
        bool Is_Preload;
        int Total_Preload_Path_Count;
        System.Action<int, int> PreloadDoingCallback;
        System.Action PreloadCompleteCallback;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void CallInstance()
        {
            _ = Instance;
        }

        protected override void Initialize()
        {
            Pool_List = new Dictionary<string, PoolManager>();
            Preload_Prefab_Path_List = new List<string>();
            Is_Preload = false;
            Total_Preload_Path_Count = 0;
            PreloadDoingCallback = null;
            PreloadCompleteCallback = null;

            CreateRecycledObjectContainer();
        }

        /// <summary>
        /// 회수 후 보관할 컨테이너 생성
        /// </summary>
        void CreateRecycledObjectContainer()
        {
            if (Recycled_Object_Container == null)
            {
                var container = new GameObject();
                container.transform.SetParent(this.transform);
                container.SetActive(false);
                container.name = nameof(Recycled_Object_Container);
                Recycled_Object_Container = container.GetComponent<Transform>();
            }
        }

        /// <summary>
        /// 해당 PoolManager가 이미 생성되어 있는지 여부 판단
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsExistPool(string path)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return Pool_List.ContainsKey(name);
        }

        /// <summary>
        /// 체크하는 프리팹 리스트 중에서 아직 PoolManager가 생성되어 있지 않은 패스만 반환해준다.
        /// </summary>
        /// <param name="prefabs_path"></param>
        /// <returns></returns>
        public List<string> NotExistPoolByPrefabList(List<string> prefabs_path)
        {
            var result = new List<string>();
            int cnt = prefabs_path.Count;
            for (int i = 0; i < cnt; i++)
            {
                string path = prefabs_path[i];
                string name = System.IO.Path.GetFileNameWithoutExtension(path);
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                if (!Pool_List.ContainsKey(name))
                {
                    result.Add(path);
                }
            }

            return result;
        }

        /// <summary>
        /// Addressable Asset으로부터 prefab을 생성 GameObject 반환
        /// path를 이용하여 name을 찾고, name 별로 PoolManager를 보관하고 있으며,
        /// 해당 PoolManager에 GameObject가 존재할 경우 찾아서 반환
        /// PoolManager가 없거나, GameObject가 없을 경우 새로 생성하여 반환.
        /// PoolManager 를 새로 생성할 때는 name을 Key로 사용하여 Map에서 재사용이 가능하도록 관리한다.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public async Task<GameObject> GetGameObjectAsync(string path, Transform parent)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            PoolManager p = null;
            if (Pool_List.ContainsKey(name))
            {
                p = Pool_List[name];
            }
            else
            {
                p = new PoolManager(Recycled_Object_Container);
                Pool_List.Add(name, p);
            }

            if (p != null)
            {
                return await p.PopAsync(path, parent);
            }

            return null;
        }

        /// <summary>
        /// Addressable Asset으로부터 prefab을 생성 GameObject 반환
        /// 실시간 반환이지만, 비동기적으로 구현하기 때문에 시차 발생 있을 수 있음.
        /// path를 이용하여 name을 찾고, name 별로 PoolManager를 보관하고 있으며,
        /// 해당 PoolManager에 GameObject가 존재할 경우 찾아서 반환
        /// PoolManager가 없거나, GameObject가 없을 경우 새로 생성하여 반환.
        /// PoolManager 를 새로 생성할 때는 name을 Key로 사용하여 Map에서 재사용이 가능하도록 관리한다.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public async void GetGameObject(string path, Transform parent, System.Action<GameObject> cb)
        {
            var result = await GetGameObjectAsync(path, parent);
            cb?.Invoke(result);
        }

        /// <summary>
        /// path의 prefab을 GameObject로 생성 반환.
        /// path를 이용하여 name을 찾고, name 별로 PoolManager를 보관하고 있으며,
        /// 해당 PoolManager에 GameObject가 존재할 경우 찾아서 반환
        /// PoolManager가 없거나 GameObject가 없을 경우 새로 생성하여 반환한다
        /// PoolManager를 새로 생성할 때는 name을 Key로 사용하여 Map에 저장하여 재사용 할 수 있도록 한다.
        /// </summary>
        /// <param name="path">Resources 아래의 Prefab Path</param>
        /// <param name="parent">GameObject 의 상위 Transform</param>
        /// <returns></returns>
        public GameObject GetGameObject(string path, Transform parent)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            PoolManager p = null;
            if (Pool_List.ContainsKey(name))
            {
                p = Pool_List[name];
            }
            else
            {
                p = new PoolManager(Recycled_Object_Container);
                Pool_List.Add(name, p);
            }

            if (p != null)
            {
                return p.Pop(path, parent);
            }
            return null;
        }

        /// <summary>
        /// 오브젝트를 생성/소환 할 때 좌표를 미리 적용한 후 Parent를 변경하는 방식
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public GameObject GetGameObject(string path, Transform parent, Vector3 pos)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            PoolManager p = null;
            if (Pool_List.ContainsKey(name))
            {
                p = Pool_List[name];
            }
            else
            {
                p = new PoolManager(Recycled_Object_Container);
                Pool_List.Add(name, p);
            }

            if (p != null)
            {
                return p.Pop(path, parent, pos);
            }
            return null;
        }

        /// <summary>
        /// path의 prefab을 GameObject로 생성하여 재사용 컨테이너에 저장해둔다.
        /// 게임내에서 사용하기 전에 미리 생성해두는 것으로, 실시간 생성시 노드가 발생할 수 있으니,
        /// Loading 화면에서 미리 로드할 수있는 GameObject를 알고 있다면 미리 로드하는 것도 좋다.
        /// </summary>
        /// <param name="path"></param>
        public void PreloadGameObject(string path)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            PoolManager p = null;
            if (Pool_List.ContainsKey(name))
            {
                p = Pool_List[name];
            }
            else
            {
                p = new PoolManager(Recycled_Object_Container);
                Pool_List.Add(name, p);
            }

            if (p != null)
            {
                //  todo
                p.PreLoad(path);
            }
        }

        public async void PreloadGameObjectPrefabsAsync(List<string> path_list, System.Action<int, int> callback)
        {
            var all_tasks = new List<Task<int>>();

            int total_cnt = path_list.Count;
            for (int i = 0; i < total_cnt; i++)
            {
                string path = path_list[i];
                string name = System.IO.Path.GetFileNameWithoutExtension(path);
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                PoolManager p = null;
                if (Pool_List.ContainsKey(name))
                {
                    p = Pool_List[name];
                }
                else
                {
                    p = new PoolManager(Recycled_Object_Container);
                    Pool_List.Add(name, p);
                }
                if (p != null)
                {
                    var tid = p.PreLoadAsset(path);
                    all_tasks.Add(tid);
                }
            }
            total_cnt = all_tasks.Count;
            callback?.Invoke(total_cnt - all_tasks.Count, total_cnt);

            while (all_tasks != null && all_tasks.Count > 0)
            {
                var finish_task = await Task.WhenAny(all_tasks);
                if (all_tasks.Contains(finish_task))
                {
                    all_tasks.Remove(finish_task);
                    callback?.Invoke(total_cnt - all_tasks.Count, total_cnt);
                }
            }
        }

        public void AddPreloadGameObjectPath(string path)
        {
            lock (Preload_Prefab_Path_List)
            {
                Preload_Prefab_Path_List.Add(path);
            }
        }
        public void SetPreloadCallback(System.Action<int, int> doCb, System.Action completeCb)
        {
            PreloadDoingCallback = doCb;
            PreloadCompleteCallback = completeCb;
            Is_Preload = true;
        }

        private void Update()
        {
            //  preload 가능 상태
            if (Is_Preload)
            {
                lock (Preload_Prefab_Path_List)
                {
                    //  로딩할게 남아 있다면
                    if (Preload_Prefab_Path_List.Count > 0)
                    {
                        //  프리 로딩
                        var path = Preload_Prefab_Path_List[0];
                        Preload_Prefab_Path_List.RemoveAt(0);
                        PreloadGameObject(path);
                        //  현재까지 로드한 개수 / 총 개수를 반환
                        int loaded_count = Total_Preload_Path_Count - Preload_Prefab_Path_List.Count;
                        PreloadDoingCallback?.Invoke(loaded_count, Total_Preload_Path_Count);
                    }
                    else
                    {
                        PreloadCompleteCallback?.Invoke();
                        Is_Preload = false;

                        PreloadDoingCallback = null;
                        PreloadCompleteCallback = null;
                    }
                }
            }
        }

        /// <summary>
        /// GameObject 회수
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="is_out_move">화면밖으로 강제 이동 시킬 것인지 여부. 기본값은 true</param>
        public void UnusedGameObject(GameObject obj, bool is_out_move = true)
        {
            PoolManager p = null;
            string name = obj.name;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            if (Pool_List.ContainsKey(name))
            {
                p = Pool_List[name];
            }
            else
            {
                p = new PoolManager(Recycled_Object_Container);
                Pool_List.Add(name, p);
            }
            if (p != null)
            {
                p.Push(obj, is_out_move);
            }
            else
            {
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// 사용 중지 및 삭제
        /// </summary>
        /// <param name="obj"></param>
        public void UnusedGameObjectDestroy(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }
            //GameObject.DestroyImmediate(obj);
            GameObject.Destroy(obj, 0.01f);

            //PoolManager p = null;
            //string name = obj.name;
            //if (string.IsNullOrEmpty(name))
            //{
            //    return;
            //}

            //if (Pool_List.ContainsKey(name))
            //{
            //    p = Pool_List[name];
            //}
            //else
            //{
            //    p = new PoolManager(Recycled_Object_Container);
            //    Pool_List.Add(name, p);
            //}
            //if (p != null)
            //{
            //    p.Push(obj, false);
            //}
            //else
            //{
            //    Debug.Assert(false);
            //}
        }

        [ContextMenu("ToStringGameObjectPoolManager")]
        public void ToStringGameObjectPoolManager()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("Pool_List Count [{0}]", Pool_List.Count).AppendLine();
            sb.AppendLine("==========");
            foreach (var item in Pool_List)
            {
                var p = item.Value;
                sb.AppendFormat("[{0}] => {1}", item.Key, p.ToString()).AppendLine();
                sb.AppendLine("==========");
            }

            Debug.Log(sb.ToString());
        }

        public void AllDestroy()
        {
            foreach (var item in Pool_List)
            {
                var p = item.Value;
                p.Dispose();
            }
            Pool_List.Clear();
            Resources.UnloadUnusedAssets(); //  Resources로 호출되었던 사용하지 않는 에셋만 해제
        }

        public void OnDestroy()
        {
            AllDestroy();
        }
    }
}

