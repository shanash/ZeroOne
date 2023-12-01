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
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }
    }
}
#endif