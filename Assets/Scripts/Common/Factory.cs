using System;
using System.Diagnostics;
using System.Reflection;

/// <summary>
/// 매개변수의 유효성을 검사하고
/// 유효하지 않으면 인스턴스를 생성시키지 않기 위해서
/// 정적 팩토리 메소드를 사용한다.
/// 리플렉션을 사용하여 매 프레임마다 쓰면 느리기에
/// 로딩 및 초기화시에만 사용합시다.
/// </summary>
public static class Factory
{
    public static bool Is_Factory_Creating = false;
    public static T Create<T>(params object[] args) where T : FactoryProductBase
    {
        Is_Factory_Creating = true;
        T instance = (T)Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic, null, null, null);

        // Initialize 메소드 호출
        MethodInfo initializeMethod = typeof(T).GetMethod("Initialize_On_Base", BindingFlags.Instance | BindingFlags.NonPublic);
        if ((bool)initializeMethod.Invoke(instance, new object[] { args }) == false)
        {
            Debug.Assert(false, $"Factory::Create<{typeof(T)}> 생성실패!!");
            return null;
        }

        Is_Factory_Creating = false;

        return instance; // 객체 생성 성공
    }
}
