#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RuntimeInitLoad
{
    static readonly Type GameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
    static readonly PropertyInfo ShowToolbarProperty = GameViewType.GetProperty("showToolbar", BindingFlags.Instance | BindingFlags.NonPublic);
    static readonly object False = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeSetup()
    {
        bool is_full_screen = EditorPrefs.GetBool(FluffyDuck.EditorUtil.UpperMenu.FluffyDuck.TogglePlayFullscreen_PrefKey, false);

        if (is_full_screen)
        {
            EditorWindow instance = (EditorWindow)ScriptableObject.CreateInstance(GameViewType);

            ShowToolbarProperty?.SetValue(instance, False);

            var desktopResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            var fullscreenRect = new Rect(Vector2.zero, desktopResolution);
            instance.ShowPopup();
            instance.position = fullscreenRect;
            instance.Focus();
        }

        var numScenes = SceneManager.sceneCountInBuildSettings;
        List<string> scenes_in_build_names = new List<string>(numScenes);
        for (int i = 0; i < numScenes; ++i)
        {
            scenes_in_build_names.Add(SceneManager.GetSceneByBuildIndex(i).name);
        }

        // 플레이 시작할 때 지금 열려 있는 Scene이
        var str_scene_name = SceneManager.GetActiveScene().name;

        // SceneName에는 정의되어 있는데
        if (Enum.TryParse(str_scene_name, out SceneName scene_name))
        {
            var index = scenes_in_build_names.FindIndex(x => x == str_scene_name);
            /*
            // Skill Editor때문에 만든녀석들
            // Scenes In Build에 포함되어 있지 않은 애들은
            // - 무조건 index가 6로 오게 된다. 현재 0 ~ 5까지 쓰고 있어서
            if (index.Equals(6))
            {
                // 로딩 후 지금 열려있는 Scene으로 다시 오게 해 준다.
                SCManager.I.Default_Scene = scene_name;
            }
            */

            // SceneName에 정의되어 있는 애들은 아무튼 로딩을 거치게 해 줍니다.
            _ = SCManager.I.LoadSceneImmidiatlyAsync(SceneName.title.ToString());
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AfterSetup()
    {
    }
}
#endif
