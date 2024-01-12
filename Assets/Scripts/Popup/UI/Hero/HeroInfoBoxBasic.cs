using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoBoxBasic : MonoBehaviour
{
    [SerializeField]
    TMP_Text Combat_Subject;
    [SerializeField]
    TMP_Text Attack_Subject;
    [SerializeField]
    TMP_Text Defense_Subject;
    [SerializeField]
    TMP_Text Life_Subject;
    [SerializeField]
    TMP_Text Advance_Skill;
    [SerializeField]
    TMP_Text Advance_Equipment;
    [SerializeField]
    TMP_Text Attack_Weapon;

    [SerializeField, Tooltip("Combat Text")]
    protected TMP_Text Combat_Number_Text;

    [SerializeField, Tooltip("Attack Text")]
    protected TMP_Text Attack_Number_Text;

    [SerializeField, Tooltip("Defense Text")]
    protected TMP_Text Defense_Number_Text;

    [SerializeField, Tooltip("Health Text")]
    protected TMP_Text Life_Number_Text;

    [SerializeField, Tooltip("Skill Icons")]
    protected List<Image> Skill_Icons;

    BattleUnitData Unit_Data;
    BattleSkillManager Skill_Mng;

    public void SetHeroData(BattleUnitData data)
    {
        Unit_Data = data;

        FixedUpdatePopup();
    }

    public void FixedUpdatePopup()
    {
        Combat_Subject.text = ConstString.Hero.COMBAT_POWER;
        Attack_Subject.text = ConstString.Hero.ATTACK_POWER;
        Defense_Subject.text = ConstString.Hero.DEFENCE_POINT;
        Life_Subject.text = ConstString.Hero.LIFE_POINT;

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
        Life_Number_Text.text = Unit_Data.GetLifePoint().ToString("N0");

        Skill_Mng = new BattleSkillManager();
        Skill_Mng.SetPlayerCharacterSkillGroups(Unit_Data.GetSkillPattern());

        for (int i = 0; i < Skill_Mng.Skill_Groups.Count; i++)
        {
            // 콜백 참조 때문에 별도로 변수를 선언해야 합니다
            int index = i;
            CommonUtils.GetResourceFromAddressableAsset<Sprite>(Skill_Mng.Skill_Groups[i].GetSkillIconPath(), (spr) =>
            {
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
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }

    public void OnClickStatusDetailButon()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Hero/StatusPopup", (popup) =>
        {
            popup.ShowPopup(Unit_Data);
        });
    }

    public void OnClickEquipmentButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }

    public void OnClickWeaponButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }
}
