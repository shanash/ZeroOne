using Cysharp.Text;
using FluffyDuck.UI;
using Gpm.Ui;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageListCellNode : InfiniteScrollItem
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Stage Number")]
    TMP_Text Stage_Number;

    [SerializeField, Tooltip("Star List")]
    List<Image> Stage_Star_Points;

    [SerializeField, Tooltip("Stage Name")]
    TMP_Text Stage_Name;

    [SerializeField, Tooltip("Entrance Remain Count")]
    TMP_Text Daily_Entrance_Remain_Count;

    [SerializeField, Tooltip("Reward Icon")]
    RewardItemCard Item_Card;

    [SerializeField, Tooltip("Entrance Btn")]
    UIButtonBase Entrance_Btn;

    [SerializeField, Tooltip("Lock Cover")]
    RectTransform Lock_Cover;

    public override void UpdateData(InfiniteScrollData sdata)
    {
        base.UpdateData(sdata);

        var data = (StageListData)sdata;

        //  stage number
        Stage_Number.text = ZString.Format("{0}-{1}", data.Zone.zone_ordering, data.Stage.stage_ordering);

        //  stage name
        Stage_Name.text = GameDefine.GetLocalizeString(data.Stage.stage_name_id);

        //  star point
        if (data.IsExistUserData())
        {
            MarkStarPoint(data.User_Data.GetStarPoint());
        }
        else
        {
            MarkStarPoint(0);
        }

        //  lock cover
        if (data.IsExistUserData())
        {
            Lock_Cover.gameObject.SetActive(false);
        }
        else
        {
            Lock_Cover.gameObject.SetActive(true);
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

    public void OnClickStageEntrance()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Mission/StageInfoPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            var data = (StageListData)this.scrollData;

            popup.ShowPopup(data.World, data.Zone, data.Stage);
        });
    }

}
