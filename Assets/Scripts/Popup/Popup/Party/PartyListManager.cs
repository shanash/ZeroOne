using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartyListManager : MonoBehaviour
{
    [SerializeField, Tooltip("Party Slots")]
    List<PartySlotNode> Party_Slots;

    GAME_TYPE Game_Type = GAME_TYPE.NONE;

    public void SetGameType(GAME_TYPE gtype)
    {
        Game_Type = gtype;
    }

    /// <summary>
    /// 덱 구성 초기화 해준다.
    /// </summary>
    public void UpdateUserPartySettings()
    {
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Game_Type);
        var hero_list = deck.GetDeckHeroes();

        int cnt = Party_Slots.Count;
        for (int i = 0; i < cnt; i++)
        {
            var slot = Party_Slots[i];
            if (i < hero_list.Count)
            {
                slot.SetUserHeroDeckMountData(hero_list[i], Game_Type);
            }
            else
            {
                slot.SetUserHeroDeckMountData(null, Game_Type);
            }
        }
    }

    public void SetSlotCardChoiceCallback(UnityAction<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> cb)
    {
        int cnt = Party_Slots.Count;
        for (int i = 0; i < cnt; i++)
        {
            Party_Slots[i].Click_Callback.AddListener(cb);
        }
    }

}
