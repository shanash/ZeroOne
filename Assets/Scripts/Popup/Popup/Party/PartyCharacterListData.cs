using Gpm.Ui;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartyCharacterListData : InfiniteScrollData
{
    List<UserHeroData> User_Hero_Data_List = new List<UserHeroData>();

    List<BattlePcData> Battle_Hero_Data_List = new List<BattlePcData>();

    public UnityAction<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> Click_Hero_Callback;

    public GAME_TYPE Game_Type { get; protected set; } = GAME_TYPE.NONE;

    public CHARACTER_SORT Filter_Type { get; protected set; } = CHARACTER_SORT.NAME;

    public void SetUserHeroDataList(List<UserHeroData> list)
    {
        User_Hero_Data_List.Clear();
        if (list != null)
        {
            User_Hero_Data_List.AddRange(list);
        }
    }
    public void SetBattleHeroDataList(List<BattlePcData> list)
    {
        Battle_Hero_Data_List.Clear();
        if (list != null)
        {
            Battle_Hero_Data_List.AddRange(list);
        }
    }
    public void SetGameType(GAME_TYPE gtype)
    {
        Game_Type = gtype;
    }

    public void SetFilterType(CHARACTER_SORT filter)
    {
        Filter_Type = filter;
    }

    public List<UserHeroData> GetUserHeroDataList()
    {
        return User_Hero_Data_List;
    }
    public List<BattlePcData> GetBattleHeroDataList()
    {
        return Battle_Hero_Data_List;
    }
}
