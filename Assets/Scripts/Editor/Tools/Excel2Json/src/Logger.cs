#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

namespace Excel2Json
{
    public class Logger
    {
        public static void Log(string message)
        {
#if UNITY_5_3_OR_NEWER
            Debug.Log(message);
#else
            Console.WriteLine(message);
#endif
        }

        public static void LogError(string message)
        {
#if UNITY_5_3_OR_NEWER
            Debug.LogError(message);
#else
            Console.Error.WriteLine(message);
#endif
        }
    }
}
