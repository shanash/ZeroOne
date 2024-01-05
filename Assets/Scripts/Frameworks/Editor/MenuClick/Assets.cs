#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;

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
        static void Create_CsharpFactoryScript()
        {
            CreateScript("FactoryInstance", "Create C# Factory Script");
        }

        [MenuItem("Assets/Create/C# MonoBehaviour Factory Script", priority = 19)]
        static void Create_CsharpMonobehaviourFactoryScript()
        {
            CreateScript("MonoFactoryInstance", "Create C# MonoBehaviour Factory Script");
        }

        [MenuItem("Assets/Create/C# Singleton Script", priority = 19)]
        static void Create_CsharpSingletonScript()
        {
            CreateScript("SingletonBase", "Create C# Singleton Script");
        }

        static void CreateScript(string defaultName, string menuName)
        {
            // 사용자 정의 스크립트 템플릿 경로
            string templatePath = $"{CurrentScriptDirectory}/ScriptTemplates/{defaultName}.txt";

            // 파일 저장 대화 상자를 통해 사용자로부터 파일 이름 받기
            string path = EditorUtility.SaveFilePanel(menuName, AssetDatabase.GetAssetPath(Selection.activeObject), defaultName, "cs");
            if (string.IsNullOrEmpty(path)) return;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            string copyPath = path.Replace('\\', '/');
            int index = copyPath.IndexOf("Assets/");
            string relativePath = copyPath.Substring(index);

            // 템플릿 파일 복사
            File.Copy(templatePath, copyPath, true);

            // 클래스 이름 변경
            string fileContent = File.ReadAllText(copyPath);
            fileContent = fileContent.Replace("#SCRIPTNAME#", fileNameWithoutExtension);
            File.WriteAllText(copyPath, fileContent);

            AssetDatabase.Refresh();

            // 생성된 스크립트 파일 선택
            UnityEngine.Object createdScript = AssetDatabase.LoadAssetAtPath(relativePath, typeof(UnityEngine.Object));
            Selection.activeObject = createdScript;
        }
    }
}
#endif
