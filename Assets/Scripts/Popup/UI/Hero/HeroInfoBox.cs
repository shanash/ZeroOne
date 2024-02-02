using FluffyDuck.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroInfoBox : MonoBehaviour
{
    [SerializeField]
    List<TMP_Text> Tab_Off_Names;

    [SerializeField]
    List<TMP_Text> Tab_On_Names;

    [SerializeField, Tooltip("Tab Controller")]
    Gpm.Ui.TabController Tab_Controller;

    [SerializeField, Tooltip("Hero Info Box Basic")]
    HeroInfoBoxBasic Hero_Info_Box_Basic;

    [SerializeField, Tooltip("Hero Info Box LevelUp")]
    HeroInfoBoxLevelUp Hero_Info_Box_LevelUp;

    [SerializeField, Tooltip("Hero Info Box LevelUp")]
    HeroInfoBoxAdvance Hero_Info_Box_Advance;

    [SerializeField, Tooltip("Hero Essence Info")]
    HeroInfoBoxEssence Hero_Info_Box_Essence;

    BattlePcData User_Hero_Data;

    public void SetHeroData(BattlePcData data)
    {
        User_Hero_Data = data;
        FixedUpdatePopup();
    }

    public void FixedUpdatePopup()
    {
        int cnt = ConstString.HeroInfoUI.TAB_NAMES.Length;

        for (int i = 0; i < cnt; i++)
        {
            Tab_Off_Names[i].text = ConstString.HeroInfoUI.TAB_NAMES[i];
            Tab_On_Names[i].text = ConstString.HeroInfoUI.TAB_NAMES[i];
        }

        Tab_Controller.SelectFirstTab();

        // 레벨업 탭과 승급 탭은 일단 막아놓습니다.
        Tab_Controller.GetTab(1).SetBlockTab(true);

        Refresh();
    }

    public void Refresh()
    {
        Hero_Info_Box_Basic.SetHeroData(User_Hero_Data);
        Hero_Info_Box_LevelUp.SetHeroData(User_Hero_Data);
        Hero_Info_Box_Advance.SetHeroData(User_Hero_Data);
        Hero_Info_Box_Essence.SetHeroData(User_Hero_Data);
    }

    public void OnBlocked(Gpm.Ui.Tab tab)
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }
}
