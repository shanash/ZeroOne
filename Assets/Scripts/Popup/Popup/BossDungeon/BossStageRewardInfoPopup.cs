using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStageRewardInfoPopup : PopupBase
{
    [SerializeField, Tooltip("Boss Stage Name")]
    TMP_Text Boss_Stage_Name;
    [SerializeField, Tooltip("Limit Time")]
    TMP_Text Limit_Time;

    [SerializeField, Tooltip("First Reward Tag")]
    TMP_Text First_Reward_Tag;
    [SerializeField, Tooltip("First Reward List View")]
    ScrollRect First_Reward_List_View;

    [SerializeField, Tooltip("Repeat Reward Tag")]
    TMP_Text Repeat_Reward_Tag;
    [SerializeField, Tooltip("Repeat Reward List View")]
    ScrollRect Repeat_Reward_List_View;

    [SerializeField, Tooltip("Entrance Btn")]
    UIButtonBase Entrance_Btn;

    List<RewardItemCard> Used_Reward_Item_Card_List = new List<RewardItemCard>();

    Boss_Stage_Data Stage;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1)
        {
            return false;
        }
        Stage = (Boss_Stage_Data)data[0];

        InitAssets();
        return true;
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/UI/Card/RewardItemCard");
        asset_list.Add("Assets/AssetResources/Prefabs/UI/ItemTooltip");

        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);
    }
    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (load_cnt >= total_cnt)
        {
            FixedUpdatePopup();
        }
    }

    protected override void FixedUpdatePopup()
    {
        ClearRewardItemListNode();
        var m = MasterDataManager.Instance;
        var pool = GameObjectPoolManager.Instance;
        //  stage name
        Boss_Stage_Name.text = GameDefine.GetLocalizeString(Stage.stage_name);

        //  제한 시간 - Stage를 구성하는 wave 제한 시간의 합산
        var wave_list = m.Get_WaveDataList(Stage.wave_group_id);
        int sum_limit_time = 0;
        for (int i = 0; i < wave_list.Count; i++)
        {
            sum_limit_time += wave_list[i].wave_time;
        }
        var time = TimeSpan.FromSeconds(sum_limit_time);
        Limit_Time.text = ZString.Format("제한시간 {0:D2}:{1:D2}", time.Minutes, time.Seconds);

        //  리워드
        string reward_prefab = "Assets/AssetResources/Prefabs/UI/Card/RewardItemCard";

        //  first reward
        var first_reward_data_list = m.Get_RewardSetDataList(Stage.first_reward_group_id);
        int cnt = first_reward_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var reward_data = first_reward_data_list[i];
            if (!reward_data.is_use)
            {
                continue;
            }
            var obj = pool.GetGameObject(reward_prefab, First_Reward_List_View.content);
            var item = obj.GetComponent<RewardItemCard>();
            item.InitializeData(reward_data, RewardItemCallback);
            item.SetScale(Vector2.one);
            Used_Reward_Item_Card_List.Add(item);
        }

        //  repeat reward
        var repeat_reward_data_list = m.Get_RewardSetDataList(Stage.repeat_reward_group_id);
        cnt = repeat_reward_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var reward_data = repeat_reward_data_list[i];
            if (!reward_data.is_use)
            {
                continue;
            }
            var obj = pool.GetGameObject(reward_prefab, Repeat_Reward_List_View.content);
            var item = obj.GetComponent<RewardItemCard>();
            item.InitializeData(reward_data, RewardItemCallback);
            item.SetScale(Vector2.one);
            Used_Reward_Item_Card_List.Add(item);
        }

        var boss_mng = GameData.Instance.GetUserBossStageDataManager();
        Entrance_Btn.interactable = boss_mng.IsEnableEntranceBossDungeon();
    }


    void ClearRewardItemListNode()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Reward_Item_Card_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Reward_Item_Card_List[i].gameObject);
        }
        Used_Reward_Item_Card_List.Clear();
    }

    public void OnClickBossStageEntrance()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        var boss_mng = GameData.Instance.GetUserBossStageDataManager();
        var user_dungeon = boss_mng.FindUserBossDungeonData(Stage.boss_stage_id);
        if (user_dungeon == null)
        {
            return;
        }
        //  보스 던전 입장권이 남아있다면 true
        if (!boss_mng.IsEnableEntranceBossDungeon())
        {
            CommonUtils.ShowToast("입장 횟수를 모두 소진하였습니다.", TOAST_BOX_LENGTH.SHORT);
            return;
        }

        HidePopup(() =>
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/PartySettingPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup(GAME_TYPE.BOSS_DUNGEON_MODE, Stage.boss_stage_id);
            });
        });
    }

    public void RewardItemCallback(TOUCH_RESULT_TYPE result, System.Func<bool, Rect> hole, object reward_data_obj)
    {
        switch (result)
        {
            case TOUCH_RESULT_TYPE.LONG_PRESS:
                if (reward_data_obj == null || reward_data_obj is not RewardDataBase)
                {
                    Debug.LogWarning("표시 가능한 보상정보가 없습니다!");
                    return;
                }
                RewardDataBase reward_data = reward_data_obj as RewardDataBase;
                TooltipManager.I.Add("Assets/AssetResources/Prefabs/UI/ItemTooltip", hole(false), reward_data);
                break;
        }
    }

    public void OnClickDim()
    {
        if (Ease_Base != null && Ease_Base.IsPlaying())
        {
            return;
        }
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }
}
