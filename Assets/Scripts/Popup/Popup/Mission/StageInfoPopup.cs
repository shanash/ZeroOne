using Cysharp.Text;
using FluffyDuck.UI;
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


    Stage_Data Stage;
    UserStoryStageData User_Data;

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
    }

    protected override void ShowPopupAniEndCallback()
    {
        FixedUpdatePopup();
    }

    protected override void HidePopupAniEndCallback()
    {
        base.HidePopupAniEndCallback();
    }


    protected override void FixedUpdatePopup()
    {
        var m = MasterDataManager.Instance;
        var zone = m.Get_ZoneData(Stage.zone_id);

        Stage_Number.text = ZString.Format("{0}-{1}", zone.zone_ordering, Stage.stage_ordering);

        Stage_Name.text = Stage.stage_name;

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

    public void OnClickCancel()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }

    public void OnClickEntrance()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }

    public override void Spawned()
    {
        base.Spawned();

        if (Ease_Base != null)
        {
            Ease_Base.transform.localScale = new Vector2(0f, 0f);
        }

    }

    public override void Despawned()
    {
        Stage_Number.text = "";
        Stage_Name.text = "";
        MarkStarPoint(0);
    }
}
