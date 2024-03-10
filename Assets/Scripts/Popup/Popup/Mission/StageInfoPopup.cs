using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoPopup : PopupBase
{
    [Space()]
    [SerializeField, Tooltip("Stage Number")]
    TMP_Text Stage_Number;

    [SerializeField, Tooltip("Stage Name")]
    TMP_Text Stage_Name;

    [SerializeField, Tooltip("Star Points")]
    List<Image> Stage_Star_Points;

    [SerializeField, Tooltip("Action Point Icon")]
    Image Action_Point_Icon;

    [SerializeField, Tooltip("Aciton Point Current")]
    TMP_Text Action_Point_Current;

    [SerializeField, Tooltip("Action Point Used")]
    TMP_Text Action_Point_Used;

    [SerializeField, Tooltip("Entrance Btn")]
    UIButtonBase Entrance_Btn;

    [SerializeField, Tooltip("Npc List View")]
    ScrollRect Npc_List_View;

    [SerializeField, Tooltip("First Reward List View")]
    ScrollRect First_Reward_List_View;

    [SerializeField, Tooltip("Repeat Reward List View")]
    ScrollRect Repeat_Reward_List_View;

    [SerializeField, Tooltip("Current Stamina")]
    TMP_Text Current_Stamina;
    [SerializeField, Tooltip("Used Stamina")]   //  2B8FFF
    TMP_Text Used_Stamina;

    [SerializeField, Tooltip("일반 스테이지 진입 버튼 정보 박스")]
    RectTransform Normal_Stage_Btn_Box;
    [SerializeField, Tooltip("일반 스테이지 진입 버튼 텍스트")]
    TMP_Text Normal_Stage_Btn_Text;

    [SerializeField, Tooltip("하드 스테이지 진입 버튼 정보 박스")]
    RectTransform Hard_Stage_Btn_Box;
    [SerializeField, Tooltip("하드 스테이지 남은 횟수 타이틀")]
    TMP_Text Hard_Stage_Remain_Daily_Count_Title;
    [SerializeField, Tooltip("하드 스테이지 남은 횟수")]
    TMP_Text Hard_Stage_Remain_Daily_Count;

    World_Data World;
    Zone_Data Zone;
    Stage_Data Stage;
    UserStoryStageData User_Data;

    List<NpcCardBase> Used_Npc_List = new List<NpcCardBase>();
    List<RewardItemCard> Used_Reward_Item_List = new List<RewardItemCard>();

    // 현재 표시되고 있는 몬스터 툴팁
    GameObject Tooltip = null;

    bool Is_Animation_End;
    bool Is_Load_Complete;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 3)
        {
            return false;
        }
        World = (World_Data)data[0];
        Zone = (Zone_Data)data[1];
        Stage = (Stage_Data)data[2];
        User_Data = GameData.Instance.GetUserStoryStageDataManager().FindUserStoryStageData(Stage.stage_id);
        InitAssets();
        UpdatePopup();
        return true;
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/UI/Card/NpcCard");
        asset_list.Add("Assets/AssetResources/Prefabs/UI/Card/RewardItemCard");
        asset_list.Add("Assets/AssetResources/Prefabs/UI/MonsterTooltip");
        asset_list.Add("Assets/AssetResources/Prefabs/UI/CommonTooltip");

        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);
    }

    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (load_cnt == total_cnt)
        {
            Is_Load_Complete = true;
            if (Is_Load_Complete && Is_Animation_End)
            {
                FixedUpdatePopup();
                return;
            }
        }
    }

    protected override void ShowPopupAniEndCallback()
    {
        Is_Animation_End = true;
        if (Is_Animation_End && Is_Load_Complete)
        {
            FixedUpdatePopup();
            UpdatePopup();
        }

    }

    protected override void HidePopupAniEndCallback()
    {
        base.HidePopupAniEndCallback();
    }


    protected override void FixedUpdatePopup()
    {
        var m = MasterDataManager.Instance;
        var pool = GameObjectPoolManager.Instance;

        Stage_Number.text = ZString.Format("{0}-{1}", Zone.zone_ordering, Stage.stage_ordering);

        Stage_Name.text = GameDefine.GetLocalizeString(Stage.stage_name_id);

        int cnt = 0;
        //  npc list
        string npc_prefab = "Assets/AssetResources/Prefabs/UI/Card/NpcCard";
        var wave_data_list = m.Get_WaveDataList(Stage.wave_group_id);

        List<BattleNpcData> npc_data_list = new List<BattleNpcData>();

        cnt = wave_data_list.Count;
        //  npc 데이터 정보를 중복없이 리스트에 정리
        for (int i = 0; i < cnt; i++)
        {
            var wave = wave_data_list[i];
            for (int e = 0; e < wave.enemy_appearance_info.Length; e++)
            {
                int npc_id = wave.enemy_appearance_info[e];
                int npc_lv = wave.npc_levels[e];
                int stat_id = wave.npc_stat_ids[e];
                int skill_lv = wave.npc_skill_levels[e];
                int ultimate_lv = wave.npc_ultimate_skill_levels[e];

                if (!npc_data_list.Exists(x => x.GetUnitID() == npc_id))
                {
                    var new_data = new BattleNpcData();
                    new_data.SetUnitID(npc_id, npc_lv, stat_id, skill_lv, ultimate_lv);
                    npc_data_list.Add(new_data);
                }
            }
        }
        //  접근 사거리 기준으로 짧은 순으로 오름 차순 정렬
        npc_data_list.Sort((a, b) => a.GetApproachDistance().CompareTo(b.GetApproachDistance()));
        cnt = npc_data_list.Count;
        for (int n = 0; n < cnt; n++)
        {
            var npc = npc_data_list[n];
            var obj = pool.GetGameObject(npc_prefab, Npc_List_View.content);
            var npc_card = obj.GetComponent<NpcCardBase>();
            npc_card.SetNpcID(npc.GetUnitID());
            npc_card.TooltipButton.Touch_Tooltip_Callback.AddListener(TouchEventCallback);
            Used_Npc_List.Add(npc_card);
        }

        //  리워드
        string reward_prefab = "Assets/AssetResources/Prefabs/UI/Card/RewardItemCard";

        //  first reward list
        var f_reward_data_list = m.Get_RewardSetDataList(Stage.first_reward_group_id);
        cnt = f_reward_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var reward_data = f_reward_data_list[i];
            if (!reward_data.is_use)
            {
                continue;
            }
            var obj = pool.GetGameObject(reward_prefab, First_Reward_List_View.content);
            var reward_item = obj.GetComponent<RewardItemCard>();
            reward_item.InitializeData(reward_data, RewardItemCallback);
            Used_Reward_Item_List.Add(reward_item);
        }

        //  star reward list
        var star_reward_data_list = m.Get_RewardSetDataList(Stage.star_reward_group_id);
        cnt = star_reward_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var reward_data = star_reward_data_list[i];
            if (!reward_data.is_use)
            {
                continue;
            }
            var obj = pool.GetGameObject(reward_prefab, First_Reward_List_View.content);
            var reward_item = obj.GetComponent<RewardItemCard>();
            reward_item.InitializeData(reward_data, RewardItemCallback);
            Used_Reward_Item_List.Add(reward_item);
        }


        //  repeat reward list
        var repeat_reward_data_list = m.Get_RewardSetDataList(Stage.repeat_reward_group_id);
        cnt = repeat_reward_data_list.Count;
        for(int i = 0;i < cnt;i++)
        {
            var reward_data = repeat_reward_data_list[i];
            if (!reward_data.is_use)
            {
                continue;
            }
            var obj = pool.GetGameObject(reward_prefab, Repeat_Reward_List_View.content);
            var reward_item = obj.GetComponent<RewardItemCard>();
            reward_item.InitializeData(reward_data, RewardItemCallback);
            Used_Reward_Item_List.Add(reward_item);
        }

        ////  stamina
        //int need_stamina = Stage.use_stamina;
        //var stamina_item = GameData.Instance.GetUserChargeItemDataManager().FindUserChargeItemData(REWARD_TYPE.STAMINA);

        //Current_Stamina.text = stamina_item.GetCount().ToString("N0");

        //int used_stamina_count = stamina_item.GetCount() - need_stamina;

        //if (stamina_item.IsUsableChargeItemCount(need_stamina))
        //{
        //    Used_Stamina.text = ZString.Format("<color=#2B8FFF>{0:N0}</color>", used_stamina_count);
        //}
        //else
        //{
        //    Used_Stamina.text = ZString.Format("<color=#ff0000>{0:N0}</color>", used_stamina_count);
        //}

        //if (User_Data != null)
        //{
        //    MarkStarPoint(User_Data.GetStarPoint());
        //}
        //else
        //{
        //    MarkStarPoint(0);
        //}
    }

    public override void UpdatePopup()
    {
        //  stamina
        int need_stamina = Stage.use_stamina;
        var stamina_item = GameData.Instance.GetUserChargeItemDataManager().FindUserChargeItemData(REWARD_TYPE.STAMINA);

        Current_Stamina.text = stamina_item.GetCount().ToString("N0");

        int used_stamina_count = stamina_item.GetCount() - need_stamina;

        if (stamina_item.IsUsableChargeItemCount(need_stamina))
        {
            Used_Stamina.text = ZString.Format("<color=#2B8FFF>{0:N0}</color>", used_stamina_count);
        }
        else
        {
            Used_Stamina.text = ZString.Format("<color=#ff0000>{0:N0}</color>", used_stamina_count);
        }

        if (User_Data != null)
        {
            MarkStarPoint(User_Data.GetStarPoint());
        }
        else
        {
            MarkStarPoint(0);
        }
    }

    void MarkStarPoint(int pt)
    {
        int cnt = Stage_Star_Points.Count;
        for (int i = 0; i < cnt; i++)
        {
            Stage_Star_Points[i].gameObject.SetActive(false);
        }
        if (pt >= cnt)
        {
            pt = cnt;
        }
        for (int i = 0; i < pt; i++)
        {
            Stage_Star_Points[i].gameObject.SetActive(true);
        }
    }

    public void TouchEventCallback(TOUCH_RESULT_TYPE result, System.Func<bool, Rect> hole, object data)
    {
        switch (result)
        {
            case TOUCH_RESULT_TYPE.LONG_PRESS:
                if (data == null || data is not Npc_Data)
                {
                    Debug.LogWarning("표시 가능한 적정보가 없습니다!");
                    return;
                }
                Npc_Data npc_data = data as Npc_Data;
                Tooltip = GameObjectPoolManager.Instance.GetGameObject("Assets/AssetResources/Prefabs/UI/MonsterTooltip", transform.parent);
                var tooltip = Tooltip.GetComponent<MonsterTooltip>();
                tooltip.Initialize(hole(false), npc_data);
                break;
            case TOUCH_RESULT_TYPE.RELEASE:
                GameObjectPoolManager.Instance.UnusedGameObject(Tooltip);
                break;
        }
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
                Tooltip = GameObjectPoolManager.Instance.GetGameObject("Assets/AssetResources/Prefabs/UI/CommonTooltip", transform.parent);
                var tooltip = Tooltip.GetComponent<CommonTooltip>();
                tooltip.Initialize(hole(false), reward_data);
                break;
            case TOUCH_RESULT_TYPE.RELEASE:
                GameObjectPoolManager.Instance.UnusedGameObject(Tooltip);
                break;
        }
    }

    public void OnClickCancel()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }

    public void OnClickEntrance()
    {
        int need_stamina = Stage.use_stamina;
        if (!GameData.Instance.GetUserChargeItemDataManager().IsUsableChargeItemCount(REWARD_TYPE.STAMINA, need_stamina))
        {
            CommonUtils.ShowToast("스테미너가 부족합니다.", TOAST_BOX_LENGTH.SHORT);
            return;
        }

        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        
        HidePopup(() =>
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/PartySettingPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup(GAME_TYPE.STORY_MODE, Stage.stage_id);
            });
        });
    }

    void ClearNpcList()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Npc_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Npc_List[i].gameObject);
        }
        Used_Npc_List.Clear();
    }
    void ClearRewardItemList()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Reward_Item_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Reward_Item_List[i].gameObject);
        }
        Used_Reward_Item_List.Clear();
    }

    public override void Spawned()
    {
        base.Spawned();

        Is_Animation_End = false;
        Is_Load_Complete = false;
    }

    public override void Despawned()
    {
        Stage_Number.text = "";
        Stage_Name.text = "";
        MarkStarPoint(0);

        ClearNpcList();
        ClearRewardItemList();
    }
}
