using FluffyDuck.UI;
using UnityEngine;

public class HeroInfoBox : MonoBehaviour
{
    [SerializeField, Tooltip("Tab Controller")]
    Gpm.Ui.TabController Tab_Controller;

    [SerializeField, Tooltip("Hero Info Box Basic")]
    HeroInfoBoxBasic Hero_Info_Box_Basic;

    [SerializeField, Tooltip("Hero Info Box LevelUp")]
    HeroInfoBoxLevelUp Hero_Info_Box_LevelUp;

    BattleUnitData User_Hero_Data;

    public void SetHeroData(BattleUnitData data)
    {
        User_Hero_Data = data;
        Refresh();
    }

    public void Refresh()
    {
        Hero_Info_Box_Basic.SetHeroData(User_Hero_Data);
        Hero_Info_Box_LevelUp.SetHeroData(User_Hero_Data);
    }

    public void OnBlocked(Gpm.Ui.Tab tab)
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }
}
