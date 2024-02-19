using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RuntimeInitLoad
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeSetup()
    {
        var numScenes = SceneManager.sceneCountInBuildSettings;
        List<string> scenes_in_build_names = new List<string>(numScenes);
        for (int i = 0; i < numScenes; ++i)
        {
            Debug.Log($"SceneManager.GetSceneByBuildIndex(i).name : {SceneManager.GetSceneByBuildIndex(i).path}");
            scenes_in_build_names.Add(SceneManager.GetSceneByBuildIndex(i).name);
        }

        // 플레이 시작할 때 지금 열려 있는 Scene이
        var str_scene_name = SceneManager.GetActiveScene().name;

        // SceneName에는 정의되어 있는데
        if (Enum.TryParse(str_scene_name, out SceneName scene_name))
        {
            var index = scenes_in_build_names.FindIndex(x => x == str_scene_name);
            // Scenes In Build에 포함되어 있지 않은 애들은
            if (index.Equals(4))
            {
                // 로딩 후 지금 열려있는 Scene으로 다시 오게 해 준다.
                SCManager.I.Default_Scene = scene_name;
            }

            // SceneName에 정의되어 있는 애들은 아무튼 로딩을 거치게 해 줍니다.
            _ = SCManager.I.LoadSceneImmidiatlyAsync(SceneName.title.ToString());
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AfterSetup()
    {
    }
}
