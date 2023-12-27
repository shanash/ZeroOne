using FluffyDuck.UI;
using FluffyDuck.Util;
using FluffyDuck.Memorial;
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

    private void Start()
    {
        var audio = AudioManager.Instance;

        List<string> audio_clip_list = new List<string>();
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");

        audio.PreloadAudioClipsAsync(audio_clip_list, null);

        Factory.Create<Producer>(10000200, MEMORIAL_TYPE.MAIN_LOBBY, Memorial_Parent);
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
        CommonUtils.ShowToast("캐릭터 리스트", TOAST_BOX_LENGTH.SHORT);
    }

    public void OnClickDeck()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Deck/PCDeckSettingUI", (popup) =>
        {
            popup.ShowPopup();
        });
    }

    public void OnClickHouse()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        CommonUtils.ShowToast("준비중 입니다.", TOAST_BOX_LENGTH.SHORT);
    }

    public void OnClickSearch()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        CommonUtils.ShowToast("준비중 입니다.", TOAST_BOX_LENGTH.SHORT);
    }
    public void OnClickShop()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        CommonUtils.ShowToast("준비중 입니다.", TOAST_BOX_LENGTH.SHORT);
    }

    public void OnClickMission()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        CommonUtils.ShowToast("미션 페이지 이동", TOAST_BOX_LENGTH.SHORT);
    }

    public void OnClickPlay()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        CommonUtils.ShowToast("임무 페이지 이동", TOAST_BOX_LENGTH.SHORT);
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
    }
    public void OnUIShowBegin()
    {
        Fade_In_Box.gameObject.SetActive(false);
    }
    #endregion
}
