using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FluffyDuck.Util
{
    /// <summary>
    /// 매개변수의 유효성을 검사하고 유효하지 않으면 인스턴스를 생성시키지 않기 위해서 정적 팩토리 메소드를 사용한다.
    /// 리플렉션을 사용하여 매 프레임마다 쓰면 느리기에 로딩 및 초기화시에만 사용합시다.
    /// IProduct는 생성자를 private로 선언해야 하고 private bool Initialize 초기화 메소드가 필요합니다
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// 팩토리로 생성되는 상품지정용 인터페이스
        /// </summary>
        public interface IProduct { }

        /// <summary>
        /// 매개변수 데이터와 제네릭 타입에 기반한 타입 결정 로직을 저장하는 딕셔너리
        /// </summary>
        private static readonly Dictionary<Type, Func<object[], Type>> type_resolvers = new Dictionary<Type, Func<object[], Type>>();

        /// <summary>
        /// 매핑을 추가하는 메서드
        /// </summary>
        /// <typeparam name="T">매핑용 키, Create시에 같은 T로 지정해야 하고 리턴타입도 T다</typeparam>
        /// <param name="resolver">object[] 인자에 따라 실질적으로 어떤타입을 만들지를 판단합니다</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AddTypeMapping<T>(Func<object[], Type> resolver) where T : class, IProduct
        {
            if (type_resolvers.ContainsKey(typeof(T)))
            {
                return;
            }

            // 한번 더 래핑해서 넣는 이유는 IProduct로 지정된 친구들만 생성되게 제한하기 위해서
            Func<object[], Type> type_resolver = args =>
            {
                Type result_type = resolver(args);
                if (!typeof(IProduct).IsAssignableFrom(result_type))
                {
                    throw new InvalidOperationException($"타입 {result_type.Name}가 IProduct를 구현하지 않았습니다!");
                }
                return result_type;
            };

            type_resolvers[typeof(T)] = type_resolver;
        }

        /// <summary>
        /// 매핑메소드를 참고해서 IProduct를 생성합니다
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T Create<T>(params object[] args) where T : class, IProduct
        {
            if (!type_resolvers.ContainsKey(typeof(T)))
            {
                Debug.Assert(false, $"T {typeof(T).Name}은 매핑되지 않은 타입입니다.");
                return null;
            }

            Type target_type = type_resolvers[typeof(T)](args);

            // Create<T> 메서드에 대한 MethodInfo를 가져옵니다.
            MethodInfo createMethod = typeof(Factory).GetMethod(nameof(Instantiate), BindingFlags.Public | BindingFlags.Static);
            // 제네릭 메서드를 타겟 타입으로 만듭니다.
            MethodInfo genericCreateMethod = createMethod.MakeGenericMethod(target_type);
            // 아래 Instantiate메서드를 호출합니다.
            return (T)genericCreateMethod.Invoke(null, new object[] { args });
        }

        /// <summary>
        /// IProduct 인스턴스 생성
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T Instantiate<T>(params object[] args) where T : class, IProduct
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
}
