#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AndroidPlatformSetter
{
    static AndroidPlatformSetter()
    {
        EditorApplication.delayCall += SetAndroidPlatform;
    }

    static void SetAndroidPlatform()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            Debug.Log("Setting platform to Android");
            if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android))
            {
                EditorUtility.DisplayDialog("Error", $"Android Build Support 모듈이 설치되지 않았습니다.\n 모듈 설치 후에 다시 프로젝트를 열어 주세요.", "OK");
            }
        }
    }
}
#endif
