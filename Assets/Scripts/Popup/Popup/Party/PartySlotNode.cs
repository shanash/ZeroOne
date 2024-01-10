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


    UserHeroDeckMountData User_Data;
    UserHeroData User_Hero;

    System.Action<PartySlotNode> Click_Callback;

    public void SetUserHeroDeckMountData(UserHeroDeckMountData ud)
    {
        User_Data = ud;
        if (User_Data != null)
        {
            User_Hero = GameData.Instance.GetUserHeroDataManager().FindUserHeroData(User_Data.Player_Character_ID, User_Data.Player_Character_Num);
        }
        UpdatePartySlot();
    }

    public UserHeroDeckMountData GetUserHeroDeckMountData()
    {
        return User_Data;
    }

    public void UpdatePartySlot()
    {
        if (User_Data == null)
        {
            Card.gameObject.SetActive(false);
            return;
        }

        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);
        Card.gameObject.SetActive(deck.IsExistHeroInDeck(User_Data.Player_Character_ID, User_Data.Player_Character_Num));

        // card update
        Card.SetHeroDataID(User_Data.Player_Character_ID);

        if (User_Hero != null)
        {
            //  level
            Card.SetLevel(User_Hero.GetLevel());
            //  star
            Card.SetStarGrade(User_Hero.GetStarGrade());
            //  role type
            Card.SetRoleType(User_Hero.GetPlayerCharacterData().role_type);
        }
    }

    public void SetSlotCardChoiceCallback(System.Action<PartySlotNode> cb)
    {
        Click_Callback = cb;
    }

    private void OnEnable()
    {
        if (Card != null)
        {
            Card.AddTouchEventCallback(TouchEventCallback);
        }
    }

    private void OnDisable()
    {
        if (Card != null)
        {
            Card.RemoveTouchEventCallback(TouchEventCallback);
        }
    }

    void TouchEventCallback(TOUCH_RESULT_TYPE result)
    {
        if (result == TOUCH_RESULT_TYPE.CLICK)
        {
            if (User_Data == null)
            {
                return;
            }
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
            Click_Callback?.Invoke(this);
        }
    }




}
