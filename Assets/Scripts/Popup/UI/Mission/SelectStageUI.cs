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
    [SerializeField, Tooltip("Zone Number")]
    TMP_Text Zone_Number;

    [SerializeField, Tooltip("Zone Name")]
    TMP_Text Zone_Name;

    [SerializeField, Tooltip("Zone Desc")]
    TMP_Text Zone_Desc;

    [SerializeField, Tooltip("Stage Image")]
    Image Stage_Image;

    [SerializeField, Tooltip("Zone Star Proceed Text")]
    TMP_Text Zone_Star_Proceed_Text;

    [SerializeField, Tooltip("Zone Star Guague")]
    Slider Zone_Star_Gauge;


    [SerializeField, Tooltip("Stage List View")]
    InfiniteScroll Stage_List_View;

    int Zone_ID;
    Zone_Data Zone;

    protected override bool Initialize(params object[] data)
    {
        if (data.Length != 1)
        {
            return false;
        }

        Zone_ID = (int)data[0];
        Zone = MasterDataManager.Instance.Get_ZoneData(Zone_ID);

        FixedUpdatePopup();
        UpdatePopup();

        return true;
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
                    popup.ShowPopup(stage);
                });
            }

            BlackBoard.Instance.RemoveBlackBoardData(BLACK_BOARD_KEY.OPEN_STORY_STAGE_DUNGEON_ID);
        }

        Stage_List_View.Clear();

        var stage_mng = GameData.Instance.GetUserStoryStageDataManager();
        var last_stage = stage_mng.GetLastOpenStage();

        
        var stage_list = m.Get_StageDataListByZoneID(Zone_ID);

        int last_index = stage_list.ToList().FindIndex(x => x.stage_id == last_stage.Stage_ID);

        int cnt = stage_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var stage = stage_list[i];
            var new_stage = new StageListData();
            new_stage.SetStageID(stage.stage_id);
            Stage_List_View.InsertData(new_stage);
        }

        Stage_List_View.MoveTo(last_index, InfiniteScroll.MoveToType.MOVE_TO_TOP);
    }

    public override void UpdatePopup()
    {
        var smng = GameData.Instance.GetUserStoryStageDataManager();
        //  zone number
        Zone_Number.text = ZString.Format("ZONE {0}", Zone.zone_ordering);

        //  zone name
        Zone_Name.text = Zone.zone_name;

        //  zone desc
        Zone_Desc.text = Zone.zone_tooltip;

        //  Zone_Star_Proceed_Text
        int gain_star = smng.GetGainStarPoints(Zone_ID);
        int total_star = smng.GetTotalStarCount(Zone_ID);
        Zone_Star_Proceed_Text.text = ZString.Format("스테이지 진행도({0}/{1})", gain_star, total_star);

        float per = (float)gain_star / (float)total_star;
        Zone_Star_Gauge.value = Mathf.Clamp01(per);
    }

    public void OnClickZoneStarReward()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }
}
