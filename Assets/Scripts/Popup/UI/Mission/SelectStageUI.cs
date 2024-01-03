using Cysharp.Text;
using FluffyDuck.UI;
using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
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

    public override void ShowPopup(params object[] data)
    {
        if (data.Length != 1)
        {
            HidePopup();
            return;
        }

        Zone_ID = (int)data[0];
        Zone = MasterDataManager.Instance.Get_ZoneData(Zone_ID);

        base.ShowPopup(data);

        FixedUpdatePopup();
        UpdatePopup();
    }

    protected override void FixedUpdatePopup()
    {
        Stage_List_View.Clear();

        var m = MasterDataManager.Instance;
        List<Stage_Data> stage_list = new List<Stage_Data>();

        m.Get_StageDataList(Zone_ID, ref stage_list);
        int cnt = stage_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var stage = stage_list[i];
            var new_stage = new StageListData();
            new_stage.SetStageID(stage.stage_id);
            Stage_List_View.InsertData(new_stage);
        }
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
        Zone_Star_Proceed_Text.text = ZString.Format("스테이지 진행도({0}/{1})", smng.GetGainStarPoints(Zone_ID), smng.GetTotalStarCount(Zone_ID));
    }


    public void OnClickZoneStarReward()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }
}
