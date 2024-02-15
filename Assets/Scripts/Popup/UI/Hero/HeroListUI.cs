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

    List<UserHeroData> User_Hero_Datas;
    CHARACTER_SORT Filter_Type;
    bool Is_Ascended_Sort;

    void Reset()
    {
        User_Hero_Datas = null;
        Filter_Type = CHARACTER_SORT.NAME;
        Is_Ascended_Sort = true;
    }

    protected override bool Initialize(object[] data)
    {
        Filter_Type = (CHARACTER_SORT)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, CHARACTER_SORT.NAME);
        FixedUpdatePopup();

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
        User_Hero_Datas = new List<UserHeroData>();
        hero_mng.GetUserHeroDataList(ref User_Hero_Datas);

        switch (Filter_Type)
        {
            case CHARACTER_SORT.NAME:
                User_Hero_Datas.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    GameDefine.GetLocalizeString(a.GetPlayerCharacterData().name_id).CompareTo(GameDefine.GetLocalizeString(b.GetPlayerCharacterData().name_id)) :
                    GameDefine.GetLocalizeString(b.GetPlayerCharacterData().name_id).CompareTo(GameDefine.GetLocalizeString(a.GetPlayerCharacterData().name_id));
                });
                break;
            case CHARACTER_SORT.LEVEL_CHARACTER:
                User_Hero_Datas.Sort((a, b) =>
                {
                    return
                    Is_Ascended_Sort ?
                    a.GetLevel().CompareTo(b.GetLevel()) :
                    b.GetLevel().CompareTo(a.GetLevel());
                });
                User_Hero_Datas.Sort((a, b) => a.GetLevel().CompareTo(b.GetLevel()));
                break;
            case CHARACTER_SORT.STAR:
                User_Hero_Datas.Sort((a, b) =>
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
        int hero_count = User_Hero_Datas.Count;
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
                new_data.SetUserHeroDataList(User_Hero_Datas.GetRange(start, column_count));
            }
            else
            {
                new_data.SetUserHeroDataList(User_Hero_Datas.GetRange(start, hero_count - start));
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
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        int index = User_Hero_Datas.IndexOf(hero.User_Data);

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Hero/HeroInfoUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
        {
            popup.ShowPopup(User_Hero_Datas, index);
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

        UpdatePopup();
    }
}
