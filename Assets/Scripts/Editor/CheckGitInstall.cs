using UnityEditor;
using System.Diagnostics;
using System;

public class CheckGitInstall
{
    [InitializeOnLoadMethod]
    private static void CheckGitInstallation()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = "--version",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };

            using (Process process = Process.Start(psi))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (!result.StartsWith("git version"))
                    {
                        PromptInstallGit();
                    }
                }
            }
        }
        catch (Exception)
        {
            PromptInstallGit();
        }
    }

    private static void PromptInstallGit()
    {
        // TODO: 일단은 아무것도 하지 않습니다.
        // 나중에 Git이 필수가 된다던지
        /*
        if (EditorUtility.DisplayDialog("Git Not Found",
            "Git is not installed or not in the system path. Git is required for this project. " +
            "Please install Git and restart Unity.", "Install Git", "Cancel"))
        {
            Process.Start("https://git-scm.com/downloads");
        }
        */
    }
}
