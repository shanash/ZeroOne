using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManager_V2 : MonoBehaviour
{

    [SerializeField, Tooltip("UI Manager")]
    protected BattleUIManager_V2 UI_Mng;
    [SerializeField, Tooltip("Virtual Cam Manager")]
    protected VirtualCineManager Cine_Mng;

    [SerializeField, Tooltip("Stage Proceeding Manager")]
    protected StageProceedingManager Stage_Mng;

    [SerializeField, Tooltip("Fade In Out Layer")]
    UIEaseCanvasGroupAlpha Fade_In_Out_Layer;

    protected List<TeamManager_V2> Used_Team_List = new List<TeamManager_V2>();

    protected BattleField Field;

    protected GAME_TYPE Game_Type = GAME_TYPE.NONE;


    protected BattleDungeonData Dungeon_Data;


    public VirtualCineManager GetVirtualCineManager()
    {
        return Cine_Mng;
    }

    public BattleField GetBattleField() { return Field; }

    public EffectFactory GetEffectFactory() { return Field.GetEffectFactory(); }

    protected void InitBattleField()
    {
        CreateBattleField();
        //CreateTeamManagers();
    }

    protected void CreateBattleField()
    {
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Fields/Battle_Field_01", this.transform);
        Field = obj.GetComponent<BattleField>();
        Field.transform.localPosition = Vector3.zero;

        int cnt = Used_Team_List.Count;
        for ( int i = 0; i < cnt; i++ )
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
    }


    protected void StartStageProceeding()
    {
        if (Stage_Mng == null)
        {
            return;
        }
        WAVE_POINT_TYPE[] points = new WAVE_POINT_TYPE[] { WAVE_POINT_TYPE.START_POINT, WAVE_POINT_TYPE.MID_POINT, WAVE_POINT_TYPE.MID_POINT, WAVE_POINT_TYPE.BOSS_POINT  };
        Stage_Mng.StartStageProceeding(points);
    }

    protected void WaveInfoCloseCallback()
    {
        ChangeState(GAME_STATES.PLAYING);
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
