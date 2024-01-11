using UnityEngine;
using FluffyDuck.UI;
using Gpm.Ui;
using System.Collections.Generic;
using FluffyDuck.Util;
using TMPro;

public class HeroListUI : PopupBase
{
    [SerializeField, Tooltip("Character List View")]
    InfiniteScroll Character_LIst_View;

    [SerializeField, Tooltip("Filter Name")]
    TMP_Text Filter_Name;

    [SerializeField, Tooltip("Filter Arrow Image's RectTransform By Sort Direction")]
    RectTransform Filter_Sort_Direction_Image;

    CHARACTER_SORT Filter_Type;
    bool Is_Ascended_Sort;

    protected override void Initialize()
    {
        base.Initialize();
        Filter_Type = CHARACTER_SORT.NAME;
        Is_Ascended_Sort = true;
    }

    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);

        Filter_Type = (CHARACTER_SORT)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, CHARACTER_SORT.NAME);
        FixedUpdatePopup();
        UpdatePopup();
    }

    protected override void FixedUpdatePopup()
    {
        Character_LIst_View.Clear();

        const int column_count = 5;
        var gd = GameData.Instance;
        var hero_mng = gd.GetUserHeroDataManager();
        List<UserHeroData> user_hero_list = new List<UserHeroData>();
        hero_mng.GetUserHeroDataList(ref user_hero_list);

        switch (Filter_Type)
        {
            case CHARACTER_SORT.NAME:
                user_hero_list.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetPlayerCharacterData().name_kr.CompareTo(b.GetPlayerCharacterData().name_kr) :
                    b.GetPlayerCharacterData().name_kr.CompareTo(a.GetPlayerCharacterData().name_kr);
                });
                break;
            case CHARACTER_SORT.LEVEL_CHARACTER:
                user_hero_list.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetLevel().CompareTo(b.GetLevel()) :
                    b.GetLevel().CompareTo(a.GetLevel());
                });
                user_hero_list.Sort((a, b) => a.GetLevel().CompareTo(b.GetLevel()));
                break;
            case CHARACTER_SORT.STAR:
                user_hero_list.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetStarGrade().CompareTo(b.GetStarGrade()) :
                    b.GetStarGrade().CompareTo(a.GetStarGrade());
                });
                break;
            case CHARACTER_SORT.DESTINY:
                break;
            case CHARACTER_SORT.SKILL_LEVEL:
                break;
            case CHARACTER_SORT.EX_SKILL_LEVEL:
                break;
            case CHARACTER_SORT.ATTACK:
                break;
            case CHARACTER_SORT.DEFEND:
                break;
            case CHARACTER_SORT.RANGE:
                break;
            case CHARACTER_SORT.LIKEABILITY:
                break;
        }

        //  가로 컬럼 5개(보유중인 영웅 리스트 불러오기)
        int start = 0;
        int hero_count = user_hero_list.Count;
        int rows = hero_count / column_count;
        if (hero_count % column_count > 0)
        {
            rows += 1;
        }
        for (int r = 0; r < rows; r++)
        {
            start = r * column_count;

            HeroListData new_data = new HeroListData();
            new_data.Filter_Type = Filter_Type;
            new_data.Click_Hero_Callback = SelectCharacterCallback;

            if (start + column_count < hero_count)
            {
                new_data.SetUserHeroDataList(user_hero_list.GetRange(start, column_count));
            }
            else
            {
                new_data.SetUserHeroDataList(user_hero_list.GetRange(start, hero_count - start));
            }
            Character_LIst_View.InsertData(new_data);
        }

        UpdateFilterType();
    }

    /// <summary>
    /// 영웅 리스트에서 영웅 선택시 콜백
    /// </summary>
    /// <param name="ud"></param>
    void SelectCharacterCallback(HeroListItem hero)
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        var PC_Battle_Data = new BattlePcData();
        PC_Battle_Data.SetUnitID(hero.User_Data.GetPlayerCharacterID(), hero.User_Data.Player_Character_Num);

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Hero/HeroInfoUI", (popup) =>
        {
            popup.ShowPopup(PC_Battle_Data);
        });
    }

    void UpdateFilterType()
    {
        switch (Filter_Type)
        {
            case CHARACTER_SORT.NAME:
                Filter_Name.text = "이름";
                break;
            case CHARACTER_SORT.LEVEL_CHARACTER:
                Filter_Name.text = "레벨";
                break;
            case CHARACTER_SORT.STAR:
                Filter_Name.text = "성급";
                break;
            case CHARACTER_SORT.DESTINY:
                Filter_Name.text = "인연 레벨";
                break;
            case CHARACTER_SORT.SKILL_LEVEL:
                Filter_Name.text = "스킬 레벨";
                break;
            case CHARACTER_SORT.EX_SKILL_LEVEL:
                Filter_Name.text = "궁극 스킬 레벨";
                break;
            case CHARACTER_SORT.ATTACK:
                Filter_Name.text = "공격력";
                break;
            case CHARACTER_SORT.DEFEND:
                Filter_Name.text = "방어력";
                break;
            case CHARACTER_SORT.RANGE:
                Filter_Name.text = "사정거리";
                break;
            case CHARACTER_SORT.LIKEABILITY:
                Filter_Name.text = "호감도";
                break;
        }

        Filter_Sort_Direction_Image.eulerAngles = Is_Ascended_Sort ? Vector3.zero : new Vector3(0, 0, 180);
    }
    public void OnClickFilterPopup()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/FilterPopup", (popup) =>
        {
            popup.SetHideCompleteCallback(() =>
            {
                Filter_Type = GameConfig.Instance.GetGameConfigValue<CHARACTER_SORT>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, CHARACTER_SORT.NAME);
                FixedUpdatePopup();
            });
            popup.ShowPopup();
        });
    }

    public void OnClickSort()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        Is_Ascended_Sort = !Is_Ascended_Sort;

        FixedUpdatePopup();
    }
}
