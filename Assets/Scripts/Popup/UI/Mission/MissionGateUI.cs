using FluffyDuck.UI;
using FluffyDuck.Util;

public class MissionGateUI : PopupBase
{

    protected override bool Initialize(object[] data)
    {
        FixedUpdatePopup();
        return true;
    }

    protected override void FixedUpdatePopup()
    {
        var board = BlackBoard.Instance;
        int open_dungeon_id = board.GetBlackBoardData<int>(BLACK_BOARD_KEY.OPEN_STORY_STAGE_DUNGEON_ID, 0);
        if (open_dungeon_id > 0)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Mission/SelectStageUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
            {
                var mng = GameData.Instance.GetUserStoryStageDataManager();

                popup.ShowPopup(mng.GetCurrentWorldID(), mng.GetCurrentZoneID());
            });
        }
    }


    public void OnClickMainMission()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Mission/SelectStageUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
        {
            var mng = GameData.Instance.GetUserStoryStageDataManager();

            popup.ShowPopup(mng.GetCurrentWorldID(), mng.GetCurrentZoneID());
        });
    }

    public void OnClickSpecialMission()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }
    public void OnClickSearch()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }
    public void OnClickDungeon()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }
    public void OnClickClanBattle()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }
    public void OnClickBossBattle()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }
}
