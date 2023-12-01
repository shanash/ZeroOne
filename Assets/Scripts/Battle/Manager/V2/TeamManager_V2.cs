using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class TeamManager_V2 : IDisposable
{
    public TEAM_TYPE Team_Type { get; private set; }
    public GAME_TYPE Game_Type { get; private set; }

    BattleManager_V2 Battle_Mng;
    BattleUIManager_V2 UI_Mng;

    Transform Unit_Container;


    List<HeroBase_V2> Used_Members = new List<HeroBase_V2>();

    BattleDungeonData Stage;

    private bool disposed = false;

    public TeamManager_V2(TEAM_TYPE ttype, Transform hero_parent)
    {
        Unit_Container = hero_parent;
        SetTeamType(ttype);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                //  관리되는 자원 해제
                ResetTeamManager();
            }
            disposed = true;
        }
    }
    protected virtual void ResetTeamManager() { }

    public void SetBattleDungeonData(BattleDungeonData stage)
    {
        this.Stage = stage;
    }

    public void SetGameType(GAME_TYPE gameType)
    {
        Game_Type = gameType;
    }
    public void SetTeamType(TEAM_TYPE team_type)
    {
        Team_Type = team_type;
    }
    public void SetManagers(BattleManager_V2 mng, BattleUIManager_V2 ui)
    {
        Battle_Mng = mng;
        UI_Mng = ui;
    }

    /// <summary>
    /// 팀별로 영웅 정보를 가져오고, 소환한다.
    /// 웨이브에 따라 각기 맞는 영웅 정보를 가져온다.
    /// 아군은 웨이브에 상관없이 최초 1회만 소환
    /// 적군은 각 웨이브에 구성되어 있는 적들을 소환.
    /// 미리 화면상에 소환해두지 않는다.
    /// Preload 해두고 다음 웨이브 이동할 때 가져오도록 하자.
    /// </summary>
    /// <param name="wave"></param>
    public void SpawnHeros()
    {
        if (Team_Type == TEAM_TYPE.LEFT)
        {
            //  0,0 부터 시작
            MyTeamSpawn();
        }
        else
        {
            //  상대팀이 0,0에 있다는 전제하에 거리별로 구분
            EnemyTeamSpawn();
        }
    }

    /// <summary>
    /// 아군의 플레이어 캐릭터를 스폰한다.
    /// 0,0부터 시작하고, 가장 가까운 캐릭터를 기준으로 거리를 계산한다.
    /// </summary>
    void MyTeamSpawn()
    {
        float size = Camera.main.fieldOfView;
        var pool = GameObjectPoolManager.Instance;
        var deck = GameData.Instance.GetUserHeroDeckMountDataManager().FindSelectedDeck(Game_Type);
        var hero_list = deck.GetDeckHeroes();
        
        int cnt = hero_list.Count;

        

        float offset_z = 0f;
        float offset_x = -size;
        float interval = 12f;
        float position_offset = 0f;

        for (int i = 0; i < cnt; i++)
        {
            UserHeroDeckMountData user_deck_hero = hero_list[i];
            UserHeroData user_data = user_deck_hero.GetUserHeroData();

            position_offset = (int)user_data.GetPositionType() * interval;
           
            float distance = user_data.GetDistance();
            var obj = pool.GetGameObject(user_data.GetPlayerCharacterData().prefab_path, Unit_Container);
            HeroBase_V2 hero = obj.GetComponent<HeroBase_V2>();
            hero.SetTeamManager(this);
            hero.SetUserHeroData(user_data);
            hero.SetDeckOrder(i);
            hero.Lazy_Init(Battle_Mng, UI_Mng, UNIT_STATES.INIT);

            List<UserHeroDeckMountData> same_positions = hero_list.FindAll(x => x.GetUserHeroData().GetPositionType() == user_data.GetPositionType());
            same_positions = same_positions.OrderBy(x => x.GetUserHeroData().GetDistance()).ToList();
            Debug.Assert(same_positions.Count > 0);

            int find_index = same_positions.IndexOf(user_deck_hero);
            
            hero.transform.localPosition = new Vector3(offset_x - position_offset, 0, offset_z + (find_index * 5));

            AddMember(hero);
        }
    }
    /// <summary>
    /// 적군의 npc 캐릭터를 스폰한다.
    /// 0,0에 상대팀의 캐릭터가 있다는 전제하에 거리를 계산한다.
    /// </summary>
    void EnemyTeamSpawn()
    {
        switch (Game_Type)
        {
            case GAME_TYPE.STORY_MODE:
                StoryModeMonsterTeamSpawn();
                break;
        }
    }

    /// <summary>
    /// 스토리 모드 몬스터 스폰
    /// </summary>
    void StoryModeMonsterTeamSpawn()
    {
        var pool = GameObjectPoolManager.Instance;
        var m = MasterDataManager.Instance;
        float size = Camera.main.fieldOfView;

        float offset_z = 0f;
        float offset_x = size;
        float interval = 12f;
        float position_offset = 0f;

        var wdata = (Wave_Data)Stage.GetWaveData();

        List<BattleNpcData> battle_npc_list = new List<BattleNpcData>();

        int len = wdata.enemy_appearance_info.Length;
        //  battle npc data
        for (int i = 0; i < len; i++)
        {
            var npc = new BattleNpcData();
            npc.SetNpcDataID(wdata.enemy_appearance_info[i]);
            battle_npc_list.Add(npc);
        }
        //  사거리가 짧은 순으로 정렬
        battle_npc_list.Sort(delegate(BattleNpcData a, BattleNpcData b) 
        {
            if (a.GetDistance() > b.GetDistance())
            {
                return 1;
            }
            return -1;
        });

        //  npc gameobject
        for (int i = 0; i < len; i++)
        {
            var npc = battle_npc_list[i];
            position_offset = (int)npc.GetPositionType() * interval;

            var obj = pool.GetGameObject(npc.GetNpcData().prefab_path, Unit_Container);
            MonsterBase_V2 monster = obj.GetComponent<MonsterBase_V2>();
            monster.SetTeamManager(this);

            monster.SetBattleNpcData(npc);
            monster.SetFlipX(true);
            monster.SetDeckOrder(i);
            monster.Lazy_Init(Battle_Mng, UI_Mng, UNIT_STATES.INIT);

            List<BattleNpcData> same_positions = battle_npc_list.FindAll(x => x.GetPositionType() == npc.GetPositionType());
            same_positions = same_positions.OrderBy(x => x.GetDistance()).ToList();
            Debug.Assert(same_positions.Count > 0);

            int find_index = same_positions.IndexOf(npc);

            monster.transform.localPosition = new Vector3(offset_x + position_offset, 0, offset_z + (find_index * 5));

            AddMember(monster);
        }

    }
    /// <summary>
    /// 멤버 추가
    /// </summary>
    /// <param name="hero"></param>
    void AddMember(HeroBase_V2 hero)
    {
        if (!Used_Members.Exists(x => object.ReferenceEquals(x, hero)))
        {
            Used_Members.Add(hero);
        }
    }

    public List<HeroBase_V2> GetMembers() { return Used_Members; }

    public TeamManager_V2 GetEnemyTeam()
    {
        if (Battle_Mng != null)
        {
            return Battle_Mng.FindTeamManager(Team_Type == TEAM_TYPE.LEFT ? TEAM_TYPE.RIGHT : TEAM_TYPE.LEFT);
        }
        return null;
    }

    public bool IsAliveMembers()
    {
        return Used_Members.Exists(x => x.IsAlive());
    }
    public List<HeroBase_V2> GetAliveMembers()
    {
        return Used_Members.FindAll(x => x.IsAlive());
    }

    /// <summary>
    /// 살아있는 멤버 중, 중앙을 중심으로 지정한 거리내에 있는 영웅 모두 반환
    /// 거리에 따른 오름차순으로 정렬 (가장 가까운 유닛이 우선)
    /// </summary>
    /// <param name="center"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public List<HeroBase_V2> GetInRangeMembersAsc(HeroBase_V2 center, float distance)
    {
        var list = Used_Members.FindAll(x => x.IsAlive() && x.InRange(center, distance));
        list.Sort(delegate (HeroBase_V2 a, HeroBase_V2 b)
        {
            if (a.GetDistanceFromCenter(center) > b.GetDistanceFromCenter(center))
            {
                return 1;
            }
            return -1;
        });
        return list;
    }

    /// <summary>
    /// 살아있는 멤버 중, 중앙을 중심으로 지정한 거리내에 있는 영웅 모두 반환
    /// 거리에 따른 내림차순으로 정렬(가장 먼 유닛이 우선)
    /// </summary>
    /// <param name="center"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public List<HeroBase_V2> GetInRangeMembersDesc(HeroBase_V2 center, float distance)
    {
        var list = Used_Members.FindAll(x => x.IsAlive() && x.InRange(center, distance));
        list.Sort(delegate (HeroBase_V2 a, HeroBase_V2 b)
        {
            if (a.GetDistanceFromCenter(center) < b.GetDistanceFromCenter(center))
            {
                return 1;
            }
            return -1;
        });
        return list;
    }
    
    /// <summary>
    /// 팀원 전체의 상태를 변경하기 위한 함수
    /// </summary>
    /// <param name="trans"></param>
    public void ChangeStateTeamMembers(UNIT_STATES trans)
    {
        int cnt = Used_Members.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Members[i].ChangeState(trans);
        }
    }
    /// <summary>
    /// 팀원 전체의 상태를 되돌리기
    /// </summary>
    public void RevertStateTeamMembers()
    {
        int cnt = Used_Members.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Members[i].RevertState();
        }
    }


    public void RemoveDeathMember(HeroBase_V2 hero)
    {
        GameObjectPoolManager.Instance.UnusedGameObject(hero.gameObject);
        Used_Members.Remove(hero);
    }

    public bool IsAllMembersState(UNIT_STATES trans)
    {
        return !Used_Members.Exists(x => x.GetCurrentState() != trans);
    }
}
