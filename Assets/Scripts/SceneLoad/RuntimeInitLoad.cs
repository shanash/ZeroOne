using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RuntimeInitLoad
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void BeforeSetup() 
    {
        List<string> scene_list = new List<string>();
        scene_list.Add("load");
        scene_list.Add("home");
        scene_list.Add("battlev2");
        scene_list.Add("memorial");

        string scene_name = SceneManager.GetActiveScene().name;

        //if (!SceneManager.GetActiveScene().name.Equals(GameDefine.SCENE_LOAD))
        if (scene_list.Contains(scene_name))
        {
            // 일단 개발이 불편해서 이렇게 임시로 해놓습니다
            // 씬 이동 히스토리가 있으면 이렇게 할 필요가 없을것 같은데
            // TODO: 씬 이동 관리가 있으면 좋을 것 같습니다.
            SceneLoad.Start_Scene_Name = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(GameDefine.SCENE_LOAD);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AfterSetup()
    {
    }
}
