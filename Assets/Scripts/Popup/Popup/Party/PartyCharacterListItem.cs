using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartyCharacterListItem : MonoBehaviour
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    System.Action<PartyCharacterListItem> Click_Hero_Callback;

    UserHeroData User_Data;

    GAME_TYPE Game_Type = GAME_TYPE.NONE;

    Vector2 Press_Scale = new Vector2(0.96f, 0.96f);

    public void SetUserHeroData(UserHeroData ud, GAME_TYPE gtype)
    {
        User_Data = ud;
        Game_Type = gtype;
        UpdateCellItem();
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
        
    }

    public void SetClickHeroCallback(System.Action<PartyCharacterListItem> callback)
    {
        Click_Hero_Callback = callback;
    }

    private void OnEnable()
    {
        if (Card != null)
        {
            Card.TouchDown += TouchDownCallback;
            Card.TouchUp += TouchUpCallback;
            Card.Click += ClickCallback;
        }
    }

    private void OnDisable()
    {
        if (Card != null)
        {
            Card.TouchDown -= TouchDownCallback;
            Card.TouchUp -= TouchUpCallback;
            Card.Click -= ClickCallback;
        }
    }

    void TouchDownCallback(PointerEventData evt)
    {
        Box.transform.localScale = Press_Scale;
    }
    void TouchUpCallback(PointerEventData evt)
    {
        Box.transform.localScale = Vector2.one;
    }
    void ClickCallback(PointerEventData evt)
    {
        if (User_Data == null)
        {
            return;
        }
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        Click_Hero_Callback?.Invoke(this);
    }
}
