using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class TeamManager_V2 : IDisposable
{
    public TEAM_TYPE Team_Type { get; private set; }
    public GAME_TYPE Game_Type { get; private set; }

    protected BattleManager_V2 Battle_Mng;
    protected BattleUIManager_V2 UI_Mng;

    protected Transform Unit_Container;


    protected List<HeroBase_V2> Used_Members = new List<HeroBase_V2>();

    protected BattleDungeonData Dungeon;

    protected int Total_Member_Count;
    protected int Alive_Member_Count;

    /// <summary>
    /// 게임 배속
    /// </summary>
    protected float Battle_Speed_Multiple = 1f;

    private bool disposed = false;

    public TeamManager_V2(TEAM_TYPE ttype)
    {
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
    protected virtual void ResetTeamManager() 
    {
        int cnt = Used_Members.Count;
        //var pool = GameObjectPoolManager.Instance;
        //if (pool == null)
        //{
        //    return;
        //}
        //for (int i = 0; i < cnt; i++)
        //{
        //    pool.UnusedGameObject(Used_Members[i].gameObject);
        //}
        Used_Members.Clear();
    }

    public void SetBattleSpeed(float speed)
    {
        Battle_Speed_Multiple = speed;
        int cnt = Used_Members.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Members[i].SetBattleSpeed(speed);
        }
    }

    public void SetHeroContainer(Transform hero_parent)
    {
        Unit_Container = hero_parent;
    }

    public void SetBattleDungeonData(BattleDungeonData stage)
    {
        this.Dungeon = stage;
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
            if (Game_Type != GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE && Game_Type != GAME_TYPE.EDITOR_SKILL_EDIT_MODE)
            {
                MyTeamSpawn();
            }
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
    protected void MyTeamSpawn()
    {
        var pool = GameObjectPoolManager.Instance;
        var deck = GameData.Instance.GetUserHeroDeckMountDataManager().FindSelectedDeck(Game_Type);
        var hero_list = deck.GetDeckHeroes();

        int cnt = hero_list.Count;
        Total_Member_Count = cnt;
        Alive_Member_Count = cnt;

        for (int i = 0; i < cnt; i++)
        {
            UserHeroDeckMountData user_deck_hero = hero_list[i];
            UserHeroData user_data = user_deck_hero.GetUserHeroData();

            var unit_data = new BattlePcData();
            unit_data.SetUnitID(user_data.GetPlayerCharacterID(), user_data.Player_Character_Num);
            unit_data.AddTeamAttributeSynergy(deck.GetTeamSynergyList());

            var obj = pool.GetGameObject(user_data.GetPlayerCharacterData().prefab_path, Unit_Container);
            HeroBase_V2 hero = obj.GetComponent<HeroBase_V2>();
            hero.SetTeamManager(this);
            hero.SetBattleUnitData(unit_data);
            hero.SetBattleSpeed(Battle_Speed_Multiple);
            hero.SetDeckOrder(i);
            hero.Lazy_Init(Battle_Mng, UI_Mng, UNIT_STATES.INIT);

            AddMember(hero);
        }
        LeftTeamPosition();
    }

    public void LeftTeamResetPosition()
    {
        int cnt = Used_Members.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Members[i].ResetTeamFieldPosition();
        }
    }

    /// <summary>
    /// Left 팀 최초 포지션 설정
    /// </summary>
    void LeftTeamPosition()
    {
        float field_size = 13f;
        //  front
        List<Vector3> front_pos_list = new List<Vector3>();
        front_pos_list.Add(new Vector3(0, 0, 0));
        front_pos_list.Add(new Vector3(0, 0, -2f));
        front_pos_list.Add(new Vector3(0, 0, 2f));
        front_pos_list.Add(new Vector3(0, 0, -1f));
        front_pos_list.Add(new Vector3(0, 0, 1f));
        //  middle        
        List<Vector3> middle_pos_list = new List<Vector3>();
        middle_pos_list.Add(new Vector3(-2.5f, 0, 0));
        middle_pos_list.Add(new Vector3(-2.5f, 0, -2f));
        middle_pos_list.Add(new Vector3(-2.5f, 0, 2f));
        middle_pos_list.Add(new Vector3(-2.5f, 0, -1f));
        middle_pos_list.Add(new Vector3(-2.5f, 0, 1f));
        //  back
        List<Vector3> back_pos_list = new List<Vector3>();
        back_pos_list.Add(new Vector3(-5, 0, 0));
        back_pos_list.Add(new Vector3(-5, 0, -2f));
        back_pos_list.Add(new Vector3(-5, 0, 2f));
        back_pos_list.Add(new Vector3(-5, 0, -1f));
        back_pos_list.Add(new Vector3(-5, 0, 1f));

        int idx = 0;
       
        for (POSITION_TYPE p = POSITION_TYPE.FRONT; p <= POSITION_TYPE.BACK; p++)
        {
            List<Vector3> pos_list = null;
            switch (p)
            {
                case POSITION_TYPE.FRONT:
                    pos_list = front_pos_list;
                    break;
                case POSITION_TYPE.MIDDLE:
                    pos_list = middle_pos_list;
                    break;
                case POSITION_TYPE.BACK:
                    pos_list = back_pos_list;
                    break;
            }
            if (pos_list == null)
            {
                continue;
            }

            var members = Used_Members.FindAll(x => x.GetPositionType() == p);
            for (int i = 0; i < members.Count; i++)
            {
                var mem = members[i];
                if (idx < pos_list.Count)
                {
                    Vector3 pos = pos_list[idx];
                    pos.x -= field_size;
                    mem.SetTeamFieldPosition(pos);
                }
                idx++;
            }
        }

    }


    
    /// <summary>
    /// Right 팀 최초 포지션 설정
    /// </summary>
    void RightTeamPosition()
    {
        float field_size = 13f;
        //  front
        List<Vector3> front_pos_list = new List<Vector3>();
        front_pos_list.Add(new Vector3(0, 0, 0));
        front_pos_list.Add(new Vector3(0, 0, -2f));
        front_pos_list.Add(new Vector3(0, 0, 2f));
        front_pos_list.Add(new Vector3(0, 0, -1f));
        front_pos_list.Add(new Vector3(0, 0, 1f));
        
        //  middle
        List<Vector3> middle_pos_list = new List<Vector3>();
        middle_pos_list.Add(new Vector3(2.5f, 0, 0));
        middle_pos_list.Add(new Vector3(2.5f, 0, -2f));
        middle_pos_list.Add(new Vector3(2.5f, 0, 2f));
        middle_pos_list.Add(new Vector3(2.5f, 0, -1f));
        middle_pos_list.Add(new Vector3(2.5f, 0, 1f));
        
        //  back
        List<Vector3> back_pos_list = new List<Vector3>();
        back_pos_list.Add(new Vector3(5, 0, 0));
        back_pos_list.Add(new Vector3(5, 0, -2f));
        back_pos_list.Add(new Vector3(5, 0, 2f));
        back_pos_list.Add(new Vector3(5, 0, -1f));
        back_pos_list.Add(new Vector3(5, 0, 1f));
        

        int idx = 0;
        for (POSITION_TYPE p = POSITION_TYPE.FRONT; p <= POSITION_TYPE.BACK; p++)
        {
            List<Vector3> pos_list = null;
            switch (p)
            {
                case POSITION_TYPE.FRONT:
                    pos_list = front_pos_list;
                    break;
                case POSITION_TYPE.MIDDLE:
                    pos_list = middle_pos_list;
                    break;
                case POSITION_TYPE.BACK:
                    pos_list = back_pos_list;
                    break;
            }
            if (pos_list == null)
            {
                continue;
            }

            var members = Used_Members.FindAll(x => x.GetPositionType() == p);
            for (int i = 0; i < members.Count; i++)
            {
                var mem = members[i];
                if (idx < pos_list.Count)
                {
                    Vector3 pos = pos_list[idx];
                    pos.x += field_size;
                    mem.SetTeamFieldPosition(pos);
                }
                idx++;
            }
        }

    }


    /// <summary>
    /// 적군의 npc 캐릭터를 스폰한다.
    /// 0,0에 상대팀의 캐릭터가 있다는 전제하에 거리를 계산한다.
    /// </summary>
    protected void EnemyTeamSpawn()
    {
        switch (Game_Type)
        {
            case GAME_TYPE.STORY_MODE:
                DungeonMonsterTeamSpawn();
                break;
            case GAME_TYPE.BOSS_DUNGEON_MODE:
                //BossStageModeMonsterTeamSpawn();
                DungeonMonsterTeamSpawn();
                break;
            case GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE:
                Editor_MonsterTeamSpawn();
                break;
        }
    }

    /// <summary>
    /// 스토리 모드 몬스터 스폰
    /// </summary>
    void DungeonMonsterTeamSpawn()
    {
        ClearMembers();
        var pool = GameObjectPoolManager.Instance;

        var wdata = (Wave_Data)Dungeon.GetWaveData();

        int len = wdata.enemy_appearance_info.Length;

        Total_Member_Count = len;
        Alive_Member_Count = len;


        var lv_list = wdata.npc_levels;
        var stat_list = wdata.npc_stat_ids;
        var skill_lv_list = wdata.npc_skill_levels;
        var ultimate_skill_lv_list = wdata.npc_ultimate_skill_levels;

        //  battle npc data
        for (int i = 0; i < len; i++)
        {
            var npc = new BattleNpcData();
            //  npc id, npc lv, stat id, skill lv, ultimate lv
            npc.SetUnitID(wdata.enemy_appearance_info[i], lv_list[i], stat_list[i], skill_lv_list[i], ultimate_skill_lv_list[i]);

            //if (i < lv_list.Length)
            //{
            //    npc.SetLevel(lv_list[i]);
            //}
            //if (i < stat_list.Length)
            //{
            //    npc.SetStatDataID(stat_list[i]);
            //}

            var obj = pool.GetGameObject(npc.GetPrefabPath(), Unit_Container);
            obj.transform.localScale = new Vector2(npc.GetUnitScale(), npc.GetUnitScale());
            MonsterBase_V2 monster = obj.GetComponent<MonsterBase_V2>();
            monster.SetTeamManager(this);

            monster.SetBattleUnitData(npc);
            monster.SetBattleSpeed(Battle_Speed_Multiple);

            monster.SetDeckOrder(i);
            monster.Lazy_Init(Battle_Mng, UI_Mng, UNIT_STATES.INIT);

            AddMember(monster);
        }


        RightTeamPosition();

    }


    /// <summary>
    /// 멤버 추가
    /// </summary>
    /// <param name="hero"></param>
    protected void AddMember(HeroBase_V2 hero)
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
    /// <param name="approach_distance"></param>
    /// <returns></returns>
    public List<HeroBase_V2> GetInRangeMembersAsc(HeroBase_V2 center, float approach_distance)
    {
        var list = Used_Members.FindAll(x => x.IsAlive() && x.InRange(center, approach_distance));
        //  내림 차순
        list.Sort((a, b) => a.GetDistanceFromCenter(center).CompareTo(b.GetDistanceFromCenter(center)));
        return list;
    }

    /// <summary>
    /// 팀원들 웨이브 종료시 체력 회복 요청
    /// </summary>
    public void RecoveryLifeWaveEndMembers()
    {
        var member_list = GetAliveMembers();
        member_list.ForEach(x => x.WaveEndRecoveryLife());
    }

    /// <summary>
    /// 팀원 전체의 상태를 변경하기 위한 함수
    /// </summary>
    /// <param name="trans"></param>
    public void ChangeStateTeamMembers(UNIT_STATES trans)
    {
        var member_list = GetAliveMembers();
        int cnt = member_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            member_list[i].ChangeState(trans);
        }
    }
    /// <summary>
    /// 팀원 전체의 상태를 되돌리기
    /// </summary>
    public void RevertStateTeamMembers()
    {
        var member_list = GetAliveMembers();
        int cnt = member_list.Count;
        
        for (int i = 0; i < cnt; i++)
        {
            member_list[i].RevertState();
        }
    }
    /// <summary>
    /// 살아있는 모든 유닛을 숨김 처리 (지정 타겟 제외)<br/>
    /// 죽은 유닛은 어차피 사라질 것이기 때문에 제외
    /// </summary>
    /// <param name="targets"></param>
    public void HideAllUnitWithoutTargets(List<HeroBase_V2> targets) 
    {
        var member_list = GetAliveMembers();
        int cnt = member_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var member = member_list[i];
            if (!targets.Contains(member))
            {
                member.SetAlphaAnimation(0f, 0.5f, true);
            }
        }
    }
    /// <summary>
    /// 모든 유닛을 보여줌.<br/>
    /// 죽은 유닛도 보여줌. 궁극기로 인해 죽었을 수도 있기 때문에.<br/>
    /// 죽은 이후의 처리는 상태가 변경되면 알아서 처리할 것임
    /// </summary>
    /// <param name="targets"></param>
    public void ShowAllUnitWithoutTargets(List<HeroBase_V2> targets)
    {
        var member_list = GetMembers();
        int cnt = member_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var member = member_list[i];
            if (!targets.Contains(member))
            {
                member.SetAlphaAnimation(1f, 0.5f, false);
            }
        }
    }
    /// <summary>
    /// 모든 유닛 보여줌
    /// </summary>
    public void ShowAllUnits()
    {
        var member_list = GetMembers();
        int cnt = member_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            member_list[i].SetAlphaAnimation(1f, 0.5f, false);
        }
    }

    /// <summary>
    /// 살아있는 모든 유닛을 궁극기 사용을 위해 멈춤<br/>
    /// 궁극기 사용 유닛만 남김
    /// </summary>
    /// <param name="hero"></param>
    public void AllPauseTeamMembersWithoutHero(HeroBase_V2 hero)
    {
        var member_list = GetAliveMembers();
        int cnt = member_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var member = member_list[i];
            if (!object.ReferenceEquals(member, hero))
            {
                member.ChangeState(UNIT_STATES.ULTIMATE_PAUSE);
            }
        }
    }

    /// <summary>
    /// 궁극기를 얻어 맞고 죽을 수도 있으니, GetAliveMembers() 사용을 할 수 없음.<br/>
    /// 현재 모든 멤버를 이전 상태로 돌려두고, 죽을 놈은 알아서 죽게 해야함
    /// </summary>
    /// <param name="hero"></param>
    public void AllResumeTeamMembersWithoutHero(HeroBase_V2 hero)
    {
        var member_list = GetMembers();
        int cnt = member_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var member = member_list[i];
            if (!object.ReferenceEquals(member, hero))
            {
                member.RevertState();
            }
        }
    }

    /// <summary>
    /// 죽은 멤버 제거
    /// </summary>
    /// <param name="hero"></param>
    public void RemoveDeathMember(HeroBase_V2 hero)
    {
        GameObjectPoolManager.Instance.UnusedGameObject(hero.gameObject);
        Used_Members.Remove(hero);
        Alive_Member_Count = Used_Members.Count;
    }

    void ClearMembers()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Members.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Members[i].gameObject);
        }
        Used_Members.Clear();
    }

    public int GetTotalMemberCount()
    {
        return Total_Member_Count;
    }
    public int GetAliveMemberCount()
    {
        return Alive_Member_Count;
    }

    public bool IsAllMembersState(UNIT_STATES trans)
    {
        var member_list = GetAliveMembers();
        bool is_all_same = true;
        int cnt = member_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            if (member_list[i].GetCurrentState() != trans)
            {
                is_all_same = false;
                break;
            }
        }
        return is_all_same;
    }

    public void GetHeroPrefabsPath(ref List<string> list)
    {
        switch (Game_Type)
        {
            case GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE:
                //  nothing
                break;
            case GAME_TYPE.EDITOR_SKILL_EDIT_MODE:
                break;
            case GAME_TYPE.STORY_MODE:
                GetPlayerCharacterDeckPrefabs(Game_Type, ref list);
                break;
            case GAME_TYPE.BOSS_DUNGEON_MODE:
                GetPlayerCharacterDeckPrefabs(Game_Type, ref list);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }


    /// <summary>
    /// 스토리 모드의 플레이어 캐릭터 프리팹 반환
    /// </summary>
    /// <param name="list"></param>
    protected void GetPlayerCharacterDeckPrefabs(GAME_TYPE gtype, ref List<string> list)
    {
        var deck = GameData.Instance.GetUserHeroDeckMountDataManager().FindSelectedDeck(gtype);
        var deck_heroes = deck.GetDeckHeroes();
        int cnt = deck_heroes.Count;
        for (int i = 0; i < cnt; i++)
        {
            UserHeroDeckMountData hero = deck_heroes[i];
            Player_Character_Data hdata = hero.GetUserHeroData().GetPlayerCharacterData();
            if (hdata != null)
            {
                GetPcSkillEffectPrefabPath(hero.GetUserHeroData(), ref list);
            }
        }
    }

    /// <summary>
    /// 플레이어 캐릭터의 스킬 이펙트 리스트 가져오기
    /// </summary>
    /// <param name="hero"></param>
    /// <param name="list"></param>
    protected void GetPcSkillEffectPrefabPath(UserHeroData user_data, ref List<string> list)
    {
        var pc_data = user_data.GetPlayerCharacterData();
        var m = MasterDataManager.Instance;
        List<Player_Character_Skill_Data> skill_list = new List<Player_Character_Skill_Data>();

        //  영웅의 Battle SD 프리팹
        if (!list.Contains(pc_data.prefab_path))
        {
            list.Add(pc_data.prefab_path);
        }
        //  영웅의 UI SD 프리팹 (전투 결과 창에서 사용)
        if (!list.Contains(pc_data.sd_prefab_path))
        {
            list.Add(pc_data.sd_prefab_path);
        }

        //  battle data
        var bdata = user_data.GetPlayerCharacterBattleData();

        if (bdata != null)
        {
            List<int> skill_group_ids = new List<int>();
            skill_group_ids.AddRange(bdata.skill_pattern);
            skill_group_ids.Add(bdata.special_skill_group_id);

            int grp_cnt = skill_group_ids.Count;

            for (int g = 0; g < grp_cnt; g++)
            {
                int gid = skill_group_ids[g];
                if (gid == 0)
                {
                    continue;
                }
                //  skill group
                var skill_group = m.Get_PlayerCharacterSkillGroupData(gid);
                if (skill_group == null)
                {
                    Debug.Assert(false);
                    continue;
                }
                //  skill group cast effect
                if (skill_group.cast_effect_path != null)
                {
                    for (int c = 0; c < skill_group.cast_effect_path.Length; c++)
                    {
                        string cast_path = skill_group.cast_effect_path[c];
                        if (!string.IsNullOrEmpty(cast_path) && !list.Contains(cast_path))
                        {
                            list.Add(cast_path);
                        }
                    }
                }
                
                //  skill list
                m.Get_PlayerCharacterSkillDataListBySkillGroup(skill_group.pc_skill_group_id, ref skill_list);
                int skill_cnt = skill_list.Count;
                for (int s = 0; s < skill_cnt; s++)
                {
                    var pc_skill = skill_list[s];
                    //  pc skill effect
                    if (!string.IsNullOrEmpty(pc_skill.trigger_effect_path) && !list.Contains(pc_skill.trigger_effect_path))
                    {
                        list.Add(pc_skill.trigger_effect_path);
                    }
                    if (pc_skill.onetime_effect_ids != null)
                    {
                        //  onetime skill iist
                        for (int o = 0; o < pc_skill.onetime_effect_ids.Length; o++)
                        {
                            int onetime_skill_id = pc_skill.onetime_effect_ids[o];
                            if (onetime_skill_id == 0)
                            {
                                continue;
                            }
                            var onetime_data = m.Get_PlayerCharacterSkillOnetimeData(onetime_skill_id);
                            Debug.Assert(onetime_data != null);
                            if (!string.IsNullOrEmpty(onetime_data.effect_path) && !list.Contains(onetime_data.effect_path))
                            {
                                list.Add(onetime_data.effect_path);
                            }
                        }
                    }


                    //  duration skill list
                    if (pc_skill.duration_effect_ids != null)
                    {
                        for (int d = 0; d < pc_skill.duration_effect_ids.Length; d++)
                        {
                            int duration_skill_id = pc_skill.duration_effect_ids[d];
                            if (duration_skill_id == 0)
                            {
                                continue;
                            }
                            var duration_data = m.Get_PlayerCharacterSkillDurationData(duration_skill_id);
                            Debug.Assert(duration_data != null);
                            if (!string.IsNullOrEmpty(duration_data.effect_path) && !list.Contains(duration_data.effect_path))
                            {
                                list.Add(duration_data.effect_path);
                            }
                            //  반복 효과용 일회성 스킬 이펙트
                            int repeat_len = duration_data.repeat_pc_onetime_ids.Length;
                            for (int r = 0; r < repeat_len; r++)
                            {
                                int repeat_id = duration_data.repeat_pc_onetime_ids[r];
                                if (repeat_id == 0)
                                    continue;
                                var repeat_onetime = m.Get_PlayerCharacterSkillOnetimeData(repeat_id);
                                if (!string.IsNullOrEmpty(repeat_onetime.effect_path) && !list.Contains(repeat_onetime.effect_path))
                                {
                                    list.Add(repeat_onetime.effect_path);
                                }
                            }

                            //  종료 효과용 일회성 스킬 이펙트
                            int finish_len = duration_data.finish_pc_onetime_ids.Length;
                            for (int f = 0; f < finish_len; f++)
                            {
                                int finish_id = duration_data.finish_pc_onetime_ids[f];
                                if (finish_id == 0)
                                    continue;
                                var finish_onetime = m.Get_PlayerCharacterSkillOnetimeData(finish_id);
                                if (!string.IsNullOrEmpty(finish_onetime.effect_path) && !list.Contains(finish_onetime.effect_path))
                                {
                                    list.Add(finish_onetime.effect_path);
                                }
                            }
                        }
                    }


                }
            }
        }
    }

    /// <summary>
    /// 팀원 전체를 최상위로 올려준다.
    /// </summary>
    public void SetTeamLastSibling()
    {
        for (int i = 0; i < Used_Members.Count; i++)
        {
            Used_Members[i].transform.SetAsLastSibling();
        }
    }


}
