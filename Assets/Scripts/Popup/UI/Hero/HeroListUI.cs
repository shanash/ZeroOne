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

    List<BattlePcData> Battle_Pc_Data;
    CHARACTER_SORT Filter_Type;
    bool Is_Ascended_Sort;

    void Reset()
    {
        Battle_Pc_Data = null;
        Filter_Type = CHARACTER_SORT.NAME;
        Is_Ascended_Sort = true;
    }

    protected override bool Initialize(object[] data)
    {
        Filter_Type = (CHARACTER_SORT)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, CHARACTER_SORT.NAME);
        FixedUpdatePopup();

        if (data.Length == 1 && data[0] is BattlePcData)
        {
            var pc_data = data[0] as BattlePcData;
            int index = Battle_Pc_Data.FindIndex(x => x.User_Data.Player_Character_Num == pc_data.User_Data.Player_Character_Num);

            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Hero/HeroInfoUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
            {
                popup.ShowPopup(Battle_Pc_Data, index, 3);
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

        Battle_Pc_Data = new List<BattlePcData>();

        foreach (var data in user_hero_datas)
        {
            var battle_pc_data = new BattlePcData();
            battle_pc_data.SetUnitID(data.GetPlayerCharacterID(), data.Player_Character_Num);

            Battle_Pc_Data.Add(battle_pc_data);
        }

        switch (Filter_Type)
        {
            case CHARACTER_SORT.NAME:
                Battle_Pc_Data.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    GameDefine.GetLocalizeString(a.Data.name_id).CompareTo(GameDefine.GetLocalizeString(b.Data.name_id)) :
                    GameDefine.GetLocalizeString(b.Data.name_id).CompareTo(GameDefine.GetLocalizeString(a.Data.name_id));
                });
                break;
            case CHARACTER_SORT.LEVEL_CHARACTER:
                Battle_Pc_Data.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetLevel().CompareTo(b.GetLevel()) :
                    b.GetLevel().CompareTo(a.GetLevel());
                });
                break;
            case CHARACTER_SORT.STAR:
                Battle_Pc_Data.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetStarGrade().CompareTo(b.GetStarGrade()) :
                    b.GetStarGrade().CompareTo(a.GetStarGrade());
                });
                break;
            case CHARACTER_SORT.DESTINY:
                // TODO: 미구현
                break;
            case CHARACTER_SORT.SKILL_LEVEL:
                /*
                Battle_Pc_Data.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetSumSkillsLevel().CompareTo(b.GetSumSkillsLevel()) :
                    b.GetSumSkillsLevel().CompareTo(a.GetSumSkillsLevel());
                });
                */
                break;
            case CHARACTER_SORT.EX_SKILL_LEVEL:
                /*
                Battle_Pc_Data.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.Skill_Mng.GetSpecialSkillGroup().GetSkillLevel().CompareTo(b.Skill_Mng.GetSpecialSkillGroup().GetSkillLevel()) :
                    b.Skill_Mng.GetSpecialSkillGroup().GetSkillLevel().CompareTo(a.Skill_Mng.GetSpecialSkillGroup().GetSkillLevel());
                });
                */
                break;
            case CHARACTER_SORT.ATTACK:
                Battle_Pc_Data.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    (a.GetPhysicsAttackPoint() + a.GetMagicAttackPoint()).CompareTo(b.GetPhysicsAttackPoint() + b.GetMagicAttackPoint()) :
                    (b.GetPhysicsAttackPoint() + b.GetMagicAttackPoint()).CompareTo(a.GetPhysicsAttackPoint() + a.GetMagicAttackPoint());
                });
                break;
            case CHARACTER_SORT.DEFEND:
                Battle_Pc_Data.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    (a.GetPhysicsDefensePoint() + a.GetMagicDefensePoint()).CompareTo(b.GetPhysicsDefensePoint() + b.GetMagicDefensePoint()) :
                    (b.GetPhysicsDefensePoint() + b.GetMagicDefensePoint()).CompareTo(a.GetPhysicsDefensePoint() + a.GetMagicDefensePoint());
                });
                break;
            case CHARACTER_SORT.RANGE:
                Battle_Pc_Data.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetApproachDistance().CompareTo(b.GetApproachDistance()) :
                    b.GetApproachDistance().CompareTo(a.GetApproachDistance());
                });
                break;
            case CHARACTER_SORT.LIKEABILITY:
                Battle_Pc_Data.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.User_Data.GetLoveLevel().CompareTo(b.User_Data.GetLoveLevel()) :
                    b.User_Data.GetLoveLevel().CompareTo(a.User_Data.GetLoveLevel());
                });
                break;
        }

        //  가로 컬럼 5개(보유중인 영웅 리스트 불러오기)
        int start = 0;
        int hero_count = Battle_Pc_Data.Count;
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
                new_data.SetUserHeroDataList(Battle_Pc_Data.GetRange(start, column_count));
            }
            else
            {
                new_data.SetUserHeroDataList(Battle_Pc_Data.GetRange(start, hero_count - start));
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
        int index = Battle_Pc_Data.IndexOf(hero.BattlePcData);

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Hero/HeroInfoUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
        {
            popup.ShowPopup(Battle_Pc_Data, index);
        });
    }

    void UpdateFilterType()
    {
        Filter_Name.text = ConstString.Hero.SORT_FILLTER[(int)Filter_Type];
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

        Is_Ascended_Sort = !Is_Ascended_Sort;
        Debug.Log($"Is_Ascended_Sort : {Is_Ascended_Sort}");

        UpdatePopup();
    }
}
