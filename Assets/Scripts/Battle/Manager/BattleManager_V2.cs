using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;
using FluffyDuck.UI;
using System.Collections;

public partial class BattleManager_V2 : SceneControllerBase
{
    [SerializeField, Tooltip("UI Manager")]
    protected BattleUIManager_V2 UI_Mng;
    [SerializeField, Tooltip("Virtual Cam Manager")]
    protected VirtualCineManager Cine_Mng;

    [SerializeField, Tooltip("Fade In Out Layer")]
    protected UIEaseCanvasGroupAlpha Fade_In_Out_Layer;

    [SerializeField, Tooltip("Skill Slot Manager")]
    protected BattleSkillSlotManager Skill_Slot_Mng;

    protected List<TeamManager_V2> Used_Team_List = new List<TeamManager_V2>();

    protected BattleField Field;

    protected GAME_TYPE Game_Type = GAME_TYPE.NONE;


    protected BattleDungeonData Dungeon_Data;

    protected float Battle_Speed_Multiple = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.NORMAL_TYPE];


    public VirtualCineManager GetVirtualCineManager()
    {
        return Cine_Mng;
    }

    public BattleField GetBattleField() { return Field; }

    public EffectFactory GetEffectFactory() { return Field.GetEffectFactory(); }

    public int GetMaxWave()
    {
        return Dungeon_Data.GetMaxWaveCount();
    }
    public int GetCurrentWave()
    {
        return Dungeon_Data.GetWave();
    }

    protected void InitBattleField()
    {
        CreateBattleField();

        BATTLE_SPEED_TYPE speed_type = (BATTLE_SPEED_TYPE)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.BATTLE_SPEED_TYPE, 0);
        float speed = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.NORMAL_TYPE];
        if (speed_type == BATTLE_SPEED_TYPE.FAST_SPEED_X2)
        {
            speed = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.FAST_SPEED_X2];
        }
        else if (speed_type == BATTLE_SPEED_TYPE.FAST_SPEED_X3)
        {
            speed = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.FAST_SPEED_X3];
        }
        SetBattleFastSpeed(speed);
    }

    public void SetBattleFastSpeed(float speed)
    {
        Battle_Speed_Multiple = speed;
        //  team speed
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].SetBattleSpeed(speed);
        }
        //  effect speed
        GetEffectFactory().SetEffectSpeedMultiple(Battle_Speed_Multiple);

    }

    protected void CreateBattleField()
    {
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Fields/Battle_Field_01", this.transform);
        Field = obj.GetComponent<BattleField>();
        Field.transform.localPosition = Vector3.zero;

        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].SetHeroContainer(Field.GetUnitContainer());
        }
    }

    protected void CreateTeamManagers()
    {
        var left_team = new TeamManager_V2(TEAM_TYPE.LEFT);
        left_team.SetManagers(this, UI_Mng);
        left_team.SetGameType(Game_Type);
        left_team.SetBattleDungeonData(Dungeon_Data);
        Used_Team_List.Add(left_team);

        var right_team = new TeamManager_V2(TEAM_TYPE.RIGHT);
        right_team.SetManagers(this, UI_Mng);
        right_team.SetGameType(Game_Type);
        right_team.SetBattleDungeonData(Dungeon_Data);
        Used_Team_List.Add(right_team);

       
    }

    /// <summary>
    /// 양 팀의 유닛을 모두 스폰해준다.
    /// </summary>
    protected void SpawnUnits()
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].SpawnHeros();
        }

        var myteam = FindTeamManager(TEAM_TYPE.LEFT);
        Skill_Slot_Mng.SetHeroMembers(myteam.GetMembers());
        myteam.SetTeamLastSibling();
    }




    protected void WaveInfoCloseCallback()
    {
        ChangeState(GAME_STATES.MOVE_IN);
    }

    protected void TimeOutInfoCloseCallback()
    {
        ChangeState(GAME_STATES.GAME_OVER_LOSE);
    }

    private void OnDestroy()
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].Dispose();
        }
        Used_Team_List.Clear();

        Dungeon_Data?.Dispose();
        Dungeon_Data = null;
    }

    public TeamManager_V2 FindTeamManager(TEAM_TYPE team_type)
    {
        return Used_Team_List.Find(x => x.Team_Type == team_type);
    }

    public void StartUltimateSkill()
    {
        ChangeState(GAME_STATES.ULTIMATE_SKILL);
    }
    public void FinishUltimateSkill(HeroBase_V2 caster)
    {
        ShowAllUnits();
        var all_death_team = Used_Team_List.Find(x => !x.IsAliveMembers());
        //  궁극기 사용 후 전멸한 팀이 존재할 경우
        if (all_death_team != null)
        {
            //  wait and playing
            StartCoroutine(DelayChangeStatePlaying(.4f / Battle_Speed_Multiple, caster));
        }
        else
        {
            caster.ChangeState(UNIT_STATES.ATTACK_READY_1);
            AllResumeUnitWithoutHero(caster);
            GetEffectFactory().OnResumeAndShow();
            ChangeState(GAME_STATES.PLAYING);
        }
    }
    /// <summary>
    /// 궁극기 사용 후 전멸한 팀이 존재할 경우<br/>
    /// 잠시 대기 후 상태 변경
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="caster"></param>
    /// <returns></returns>
    IEnumerator DelayChangeStatePlaying(float delay, HeroBase_V2 caster)
    {
        yield return new WaitForSeconds(delay);
        caster.ChangeState(UNIT_STATES.ATTACK_READY_1);
        AllResumeUnitWithoutHero(caster);
        GetEffectFactory().OnResumeAndShow();
        ChangeState(GAME_STATES.PLAYING);
    }

    /// <summary>
    /// 일부 타겟을 제외한 나머지 유닛 감추기
    /// </summary>
    /// <param name="exclude_targets"></param>
    public void HideAllUnitWithoutTargets(List<HeroBase_V2> exclude_targets)
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].HideAllUnitWithoutTargets(exclude_targets);
        }
    }

    /// <summary>
    /// 일부 타겟을 제외한 나머지 유닛 보이기
    /// </summary>
    /// <param name="exclude_targets"></param>
    public void ShowAllUnitWithoutTargets(List<HeroBase_V2> exclude_targets)
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].ShowAllUnitWithoutTargets(exclude_targets);
        }
    }
    /// <summary>
    /// 지정 유닛만 보이도록
    /// </summary>
    /// <param name="include_target"></param>
    public void ShowUnitTargets(List<HeroBase_V2> include_target)
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].ShowUnitTargets(include_target);
        }
    }
    
    /// <summary>
    /// 모든 유닛 보이기
    /// </summary>
    public void ShowAllUnits()
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].ShowAllUnits();
        }
    }
    /// <summary>
    /// 모든 유닛 감추기
    /// </summary>
    public void HideAllUnits()
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].HideAllUnits();
        }
    }
    /// <summary>
    /// 시전자를 제외한 나머지 모든 유닛 일시 정지
    /// </summary>
    /// <param name="hero"></param>
    public void AllPauseUnitWithoutHero(HeroBase_V2 hero)
    {
        int cnt = Used_Team_List.Count;
        for(int i = 0;i < cnt;i++)
        {
            Used_Team_List[i].AllPauseTeamMembersWithoutHero(hero);
        }
    }
    /// <summary>
    /// 시전자를 제외한 나머지 모든 유닛 일시 정지 해제
    /// </summary>
    /// <param name="hero"></param>
    public void AllResumeUnitWithoutHero(HeroBase_V2 hero)
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].AllResumeTeamMembersWithoutHero(hero);
        }
    }


    /// <summary>
    /// 전투에서 사용했던 모든 오브젝트들 모두 제거<br/>
    /// CustomUpdate 를 사용하는 오브젝트만
    /// </summary>
    public void ReleaseAllBattleObjects()
    {
        //int cnt = Used_Team_List.Count;
        //for (int i = 0; i < cnt; i++)
        //{
        //    Used_Team_List[i].Dispose();
        //}
    }


    /// <summary>
    /// 각 팀에 공통적인 상태를 변경시키기 위한 함수
    /// </summary>
    /// <param name="trans"></param>
    protected void TeamMembersChangeState(UNIT_STATES trans)
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].ChangeStateTeamMembers(trans);
        }
    }

    /// <summary>
    /// 각 팀의 멤버들의 상태를 이전상태로 돌리기 위한 함수
    /// </summary>
    protected void TeamMembersRevertState()
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].RevertStateTeamMembers();
        }
    }
    /// <summary>
    /// 던전 제한시간 갱신
    /// </summary>
    protected void CalcDungeonLimitTime()
    {
        float dt = Time.deltaTime * Battle_Speed_Multiple;
        if (Dungeon_Data.CalcDundeonLimitTime(dt))
        {
            //  타임 아웃
            if (GetCurrentState() != GAME_STATES.ULTIMATE_SKILL)
            {
                ChangeState(GAME_STATES.TIME_OUT);
            }
        }
        UI_Mng.UpdateTimeLimit(Dungeon_Data.Dungeon_Limit_Time);
    }
}
