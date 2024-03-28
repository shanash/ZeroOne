using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
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

    [SerializeField, Tooltip("Skill Nodes")]
    protected List<PartySelectSkillNode> Skills;

    BattlePcData Unit_Data = null;

    public void SetHeroData(BattlePcData data)
    {
        Unit_Data = data;

        FixedUpdatePopup();
    }

    public void FixedUpdatePopup()
    {
        Combat_Subject.text = GameDefine.GetLocalizeString("system_stat_battlepower");
        Attack_Subject.text = GameDefine.GetLocalizeString("system_stat_physics_attack");
        Defense_Subject.text = GameDefine.GetLocalizeString("system_stat_physics_defence");
        Life_Subject.text = GameDefine.GetLocalizeString("system_stat_life");

        Refresh();
    }

    void Refresh()
    {
        if (Unit_Data == null)
        {
            return;
        }

        Combat_Number_Text.text = Unit_Data.GetCombatPoint().ToString("N0");
        Attack_Number_Text.text = Unit_Data.GetPhysicsAttackPoint().ToString("N0");
        Defense_Number_Text.text = Unit_Data.GetPhysicsDefensePoint().ToString("N0");
        Life_Number_Text.text = Unit_Data.GetMaxLifePoint().ToString("N0");

        for (int i = 0; i < Skills.Count; i++)
        {
            SKILL_TYPE type = SKILL_TYPE.NONE;
            switch (i)
            {
                case 0:
                    type = SKILL_TYPE.SKILL_01;
                    break;
                case 1:
                    type = SKILL_TYPE.SKILL_02;
                    break;
                case 2:
                    type = SKILL_TYPE.SPECIAL_SKILL;
                    break;
            }
            try
            {
                Skills[i].Initialize(Unit_Data.User_Data.GetPlayerCharacterID(), Unit_Data.User_Data.Player_Character_Num, type);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    public void OnSelectedTab(Gpm.Ui.Tab tab)
    {
        Refresh();
    }

    public void OnClickSkillButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Hero/SkillLevelPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            //TODO:
            popup.AddClosedCallbackDelegate(OnConfirmSkillLevelUp);
            popup.ShowPopup(GameData.Instance.GetUserHeroSkillDataManager().GetUserHeroSkillDataList(Unit_Data.User_Data.GetPlayerCharacterID(), Unit_Data.User_Data.Player_Character_Num));
        });
    }

    void OnConfirmSkillLevelUp(params object[] data)
    {
        Refresh();
    }

    public void OnClickStatusDetailButon()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Hero/StatusPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            List<StatusItemData> status_list = new List<StatusItemData>
            {
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_battlepower"), Unit_Data.GetCombatPoint().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_life"), Unit_Data.GetMaxLifePoint().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_physics_attack"), Unit_Data.GetPhysicsAttackPoint().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_magic_attack"), Unit_Data.GetMagicAttackPoint().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_physics_defence"), Unit_Data.GetPhysicsDefensePoint().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_magic_defence"), Unit_Data.GetMagicDefensePoint().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_physics_critical_chance"), Unit_Data.GetPhysicsCriticalChance().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_magic_critical_chance"), Unit_Data.GetMagicCriticalChance().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_physics_critical_power_add"), Unit_Data.GetPhysicsCriticalPowerAdd().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_magic_critical_power_add"), Unit_Data.GetMagicCriticalPowerAdd().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_attack_life_recovery"), Unit_Data.GetAttackLifeRecovery().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_evasion"), Unit_Data.GetEvasionPoint().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_accuracy"), Unit_Data.GetAccuracyPoint().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_heal"), Unit_Data.GetLifeRecoveryInc().ToString("N0")),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_resist"), Unit_Data.GetResistPoint().ToString("N0"))
            };

            popup.ShowPopup(GameDefine.GetLocalizeString("system_stat_detail_information"), status_list, 0);
        });
    }

    public void OnClickEquipmentButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, GameDefine.GetLocalizeString("system_alert_preparing"));
        });
    }

    public void OnClickWeaponButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, GameDefine.GetLocalizeString("system_alert_preparing"));
        });
    }
}
