using UnityEngine;
using FluffyDuck.Util;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public enum SceneName
{
    None = 0,
    title,
    load,
    home,
    essence,
    dialogue_Intro,
    battle,
    dialogue_StoryScene1_1,
    dialogue_StoryScene1_2,
    dialogue_StoryScene2_1,
    dialogue_StoryScene2_2,
    dialogue_StoryScene3,
    dialogue_StoryScene4,
    dialogue_StoryScene5,
    dialogue_StoryScene6,
    dialogue_StoryScene7
}

public class SCManager : Singleton<SCManager>
{
    #region Variable Members
    bool Is_Changing = false;
    SceneName Current_SceneName = SceneName.None;

    string[] Callback_Method_Names = null;
    #endregion

    public SceneControllerBase Current_Controller { get; private set; }
    public SceneName Default_Scene { get; set; } = SceneName.home;

    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CallInstance()
    {
        _ = Instance;
    }

    SCManager() { }

    protected override void Initialize() { }

    /// <summary>
    /// Scene Controller 측에서 초기화 종료 후에 호출해줘야 하는 함수
    /// 호출하지 않으면 ChangeSceneAsync가 종료되지 않는다 
    /// </summary>
    /// <param name="current"></param>
    public void SetCurrent(SceneControllerBase current, params string[] callback_method_names)
    {
        Debug.Log("Start SetCurrent");
        Current_Controller = current;
        Debug.Log($"SetCurrent {Current_SceneName}");
        Callback_Method_Names = callback_method_names;
        if (!Enum.TryParse(SceneManager.GetActiveScene().name, out Current_SceneName))
        {
            Debug.Assert(false, $"enum SceneName에 {SceneManager.GetActiveScene().name}이 정의되어 있지 않습니다.");
        }
    }

    public void ChangeScene(string scene_name, params object[] parameters)
    {
        if (Enum.TryParse(scene_name, out SceneName sname))
        {
            ChangeScene(sname, parameters);
        }
    }

    public void ChangeScene(SceneName scene_name = SceneName.None, params object[] parameters)
    {
        if (Is_Changing)
            return;

        if (scene_name == SceneName.None)
        {
            scene_name = Default_Scene;

            // TODO: 이 세팅을 여기에 둬야 할까요?.....
            //if (Default_Scene.Equals(SceneName.skill_preview))
            //{
            //    BlackBoard.Instance.SetBlackBoard(BLACK_BOARD_KEY.GAME_TYPE, GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE);
            //}
            //else if (Default_Scene.Equals(SceneName.skill_editor))
            //{
            //    BlackBoard.Instance.SetBlackBoard(BLACK_BOARD_KEY.GAME_TYPE, GAME_TYPE.EDITOR_SKILL_EDIT_MODE);
            //}
        }

        if (scene_name.ToString().Equals(Current_SceneName))
            return;

        // TODO: 올바른 scene이름인지 확인합니다.
        _ = ChangeSceneAsync(scene_name, parameters);
    }

    async UniTask ChangeSceneAsync(SceneName sceneName, params object[] parameters)
    {
        Is_Changing = true;

        //await ScreenEffectManager.Instance.StartActionAsync(ScreenEffect.FADE_OUT, null, 0.3f);

        await LoadSceneImmidiatlyAsync(sceneName.ToString());

        await Resources.UnloadUnusedAssets();
        GC.Collect();

        await UniTask.WaitUntil(() => sceneName == Current_SceneName);

        if (parameters.Length > 0)
        {
            InvokeOnReceiveData(Current_Controller, Callback_Method_Names, parameters);
        }

        //await ScreenEffectManager.Instance.StartActionAsync(ScreenEffect.FADE_IN, null, 0.3f);

        Is_Changing = false;
    }


    public async UniTask LoadSceneImmidiatlyAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        //while (!op.isDone)
        //{
        //    await UniTask.Yield();

        //    // TODO: 로딩 이미지는 이 수치로 업데이트 하면 됩니다
        //    //Debug.Log($"op.progress : {op.progress}");
        //    Progress_Callback?.Invoke(op.progress);
        //    if (op.progress >= 0.9f)
        //    {
        //        //op.allowSceneActivation = true;
        //        break;
        //    }
        //}
        do 
        {
            // TODO: 로딩 이미지는 이 수치로 업데이트 하면 됩니다
            //Debug.Log($"op.progress : {op.progress}");
            if (op.progress >= 0.9f)
            {
                //op.allowSceneActivation = true;
                break;
            }

            await UniTask.Yield();
        } while (!op.isDone);
        op.allowSceneActivation = true;
        
    }

    private void InvokeOnReceiveData(SceneControllerBase controller, string[] callback_method_names, object[] parameters)
    {
        // 현재 컨트롤러에서 모든 메소드를 가져옵니다.
        var methods = controller.GetType().GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        
        foreach (var method in methods)
        {
            if (callback_method_names.Contains(method.Name))
            {
                if (method.GetParameters().Length == parameters.Length)
                {
                    // 파라미터 타입이 일치하는지 확인합니다.
                    var paramInfos = method.GetParameters();
                    bool match = true;
                    for (int i = 0; i < paramInfos.Length; i++)
                    {
                        if (!paramInfos[i].ParameterType.IsAssignableFrom(parameters[i].GetType()))
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        // 일치하는 메소드를 찾았으므로 호출합니다.
                        /*var result = */method.Invoke(controller, parameters);
                        
                        //if (result is bool)
                        //{
                            // 결과 처리 (필요한 경우)
                        //    Debug.Log("OnReceiveData 호출 성공: " + success);
                        //}

                        return; // 메소드를 호출했으므로 반복문을 종료합니다.
                    }
                }
            }
        }

        // 일치하는 메소드를 찾지 못한 경우의 처리
        Debug.LogWarning("적절한 OnReceiveData 메소드를 찾지 못했습니다.");
    }
}
