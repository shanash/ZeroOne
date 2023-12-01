using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TeamManager : IDisposable
{
    public TEAM_TYPE Team_Type {  get; private set; }
    public GAME_TYPE Game_Type { get; private set; }

    BattleManager Battle_Mng;
    BattleUIManager UI_Mng;

    Transform Hero_Container;

    List<HeroBase> Used_Member_List = new List<HeroBase>();

    List<Vector3> Positions = new List<Vector3>();

    private bool disposed = false;

    public TeamManager(TEAM_TYPE ttype, Transform hero_parent)
    {
        ResetTeamManager();
        Hero_Container = hero_parent;
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


    protected void ResetTeamManager() 
    {
        //int cnt = Used_Member_List.Count;
        //var pool = GameObjectPoolManager.Instance;
        //for (int i = 0; i < cnt; i++)
        //{
        //    pool.UnusedGameObject(Used_Member_List[i].gameObject);
        //}
        //Used_Member_List.Clear();
    }

    public void SetGameType(GAME_TYPE gameType)
    {
        Game_Type = gameType;
    }
    public void SetTeamType(TEAM_TYPE teamType)
    {
        Team_Type = teamType;

        Positions.Clear();

        if (Team_Type == TEAM_TYPE.LEFT)
        {
            Positions.Add(new Vector3(-7, 0, 2));
            Positions.Add(new Vector3(-11, 0, -5));
            Positions.Add(new Vector3(-15, 0, -12));

            Positions.Add(new Vector3(-14, 0, 2));
            Positions.Add(new Vector3(-18, 0, -5));
            Positions.Add(new Vector3(-22, 0, -12));

            Positions.Add(new Vector3(-21, 0, 2));
            Positions.Add(new Vector3(-25, 0, -5));
            Positions.Add(new Vector3(-29, 0, -12));

        }
        else
        {
            Positions.Add(new Vector3(7, 0, 2));
            Positions.Add(new Vector3(11, 0, -5));
            Positions.Add(new Vector3(15, 0, -12));

            Positions.Add(new Vector3(14, 0, 2));
            Positions.Add(new Vector3(18, 0, -5));
            Positions.Add(new Vector3(22, 0, -12));

            Positions.Add(new Vector3(21, 0, 2));
            Positions.Add(new Vector3(25, 0, -5));
            Positions.Add(new Vector3(29, 0, -12));
        }
    }

    public void SetManager(BattleManager mng, BattleUIManager ui)
    {
        Battle_Mng = mng;
        UI_Mng = ui;
    }

    public void SpawnHeroes()
    {
        var pool = GameObjectPoolManager.Instance;
        List<TEAM_POSITION_TYPE> pos_types = new List<TEAM_POSITION_TYPE>();
        pos_types.Add(TEAM_POSITION_TYPE.FRONT_TOP);
        pos_types.Add(TEAM_POSITION_TYPE.FRONT_MIDDLE);
        pos_types.Add(TEAM_POSITION_TYPE.FRONT_BOTTOM);
        pos_types.Add(TEAM_POSITION_TYPE.CENTER_TOP);
        pos_types.Add(TEAM_POSITION_TYPE.CENTER_MIDDLE);
        pos_types.Add(TEAM_POSITION_TYPE.CENTER_BOTTOM);
        pos_types.Add(TEAM_POSITION_TYPE.REAR_TOP);
        pos_types.Add(TEAM_POSITION_TYPE.REAR_MIDDLE);
        pos_types.Add(TEAM_POSITION_TYPE.REAR_BOTTOM);
        pos_types.Shuffle();


        for (int i = 0; i < 5; i++)
        {
            var ptype = pos_types[i];
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Heros/Sorceress", Hero_Container);
            var hero = obj.GetComponent<HeroBase>();
            hero.SetTeamManager(this);
            hero.SetTeamPositionType(ptype);
            var pos = Positions[(int)ptype];
            hero.transform.localPosition = pos;
            
            hero.SetFlipX(Team_Type == TEAM_TYPE.RIGHT);
            hero.Lazy_Init(Battle_Mng, UI_Mng, UNIT_STATES.INIT);
            AddMember(hero);
        }
        SetBattleBeginRapidityPoint();

    }
    /// <summary>
    /// 팀원 추가
    /// </summary>
    /// <param name="hero"></param>
    public void AddMember(HeroBase hero)
    {
        if (!Used_Member_List.Exists(x => object.ReferenceEquals(x, hero)))
        {
            Used_Member_List.Add(hero);
        }

    }

    /// <summary>
    /// 현재 진행중인 멤버 반환(보통 살아있는 멤버만 있지만, 타이밍에 따라 죽은 멤버가 포함될 수도 있음)
    /// </summary>
    /// <returns></returns>
    public List<HeroBase> GetMembers()
    {
        return Used_Member_List;
    }

    /// <summary>
    /// 상대팀의 팀 매니저 정보 반환
    /// </summary>
    /// <returns></returns>
    public TeamManager GetEnemyTeam()
    {
        if (Battle_Mng != null)
        {
            return Battle_Mng.FindTeamManager(Team_Type == TEAM_TYPE.LEFT ? TEAM_TYPE.RIGHT : TEAM_TYPE.LEFT);
        }
        return null;
    }

    /// <summary>
    /// 모든 팀원 살아있는지 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsAliveMembers()
    {
        return Used_Member_List.Exists(x => x.IsAlive());
    }
    /// <summary>
    /// 살아있는 팀원 반환
    /// </summary>
    /// <returns></returns>
    public List<HeroBase> GetAliveMembers()
    {
        return Used_Member_List.FindAll(x => x.IsAlive());
    }

    /// <summary>
    /// 팀원이 죽으면 삭제
    /// </summary>
    /// <param name="hero"></param>
    public void TeamMemberDeath(HeroBase hero)
    {
        GameObjectPoolManager.Instance.UnusedGameObject(hero.gameObject);
        Used_Member_List.Remove(hero);

    }
    /// <summary>
    /// 모든 팀원들의 상태가 지정한 상태와 동일한지 여부 판단
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public bool IsAllMembersState(UNIT_STATES trans)
    {
        return !Used_Member_List.Exists(x => x.GetCurrentState() != trans);
    }

    /// <summary>
    /// 멤버들의 행동력 초기화(전투 시작시 각 영웅별 3%내의 속도 변수를 적용)
    /// </summary>
    public void SetBattleBeginRapidityPoint()
    {
        double random_rate = 0;
        int cnt = Used_Member_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            random_rate = UnityEngine.Random.Range(0, 300);
            HeroBase member = Used_Member_List[i];
            member.BattleBeginRapidityPoint(random_rate);
        }
    }

    /// <summary>
    /// 지정 멤버를 제외한 다른 멤버들의 행동력 증가
    /// </summary>
    /// <param name="hero"></param>
    public void IncRapicityPoint(HeroBase hero)
    {
        int cnt = Used_Member_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            HeroBase h = Used_Member_List[i];
            if (!object.ReferenceEquals(h, hero))
            {
                h.CalcIncRapidityPoint();
            }
        }
    }
}
