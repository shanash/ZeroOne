using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManager : MonoBehaviour
{
    [SerializeField, Tooltip("UI Manager")]
    BattleUIManager UI_Mng;

    [SerializeField, Tooltip("Virtual Cam Manager")]
    VirtualCineManager Cine_Mng;

    [SerializeField, Tooltip("Behavior Manager")]
    BehaviorManager Behavior_Mng;

    BattleField Field;
    
    List<TeamManager> Used_Team_List = new List<TeamManager>();
    
    public VirtualCineManager GetVirtualCineManager()
    {
        return Cine_Mng;
    }

    public BattleField GetBattleField() { return Field; }

    /// <summary>
    /// 필드 리소스 로딩
    /// </summary>
    void InitBattleField()
    {
        CreateBattleField();
        //_ = InputCanvas.Instance;
    }

    /// <summary>
    /// 배경 만들기
    /// </summary>
    void CreateBattleField()
    {
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Fields/Battle_Field_01", this.transform);
        Field = obj.GetComponent<BattleField>();
        Field.transform.localPosition = Vector3.zero;
    }
    /// <summary>
    /// 영웅 및 몬스터 스폰
    /// </summary>
    void SpawnHeroes()
    {
        var left_team = new TeamManager(TEAM_TYPE.LEFT, Field.GetUnitContainer());
        left_team.SetManager(this, UI_Mng);
        left_team.SpawnHeroes();
        Used_Team_List.Add(left_team);

        Behavior_Mng.AddHeroList(left_team.GetMembers());

        var right_team = new TeamManager (TEAM_TYPE.RIGHT, Field.GetUnitContainer());
        right_team.SetManager (this, UI_Mng);
        right_team.SpawnHeroes();
        Used_Team_List.Add(right_team);

        Behavior_Mng.AddHeroList(right_team.GetMembers());
    }

    /// <summary>
    /// 행동력 우선순위가 가장 높은 영웅의 턴 시작
    /// </summary>
    void HeroTurnOnCheck()
    {
        var first_node = Behavior_Mng.GetFirstBehaviorNode();
        if (first_node != null) 
        {
            var hero = first_node.GetHeroBase();
            hero.TurnOnHero();
        }
        else
        {
            ChangeState(GAME_STATES.GAME_OVER_WIN);
        }
    }

    /// <summary>
    /// 턴이 종료된 영웅 이벤트 트리거
    /// </summary>
    /// <param name="hero"></param>
    public void TurnFinishHero(HeroBase hero)
    {
        //  방금 턴이 종료된 영웅은 행동력 초기화
        hero.TurnEndRapidityPointReset();
        //  다른 모든 영웅들은 행동력 증가 계산(각각의 상화에 맞도록)
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var team = Used_Team_List[i];
            team.IncRapicityPoint(hero);
        }
        Behavior_Mng.UpdateHeroIconsOrder();
    }

    private void OnDestroy()
    {
        int cnt = Used_Team_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Team_List[i].Dispose();
        }
        Used_Team_List.Clear();
    }

    /// <summary>
    /// 팀 매니저 찾기
    /// </summary>
    /// <param name="ttype"></param>
    /// <returns></returns>
    public TeamManager FindTeamManager(TEAM_TYPE ttype)
    {
        return Used_Team_List.Find(x => x.Team_Type == ttype);
    }


}
