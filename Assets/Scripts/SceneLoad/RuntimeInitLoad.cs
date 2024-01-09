using UnityEngine;
using UnityEngine.SceneManagement;

public static class RuntimeInitLoad
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeSetup()
    {
        SceneLoad.Start_Scene_Name = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync(GameDefine.SCENE_TITLE);

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AfterSetup()
    {
    }
}
