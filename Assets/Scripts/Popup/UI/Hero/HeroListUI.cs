using UnityEngine;
using FluffyDuck.UI;
using Gpm.Ui;
using System.Collections.Generic;
using FluffyDuck.Util;
using TMPro;

public class HeroListUI : PopupBase
{
    [SerializeField]
    TMP_Text Title;

    [SerializeField, Tooltip("Character List View")]
    InfiniteScroll Character_LIst_View;

    [SerializeField, Tooltip("Filter Name")]
    TMP_Text Filter_Name;

    [SerializeField, Tooltip("Filter Arrow Image's RectTransform By Sort Direction")]
    RectTransform Filter_Sort_Direction_Image;

    List<BattlePcData> Battle_Hero_Data_List;
    CHARACTER_SORT Filter_Type;
    SORT_ORDER Sort_Order = SORT_ORDER.ASC;

    bool Is_Ascended_Sort => Sort_Order == SORT_ORDER.ASC;

    void Reset()
    {
        Battle_Hero_Data_List = null;
        Filter_Type = CHARACTER_SORT.NAME;
        Sort_Order = SORT_ORDER.ASC;
    }

    protected override bool Initialize(object[] data)
    {
        Filter_Type = (CHARACTER_SORT)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, CHARACTER_SORT.NAME);
        FixedUpdatePopup();

        if (data.Length == 1 && data[0] is BattlePcData)
        {
            var pc_data = data[0] as BattlePcData;
            int index = Battle_Hero_Data_List.FindIndex(x => x.User_Data.Player_Character_Num == pc_data.User_Data.Player_Character_Num);

            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Hero/HeroInfoUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
            {
                popup.ShowPopup(Battle_Hero_Data_List, index, 3);
            });
        }

        return true;
    }

    protected override void FixedUpdatePopup()
    {
        Title.text = ConstString.HeroListUI.TITLE;
        UpdatePopup();
    }

    public override void UpdatePopup()
    {
        base.UpdatePopup();

        Character_LIst_View.Clear();

        const int column_count = 5;
        var gd = GameData.Instance;
        var hero_mng = gd.GetUserHeroDataManager();
        var user_hero_datas = new List<UserHeroData>();
        hero_mng.GetUserHeroDataList(ref user_hero_datas);

        Battle_Hero_Data_List = new List<BattlePcData>();

        foreach (var data in user_hero_datas)
        {
            var battle_pc_data = new BattlePcData();
            battle_pc_data.SetUnitID(data.GetPlayerCharacterID(), data.Player_Character_Num);

            Battle_Hero_Data_List.Add(battle_pc_data);
        }

        switch (Filter_Type)
        {
            case CHARACTER_SORT.NAME:
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetUnitName().CompareTo(b.GetUnitName()) :
                    b.GetUnitName().CompareTo(a.GetUnitName());
                });
                break;
            case CHARACTER_SORT.LEVEL_CHARACTER:
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetLevel().CompareTo(b.GetLevel()) :
                    b.GetLevel().CompareTo(a.GetLevel());
                });
                break;
            case CHARACTER_SORT.STAR:
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetStarGrade().CompareTo(b.GetStarGrade()) :
                    b.GetStarGrade().CompareTo(a.GetStarGrade());
                });
                break;
            case CHARACTER_SORT.DESTINY:
                Debug.Assert(false);
                break;
            case CHARACTER_SORT.SKILL_LEVEL:
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetNormalSkillLevelSum().CompareTo(b.GetNormalSkillLevelSum()) :
                    b.GetNormalSkillLevelSum().CompareTo(a.GetNormalSkillLevelSum());
                });
                break;
            case CHARACTER_SORT.EX_SKILL_LEVEL:
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetSpecialSkillLevel().CompareTo(b.GetSpecialSkillLevel()) :
                    b.GetSpecialSkillLevel().CompareTo(a.GetSpecialSkillLevel());
                });
                break;
            case CHARACTER_SORT.ATTACK:
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetTotalAttackPoint().CompareTo(b.GetTotalAttackPoint()) :
                    b.GetTotalAttackPoint().CompareTo(a.GetTotalAttackPoint());
                });
                break;
            case CHARACTER_SORT.DEFEND:
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetTotalDefensePoint().CompareTo(b.GetTotalDefensePoint()) :
                    b.GetTotalDefensePoint().CompareTo(a.GetTotalDefensePoint());
                });
                break;
            case CHARACTER_SORT.RANGE:
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetApproachDistance().CompareTo(b.GetApproachDistance()) :
                    b.GetApproachDistance().CompareTo(a.GetApproachDistance());
                });
                break;
            case CHARACTER_SORT.LIKEABILITY:
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.User_Data.GetLoveLevel().CompareTo(b.User_Data.GetLoveLevel()) :
                    b.User_Data.GetLoveLevel().CompareTo(a.User_Data.GetLoveLevel());
                });
                break;
            case CHARACTER_SORT.ATTRIBUTE:
                //  전기/베리타리움/요력/마력(오름 차순)
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetAttributeType().CompareTo(b.GetAttributeType()) :
                    b.GetAttributeType().CompareTo(a.GetAttributeType());
                });
                break;
            case CHARACTER_SORT.BATTLEPOWER:
                Battle_Hero_Data_List.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetCombatPoint().CompareTo(b.GetCombatPoint()) :
                    b.GetCombatPoint().CompareTo(a.GetCombatPoint());
                });
                break;
        }

        //  가로 컬럼 5개(보유중인 영웅 리스트 불러오기)
        int start = 0;
        int hero_count = Battle_Hero_Data_List.Count;
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
                new_data.SetUserHeroDataList(Battle_Hero_Data_List.GetRange(start, column_count));
            }
            else
            {
                new_data.SetUserHeroDataList(Battle_Hero_Data_List.GetRange(start, hero_count - start));
            }
            Character_LIst_View.InsertData(new_data);
        }

        UpdateFilterType();
    }

    public override void Spawned()
    {
        base.Spawned();
        Reset();
    }

    /// <summary>
    /// 영웅 리스트에서 영웅 선택시 콜백
    /// </summary>
    /// <param name="ud"></param>
    void SelectCharacterCallback(HeroListItem hero)
    {
        int index = Battle_Hero_Data_List.IndexOf(hero.BattlePcData);

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Hero/HeroInfoUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
        {
            popup.ShowPopup(Battle_Hero_Data_List, index);
        });
    }

    void UpdateFilterType()
    {
        Filter_Name.text = GameDefine.GetFilterString(Filter_Type);
        Filter_Sort_Direction_Image.eulerAngles = Is_Ascended_Sort ? Vector3.zero : new Vector3(0, 0, 180);
    }

    public void OnClickFilterPopup()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/FilterPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
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

        Sort_Order = (Sort_Order == SORT_ORDER.ASC) ? SORT_ORDER.DESC : SORT_ORDER.ASC;

        UpdatePopup();
    }

    public override void OnEnter()
    {
        Character_LIst_View.UpdateAllData();
    }
}
