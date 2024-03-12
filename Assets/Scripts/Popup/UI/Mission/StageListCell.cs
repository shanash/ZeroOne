using Cysharp.Text;
using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageListCell : UIBase
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

    Stage_Data Stage;
    Zone_Data Zone;
    UserStoryStageData User_Data;

    public void SetStageID(int stage_id)
    {
        var m = MasterDataManager.Instance;
        Stage = m.Get_StageData(stage_id);
        Zone = m.Get_ZoneDataByStageGroupID(Stage.stage_group_id);
        User_Data = GameData.Instance.GetUserStoryStageDataManager().FindUserStoryStageData(stage_id);
        UpdateStageListCell();
    }

    void UpdateStageListCell()
    {
        //  stage number
        Stage_Number.text = ZString.Format("{0}-{1}", Zone.zone_ordering, Stage.stage_ordering);
        
        //  stage name
        Stage_Name.text = GameDefine.GetLocalizeString(Stage.stage_name_id);

        //  reward item
        if (Zone.zone_difficulty != STAGE_DIFFICULTY_TYPE.NORMAL)
        {
            //  대표 아이템 아이콘 보여주기
         
            //  남은 횟수 보여주기
        }
        

        if (User_Data != null)
        {
            //  star point
            MarkStarPoint(User_Data.GetStarPoint());

            //  lock cover
            Lock_Cover.gameObject.SetActive(false);
        }
        else
        {
            //  star point
            MarkStarPoint(0);

            //  lock cover
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

        //PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Mission/StageInfoPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        //{
        //    var data = (StageListData)this.scrollData;

        //    popup.ShowPopup(data.World, data.Zone, data.Stage);
        //});
    }

    
}
