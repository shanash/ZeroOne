using Cysharp.Threading.Tasks;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;
using ZeroOne.Input;


public class LobbyManager : SceneControllerBase
{
    [SerializeField, Tooltip("Lobby Anim")]
    Animator Lobby_Anim;

    [SerializeField, Tooltip("Memorial Parent")]
    Transform Memorial_Parent;

    [SerializeField, Tooltip("Fade in Box")]
    RectTransform Fade_In_Box;

    Producer pd = null;

    protected override void Initialize()
    {
        var audio = AudioManager.Instance;

        List<string> audio_clip_list = new List<string>();
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");

        audio.PreloadAudioClipsAsync(audio_clip_list, null);

        var pmng = PopupManager.Instance;
        pmng.SetRootOnEnter(() => pd.Resume());
        pmng.SetRootOnExit(() => pd.Pause());

        pd = Factory.Instantiate<Producer>(1010051, MEMORIAL_TYPE.MAIN_LOBBY, Memorial_Parent);
        GestureManager.Instance.Enable = false;

        _ = InitializeAsync();
    }

    async UniTask InitializeAsync()
    {
        await UniTask.WaitUntil(() => pd.Is_Init);

        var board = BlackBoard.Instance;
        int open_dungeon_id = board.GetBlackBoardData<int>(BLACK_BOARD_KEY.OPEN_STORY_STAGE_DUNGEON_ID, 0);
        if (open_dungeon_id > 0)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Mission/MissionGateUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
            {
                popup.ShowPopup();
            });
        }

        SCManager.Instance.SetCurrent(this);
    }

    public override void OnClick(UIButtonBase button)
    {
        base.OnClick(button);

        switch (button.name)
        {
            case "ChageCharacterBtn":
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Lobby/SelectLobbyCharacterPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
                {
                    popup.ShowPopup();
                });
                break;
            case "CharacterBtn":
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Hero/HeroListUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
                {
                    popup.ShowPopup();
                });
                break;
            case "LeftMemorialBtn":
                CommonUtils.ShowToast("Left 메모리얼", TOAST_BOX_LENGTH.SHORT);
                break;
            case "RightMemorialBtn":
                CommonUtils.ShowToast("Right 메모리얼", TOAST_BOX_LENGTH.SHORT);
                break;
            case "PlayBtn":
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Mission/MissionGateUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
                {
                    popup.ShowPopup();
                });
                break;
            case "UIHideBtn":
                Lobby_Anim.SetTrigger("fadeout");
                break;
            case "UIShowBtn":
                Lobby_Anim.SetTrigger("fadein");
                break;
            case "PartyBtn":
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/PartySettingPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
                {
                    popup.ShowPopup(GAME_TYPE.NONE, 0);
                });
                break;
            case "HouseBtn":
            case "MissionBtn":
            case "ShopBtn":
            case "SearchBtn":
                ShowNotYetNoti();
                break;
        }
    }

    void ShowNotYetNoti()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
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
