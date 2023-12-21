using FluffyDuck.Util;
using System.Reflection;
using System.Threading.Tasks;
using System;
using UnityEngine;


/// <summary>
/// FactoryCore의 MonoBehaviour 버전
/// Start 예약 메소드를 사용하려면 override하고 base.Start(); 를 호출하여 사용해야 합니다
/// </summary>
public abstract class MBFactory
{
    static T CreateInstance<T>(GameObject obj, params object[] args) where T : MonoBehaviour
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

        if ((bool)initializeMethod.Invoke(instance, args) == false)
        {
            Debug.Assert(false, $"MBFactoryCore::Create<{typeof(T)}> 생성 실패!!");
            GameObject.Destroy(instance);
            return null;
        }

        return instance;
    }

    public static void Create<T>(string path, Transform parent, Action<T> cb, params object[] args) where T : MonoBehaviour
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

    public async static Task<T> CreateAsync<T>(string path, Transform parent, params object[] args) where T : MonoBehaviour
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
