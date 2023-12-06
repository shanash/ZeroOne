using System;
using System.Linq;
using System.Reflection;

namespace FluffyDuck.EditorUtil
{
    public static class NamespaceReflection
    {
        public static Type[] GetTypesInNamespace(string namespaceName)
        {
            // 현재 실행 중인 어셈블리를 가져옵니다.
            Assembly assembly = Assembly.GetExecutingAssembly();

            // 해당 네임스페이스에 있는 모든 타입을 반환합니다.
            return assembly.GetTypes()
                            .Where(type => type.Namespace == namespaceName)
                            .ToArray();
        }
    }
}
