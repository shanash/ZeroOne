
using UnityEditor;

namespace FluffyDuck.Addressable
{
    public class AddressableWrapper
    {
        public static bool isTest = true;
        
        /// <summary>
        /// TODO: 이후에 코드값으로 Addressables Profiles에서 참조해도 괜찮지 않을까?
        /// </summary>
        public static string remotePath
        {
            get
            {
                string path;
                if (isTest)
                {
                    path = "http://10.10.0.20/clientdata";
                }
                else
                {
                    path = "live path";
                }

                return path;
            }
        }

        public static int Data_Version
        {
            get;set;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Addressables Profiles에서 참조하기 위한 버전 문자열
        /// </summary>
        public static string catalogVersion
        {
            get
            {
                // 에디터 시작할때 초기화 되지 않은 값을 읽어서 경고가 생긴다
                // 잠깐 쉬어주자
                if (10 > EditorApplication.timeSinceStartup)
                    return null;

                return UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.OverridePlayerVersion;
            }
        }
#endif
    }
}
