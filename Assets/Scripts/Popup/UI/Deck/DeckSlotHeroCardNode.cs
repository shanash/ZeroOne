using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 덱 세팅된 영웅들의 리스트를 보여주기 위한 카드 노드
/// </summary>
public class DeckSlotHeroCardNode : UIBase, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField, Tooltip("Box")]
    Image Box;

    [SerializeField, Tooltip("Card Box")]
    RectTransform Card_Box;

    [SerializeField, Tooltip("Hero Card Base")]
    HeroCardBase Card;

    [SerializeField, Tooltip("Leader Icon")]
    Image Leader_Icon;

    UserHeroDeckMountData User_Data;

    System.Action<DeckSlotHeroCardNode> Click_Callback;

    Vector2 Press_Scale = new Vector2(0.95f, 0.95f);

    /// <summary>
    /// 사용자 덱 영웅 정보
    /// </summary>
    /// <param name="ud"></param>
    public void SetUserHeroDeckMountData(UserHeroDeckMountData ud)
    {
        User_Data = ud;
        UpdateDeckSlot();
    }

    public UserHeroDeckMountData GetUserHeroDeckMountData()
    {
        return User_Data;
    }

    /// <summary>
    /// 덱 슬롯 업데이트
    /// </summary>
    public void UpdateDeckSlot()
    {
        if (User_Data == null)
        {
            Card_Box.gameObject.SetActive(false);
            return;
        }
        
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);
        Card_Box.gameObject.SetActive(deck.IsExistHeroInDeck(User_Data.Player_Character_ID, User_Data.Player_Character_Num));

        //  card update
        Card.SetHeroDataID(User_Data.Player_Character_ID);

        //  leader icon
        Leader_Icon.gameObject.SetActive(User_Data.Is_Leader);
    }

    public void SetSlotCardChoiceCallback(System.Action<DeckSlotHeroCardNode> cb)
    {
        Click_Callback = cb;
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
        Box.transform.localScale = Press_Scale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (User_Data == null)
        {
            return;
        }
        Box.transform.localScale = Vector2.one;
    }
}
