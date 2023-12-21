using System;
using System.Reflection;
using UnityEngine;

/*
//사용 방법
public class TestClass
{
    // Factory를 이용하려면 생성자는 nonpublic이어야 한다.
    TestClass() { }    
    bool Initialize(int a, float b, string c)
    {
        // false를 리턴하면 실패하고 null 이 결과값으로 전달된다
        return true;
    }
}
//생성
TestClass t = Factory.Create<TestClass>(0, 1.2f, "");
*/

/// <summary>
/// 매개변수의 유효성을 검사하고 유효하지 않으면 인스턴스를 생성시키지 않기 위해서 정적 팩토리 메소드를 사용한다.
/// 리플렉션을 사용하여 매 프레임마다 쓰면 느리기에 로딩 및 초기화시에만 사용합시다.
/// 생성자를 private로 선언해야 합니다
/// </summary>
public static class Factory
{
    public static T Create<T>(params object[] args) where T : class
    {
        T instance = null;
        try
        {
            instance = (T)Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic, null, null, null);

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
                Debug.Assert(false, $"Factory::Create<{typeof(T)}> 생성실패!!");
                return null;
            }
        }
        catch (MissingMethodException)
        {
            Debug.Assert(false, $"{typeof(T)}의 생성자를 private로 선언해야 합니다.");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        return instance; // 객체 생성 성공
    }
}
