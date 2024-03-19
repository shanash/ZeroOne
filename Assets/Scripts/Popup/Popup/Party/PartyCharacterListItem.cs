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

        Card.SetHeroData(Unit_Data, Filter_Type);

        //  select
        Card.SetPartySelect(deck.IsExistHeroInDeck(User_Data));
    }
}
