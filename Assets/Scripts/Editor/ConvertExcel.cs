#if UNITY_EDITOR
using UnityEditor;
using System.Diagnostics;

public static class ConvertExcel
{
    [MenuItem("FluffyDuck/Run Covert Excel To Json")]
    static void RunConsoleProgram()
    {
        string fullpath = "Android\\Excel2Json\\Excel2Json.exe"; // 폴더 경로
        string arguments = "-d Android\\ExcelData -o Assets\\AssetResources\\Master -cs Assets\\Scripts\\MasterData"; // 전달할 파라미터들

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
    }

    [MenuItem("FluffyDuck/Test")]
    static void Test()
    {

    }
}
#endif
