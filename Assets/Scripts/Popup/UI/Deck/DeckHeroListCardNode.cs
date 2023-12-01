using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 덱 구성 페이지에서 영웅의 리스트를 보여줄 카드 노드
/// </summary>
public class DeckHeroListCardNode : UIBase, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField, Tooltip("Card Box")]
    RectTransform Card_Box;

    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    [SerializeField, Tooltip("Card Dim & Check")]
    Image Card_Dim_Check;

    Vector2 Press_Scale = new Vector2(0.95f, 0.95f);


    UserHeroData User_Data;

    System.Action<DeckHeroListCardNode> Click_Callback;

    public void SetUserHeroData(UserHeroData ud)
    {
        User_Data = ud;
        UpdateCardNode();
    }

    public void UpdateCardNode()
    {
        if (User_Data == null)
        {
            Card.gameObject.SetActive(false);
            Card_Dim_Check.gameObject.SetActive(false);
            return;
        }
        if (!Card.gameObject.activeSelf)
        {
            Card.gameObject.SetActive(true);
        }
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);
        Card_Dim_Check.gameObject.SetActive(deck.IsExistHeroInDeck(User_Data));

        Card.SetHeroDataID(User_Data.GetPlayerCharacterID());
    }

    public UserHeroData GetUserHeroData()
    {
        return User_Data;
    }


    public bool IsSelected()
    {
        if (User_Data == null)
        {
            return false;
        }
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);

        return deck.IsExistHeroInDeck(User_Data);
    }

    public void SetClickCallback(System.Action<DeckHeroListCardNode> cb)
    {
        Click_Callback = cb;
    }

    public bool IsEqualUserHeroData(UserHeroData ud)
    {
        if (User_Data == null)
        {
            return false;
        }
        return User_Data.IsEquals(ud);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (User_Data == null)
        {
            return;
        }
        Click_Callback?.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (User_Data == null)
        {
            return;
        }
        Card_Box.localScale = Press_Scale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (User_Data == null)
        {
            return;
        }
        Card_Box.localScale = Vector2.one;
    }
}
