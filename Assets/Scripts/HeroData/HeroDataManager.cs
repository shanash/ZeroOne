using UnityEngine;
using FluffyDuck.Util;
using System.Collections.Generic;
using System.Linq;

public class HeroDataManager : MonoSingleton<HeroDataManager>
{
    protected override bool Is_DontDestroyOnLoad => true;

    List<UserL2dData> Hero_L2d_Data_List = new List<UserL2dData>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CallInstance()
    {
        _ = Instance;
    }

    protected override void Initialize()
    {
        // 초기화
    }

    protected void OnDestroy()
    {
        foreach(UserL2dData item in Hero_L2d_Data_List)
        {
            item.Dispose();
        }

        Hero_L2d_Data_List.Clear();
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

    /// <summary>
    /// 미보유 캐릭터의 L2dDataList 생성
    /// </summary>
    public void UpdateHeroL2dData()
    {
        var m = MasterDataManager.Instance;

        var heroIDList = GetExpectHeroDataList();

        foreach( var heroID in heroIDList)
        {
            int hero_id = heroID;
            int l2d_id = m.Get_PlayerCharacterData(heroID).lobby_basic_id;
            var data = AddUserL2dData(l2d_id, hero_id);
        }


    }

    UserL2dData AddUserL2dData(int l2d_id, int hero_id = 0)
    {
        if (l2d_id == 0)
        {
            return null;
        }

        if (hero_id == 0)
        {
            var user_hero_list = GameData.I.GetUserHeroDataManager().GetUserHeroDataList();
            foreach (var data in user_hero_list)
            {
                var hero_data = MasterDataManager.Instance.Get_PlayerCharacterData(data.GetPlayerCharacterID());
                if (hero_data.lobby_basic_id.Equals(l2d_id))
                {
                    hero_id = hero_data.player_character_id;
                }
            }

            Debug.Assert(hero_id != 0, $"입력한 L2d의 캐릭터id를 찾을 수 없습니다 : {l2d_id}");
        }

        var l2d_data = FindHeroL2dData(l2d_id);
        if (l2d_data == null)
        {
            l2d_data = new UserL2dData();
            l2d_data.SetL2dDataID(l2d_id, hero_id);
            Hero_L2d_Data_List.Add(l2d_data);
            //Is_Update_Data = true;
        }
        return l2d_data;
    }

    public UserL2dData FindHeroL2dData(int memorial_id)
    {
        return Hero_L2d_Data_List.Find(x => x.Skin_Id == memorial_id);
    }



    public List<UserL2dData> GetHeroL2dDataList()
    {
        return Hero_L2d_Data_List.ToList();
    }
}
