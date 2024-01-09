using System.Reflection;
using System.Threading.Tasks;
using System;
using UnityEngine;

/*
// 사용 방법
public class TestClass : MonoBehaviour
{
    bool Initialize(int a, float b, string c)
    {
        // false를 리턴하면 실패하고 null 이 결과값으로 전달된다
        return true;
    }
}

// 생성
MBFactory.Create<TestClass>("Assets/프리팹키", Transform, (TestClass t) => { }, 0, 1.2f, "");
*/

namespace FluffyDuck.Util
{
    /// <summary>
    /// FactoryCore의 MonoBehaviour 버전
    /// </summary>
    public abstract class MonoFactory
    {
        public interface IProduct { }

        static T CreateInstance<T>(GameObject obj, params object[] args) where T : MonoBehaviour, IProduct
        {
            T instance = obj.GetComponent<T>();

            // 매개변수 타입 배열 생성
            Type[] argTypes = Array.ConvertAll(args, arg => arg.GetType());

            // Initialize 메소드 호출
            MethodInfo initializeMethod = typeof(T).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic, null, argTypes, null);
            if (initializeMethod == null)
            {
                Debug.Assert(false, $"Initialize 메소드를 찾을 수 없습니다: {typeof(T)}");
                return null;
            }

            try
            {
                if ((bool)initializeMethod.Invoke(instance, args) == false)
                {
                    Debug.Assert(false, $"MBFactoryCore::Create<{typeof(T)}> 생성 실패!!");
                    GameObject.Destroy(obj);
                    return null;
                }
            }
            catch (TargetException e)
            {
                Debug.Assert(false, $"생성한 프리팹에 {typeof(T)} 컴포넌트가 붙어 있는지 확인해보세요.");
                Debug.LogException(e);
            }
            catch (Exception e) { Debug.LogException(e); }

            return instance;
        }

        public static void Create<T>(string path, Transform parent, Action<T> cb, params object[] args) where T : MonoBehaviour, IProduct
        {
            try
            {
                GameObjectPoolManager.Instance.GetGameObject(path, parent, (GameObject obj) =>
                {
                    T instance = CreateInstance<T>(obj, args);

                    cb?.Invoke(instance);
                });
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.ToString());
            }
        }

        public async static Task<T> CreateAsync<T>(string path, Transform parent, params object[] args) where T : MonoBehaviour, IProduct
        {
            try
            {
                var obj = await GameObjectPoolManager.Instance.GetGameObjectAsync(path, parent);
                return CreateInstance<T>(obj, args);
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.ToString());
                return null;
            }
        }
    }
}
