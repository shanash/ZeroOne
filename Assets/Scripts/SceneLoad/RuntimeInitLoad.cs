using UnityEngine;
using UnityEngine.SceneManagement;

public static class RuntimeInitLoad
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeSetup()
    {
        SceneLoad.Start_Scene_Name = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync(SceneName.title.ToString());
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AfterSetup()
    {
    }
}
