using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
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


    Stage_Data Stage;
    UserStoryStageData User_Data;


    List<NpcCardBase> Used_Npc_List = new List<NpcCardBase>();
    List<RewardItemCard> Used_Reward_Item_List = new List<RewardItemCard>();

    bool Is_Animation_End;
    bool Is_Load_Complete;

    public override void ShowPopup(params object[] data)
    {
        if (data.Length != 1)
        {
            HidePopup();
            return;
        }
        
        Stage = (Stage_Data)data[0];

        User_Data = GameData.Instance.GetUserStoryStageDataManager().FindUserStoryStageData(Stage.stage_id);

        base.ShowPopup(data);
        InitAssets();
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/UI/Card/NpcCard");
        asset_list.Add("Assets/AssetResources/Prefabs/UI/Card/RewardItemCard");

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
        var zone = m.Get_ZoneData(Stage.zone_id);

        Stage_Number.text = ZString.Format("{0}-{1}", zone.zone_ordering, Stage.stage_ordering);

        Stage_Name.text = Stage.stage_name;

        int cnt = 0;
        //  npc list
        string npc_prefab = "Assets/AssetResources/Prefabs/UI/Card/NpcCard";
        List<Wave_Data> wave_data_list = new List<Wave_Data>();
        m.Get_WaveDataList(Stage.stage_id, ref wave_data_list);
        cnt = wave_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var wave = wave_data_list[i];
            for (int e = 0; e < wave.enemy_appearance_info.Length; e++)
            {
                int npc_id = wave.enemy_appearance_info[e];
                var obj = pool.GetGameObject(npc_prefab, Npc_List_View.content);
                var npc_card = obj.GetComponent<NpcCardBase>();
                npc_card.SetNpcID(npc_id);
                Used_Npc_List.Add(npc_card);
            }
        }


        string reward_prefab = "Assets/AssetResources/Prefabs/UI/Card/RewardItemCard";

        //  first reward list
        List<First_Reward_Data> first_reward_data_list = new List<First_Reward_Data>();
        m.Get_FirstRewardDataList(Stage.first_reward_group_id, ref first_reward_data_list);
        cnt = first_reward_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item_data = first_reward_data_list[i];
            var obj = pool.GetGameObject(reward_prefab, First_Reward_List_View.content);
            var reward_item = obj.GetComponent<RewardItemCard>();
            reward_item.SetItem(item_data.item_type, item_data.item_id);
            Used_Reward_Item_List.Add(reward_item);
        }


        //  star reward list
        List<Star_Reward_Data> star_reward_data_list = new List<Star_Reward_Data>();
        m.Get_StarRewardDataList(Stage.star_reward_group_id, ref star_reward_data_list);
        cnt = star_reward_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item_data = star_reward_data_list[i];
            var obj = pool.GetGameObject(reward_prefab, First_Reward_List_View.content);
            var reward_item = obj.GetComponent<RewardItemCard>();
            reward_item.SetItem(item_data.item_type, item_data.item_id);
            Used_Reward_Item_List.Add(reward_item);
        }


        //  repeat reward list
        List<Repeat_Reward_Data> repeat_reward_data_list = new List<Repeat_Reward_Data>();
        m.Get_RepeatRewardDataList(Stage.repeat_reward_group_id, ref repeat_reward_data_list);
        cnt = repeat_reward_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item_data = repeat_reward_data_list[i];
            var obj = pool.GetGameObject(reward_prefab, Repeat_Reward_List_View.content);
            var reward_item = obj.GetComponent<RewardItemCard>();
            reward_item.SetItem(item_data.item_type, item_data.item_id);
            Used_Reward_Item_List.Add(reward_item);
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

    public override void UpdatePopup()
    {
        
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

    public void OnClickCancel()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }

    public void OnClickEntrance()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup(() =>
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Deck/PCDeckSettingUI", (popup) =>
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

        if (Ease_Base != null)
        {
            Ease_Base.transform.localScale = new Vector2(0f, 0f);
        }
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
