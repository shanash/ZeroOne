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
        var all_death_team = Used_Team_List.Find(x => !x.IsAliveMembers());
        if (all_death_team != null)
        {
            //  wait and playing
            StartCoroutine(DelayChangeStatePlaying(1f, caster));
        }
        else
        {
            caster.ChangeState(UNIT_STATES.ATTACK_READY_1);
            AllResumeUnitWithoutHero(caster);
            GetEffectFactory().OnResumeAndShow();
            ChangeState(GAME_STATES.PLAYING);
        }
    }

    IEnumerator DelayChangeStatePlaying(float delay, HeroBase_V2 caster)
    {
        yield return new WaitForSeconds(delay);
        caster.ChangeState(UNIT_STATES.ATTACK_READY_1);
        AllResumeUnitWithoutHero(caster);
        GetEffectFactory().OnResumeAndShow();
        ChangeState(GAME_STATES.PLAYING);

    }

    //void DelayChangeStatePlaying()
    //{
    //    ChangeState(GAME_STATES.PLAYING);
    //}

    public void HideAllUnitWithoutTargets(List<HeroBase_V2> targets)
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].HideAllUnitWithoutTargets(targets);
        }
    }
    public void ShowAllUnitWithoutTargets(List<HeroBase_V2> targets)
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].ShowAllUnitWithoutTargets(targets);
        }
    }
    
    public void AllPauseUnitWithoutHero(HeroBase_V2 hero)
    {
        int cnt = Used_Team_List.Count;
        for(int i = 0;i < cnt;i++)
        {
            Used_Team_List[i].AllPauseTeamMembersWithoutHero(hero);
        }
    }

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
}
