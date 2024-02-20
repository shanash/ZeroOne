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
    UserBossStageDataManager Boss_Mng;

    public void SetBossDataID(int boss_id)
    {
        Boss = MasterDataManager.Instance.Get_BossData(boss_id);
        Boss_Mng = GameData.Instance.GetUserBossStageDataManager();
        UpdateBossSlot();
    }

    void UpdateBossSlot()
    {
        if (!Boss_Mng.IsBossOpen(Boss.boss_id))
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
        if (!Boss_Mng.IsBossOpen(Boss.boss_id))
        {
            return;
        }
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/BossDungeon/BossStageChoiceUI", FluffyDuck.UI.POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
        {
            popup.ShowPopup(Boss);
        });
    }



    public override void Spawned()
    {
        
    }
}
