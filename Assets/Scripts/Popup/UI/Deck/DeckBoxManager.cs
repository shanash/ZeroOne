using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBoxManager : MonoBehaviour
{
    [SerializeField, Tooltip("Deck Slot Card")]
    List<DeckSlotHeroCardNode> Card_Slots;

    GAME_TYPE Game_Type = GAME_TYPE.NONE;

    public void SetGameType(GAME_TYPE gtype)
    {
        Game_Type = gtype;
    }

    /// <summary>
    /// 팝업의 로드가 모두 완료된 후 덱 구성을 초기화 해준다.
    /// </summary>
    public void UpdateUserDeckUpdate()
    {
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Game_Type);
        var hero_list = deck.GetDeckHeroes();
        
        int cnt = Card_Slots.Count;
        for (int i = 0; i < cnt; i++)
        {
            var slot = Card_Slots[i];
            if (i < hero_list.Count)
            {
                slot.SetUserHeroDeckMountData(hero_list[i]);
            }
            else
            {
                slot.SetUserHeroDeckMountData(null);
            }
        }
    }

    public void SetSlotCardChoiceCallback(System.Action<DeckSlotHeroCardNode> cb)
    {
        int cnt = Card_Slots.Count;
        for (int i = 0; i < cnt; i++)
        {
            Card_Slots[i].SetSlotCardChoiceCallback(cb);
        }
    }
}
