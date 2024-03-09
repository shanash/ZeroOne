using Cysharp.Text;
using FluffyDuck.Util;
using UnityEngine;
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

    GAME_TYPE Game_Type = GAME_TYPE.NONE;

    UserHeroDeckMountData User_Deck;
    UserHeroData User_Hero;

    System.Action<PartySlotNode> Click_Callback;

    public void SetUserHeroDeckMountData(UserHeroDeckMountData ud, GAME_TYPE gtype)
    {
        User_Deck = ud;
        Game_Type = gtype;
        if (User_Deck != null)
        {
            User_Hero = GameData.Instance.GetUserHeroDataManager().FindUserHeroData(User_Deck.Player_Character_ID, User_Deck.Player_Character_Num);
        }
        UpdatePartySlot();
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

    public void SetSlotCardChoiceCallback(System.Action<PartySlotNode> cb)
    {
        Click_Callback = cb;
    }

    public void TouchEventCallback(TOUCH_RESULT_TYPE result)
    {
        if (result == TOUCH_RESULT_TYPE.CLICK)
        {
            if (User_Deck == null)
            {
                return;
            }
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
            Click_Callback?.Invoke(this);
        }
    }
}
