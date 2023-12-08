#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;

namespace FluffyDuck.EditorUtil.UpperMenu
{
    public static class FluffyDuck
    {
        [MenuItem("FluffyDuck/Convert Excel To Json")]
        static void ConvertExcelToJson()
        {
            Excel2Json.Program.Main(
                "-d", "Android/ExcelData",
                "-o", "Assets/AssetResources/Master",
                "-cs", "Assets/Scripts/MasterData");
        }

        [MenuItem("FluffyDuck/Build Launcher/Build Asset For Local Editor", false, 0)]
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


        [MenuItem("FluffyDuck/Skill/Preview", false, 11)]
        static void SkillPreviewOpen()
        {
            SkillPreview.ShowWindow();
        }

        [MenuItem("FluffyDuck/Skill/Editor", false, 12)]
        static void SkillEditorOpen()
        {
            SkillEditor.ShowWindow();
        }

    }
}
#endif
