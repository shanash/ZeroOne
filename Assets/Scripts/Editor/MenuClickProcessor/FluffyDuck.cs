#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace FluffyDuck.EditorUtil.UpperMenu
{
    public static class FluffyDuck
    {
        [MenuItem("FluffyDuck/Build Launcher/Build Asset For Local Editor _%#/", false, 0)]
        static void BuildLauncher_BuildAssetForLocalEditor()
        {
            BuildLauncher.BuildAssetForLocalEditor();
        }

        [MenuItem("FluffyDuck/Build Launcher/Show Window", false, 1)]
        static void BuildLauncher_ShowWindow()
        {
            // Unity의 Build Settings 윈도우를 엽니다.
            EditorWindow.GetWindow(typeof(BuildPlayerWindow), true, "Build Settings");

            BuildLauncher.ShowWindow();
        }

        [MenuItem("FluffyDuck/Build Launcher/Delete/Delete All", false, 101)]
        static void BuildLauncher_Delete_DeleteAll()
        {
            BuildLauncher_Delete_DeleteAddressableBuildData();
            BuildLauncher_Delete_DeleteAddressableBuildInfo();
            BuildLauncher_Delete_DeleteAddressableBuildCache();
        }

        [MenuItem("FluffyDuck/Build Launcher/Delete/Delete Addressable Build Data", false, 102)]
        static void BuildLauncher_Delete_DeleteAddressableBuildData()
        {
            BuildLauncher.DeleteAddressableBuildFolder();
        }

        [MenuItem("FluffyDuck/Build Launcher/Delete/Delete Addressable Build Info", false, 103)]
        static void BuildLauncher_Delete_DeleteAddressableBuildInfo()
        {
            BuildLauncher.DeleteAddressableBuildInfo();
        }

        [MenuItem("FluffyDuck/Build Launcher/Delete/Delete Addressables Cache", false, 104)]
        static void BuildLauncher_Delete_DeleteAddressableBuildCache()
        {
            BuildLauncher.DeleteAddressableCache();
        }

        [MenuItem("FluffyDuck/Skill/Preview _&9", false, 11)]
        static void Skill_PreviewOpen()
        {
            SkillPreview.ShowWindow();
        }

        [MenuItem("FluffyDuck/Skill/Editor _&0", false, 12)]
        static void Skill_EditorOpen()
        {
            SkillEditor.ShowWindow();
        }


        [MenuItem("FluffyDuck/Move Scene/Title _&1", false, 31)]
        static void MoveScene_Title()
        {
            string go_scene = "title";
            string scene_name = EditorSceneManager.GetActiveScene().name;
            if (scene_name.Equals(go_scene))
            {
                return;
            }
            EditorSceneManager.OpenScene($"Assets/Scenes/{go_scene}.unity");
        }

        [MenuItem("FluffyDuck/Move Scene/Lobby _&2", false, 32)]
        static void MoveScene_Lobby()
        {
            string go_scene = "home";
            string scene_name = EditorSceneManager.GetActiveScene().name;
            if (scene_name.Equals(go_scene))
            {
                return;
            }
            EditorSceneManager.OpenScene($"Assets/Scenes/{go_scene}.unity");
        }
        [MenuItem("FluffyDuck/Move Scene/Battle _&3", false, 33)]
        static void MoveScene_Battle()
        {
            string go_scene = "battlev2";
            string scene_name = EditorSceneManager.GetActiveScene().name;
            if (scene_name.Equals(go_scene))
            {
                return;
            }
            EditorSceneManager.OpenScene($"Assets/Scenes/{go_scene}.unity");
        }

        [MenuItem("FluffyDuck/Move Scene/Memorial _&4", false, 34)]
        static void MoveScene_Memorial()
        {
            string go_scene = "memorial";
            string scene_name = EditorSceneManager.GetActiveScene().name;
            if (scene_name.Equals(go_scene))
            {
                return;
            }
            EditorSceneManager.OpenScene($"Assets/Scenes/{go_scene}.unity");
        }

        [MenuItem("FluffyDuck/Persistent/Open Folder _%#o", false, 50)]
        static void Persistent_OpenFolder()
        {
            string folder_path = $"{UnityEngine.Application.persistentDataPath}";

            if (System.IO.Directory.Exists(folder_path))
            {
                if (System.Environment.OSVersion.Platform == System.PlatformID.Win32NT)
                {
                    Process.Start("explorer.exe", folder_path.Replace('/', '\\'));
                }
                else if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                {
                    Process.Start("open", folder_path);
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Folder does not exist: " + folder_path);
            }
        }

        [MenuItem("FluffyDuck/Persistent/Clear Data _%#d", false, 51)]
        static void Persistent_ClearData()
        {
            try
            {
                string[] files = System.IO.Directory.GetFiles(UnityEngine.Application.persistentDataPath);

                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                    UnityEngine.Debug.Log($"Deleted file: {file}");
                }

                UnityEngine.Debug.Log("All Persistent Datas have been deleted.");
                EditorUtility.DisplayDialog("삭제 완료", "모든 Persistent 데이터가 삭제되었습니다", "확인");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
            }
        }

        [MenuItem("FluffyDuck/Convert Excel To Json _%#j", false, 200)]
        static void ConvertExcelToJson()
        {
            Excel2Json.Program.Main(
                "-d", "Android/ExcelData",
                "-o", "Assets/AssetResources/Master",
                "-cs", "Assets/Scripts/MasterData");

            AssetDatabase.Refresh();
        }
    }
}
#endif
