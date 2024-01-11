using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoBoxBasic : MonoBehaviour
{
    [SerializeField, Tooltip("Combat Text")]
    protected TMP_Text Combat_Number_Text;

    [SerializeField, Tooltip("Attack Text")]
    protected TMP_Text Attack_Number_Text;

    [SerializeField, Tooltip("Defense Text")]
    protected TMP_Text Defense_Number_Text;

    [SerializeField, Tooltip("Health Text")]
    protected TMP_Text Health_Number_Text;

    [SerializeField, Tooltip("Skill Icons")]
    protected List<Image> Skill_Icons;

    BattleUnitData Unit_Data;
    BattleSkillManager Skill_Mng;

    public void SetHeroData(BattleUnitData data)
    {
        Unit_Data = data;

        Refresh();
    }

    void Refresh()
    {
        if (Unit_Data == null)
        {
            return;
        }

        Combat_Number_Text.text = Unit_Data.GetCombatPoint().ToString("N0");
        Attack_Number_Text.text = Unit_Data.GetAttackPoint().ToString("N0");
        Defense_Number_Text.text = Unit_Data.GetDefensePoint().ToString("N0");
        Health_Number_Text.text = Unit_Data.GetLifePoint().ToString("N0");

        Skill_Mng = new BattleSkillManager();
        Skill_Mng.SetPlayerCharacterSkillGroups(Unit_Data.GetSkillPattern());

        for (int i = 0; i < Skill_Mng.Skill_Groups.Count; i++)
        {
            // 콜백 참조 때문에 별도로 변수를 선언해야 합니다
            int index = i;
            CommonUtils.GetResourceFromAddressableAsset<Sprite>(Skill_Mng.Skill_Groups[i].GetSkillIconPath(), (spr) =>
            {
                Debug.Log($"index : {index}");
                Skill_Icons[index].sprite = spr;
            });
        }
    }

    public void OnSelectedTab(Gpm.Ui.Tab tab)
    {
        Refresh();
    }

    public void OnClickSkillButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }

    public void OnClickStatusDetailButon()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }

    public void OnClickEquipmentButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }

    public void OnClickWeaponButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }
}
