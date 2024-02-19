using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RuntimeInitLoad
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeSetup()
    {
        _ = SCManager.I.LoadSceneImmidiatlyAsync(SceneName.title.ToString());
        /*
        var str_scene_name = SceneManager.GetActiveScene().name;
        if (Enum.TryParse(str_scene_name, out SceneName scene_name))
        {
            SCManager.I.Default_Scene = scene_name;
            _ = SCManager.I.LoadSceneImmidiatlyAsync(SceneName.title.ToString());
        }
        */
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AfterSetup()
    {
    }
}
