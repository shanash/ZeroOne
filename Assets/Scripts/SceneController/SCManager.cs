using UnityEngine;
using FluffyDuck.Util;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;

public enum SceneName
{
    None = 0,
    title,
    load,
    home,
    battle,
    memorial,
}

public class SCManager : Singleton<SCManager>
{
    #region Variable Members
    private bool m_isChanging = false;
    private SceneName m_curSceneName = SceneName.None;
    #endregion

    public SceneControllerBase CurController { get; private set; }

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
    public void SetCurrent(SceneControllerBase current)
    {
        CurController = current;
        Debug.Assert(Enum.TryParse(SceneManager.GetActiveScene().name, out m_curSceneName), $"enum SceneName에 {SceneManager.GetActiveScene().name}이 정의되어 있지 않습니다.");
    }

    public void ChangeScene(SceneName scene_name/*, string detailPage = ""*/)
    {
        if (m_isChanging)
            return;

        if (scene_name.ToString().Equals(m_curSceneName))
            return;

        // TODO: 올바른 scene이름인지 확인합니다.
        _ = ChangeSceneAsync(scene_name/*, detailPage*/);
    }

    private async UniTask ChangeSceneAsync(SceneName sceneName/*, string detailPage*/)
    {
        m_isChanging = true;

        await ScreenEffectManager.Instance.StartActionAsync(ScreenEffect.FADE_OUT, null, 0.3f);

        await LoadSceneAsync(sceneName.ToString());

        await Resources.UnloadUnusedAssets();
        System.GC.Collect();

        // SetCurrent가 호출될때까지 기다립니다
        await UniTask.WaitUntil(() => sceneName == m_curSceneName);

        await ScreenEffectManager.Instance.StartActionAsync(ScreenEffect.FADE_IN, null, 0.3f);

        m_isChanging = false;
    }

    async UniTask LoadSceneAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            await UniTask.Yield();

            // TODO: 로딩 이미지는 이 수치로 업데이트 하면 됩니다
            //Debug.Log($"op.progress : {op.progress}");
            if (op.progress >= 0.9f)
            {
               op.allowSceneActivation = true;
               return;
            }
        }

        SceneManager.LoadScene(sceneName);
    }
}
