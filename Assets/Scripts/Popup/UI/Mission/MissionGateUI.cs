using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using UnityEngine;

public class MissionGateUI : PopupBase
{
    [SerializeField, Tooltip("Boss Dungeon Btn")]
    UIButtonBase Boss_Dungeon_Btn;
    [SerializeField, Tooltip("Boss Dungeon Lock")]
    RectTransform Boss_Dungeon_Lock;
    

    protected override bool Initialize(object[] data)
    {
        FixedUpdatePopup();
        return true;
    }

    protected override void FixedUpdatePopup()
    {
        var gd = GameData.Instance;

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

        //  boss dungeon check
        var boss_mng = gd.GetUserBossStageDataManager();
        Boss_Dungeon_Lock.gameObject.SetActive(!boss_mng.IsBossDungeonOpen());
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
            popup.ShowPopup(3f, GameDefine.GetLocalizeString("system_alert_preparing"));
        });
    }
    public void OnClickSearch()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, GameDefine.GetLocalizeString("system_alert_preparing"));
        });
    }
    public void OnClickDungeon()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, GameDefine.GetLocalizeString("system_alert_preparing"));
        });
    }
    public void OnClickClanBattle()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, GameDefine.GetLocalizeString("system_alert_preparing"));
        });
    }
    public void OnClickBossBattle()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        var boss_mng = GameData.Instance.GetUserBossStageDataManager();
        if (!boss_mng.IsBossDungeonOpen())
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
            {
                var stage = MasterDataManager.Instance.Get_StageData(boss_mng.GetNeedClearDungeonStageID());
                string msg = ZString.Format(GameDefine.GetLocalizeString("system_warnning_boss_enter_01"), GameDefine.GetLocalizeString(stage.stage_name_id));
                popup.ShowPopup(3f, msg);
            });
            return;
        }

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/BossDungeon/BossStageEntryUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
        {
            popup.ShowPopup();
        });
    }
}
