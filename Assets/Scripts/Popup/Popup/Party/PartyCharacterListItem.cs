using Cysharp.Text;
using FluffyDuck.Util;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PartyCharacterListItem : MonoBehaviour
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    [SerializeField, Tooltip("Button")]
    UIInteractiveButton Button;

    [SerializeField, Tooltip("Attribute BG")]
    Image Attribute_BG;

    [SerializeField, Tooltip("Filter Text")]
    TMP_Text Filter_Text;

    

    public UnityEvent<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> Click_Hero_Callback => Button.Touch_Tooltip_Callback;

    BattlePcData Unit_Data;
    UserHeroData User_Data;

    GAME_TYPE Game_Type = GAME_TYPE.NONE;
    CHARACTER_SORT Filter_Type = CHARACTER_SORT.NAME;

    //public void SetUserHeroData(UserHeroData ud, GAME_TYPE gtype, CHARACTER_SORT ftype)
    //{
    //    User_Data = ud;
    //    Game_Type = gtype;
    //    Filter_Type = ftype;
    //    if (User_Data != null)
    //    {
    //        Unit_Data = new BattlePcData();
    //        Unit_Data.SetUnitID(User_Data.GetPlayerCharacterID(), User_Data.Player_Character_Num);
    //    }
    //    else
    //    {
    //        Unit_Data = null;
    //    }

    //    UpdateCellItem();

    //    if (Button != null)
    //    {
    //        Button.Tooltip_Data = this;
    //    }
    //}

    public void SetBattleUnitData(BattlePcData unit_data, GAME_TYPE gtype, CHARACTER_SORT ftype)
    {
        Unit_Data = unit_data;
        Filter_Type = ftype;
        Game_Type = gtype;
        if (unit_data != null)
        {
            User_Data = (UserHeroData)unit_data.GetUserUnitData();
        }
        UpdateCellItem_V2();

        if (Button != null)
        {
            Button.Tooltip_Data = this;
        }
    }

    //public UserHeroData GetUserHeroData()
    //{
    //    if (User_Data == null)
    //    {
    //        return (UserHeroData)Unit_Data.GetUserUnitData();
    //    }
    //    return User_Data;
    //}

    public BattlePcData GetBattleHeroData()
    {
        return Unit_Data;
    }

    //public void UpdateCellItem()
    //{
    //    if (User_Data == null)
    //    {
    //        Box.gameObject.SetActive(false);
    //        return;
    //    }
    //    if (!Box.gameObject.activeSelf)
    //    {
    //        Box.gameObject.SetActive(true);
    //    }
    //    var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
    //    var deck = deck_mng.FindSelectedDeck(Game_Type);

    //    Card.SetHeroData(Unit_Data, Filter_Type);

    //    //  select
    //    Card.SetPartySelect(deck.IsExistHeroInDeck(User_Data));
    //}

    public void UpdateCellItem_V2()
    {
        if (Unit_Data == null)
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

        Card.SetHeroData(Unit_Data, Filter_Type);

        //  select check
        Card.SetPartySelect(deck.IsExistHeroInDeck(Unit_Data.GetUnitID(), Unit_Data.GetUnitNum()));
    }
}
