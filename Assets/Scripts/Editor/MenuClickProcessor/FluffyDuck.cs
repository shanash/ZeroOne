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
            string fullpath = "Android/Excel2Json/Excel2Json.exe"; // 폴더 경로
            string arguments = "-d Android/ExcelData -o Assets/AssetResources/Master -cs Assets/Scripts/MasterData"; // 전달할 파라미터들

            Process process = new Process();
            process.StartInfo.FileName = fullpath;
            process.StartInfo.Arguments = arguments; // 파라미터 전달
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding("EUC-KR");
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            UnityEngine.Debug.Log(output); // 유니티 콘솔에 출력
            /*
            Excel2Json.Program.Main(
                "-d", "Android/ExcelData",
                "-o", "Assets/AssetResources/Master",
                "-cs", "Assets/Scripts/MasterData");
            */
        }

        [MenuItem("FluffyDuck/Build Launcher/Build Asset For Local Editor", false, 0)]
        static void BuildLauncher_BuildAssetForLocalEditor()
        {
            BuildLauncher.BuildAddressablesAndPlayer();
        }

        [MenuItem("FluffyDuck/Build Launcher/Show Window", false, 1)]
        static void BuildLauncher_ShowWindow()
        {
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
    }
}
#endif
