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
    protected virtual void ResetTeamManager() { }

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


        for (int i = 0; i < cnt; i++)
        {
            UserHeroDeckMountData user_deck_hero = hero_list[i];
            UserHeroData user_data = user_deck_hero.GetUserHeroData();

            var obj = pool.GetGameObject(user_data.GetPlayerCharacterData().prefab_path, Unit_Container);
            HeroBase_V2 hero = obj.GetComponent<HeroBase_V2>();
            hero.SetTeamManager(this);
            hero.SetBattleUnitDataID(user_data.GetPlayerCharacterID(), user_data.Player_Character_Num);
            hero.SetDeckOrder(i);
            hero.Lazy_Init(Battle_Mng, UI_Mng, UNIT_STATES.INIT);

            AddMember(hero);
        }

        LeftTeamPosition();
    }

    /// <summary>
    /// Left 팀 최초 포지션 설정
    /// </summary>
    public void LeftTeamPosition()
    {
        int cnt = Used_Members.Count;
        float size = 20f;

        float offset_z = 0f;
        float offset_x = -size;
        float interval = 2f;
        float position_offset = 0f;

        for (int i = 0; i < cnt; i++)
        {
            var member = Used_Members[i];
            position_offset = (int)member.GetPositionType() * interval;

            float distance = member.GetApproachDistance();

            List<HeroBase_V2> same_positions = Used_Members.FindAll(x => x.GetPositionType() == member.GetPositionType());
            same_positions.Sort((a, b) => a.GetApproachDistance().CompareTo(b.GetApproachDistance()));
            Debug.Assert(same_positions.Count > 0);

            int find_index = same_positions.IndexOf(member);

            member.transform.localPosition = new Vector3(offset_x - position_offset, 0, offset_z + (find_index * 1.5f));
        }
    }



    /// <summary>
    /// Right 팀 최초 포지션 설정
    /// </summary>
    public void RightTeamPosition()
    {
        int cnt = Used_Members.Count;
        float size = 20f;

        float offset_z = 0f;
        float offset_x = size;
        float interval = 2f;
        float position_offset = 0f;

        for (int i = 0; i < cnt; i++)
        {
            var member = Used_Members[i];
            position_offset = (int)member.GetPositionType() * interval;

            float distance = member.GetApproachDistance();

            List<HeroBase_V2> same_positions = Used_Members.FindAll(x => x.GetPositionType() == member.GetPositionType());
            same_positions.Sort((a, b) => a.GetApproachDistance().CompareTo(b.GetApproachDistance()));
            Debug.Assert(same_positions.Count > 0);

            int find_index = same_positions.IndexOf(member);

            member.transform.localPosition = new Vector3(offset_x + position_offset, 0, offset_z + (find_index * 1.5f));
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
                StoryModeMonsterTeamSpawn();
                break;
            case GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE:
                Editor_MonsterTeamSpawn();
                break;
        }
    }



    /// <summary>
    /// 스토리 모드 몬스터 스폰
    /// </summary>
    void StoryModeMonsterTeamSpawn()
    {
        var pool = GameObjectPoolManager.Instance;

        var wdata = (Wave_Data)Dungeon.GetWaveData();

        int len = wdata.enemy_appearance_info.Length;

        //  battle npc data
        for (int i = 0; i < len; i++)
        {
            var npc = new BattleNpcData();
            npc.SetUnitID(wdata.enemy_appearance_info[i]);

            var obj = pool.GetGameObject(npc.GetPrefabPath(), Unit_Container);
            MonsterBase_V2 monster = obj.GetComponent<MonsterBase_V2>();
            monster.SetTeamManager(this);

            monster.SetBattleUnitDataID(npc.GetUnitID());

            //monster.SetFlipX(true);
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
        Used_Members.ForEach(x => x.WaveEndRecoveryLife());
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

    /// <summary>
    /// 죽은 멤버 제거
    /// </summary>
    /// <param name="hero"></param>
    public void RemoveDeathMember(HeroBase_V2 hero)
    {
        GameObjectPoolManager.Instance.UnusedGameObject(hero.gameObject);
        Used_Members.Remove(hero);
    }

    public bool IsAllMembersState(UNIT_STATES trans)
    {
        return !Used_Members.Exists(x => x.GetCurrentState() != trans);
    }

    public void GetHeroPrefabsPath(ref List<string> list)
    {
        switch (Game_Type)
        {
            case GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE:
                //  nothing
                bool a = false;
                break;
            case GAME_TYPE.EDITOR_SKILL_EDIT_MODE:
                break;
            case GAME_TYPE.STORY_MODE:
                GetStoryModeHeroPrefabs(Game_Type, ref list);
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
    protected void GetStoryModeHeroPrefabs(GAME_TYPE gtype, ref List<string> list)
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
                GetPcSkillEffectPrefabPath(hdata, ref list);
            }
        }
    }

    /// <summary>
    /// 플레이어 캐릭터의 스킬 이펙트 리스트 가져오기
    /// </summary>
    /// <param name="hero"></param>
    /// <param name="list"></param>
    protected void GetPcSkillEffectPrefabPath(Player_Character_Data pc_data, ref List<string> list)
    {
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
        var bdata = m.Get_PlayerCharacterBattleData(pc_data.battle_info_id);
        if (bdata != null)
        {
            int grp_cnt = bdata.skill_pattern.Length;

            for (int g = 0; g < grp_cnt; g++)
            {
                //  skill group
                var skill_group = m.Get_PlayerCharacterSkillGroupData(bdata.skill_pattern[g]);
                if (skill_group == null)
                {
                    Debug.Assert(false);
                    continue;
                }
                //  skill group cast effect
                if (!string.IsNullOrEmpty(skill_group.cast_effect_path) && !list.Contains(skill_group.cast_effect_path))
                {
                    list.Add(skill_group.cast_effect_path);
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


}
