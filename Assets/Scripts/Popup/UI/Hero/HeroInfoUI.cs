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

    List<UserHeroData> User_Hero_Datas;
    int Current_Hero_Data_Index;
    BattlePcData User_Hero_Battle_Data;

    void Reset()
    {
        User_Hero_Datas = null;
        Current_Hero_Data_Index = -1;
        User_Hero_Battle_Data = null;
    }

    protected override bool Initialize(object[] data)
    {
        Reset();

        if (data.Length != 2 || data[0] is not List<UserHeroData> || data[1] is not int)
        {
            return false;
        }

        User_Hero_Datas = data[0] as List<UserHeroData>;
        SetUserHero((int)data[1]);
        FixedUpdatePopup();

        return true;
    }

    void SetUserHero(int hero_data_index)
    {
        Current_Hero_Data_Index = hero_data_index;

        var user_hero_data = User_Hero_Datas[Current_Hero_Data_Index];

        User_Hero_Battle_Data = new BattlePcData();
        User_Hero_Battle_Data.SetUnitID(user_hero_data.GetPlayerCharacterID(), user_hero_data.Player_Character_Num);

        Hero_Info_Box.SetHeroData(User_Hero_Battle_Data);
    }

    protected override void FixedUpdatePopup()
    {
        Title.text = ConstString.HeroInfoUI.TITLE;

        UpdatePopup();
    }

    public override void UpdatePopup()
    {
        base.UpdatePopup();

        var Hero_Base_Data = (Player_Character_Data)User_Hero_Battle_Data.GetUnitData();
        var Unit_Data = (UserHeroData)User_Hero_Battle_Data.GetUserUnitData();

        Level_Text.text = $"LV. {User_Hero_Battle_Data.GetLevel()}";
        Name_Text.text = GameDefine.GetLocalizeString(Hero_Base_Data.name_id);

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

    public void OnClickProfileButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Hero/ProfilePopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup((Player_Character_Data)User_Hero_Battle_Data.GetUnitData());
        });
    }

    public void OnClickMemorialButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }

    public void OnClickBack()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.RemoveLastPopupType(POPUP_TYPE.FULLPAGE_TYPE);
    }

    public void OnClickLeft()
    {
        int index = Current_Hero_Data_Index - 1;
        if (index < 0)
        {
            index = User_Hero_Datas.Count -1;
        }

        SetUserHero(index);
        UpdatePopup();
    }

    public void OnClickRight()
    {
        int index = Current_Hero_Data_Index + 1;
        if (index == User_Hero_Datas.Count)
        {
            index = 0;
        }

        SetUserHero(index);
        UpdatePopup();
    }
}
