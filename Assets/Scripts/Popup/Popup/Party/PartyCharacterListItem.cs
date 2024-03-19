using Cysharp.Text;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PartyCharacterListItem : MonoBehaviour
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    [SerializeField, Tooltip("Button")]
    UIInteractiveButton Button;

    [SerializeField, Tooltip("Filter Text")]
    TMP_Text Filter_Text;

    BattlePcData Unit_Data;

    public UnityEvent<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> Click_Hero_Callback => Button.Touch_Tooltip_Callback;

    UserHeroData User_Data;

    GAME_TYPE Game_Type = GAME_TYPE.NONE;
    CHARACTER_SORT Filter_Type = CHARACTER_SORT.NAME;

    public void SetUserHeroData(UserHeroData ud, GAME_TYPE gtype, CHARACTER_SORT ftype)
    {
        User_Data = ud;
        Game_Type = gtype;
        Filter_Type = ftype;
        if (User_Data != null)
        {
            Unit_Data = new BattlePcData();
            Unit_Data.SetUnitID(User_Data.GetPlayerCharacterID(), User_Data.Player_Character_Num);
        }
        else
        {
            Unit_Data = null;
        }
        
        UpdateCellItem();

        if (Button != null)
        {
            Button.Tooltip_Data = this;
        }
    }

    public UserHeroData GetUserHeroData()
    {
        return User_Data;
    }

    public void UpdateCellItem()
    {
        if (User_Data == null)
        {
            Box.gameObject.SetActive(false);
            return;
        }
        if (!Box.gameObject.activeSelf)
        {
            Box.gameObject.SetActive(true);
        }
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Game_Type);

        Card.SetHeroDataID(User_Data.GetPlayerCharacterID());

        //  select
        Card.SetPartySelect(deck.IsExistHeroInDeck(User_Data));

        //  level
        Card.SetLevel(User_Data.GetLevel());
        //  star
        Card.SetStarGrade(User_Data.GetStarGrade());
        //  role type
        Card.SetRoleType(User_Data.GetPlayerCharacterData().role_type);

        //  filter text
        Filter_Text.text = GetFilterString(Filter_Type);
    }

    string GetFilterString(CHARACTER_SORT ftype)
    {
        string filter = string.Empty;
        switch (ftype)
        {
            case CHARACTER_SORT.NAME:
                filter = Unit_Data.GetUnitName();
                break;
            case CHARACTER_SORT.LEVEL_CHARACTER:
                filter = ZString.Format("Lv.{0}", Unit_Data.GetLevel());
                break;
            case CHARACTER_SORT.STAR:
                filter = User_Data.GetStarGrade().ToString();
                break;
            case CHARACTER_SORT.DESTINY:
                filter = "0";
                break;
            case CHARACTER_SORT.SKILL_LEVEL:
                filter = Unit_Data.GetSumSkillsLevel().ToString();
                break;
            case CHARACTER_SORT.EX_SKILL_LEVEL:
                filter = "0";
                break;
            case CHARACTER_SORT.ATTACK:
                filter = Unit_Data.GetTotalAttackPoint().ToString("N0");
                break;
            case CHARACTER_SORT.DEFEND:
                filter = Unit_Data.GetTotalDefensePoint().ToString("N0");
                break;
            case CHARACTER_SORT.RANGE:
                filter = Unit_Data.GetApproachDistance().ToString();
                break;
            case CHARACTER_SORT.LIKEABILITY:
                filter = ZString.Format("Lv.{0}", User_Data.GetLoveLevel());
                break;
            default:
                Debug.Assert(false);
                break;
        }

        return filter;
    }
}
