using Cysharp.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PartyListManager : MonoBehaviour
{
    [SerializeField, Tooltip("Party Slots")]
    List<PartySlotNode> Party_Slots;

    [SerializeField, Tooltip("Party Synerge Slots")]
    List<PartyListSynergeItem> Party_Synerge_Slots;

    [SerializeField, Tooltip("전투력 타이틀")]
    TMP_Text Battle_Point_Title;
    [SerializeField, Tooltip("전투력")]
    TMP_Text Battle_Point;
    [SerializeField, Tooltip("전투력 추가")]
    TMP_Text Battle_Point_Inc;

    GAME_TYPE Game_Type = GAME_TYPE.NONE;

    CHARACTER_SORT Filter_Type = CHARACTER_SORT.NAME;

    public void SetGameType(GAME_TYPE gtype, CHARACTER_SORT ftype)
    {
        Game_Type = gtype;
        Filter_Type = ftype;
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

        cnt = Party_Synerge_Slots.Count;
        for (int i = 0; i < cnt; i++)
        {
            var slot = Party_Synerge_Slots[i];
            if (i < hero_list.Count)
            {
                slot.SetUserHeroDeckMountData(hero_list[i], Game_Type, Filter_Type);
            }
            else
            {
                slot.SetUserHeroDeckMountData(null, Game_Type, Filter_Type);
            }
        }

        //  battle point
        double battle_point = Party_Slots.Sum(x => x.GetBattlePoint());
        Battle_Point.text = battle_point.ToString("N0");

        //  synergy
        var synergy_list = deck.GetTeamSynergyList();
        STAT_MULTIPLE_TYPE mtype = STAT_MULTIPLE_TYPE.ATTACK_RATE;
        double sum_synergy = synergy_list.Sum(x => x.multiple_type == mtype ? x.add_damage_per : 0);
        if (sum_synergy > 0)
        {
            Battle_Point_Inc.text = ZString.Format("(+{0:P0})", sum_synergy);
        }
        else
        {
            Battle_Point_Inc.text = string.Empty;
        }
    }

    public void SetSlotCardChoiceCallback(UnityAction<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> cb)
    {
        int cnt = Party_Slots.Count;
        for (int i = 0; i < cnt; i++)
        {
            Party_Slots[i].Click_Callback.RemoveAllListeners();
            Party_Slots[i].Click_Callback.AddListener(cb);
        }
    }

}
