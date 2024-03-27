using UnityEngine;
using FluffyDuck.Util;
using System.Collections.Generic;

public class HeroDataManager : MonoSingleton<HeroDataManager>
{
    protected override bool Is_DontDestroyOnLoad => true;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CallInstance()
    {
        _ = Instance;
    }

    protected override void Initialize()
    {
        // 초기화
    }

    public List<int> GetExpectHeroDataList()
    {
        var m = MasterDataManager.Instance;

        /// 플레이어 캐릭터 아이디 리스트
        var hero_id_List = m.GetHeroIDList();

        var gd = GameData.Instance;
        var hero_mng = gd.GetUserHeroDataManager();
        var user_hero_id_list = new List<int>();

        /// 유저 보유 캐릭터 아이디 리스트
        hero_mng.GetUserHeroIDList(ref user_hero_id_list);

        List<int> result = new List<int>();

        hero_id_List.Sort();
        user_hero_id_list.Sort();

        /// 미보유 캐릭터 아이디 리스트 검색
        foreach( int _id in hero_id_List )
        {
            if(!user_hero_id_list.Contains(_id))
            { 
                result.Add(_id); 
            }
        }

        return result;
    }
}
