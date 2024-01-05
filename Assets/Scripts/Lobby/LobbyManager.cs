using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LobbyManager : MonoBehaviour
{
    [SerializeField, Tooltip("Lobby Anim")]
    Animator Lobby_Anim;

    [SerializeField, Tooltip("Memorial Parent")]
    Transform Memorial_Parent;

    [SerializeField, Tooltip("Fade in Box")]
    RectTransform Fade_In_Box;

    Vector2 Press_Scale = new Vector2(0.96f, 0.96f);

    Producer pd;

    private void Start()
    {
        var audio = AudioManager.Instance;

        List<string> audio_clip_list = new List<string>();
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");

        audio.PreloadAudioClipsAsync(audio_clip_list, null);

        var pmng = PopupManager.Instance;
        pmng.SetRootOnEnter(LobbyRootOnEnter);
        pmng.SetRootOnExit(LobbyRootOnExit);

        pd = Factory.Create<Producer>(10000200, MEMORIAL_TYPE.MAIN_LOBBY, Memorial_Parent);
        GestureManager.Instance.Enable = false;
    }

    /// <summary>
    /// 로비 화면위의 모든 팝업이 사라졌을때 호출되는 함수
    /// </summary>
    void LobbyRootOnEnter()
    {
        Debug.Log("LobbyRootOnEnter");
        pd.Resume();
    }
    /// <summary>
    /// 로비 화면을 가리는 팝업이 생성되었을때 호출되는 함수
    /// </summary>
    void LobbyRootOnExit()
    {
        Debug.Log("LobbyRootOnExit");
        pd.Pause();
    }


    public void OnClickChangeCharacter()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Lobby/SelectLobbyCharacterPopup", (popup) =>
        {
            popup.ShowPopup();
        });
    }

    public void OnClickUIHide()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        Lobby_Anim.SetTrigger("fadeout");
    }

    public void OnClickUIShow()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        Lobby_Anim.SetTrigger("fadein");
    }

    public void OnClickCharacterList()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        //CommonUtils.ShowToast("캐릭터 리스트", TOAST_BOX_LENGTH.SHORT);

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "캐릭터 리스트 준비중 입니다.");
        });
    }

    public void OnClickDeck()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/PartySettingPopup", (popup) =>
        {
            popup.ShowPopup(GAME_TYPE.NONE, 0);
        });

    }

    public void OnClickHouse()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        //CommonUtils.ShowToast("준비중 입니다.", TOAST_BOX_LENGTH.SHORT);
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }

    public void OnClickSearch()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }
    public void OnClickShop()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }

    public void OnClickQuest()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }

    public void OnClickPlay()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Mission/MissionGateUI", (popup) =>
        {
            popup.ShowPopup();
        });
    }

    public void OnClickLeftMemorial()
    {
        CommonUtils.ShowToast("Left 메모리얼", TOAST_BOX_LENGTH.SHORT);
    }

    public void OnclickRightMemorial()
    {
        CommonUtils.ShowToast("Right 메모리얼", TOAST_BOX_LENGTH.SHORT);
    }

    #region UI Animation Events
    public void OnUIHideComplete()
    {
        Fade_In_Box.gameObject.SetActive(true);
        GestureManager.Instance.Enable = true;
    }

    public void OnUIShowBegin()
    {
        Fade_In_Box.gameObject.SetActive(false);
        GestureManager.Instance.Enable = false;
    }
    #endregion
}
