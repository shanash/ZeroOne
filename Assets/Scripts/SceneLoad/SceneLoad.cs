using FluffyDuck.Addressable;
using FluffyDuck.Util;
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

        while (!MasterDataManager.Instance.IsLoaded)
        {
            await Task.Yield();
        }
        string first_scene = "home";
#if UNITY_EDITOR
        List<string> scene_list = new List<string>();
        scene_list.Add("title");
        scene_list.Add("load");
        scene_list.Add("home");
        scene_list.Add("battle");
        scene_list.Add("memorial");

        if (!scene_list.Contains(Start_Scene_Name))
        {
            first_scene = Start_Scene_Name;
            if (first_scene.Equals("skill_preview"))
            {
                BlackBoard.Instance.SetBlackBoard(BLACK_BOARD_KEY.GAME_TYPE, GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE);
            }
            else if (first_scene.Equals("skill_editor"))
            {
                BlackBoard.Instance.SetBlackBoard(BLACK_BOARD_KEY.GAME_TYPE, GAME_TYPE.EDITOR_SKILL_EDIT_MODE);
            }
            
        }
#endif

        SCManager.Instance.ChangeScene(SceneName.home);
    }
}

