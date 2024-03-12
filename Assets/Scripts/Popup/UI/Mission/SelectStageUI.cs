using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using Gpm.Ui;
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


    [SerializeField, Tooltip("스테이지 리스트 뷰")]
    InfiniteScroll Stage_List_View;

    [SerializeField, Tooltip("존 이동 버튼(이전 존)")]
    UIButtonBase Left_Arrow_Btn;
    [SerializeField, Tooltip("존 이동 버튼(다음 존)")]
    UIButtonBase Right_Arrow_Btn;

    [SerializeField, Tooltip("Tab Toggle Group")]
    ToggleGroup Diff_Tab_Group;

    [SerializeField, Tooltip("Tab Ctrl")]
    TabController Tab_Ctrl;

    [SerializeField, Tooltip("Tab Scroll View List")]
    List<StageListView> Stage_List_Views;

    int Zone_ID;
    Zone_Data Zone;

    int World_ID;
    World_Data World;

    protected override bool Initialize(params object[] data)
    {
        if (data.Length != 2)
        {
            return false;
        }
        World_ID = (int)data[0];
        Zone_ID = (int)data[1];
        InitWorldZoneData();

        FixedUpdatePopup();
        UpdatePopup();

        return true;
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

    public override void UpdatePopup()
    {
        var m = MasterDataManager.Instance;
        Stage_List_View.Clear();

        var stage_mng = GameData.Instance.GetUserStoryStageDataManager();
        var last_stage = stage_mng.GetLastOpenStage();

        var stage_list = m.Get_StageDataListByStageGroupID(Zone.stage_group_id);

        int last_index = stage_list.ToList().FindIndex(x => x.stage_id == last_stage.Stage_ID);

        int cnt = stage_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var stage = stage_list[i];
            var new_stage = new StageListData();
            new_stage.SetStageID(World.world_id, Zone.zone_id, stage.stage_id);
            Stage_List_View.InsertData(new_stage);
        }

        Stage_List_View.MoveTo(last_index, InfiniteScroll.MoveToType.MOVE_TO_TOP);

        //  zone number
        Zone_Number.text = ZString.Format("ZONE {0}", Zone.zone_ordering);

        //  zone name
        Zone_Name.text = Zone.zone_name;

        //  zone desc
        Zone_Desc.text = Zone.zone_tooltip;

        //  Zone_Star_Proceed_Text
        
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
            UpdatePopup();
        }
    }

    public void OnClickTab(int diff)
    {
        var tab = Diff_Tab_Group.ActiveToggles().FirstOrDefault();

        if (tab != null && tab.isOn)
        {
            Debug.Log($"{tab.name} => {diff}");
        }
        
    }

    public void OnSelectTab(Gpm.Ui.Tab tab)
    {

    }

    public void OnBlockTab(Gpm.Ui.Tab tab)
    {

    }

}
