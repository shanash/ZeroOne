
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FluffyDuck.Addressable
{
    public class AddressableWrapper
    {
        static readonly string ADDRESSABLE_VERSION_FILE_PATH = $"{AddressableContentDownloader.VERSION_KEY}.txt";
        public static bool isTest = true;

        /// <summary>
        /// FTP 경로
        /// TODO: 이후에 코드값으로 Addressables Profiles에서 참조해도 괜찮지 않을까?
        /// </summary>
        public static string remotePath
        {
            get
            {
                string path;
                if (isTest)
                {
                    path = "http://cnd-test-01.fluffyduck.co.kr/clientdata";
                }
                else
                {
                    path = "live path";
                }

                return path;
            }
        }

        /// <summary>
        /// \Assets\AssetResources\Addressables\AddressableGroupVersion.txt 에 적혀있는 데이터버전
        /// Default그룹에서 가져옵니다
        /// </summary>
        public static int Data_Version { get; set; } = 0;

#if UNITY_EDITOR
        public static string dataVersion
        {
            get
            {
                // 만약 이 부분에서 문제가 생긴다면 요 코드를 살려주세요
                /*
                // 에디터 시작할때 초기화 되지 않은 값을 읽어서 경고가 생긴다
                // 잠깐 쉬어주자
                if (10 > EditorApplication.timeSinceStartup)
                    return null;
                */
                return ReadAddressableVersionText();
            }
        }

        public static string ReadAddressableVersionText()
        {
            bool asset_exist = AssetDatabase.GetMainAssetTypeAtPath(ADDRESSABLE_VERSION_FILE_PATH) != null;

            string version_text = "000";

            if (asset_exist)
            {
                using (StreamReader sr = new StreamReader(ADDRESSABLE_VERSION_FILE_PATH))
                {
                    version_text = sr.ReadLine().Trim('\r','\n');
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("파일이 없습니다");
            }

            return version_text;
        }
#endif
    }
}
