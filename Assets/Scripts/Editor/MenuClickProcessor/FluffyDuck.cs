#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace FluffyDuck.EditorUtil.UpperMenu
{
    public static class FluffyDuck
    {
        const string ClassName = "FluffyDuck";
        const string TogglePlayFullscreen_MenuItemName = "/Toggle Play FullScreen";
        public const string TogglePlayFullscreen_PrefKey = "FullScreenChecked";

        /// <summary>
        /// 어드레서블 에셋을 포함한 뒤 실행해서 Refresh해야 합니다
        /// </summary>
        [MenuItem(ClassName + "/Build Launcher/Update Asset For Local Editor _%#/", false, 0)]
        static void BuildLauncher_UpdateAssetForLocalEditor()
        {
            
            BuildLauncher.UpdateAsssetBundleForLocalEditor();
        }

        [MenuItem(ClassName + "/Build Launcher/Validate Assets", false, 1)]
        static void BuildLaucher_ValidateAssets()
        {
            var missing_asset_pathes = BuildLauncher.ValidateAssets();
            if (missing_asset_pathes == null)
            {
                EditorUtility.DisplayDialog("문제 발견", "Addressables Settings를 확인할 수 없습니다.", "확인");
            }
            else if (missing_asset_pathes.Count > 0)
            {
                EditorUtility.DisplayDialog("문제 발견", "Addressables에 빠진 Asset이 존재합니다. Addressable Group을 확인해주세요", "확인");
            }
            else
            {
                EditorUtility.DisplayDialog("완료", "Addressables에서 빠진 Asset이 없습니다.", "확인");
            }
        }

        [MenuItem(ClassName + "/Build Launcher/Show Window", false, 2)]
        static void BuildLauncher_ShowWindow()
        {
            // Unity의 Build Settings 윈도우를 엽니다.
            EditorWindow.GetWindow(typeof(BuildPlayerWindow), true, "Build Settings");

            BuildLauncher.ShowWindow();
        }

        [MenuItem(ClassName + "/Build Launcher/Delete/Delete All", false, 101)]
        static void BuildLauncher_Delete_DeleteAll()
        {
            BuildLauncher_Delete_DeleteAddressableBuildData();
            BuildLauncher_Delete_DeleteAddressableBuildInfo();
            BuildLauncher_Delete_DeleteAddressableBuildCache();
        }

        [MenuItem(ClassName + "/Build Launcher/Delete/Delete Addressable Build Data", false, 102)]
        static void BuildLauncher_Delete_DeleteAddressableBuildData()
        {
            BuildLauncher.DeleteAddressableBuildFolder();
        }

        [MenuItem(ClassName + "/Build Launcher/Delete/Delete Addressable Build Info", false, 103)]
        static void BuildLauncher_Delete_DeleteAddressableBuildInfo()
        {
            BuildLauncher.DeleteAddressableBuildInfo();
        }

        [MenuItem(ClassName + "/Build Launcher/Delete/Delete Addressables Cache", false, 104)]
        static void BuildLauncher_Delete_DeleteAddressableBuildCache()
        {
            BuildLauncher.DeleteAddressableCache();
        }

        [MenuItem(ClassName + "/Skill/Preview _&9", false, 11)]
        static void Skill_PreviewOpen()
        {
            SkillPreview.ShowWindow();
        }

        [MenuItem(ClassName + "/Skill/Editor _&0", false, 12)]
        static void Skill_EditorOpen()
        {
            SkillEditor.ShowWindow();
        }

        [MenuItem(ClassName + "/Move Scene/Title _&1", false, 31)]
        static void MoveScene_Title()
        {
            MoveScene(SceneName.title);
        }

        [MenuItem(ClassName + "/Move Scene/Lobby _&2", false, 32)]
        static void MoveScene_Lobby()
        {
            MoveScene(SceneName.home);
        }
        [MenuItem(ClassName + "/Move Scene/Battle _&3", false, 33)]
        static void MoveScene_Battle()
        {
            MoveScene(SceneName.battle);
        }

        [MenuItem(ClassName + "/Move Scene/Essence _&4", false, 34)]
        static void MoveScene_Essence()
        {
            MoveScene(SceneName.essence);
        }

        static void MoveScene(SceneName name)
        {
            string scene_name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (scene_name.Equals(name.ToString()))
            {
                return;
            }
            EditorSceneManager.OpenScene($"Assets/Scenes/{name}.unity");
        }

        [MenuItem(ClassName + "/Persistent/Open Folder _%#o", false, 50)]
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

        [MenuItem(ClassName + "/Persistent/Clear Data _%#d", false, 51)]
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

        [MenuItem(ClassName + "/Convert Excel To Json _%#j", false, 200)]
        static void ConvertExcelToJson()
        {
            Excel2Json.Program.Main(
                "-d", "Android/ExcelData",
                "-o", "Assets/AssetResources/Master",
                "-cs", "Assets/Scripts/MasterData");

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 풀스크린을 위한 값 저장만 합니다.
        /// </summary>
        [MenuItem(ClassName + TogglePlayFullscreen_MenuItemName, false, 300)]
        static void TogglePlayFullscreen()
        {
            bool currentState = EditorPrefs.GetBool(TogglePlayFullscreen_PrefKey, false);
            EditorPrefs.SetBool(TogglePlayFullscreen_PrefKey, !currentState);
        }

        /// <summary>
        /// 메뉴 아이템의 체크 상태를 설정하는 validate 함수입니다.
        /// </summary>
        /// <returns></returns>
        [MenuItem(ClassName + TogglePlayFullscreen_MenuItemName, true)]
        static bool ToggleActionValidate()
        {
            // 메뉴 아이템의 체크 상태를 EditorPrefs에서 가져온 값으로 설정합니다.
            Menu.SetChecked(TogglePlayFullscreen_MenuItemName, EditorPrefs.GetBool(TogglePlayFullscreen_PrefKey, false));
            return true; // 메뉴 아이템이 항상 활성화되어 있도록 true를 반환합니다.
        }

        [MenuItem(ClassName + "/Go To Build Page", false, 400)]
        static void GoToBuildPage()
        {
            Help.BrowseURL("http://10.10.0.3:18080");
        }
    }
}
#endif
