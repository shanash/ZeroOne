using FluffyDuck.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoUI : PopupBase
{
    [SerializeField]
    TMP_Text Title;

    [SerializeField, Tooltip("InfoBox Tap Controller")]
    Gpm.Ui.TabController InfoBox_Tab_Controller;

    [SerializeField, Tooltip("Level Text")]
    TMP_Text Level_Text;

    [SerializeField, Tooltip("Name Text")]
    TMP_Text Name_Text;

    [SerializeField, Tooltip("Star Images")]
    List<GameObject> Star_Images;

    [SerializeField, Tooltip("Position Tag View")]
    Image Position_Tag;
    [SerializeField, Tooltip("Position Tag Sprites Resources")]
    List<Sprite> Position_Tag_Sprites;

    [SerializeField, Tooltip("Tribe Box View")]
    Image Tribe_Box;
    [SerializeField, Tooltip("Tribe Box Sprites Resources")]
    List<Sprite> Tribe_Box_Sprites;

    [SerializeField, Tooltip("Tribe Tag View")]
    Image Tribe_Tag;
    [SerializeField, Tooltip("Tribe Tag Sprites Resources")]
    List<Sprite> Tribe_Tag_Sprites;

    [SerializeField, Tooltip("Tribe Text")]
    TMP_Text Tribe_Text;

    [SerializeField, Tooltip("Role Icon View")]
    Image Role_Icon;
    [SerializeField, Tooltip("Role Icon Sprites Resources")]
    List<Sprite> Role_Icon_Sprites;

    [SerializeField, Tooltip("Role Text")]
    TMP_Text Role_Text;

    [SerializeField, Tooltip("Hero Basic Info Box")]
    HeroInfoBox Hero_Info_Box;

    BattleUnitData User_Hero_Battle_Data;

    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);

        if (data.Length != 1 || data[0] is not BattleUnitData)
        {
            Debug.Assert(false, $"잘못된 HeroInfoUI 팝업 호출!!");
            HidePopup();
            return;
        }

        User_Hero_Battle_Data = data[0] as BattleUnitData;
        Hero_Info_Box.SetHeroData(User_Hero_Battle_Data);

        FixedUpdatePopup();
        Refresh();
    }

    public void Refresh()
    {
        var Hero_Base_Data = (Player_Character_Data)User_Hero_Battle_Data.GetUnitData();
        var Unit_Data = (UserHeroData)User_Hero_Battle_Data.GetUserUnitData();

        Title.text = ConstString.HeroInfoUI.TITLE;

        Level_Text.text = $"LV. {User_Hero_Battle_Data.GetLevel()}";
        Name_Text.text = Hero_Base_Data.name_kr;

        int star_grade = Unit_Data.GetStarGrade();
        for (int i = 0; i < Star_Images.Count; ++i)
        {
            Star_Images[i].SetActive(i < star_grade);
        }

        // 포지션 태그 설정
        int index = (int)User_Hero_Battle_Data.GetPositionType() - 1;
        Position_Tag.sprite = Position_Tag_Sprites[index];

        // 종족 태그 설정
        index = (int)User_Hero_Battle_Data.GetTribeType() - 1;
        Tribe_Box.sprite = Tribe_Box_Sprites[index];
        Tribe_Tag.sprite = Tribe_Tag_Sprites[index];
        Tribe_Text.text = ConstString.Hero.TRIBES[(int)Hero_Base_Data.tribe_type];

        // 역할 태그 설정
        index = (int)Hero_Base_Data.role_type - 1;
        Role_Icon.sprite = Role_Icon_Sprites[index];
        Role_Text.text = ConstString.Hero.ROLE[(int)Hero_Base_Data.role_type];

        Hero_Info_Box.Refresh();
    }

    public override void Spawned()
    {
        base.Spawned();
        InfoBox_Tab_Controller.SelectFirstTab();

        // 레벨업 탭과 승급 탭은 일단 막아놓습니다.
        InfoBox_Tab_Controller.GetTab(1).SetBlockTab(true);
        InfoBox_Tab_Controller.GetTab(2).SetBlockTab(true);
    }

    public void OnClickProfileButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Hero/ProfilePopup", (popup) =>
        {
            popup.ShowPopup((Player_Character_Data)User_Hero_Battle_Data.GetUnitData());
        });
    }

    public void OnClickMemorialButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }

    public void OnClickBack()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.RemoveLastPopupType(POPUP_TYPE.FULLPAGE_TYPE);
    }

    protected override void FixedUpdatePopup()
    {
        var skill_manager = new BattleSkillManager();
        skill_manager.SetPlayerCharacterSkillGroups(User_Hero_Battle_Data.GetSkillPattern());
        skill_manager.SetPlayerCharacterSpecialSkillGroup(User_Hero_Battle_Data.GetSpecialSkillID());

        foreach(var skill_group in skill_manager.Skill_Groups)
        {
            //PreloadSprite(skill_group.GetSkillIconPath());
        }
    }

}
