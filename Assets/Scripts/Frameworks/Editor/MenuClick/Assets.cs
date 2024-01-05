using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FluffyDuck.Editor.Menu
{
    public static class Assets
    {
        static string CurrentScriptDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_CurrentScriptDirectory))
                {
                    try
                    {
                        StackTrace stackTrace = new StackTrace(true);
                        foreach (var frame in stackTrace.GetFrames())
                        {
                            string file_path = frame.GetFileName();
                            if (file_path != null && file_path.EndsWith(".cs"))
                            {
                                string directory_path = Path.GetDirectoryName(file_path);
                                directory_path = directory_path.Replace('\\', '/');
                                var index = directory_path.IndexOf("Assets/");
                                directory_path = directory_path.Substring(index);
                                _CurrentScriptDirectory = directory_path;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogException(ex);
                    }
                }

                if (string.IsNullOrEmpty(_CurrentScriptDirectory))
                {
                    UnityEngine.Debug.LogWarning("현재 스크립트 파일의 경로를 찾아오지 못하고 있습니다");
                    throw new Exception("현재 스크립트 파일의 경로를 찾아오지 못하고 있습니다");
                }

                return _CurrentScriptDirectory;
            }
        }
        static string _CurrentScriptDirectory = string.Empty;

        [MenuItem("Assets/Create/C# Factory Script", priority = 19)]
        private static void Create_CsharpFactoryScript()
        {
            // 사용자 정의 스크립트 템플릿 경로
            string templatePath = $"{CurrentScriptDirectory}/ScriptTemplates/FactoryInstance.txt";

            // 새 스크립트 파일 경로 및 이름 설정
            string targetPath = $"{AssetDatabase.GetAssetPath(Selection.activeObject)}/FactoryInstance.cs";

            // 템플릿에서 새 스크립트 파일 생성
            File.Copy(templatePath, targetPath);
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Create/C# MonoBehaviour Factory Script", priority = 19)]
        private static void Create_CsharpMonobehaviourFactoryScript()
        {
            // 사용자 정의 스크립트 템플릿 경로
            string templatePath = $"{CurrentScriptDirectory}/ScriptTemplates//MonoFactoryInstance.txt";

            // 새 스크립트 파일 경로 및 이름 설정
            string targetPath = AssetDatabase.GetAssetPath(Selection.activeObject) + "/MonoFactoryInstance.cs";

            // 템플릿에서 새 스크립트 파일 생성
            File.Copy(templatePath, targetPath);
            AssetDatabase.Refresh();
        }
    }
}
