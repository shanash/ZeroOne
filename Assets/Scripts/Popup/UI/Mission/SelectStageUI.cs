using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageUI : PopupBase
{
    [Space()]
    [SerializeField, Tooltip("존 번호")]
    TMP_Text Zone_Number;

    [SerializeField, Tooltip("존 이름")]
    TMP_Text Zone_Name;

    [SerializeField, Tooltip("존 설명")]
    TMP_Text Zone_Desc;

    [SerializeField, Tooltip("스테이지 대표 이미지(또는 존 대표 이미지)")]
    Image Stage_Image;

    [SerializeField, Tooltip("존 별점 진행상황 텍스트")]
    TMP_Text Zone_Star_Proceed_Text;

    [SerializeField, Tooltip("존 별점 게이지")]
    Slider Zone_Star_Gauge;

    [SerializeField, Tooltip("존 이동 버튼(이전 존)")]
    UIButtonBase Left_Arrow_Btn;
    [SerializeField, Tooltip("존 이동 버튼(다음 존)")]
    UIButtonBase Right_Arrow_Btn;

    [SerializeField, Tooltip("Tab Toggle Group")]
    ToggleGroup Diff_Tab_Group;

    [SerializeField, Tooltip("Tab Ctrl")]
    TabController Tab_Ctrl;

    [SerializeField, Tooltip("Tab Btns")]
    List<Tab> Tab_Btns_List;

    [SerializeField, Tooltip("Tab Lock Icons")]
    List<Image> Tab_Btn_Lock_Icons;

    int Zone_ID;
    Zone_Data Zone;

    int World_ID;
    World_Data World;

    protected override bool Initialize(params object[] data)
    {
        var stage_mng = GameData.Instance.GetUserStoryStageDataManager();
        World_ID = stage_mng.GetCurrentWorldID();
        Zone_ID = stage_mng.GetCurrentZoneID();
        InitWorldZoneData();

        InitAssets();
        FixedUpdatePopup();

        return true;
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/UI/Mission/NormalStageListCell");
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/UI/Mission/HardStageListCell");

        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);
    }

    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (load_cnt == total_cnt)
        {
            UpdateTabs();
            UpdatePopup();
        }
    }

    void InitWorldZoneData()
    {
        var m = MasterDataManager.Instance;
        World = m.Get_WorldData(World_ID);
        Zone = m.Get_ZoneData(Zone_ID);
    }
    protected override void FixedUpdatePopup()
    {
        var m = MasterDataManager.Instance;

        var board = BlackBoard.Instance;
        int open_dungeon_id = board.GetBlackBoardData<int>(BLACK_BOARD_KEY.OPEN_STORY_STAGE_DUNGEON_ID, 0);
        if (open_dungeon_id > 0)
        {
            Stage_Data stage = m.Get_StageData(open_dungeon_id);
            if (stage != null)
            {
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Mission/StageInfoPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
                {
                    popup.ShowPopup(World, Zone, stage);
                });
            }

            BlackBoard.Instance.RemoveBlackBoardData(BLACK_BOARD_KEY.OPEN_STORY_STAGE_DUNGEON_ID);
        }
    }

    void UpdateTabs()
    {
        var m = MasterDataManager.Instance;

        var zone_list = m.Get_ZoneDataListByZoneCode(Zone.zone_code_id);
        var stage_mng = GameData.Instance.GetUserStoryStageDataManager();
        //  tab check
        for (int i = 0; i < Tab_Btns_List.Count; i++)
        {
            var tab = Tab_Btns_List[i];
            var lock_icon = Tab_Btn_Lock_Icons[i];
            STAGE_DIFFICULTY_TYPE dtype = (STAGE_DIFFICULTY_TYPE)(i + 1);
            var zone = zone_list.Find(x => x.zone_difficulty == dtype);
            bool is_exist = zone != null;
            bool is_open_zone = false;
            if (is_exist)
            {
                is_open_zone = stage_mng.IsOpenZone(zone.zone_id);
            }
            tab.SetBlockTab(!(is_exist && is_open_zone));
            lock_icon.gameObject.SetActive(!(is_exist && is_open_zone));
            var toggle = tab.GetComponent<Toggle>();
            if (toggle != null)
            {
                toggle.interactable = is_exist && is_open_zone;
            }

            if (zone != null && stage_mng.GetCurrentZoneID() == zone.zone_id)
            {
                Tab_Ctrl.Select(tab);
                toggle.isOn = true;
            }
        }
    }

    public override void UpdatePopup()
    {
        var stage_mng = GameData.Instance.GetUserStoryStageDataManager();

        //var last_stage = stage_mng.GetLastOpenStage();
        //var stage_list = m.Get_StageDataListByStageGroupID(Zone.stage_group_id);
        //int last_index = stage_list.ToList().FindIndex(x => x.stage_id == last_stage.Stage_ID);

        //  zone number
        Zone_Number.text = ZString.Format("ZONE {0}", Zone.zone_ordering);

        //  zone name
        Zone_Name.text = Zone.zone_name;

        //  zone desc
        Zone_Desc.text = Zone.zone_tooltip;

        
        int gain_star = stage_mng.GetGainStarPoints(Zone.stage_group_id);
        int total_star = stage_mng.GetTotalStarCount(Zone.stage_group_id);
        Zone_Star_Proceed_Text.text = ZString.Format("진행도({0}/{1})", gain_star, total_star);

        float per = (float)gain_star / (float)total_star;
        Zone_Star_Gauge.value = Mathf.Clamp01(per);

        //  left btn
        Left_Arrow_Btn.gameObject.SetActive(stage_mng.IsOpenPrevZone());
        //  right btn
        Right_Arrow_Btn.gameObject.SetActive(stage_mng.IsOpenNextZone());

    }

    public void OnClickZoneStarReward()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }

    public void OnClickLeftMoveZone()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        var stage_mng = GameData.Instance.GetUserStoryStageDataManager();
        if (stage_mng.MovePrevZone())
        {
            World_ID = stage_mng.GetCurrentWorldID();
            Zone_ID = stage_mng.GetCurrentZoneID();
            InitWorldZoneData();
            UpdateTabs();
            UpdatePopup();
        }
    }
    public void OnClickRightMoveZone()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        var stage_mng = GameData.Instance.GetUserStoryStageDataManager();
        if (stage_mng.MoveNextZone())
        {
            World_ID = stage_mng.GetCurrentWorldID();
            Zone_ID = stage_mng.GetCurrentZoneID();
            InitWorldZoneData();
            UpdateTabs();
            UpdatePopup();
        }
    }

    public void OnSelectTab(Gpm.Ui.Tab tab)
    {
        var stage_mng = GameData.Instance.GetUserStoryStageDataManager();
        if (Zone == null)
        {
            World_ID = stage_mng.GetCurrentWorldID();
            Zone_ID = stage_mng.GetCurrentZoneID();

            InitWorldZoneData();
        }

        var zone_list = MasterDataManager.Instance.Get_ZoneDataListByZoneCode(Zone.zone_code_id);
        int idx = Tab_Ctrl.GetTabIndex(tab);
        
        var select_zone = zone_list[idx];
        stage_mng.SetCurrentZoneID(select_zone.zone_id);
        Zone_ID = select_zone.zone_id;
        InitWorldZoneData();

        //  view update
        var view = tab.GetLinkedPage().GetComponent<StageListView>();
        view.SetWorldAndZoneID(World_ID, select_zone.zone_id);
        UpdatePopup();
    }

}
