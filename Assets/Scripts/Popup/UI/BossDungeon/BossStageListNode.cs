using Cysharp.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStageListNode : UIBase
{

    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Stage Name")]
    TMP_Text Stage_Name;

    [SerializeField, Tooltip("Star Points")]
    List<Image> Star_Points;

    [SerializeField, Tooltip("Recommand Level")]
    TMP_Text Recommand_Level;

    [SerializeField, Tooltip("Enter Btn")]
    UIButtonBase Enter_Btn;
    [SerializeField, Tooltip("Enter Btn Text")]
    TMP_Text Enter_Btn_Text;
    [SerializeField, Tooltip("Skip Btn")]
    UIButtonBase Skip_Btn;
    [SerializeField, Tooltip("Skip Btn Text")]
    TMP_Text Skip_Btn_Text;
    [SerializeField, Tooltip("Skip Lock Cover")]
    RectTransform Skip_Lock_Cover;

    [SerializeField, Tooltip("Lock Cover")]
    RectTransform Lock_Cover;

    Boss_Stage_Data Stage;
    UserBossStageData User_Dungeon;

    
    public void SetBossStageData(Boss_Stage_Data stage)
    {
        this.Stage = stage;
        User_Dungeon = GameData.Instance.GetUserBossStageDataManager().FindUserBossDungeonData(Stage.boss_stage_id);
        UpdateBossDungeonList();
    }

    void UpdateBossDungeonList()
    {
        Lock_Cover.gameObject.SetActive(User_Dungeon == null);
        //  아직 던전이 열리지 않았을 때
        if (User_Dungeon == null)
        {
            ShowStarPoint(0);
            //  던전이 열리지 않았을 경우에는 스킵 lock을 하지 않는다. 이미 lock cover가 덮여 있기 때문에
            Skip_Lock_Cover.gameObject.SetActive(false);
        }
        else
        {
            ShowStarPoint(User_Dungeon.GetStarPoint());
            //  던전이 열려있을 경우에만 skip lock cover를 체크할 필요가 있음.
            Skip_Lock_Cover.gameObject.SetActive(User_Dungeon.GetStarPoint() != 3);

            Skip_Btn.interactable = User_Dungeon.GetStarPoint() == 3;
        }
        //  stage name
        Stage_Name.text = GameDefine.GetLocalizeString(Stage.stage_name);
        //  recommand level
        Recommand_Level.text = ZString.Format("{0}{1}", GameDefine.GetLocalizeString("system_recomment_level"), Stage.recomment_level);
        //  enter btn text

        //  skip btn text
        
    }

    public void ShowStarPoint(int pt)
    {
        int cnt = Star_Points.Count;
        for (int i = 0; i < cnt; i++)
        {
            Star_Points[i].gameObject.SetActive(pt > i);
        }
    }

    public void OnClickStageEntrance()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/BossDungeon/BossStageRewardInfoPopup", FluffyDuck.UI.POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup(Stage);
        });
    }
    public void OnClickStageSkip()
    {
        if (User_Dungeon == null)
        {
            return;
        }
        if (User_Dungeon.GetStarPoint() != 3)
        {
            return;
        }
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }

    
}
