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

    BattlePcData Unit_Data;

    public void SetHeroData(BattlePcData data)
    {
        Unit_Data = data;

        FixedUpdatePopup();
    }

    public void FixedUpdatePopup()
    {
        Combat_Subject.text = ConstString.Hero.COMBAT_POWER;
        Attack_Subject.text = ConstString.Hero.ATTACK_DAMAGE;
        Defense_Subject.text = ConstString.Hero.ATTACK_DEFENSE;
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
        Attack_Number_Text.text = Unit_Data.GetPhysicsAttackPoint().ToString("N0");
        Defense_Number_Text.text = Unit_Data.GetPhysicsDefensePoint().ToString("N0");
        Life_Number_Text.text = Unit_Data.GetMaxLifePoint().ToString("N0");

        var skill_group_list = GameData.Instance.GetUserHeroSkillDataManager().GetUserHeroSkillDataList(Unit_Data.User_Data.GetPlayerCharacterID(), Unit_Data.User_Data.Player_Character_Num);

        foreach (var icon in Skill_Icons)
        {
            icon.sprite = null;
        }

        int i = 0;
        foreach (var skill_group in skill_group_list)
        {
            // 콜백 참조 때문에 별도로 변수를 선언해야 합니다
            int sprite_index = i;
            CommonUtils.GetResourceFromAddressableAsset<Sprite>(skill_group.Group_Data.icon, (spr) =>
            {
                Skill_Icons[sprite_index].sprite = spr;
            });
            i++;
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
            popup.ShowPopup(GameData.Instance.GetUserHeroSkillDataManager().GetUserHeroSkillDataList(Unit_Data.User_Data.GetPlayerCharacterID(), Unit_Data.User_Data.Player_Character_Num));
        });
    }

    public void OnClickStatusDetailButon()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Hero/StatusPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            List<StatusItemData> status_list = new List<StatusItemData>
            {
                new StatusItemData(ConstString.Hero.COMBAT_POWER, Unit_Data.GetCombatPoint().ToString("N0")),
                new StatusItemData(ConstString.Hero.LIFE_POINT, Unit_Data.GetMaxLifePoint().ToString("N0")),
                new StatusItemData(ConstString.Hero.ATTACK_DAMAGE, Unit_Data.GetPhysicsAttackPoint().ToString("N0")),
                new StatusItemData(ConstString.Hero.MAGIC_DAMAGE, Unit_Data.GetMagicAttackPoint().ToString("N0")),
                new StatusItemData(ConstString.Hero.ATTACK_DEFENSE, Unit_Data.GetPhysicsDefensePoint().ToString("N0")),
                new StatusItemData(ConstString.Hero.MAGIC_DEFENSE, Unit_Data.GetMagicDefensePoint().ToString("N0")),
                new StatusItemData(ConstString.Hero.APPROACH_DISTANCE, Unit_Data.GetApproachDistance().ToString("N0")),
                new StatusItemData(ConstString.Hero.ATTACK_RECOVERY, Unit_Data.GetAttackLifeRecovery().ToPercentage()),
                new StatusItemData(ConstString.Hero.EVASION_POINT, Unit_Data.GetEvasionPoint().ToString("N0")),
                new StatusItemData(ConstString.Hero.ACCURACY_POINT, Unit_Data.GetAccuracyPoint().ToString("N0")),
                new StatusItemData(ConstString.Hero.AUTO_RECORVERY, Unit_Data.GetAutoRecoveryLife().ToPercentage())
            };

            popup.ShowPopup(ConstString.StatusPopup.ABILITY_TITLE, status_list);
        });
    }

    public void OnClickEquipmentButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }

    public void OnClickWeaponButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }
}
