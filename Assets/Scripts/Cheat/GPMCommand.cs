using FluffyDuck.Util;
using Gpm.LogViewer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPMCommand : MonoBehaviour
{

    private void Start()
    {
        InitCommand();
    }

    void InitCommand()
    {
        var func = Function.Instance;
        //  cheat key callback
        func.AddCheatKeyCallback(CheatKeyCallback);


        //  commands
        func.AddCommand(this, "PlayerLevelUp");
        func.AddCommand(this, "AllCharacterLevelUp");
    }
    /// <summary>
    /// 플레이어 레벨업<br/>
    /// 캐릭터 레벨의 최대 레벨이 플레이어 레벨이기 때문에 플레이어 레벨을 상승 시켜야 함
    /// </summary>
    void PlayerLevelUp()
    {
        var gd = GameData.Instance;
        var player_info_mng = gd.GetUserGameInfoDataManager();
        var player_info = player_info_mng.GetCurrentPlayerInfoData();
        double need_exp = player_info.GetNextExp();
        player_info.AddExp(need_exp);
        player_info_mng.Save();

        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_PLAYER_INFO);
    }

    /// <summary>
    /// 모든 캐릭터 레벨 +1<br/>
    /// 모든 캐릭터의 레벨을 +1씩 증가<br/>
    /// 최대 레벨은 플레이어 레벨이기 때문에, 캐릭터 레벨이 상승하지 않는다면, 플레이어 레벨을 올릴 필요가 있음
    /// </summary>
    void AllCharacterLevelUp()
    {
        var gd = GameData.Instance;
        var hero_mng = gd.GetUserHeroDataManager();
        var hero_list = hero_mng.GetUserHeroDataList();
        for (int i = 0; i < hero_list.Count; i++)
        {
            var hero = hero_list[i];
            double need_exp = hero.GetNextExp();
            hero.AddExp(need_exp);
        }
        hero_mng.Save();
    }

    /// <summary>
    /// 각종 치트 키 사용 콜백<br/>
    /// 키로서 사용할 필요가 있는 경우
    /// </summary>
    /// <param name="cheat_key"></param>
    void CheatKeyCallback(string cheat_key)
    {
        if (string.IsNullOrEmpty(cheat_key))
        {
            return;
        }

        string key = cheat_key.ToLower();
        Debug.Log($"{key}");

        string[] keys = key.Split(" ");
        if (keys[0].Equals("gold"))
        {
            if (keys.Length > 1)
            {
                double gold = 0;
                if (double.TryParse(keys[1], out gold))
                {
                    GameData.Instance.GetUserGoodsDataManager().AddUserGoodsCount(GOODS_TYPE.GOLD, gold);
                }
            }
        }
        else if (keys[0].Equals("dia"))
        {
            if (keys.Length > 1)
            {
                double dia = 0;
                if (double.TryParse(keys[1], out dia))
                {
                    GameData.Instance.GetUserGoodsDataManager().AddUserGoodsCount(GOODS_TYPE.DIA, dia);
                }
            }
        }

        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ALL);
    }

}
