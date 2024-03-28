using FluffyDuck.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Addressable 로딩
/// </summary>
public class SceneLoad : SceneControllerBase
{
    /// <summary>
    /// 로딩팝업 최소 노출 보장시간
    /// </summary>
    const float SHOW_LOADINGPOPUP_MIN_DURATION = 8.0f;

    [SerializeField]
    Slider _UI_Progress = null;

    public static string Start_Scene_Name;
    SceneLoadingPopup Loading_Popup;    //  Assets/AssetResources/Prefabs/Popup/Modal/Loading/SceneLoadingPopup


    async void Start()
    {
        // 처음시간 체크
        float start_time = Time.time;

        List<string> popup_prefabs = new List<string>();
        popup_prefabs.Add("Assets/AssetResources/Prefabs/Popup/Modal/Loading/SceneLoadingPopup");
        popup_prefabs.Add("Assets/AssetResources/Prefabs/Popup/Modal/Loading/SceneLoadingPopup_Claire");
        popup_prefabs.Add("Assets/AssetResources/Prefabs/Popup/Modal/Loading/SceneLoadingPopup_Lucia");

        int r = Random.Range(0, popup_prefabs.Count);
        string prefab = popup_prefabs[r];

        PopupManager.Instance.Add(prefab, POPUP_TYPE.MODAL_TYPE, (popup) =>
        {
            popup.ShowPopup();
            Loading_Popup = (SceneLoadingPopup)popup;
        });

        float progress_min_value = 0f;

        // 로딩 종료 전까지는 느리게
        while (!MasterDataManager.Instance.IsLoaded)
        {
            await Task.Yield();
            SetProgressCallback(ref progress_min_value, 0);
        }

        // 만약 더 채울 게이지가 있다면 추가적으로 채워줍니다
        if (progress_min_value < 1)
        {
            // 처음부터 여기까지 걸린시간
            float past_time = Time.time - start_time;

            // 남은 시간 체크
            float remain_time = SHOW_LOADINGPOPUP_MIN_DURATION - past_time;

            // 남은시간 없으면 그래도 최소치(0.2초 정도?)는 보정해줍니다
            if (remain_time < 0)
            {
                remain_time = 0.2f;
            }

            float delta = 0f;
            while (delta < remain_time)
            {
                await Task.Yield();
                delta += Time.deltaTime;
                SetProgressCallback(ref progress_min_value, (delta / remain_time));
            }

            // 확실하게 풀로 채워준다
            SetProgressCallback(ref progress_min_value, 1);
        }

#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        SCManager.Instance.ChangeScene(SceneName.home);
#else
        SCManager.Instance.ChangeScene();
#endif
    }

    void SetProgressCallback(ref float progress_min_value, float progress)
    {
        
        if (!MasterDataManager.Instance.IsLoaded) 
        {
            progress_min_value += 0.001f;
            progress_min_value = Mathf.Clamp01(progress_min_value);
            Loading_Popup?.SetProgressCallback(progress_min_value);
        }
        else
        {
            float max_value = 1f - progress_min_value;
            float value = Mathf.Clamp01(progress * max_value + progress_min_value);
            Loading_Popup?.SetProgressCallback(value);
        }
        
    }
}

