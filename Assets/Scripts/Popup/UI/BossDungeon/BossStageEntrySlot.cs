using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStageEntrySlot : UIBase
{
    [SerializeField, Tooltip("Boss Blur")]
    Image Boss_Blur;

    [SerializeField, Tooltip("Boss Image")]
    Image Boss_Image;

    [SerializeField, Tooltip("Boss Nick")]
    TMP_Text Boss_Nick;
    [SerializeField, Tooltip("Boss Name")]
    TMP_Text Boss_Name;

    [SerializeField, Tooltip("Lock Cover")]
    RectTransform Lock_Cover;

    Boss_Data Boss;
    UserBossDungeonData User_Data;

    public void SetBossDataID(int boss_id)
    {
        Boss = MasterDataManager.Instance.Get_BossData(boss_id);
        User_Data = GameData.Instance.GetUserBossDungeonDataManager().FindUserBossDungeonData(boss_id);
        UpdateBossSlot();
    }

    void UpdateBossSlot()
    {
        if (User_Data == null)
        {
            Lock_Cover.gameObject.SetActive(true);
            return;
        }
        if (Lock_Cover.gameObject.activeSelf)
        {
            Lock_Cover.gameObject.SetActive(false);
        }
    }

    public void OnClickChoiceBoss()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        if (User_Data == null)
        {
            return;
        }
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/BossDungeon/BossStageChoiceUI", FluffyDuck.UI.POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
        {
            popup.ShowPopup();
        });
    }



    public override void Spawned()
    {
        
    }
}
