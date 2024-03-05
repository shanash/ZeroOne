using FluffyDuck.Addressable;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Addressable 예시를 위해 만든 에셋 다운로드 스크립트
/// </summary>
public class SceneLoad : SceneControllerBase
{
    AddressableContentDownloader _Downloader = new AddressableContentDownloader();

    [SerializeField]
    TMP_Text _UI_Text = null;
    [SerializeField]
    Slider _UI_Progress = null;

    public static string Start_Scene_Name;
    SceneLoadingPopup Loading_Popup;    //  Assets/AssetResources/Prefabs/Popup/Modal/Loading/SceneLoadingPopup

    protected override void Initialize()
    {
        SCManager.Instance.SetCurrent(this);
    }

    void SetText(string text)
    {
        if (_UI_Text != null)
        {
            _UI_Text.text = text;
        }
    }

    void SetProgress(float progress)
    {
        if (_UI_Progress != null)
        {
            _UI_Progress.value = progress;
        }
    }

    void OnProgressDownload(string group_key, float progress, double size)
    {
        SetText($"{group_key} : {progress * size:F2} / {size:F2}");
        SetProgress(progress);
    }

    async void Start()
    {
        SetText("다운로더 초기화중입니다.");
        if (!await _Downloader.Init())
        {
            SetText("초기화 실패");
            return;
        }
        SetText("버전 확인중입니다.");
        if (_Downloader.HasNewDataVersion())
        {
            _UI_Progress.gameObject.SetActive(true);
            SetText("다운로드를 시작합니다.");
            _Downloader.AddDownloadProgressHandler(OnProgressDownload);
            await _Downloader.Download();
            await Task.Delay(500);
        }
        SetText("시작 준비 완료");

        Progress_Min_Value = 0f;
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Modal/Loading/SceneLoadingPopup", POPUP_TYPE.MODAL_TYPE, (popup) =>
        {
            Loading_Popup = (SceneLoadingPopup)popup;
            Loading_Popup.ShowPopup();

        });

        while (!MasterDataManager.Instance.IsLoaded)
        {
            await Task.Yield();
            SetProgressCallback(0);
        }

        float duration = 2f;
        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            await Task.Delay(100);
            SetProgressCallback(1f - (duration / 2f));
        }

#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        SCManager.Instance.ChangeScene(SceneName.home);
#else
        SCManager.Instance.ChangeScene();
#endif

    }

    float Progress_Min_Value = 0f;
    void SetProgressCallback(float progress)
    {
        
        if (!MasterDataManager.Instance.IsLoaded) 
        {
            Progress_Min_Value += 0.001f;
            Loading_Popup?.SetProgressCallback(Progress_Min_Value);
        }
        else
        {
            float max_value = 1f - Progress_Min_Value;
            float value = Mathf.Clamp01(progress * max_value + Progress_Min_Value);
            Loading_Popup?.SetProgressCallback(value);
        }
        
    }
}

