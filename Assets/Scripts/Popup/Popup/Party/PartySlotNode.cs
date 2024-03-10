using Cysharp.Text;
using FluffyDuck.Util;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PartySlotNode : MonoBehaviour
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Empty Box")]
    Image Empty_Box;

    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    [SerializeField, Tooltip("Team Synergy Attribute Icon")]
    Image Team_Synergy_Icon;

    [SerializeField, Tooltip("Tooltip Button")]
    UIInteractiveButton Button;

    GAME_TYPE Game_Type = GAME_TYPE.NONE;

    UserHeroDeckMountData User_Deck;
    UserHeroData User_Hero;

    public UnityEvent<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> Click_Callback => Button.Touch_Tooltip_Callback;

    public void SetUserHeroDeckMountData(UserHeroDeckMountData ud, GAME_TYPE gtype)
    {
        User_Deck = ud;
        Game_Type = gtype;
        if (User_Deck != null)
        {
            User_Hero = GameData.Instance.GetUserHeroDataManager().FindUserHeroData(User_Deck.Player_Character_ID, User_Deck.Player_Character_Num);
        }
        UpdatePartySlot();
        if (Button != null)
        {
            Button.Tooltip_Data = this;
        }
    }

    public UserHeroDeckMountData GetUserHeroDeckMountData()
    {
        return User_Deck;
    }

    public void UpdatePartySlot()
    {
        if (User_Deck == null)
        {
            Card.gameObject.SetActive(false);
            return;
        }
        var m = MasterDataManager.Instance;
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Game_Type);
        Card.gameObject.SetActive(deck.IsExistHeroInDeck(User_Deck.Player_Character_ID, User_Deck.Player_Character_Num));

        // card update
        Card.SetHeroDataID(User_Deck.Player_Character_ID);

        if (User_Hero != null)
        {
            //  level
            Card.SetLevel(User_Hero.GetLevel());
            //  star
            Card.SetStarGrade(User_Hero.GetStarGrade());
            //  role type
            Card.SetRoleType(User_Hero.GetPlayerCharacterData().role_type);
            //  team synergy
            var attr_data = m.Get_AttributeIconData(User_Hero.GetAttributeType());
            string icon_path = attr_data.icon;
            if (!deck.IsExistTeamSynergyAttribute(attr_data.attribute_type))
            {
                icon_path = ZString.Format("{0}_Grey", attr_data.icon);
            }

            CommonUtils.GetResourceFromAddressableAsset<Sprite>(icon_path, (spr) =>
            {
                Team_Synergy_Icon.sprite = spr;
            });
        }
    }
}
