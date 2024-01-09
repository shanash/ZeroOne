using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageProceedingManager : MonoBehaviour
{
    [SerializeField, Tooltip("Path Box")]
    RectTransform Path_Box;

    [SerializeField, Tooltip("Team Path Container")]
    RectTransform Team_Path_Container;

    [SerializeField, Tooltip("Wave Point Box")]
    RectTransform Wave_Point_Container;

    [SerializeField, Tooltip("Stage Test Slider")]
    Slider Stage_Test_Slider;


    /// <summary>
    /// 사용중인 깃발 노드 리스트
    /// </summary>
    List<BaseFlagNode> Used_Flag_Nodes = new List<BaseFlagNode>();

    List<WavePointNode> Used_Wave_Points = new List<WavePointNode>();

    /// <summary>
    /// 팀 표시 깃발 프리팹 패스
    /// </summary>
    readonly string TEAM_FLAG_NODE_PREFAB_PATH = "Assets/AssetResources/Prefabs/StageProceed/Team_Flag_Node";
    /// <summary>
    /// 죽은 멤버 깃발 프리팹 패스
    /// </summary>
    readonly string DEATH_MEMBER_FLAG_NODE_PREFAB_PATH = "Assets/AssetResources/Prefabs/StageProceed/Death_Member_Flag_Node";

    /// <summary>
    /// Wave Point Type별 프리팹 패스
    /// </summary>
    Dictionary<WAVE_POINT_TYPE, string> Wave_Point_Paths = new Dictionary<WAVE_POINT_TYPE, string>();

    /// <summary>
    /// Path Box 길이
    /// </summary>
    float Path_Distance;

    /// <summary>
    /// 깃발 x 좌표값 보정
    /// </summary>
    readonly float FLAG_CORRECT_X = -15f;
    readonly float FLAG_OFFSET_Y = 5f;

    private void Start()
    {
        Path_Distance = Path_Box.rect.width;

        Wave_Point_Paths.Add(WAVE_POINT_TYPE.START_POINT, "Assets/AssetResources/Prefabs/StageProceed/Wave_Start_Point");
        Wave_Point_Paths.Add(WAVE_POINT_TYPE.MID_POINT, "Assets/AssetResources/Prefabs/StageProceed/Wave_Mid_Point");
        Wave_Point_Paths.Add(WAVE_POINT_TYPE.BOSS_POINT, "Assets/AssetResources/Prefabs/StageProceed/Wave_Boss_Point");
    }

    public void StartStageProceeding(WAVE_POINT_TYPE[] points)
    {
        InitWavePoints(points);
        var team = CreateTeamFlag();
        team.transform.localPosition = new Vector2(FLAG_CORRECT_X, FLAG_OFFSET_Y);
    }

    /// <summary>
    /// 각 웨이브 포인트 초기 세팅
    /// </summary>
    /// <param name="points"></param>
    void InitWavePoints(WAVE_POINT_TYPE[] points)
    {
        int len = points.Length;
        float distance = Path_Distance / (float)(len - 1);
        float offset_x = 0f;
        for (int i = 0; i < len; i++)
        {
            WAVE_POINT_TYPE type = points[i];
            string path = Wave_Point_Paths[type];
            var node = CreateWavePoint(path);
            node.transform.localPosition = new Vector2(offset_x, 0f);
            offset_x += distance;
        }
    }

    /// <summary>
    /// 팀 깃발 노드 생성
    /// </summary>
    /// <param name="cb"></param>
    /// <returns></returns>
    TeamFlagNode CreateTeamFlag()
    {
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject(TEAM_FLAG_NODE_PREFAB_PATH, Team_Path_Container);
        var node = obj.GetComponent<TeamFlagNode>();
        node.SetStageProceedingManager(this);
        Used_Flag_Nodes.Add(node);
        return node;
    }
    /// <summary>
    /// 죽은 멤버 표시 깃발 생성
    /// </summary>
    /// <returns></returns>
    DeathMemberFlagNode CreateDeathMemberFlag()
    {
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject(DEATH_MEMBER_FLAG_NODE_PREFAB_PATH, Team_Path_Container);
        var node = obj.GetComponent<DeathMemberFlagNode>();
        node.SetStageProceedingManager(this);
        node.transform.localPosition = new Vector2(FLAG_CORRECT_X, FLAG_OFFSET_Y);
        Used_Flag_Nodes.Add(node);
        return node;
    }
    /// <summary>
    /// 웨이브 포인트 노드 생성
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    WavePointNode CreateWavePoint(string path)
    {
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject(path, Wave_Point_Container);
        var node = obj.GetComponent<WavePointNode>();
        Used_Wave_Points.Add(node);
        return node;
    }


    public void UnusedFlagNode(BaseFlagNode node)
    {
        GameObjectPoolManager.Instance.UnusedGameObject(node.gameObject);
        Used_Flag_Nodes.Remove(node);
    }

    public void ClearWavePointNodes()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Wave_Points.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Wave_Points[i].gameObject);
        }
        Used_Wave_Points.Clear();
    }

    /// <summary>
    /// 팀 깃발 진행 상황 업데이트
    /// </summary>
    /// <param name="rate"></param>
    public void SetTeamProceeding(float rate)
    {
        var team = Used_Flag_Nodes.Find(x => x.GetFlagType() == FLAG_TYPE.TEAM_FLAG);
        if (team != null)
        {
            float per = Mathf.Clamp01(rate);
            float offset_x = Path_Distance * per + FLAG_CORRECT_X;
            team.transform.localPosition = new Vector2(offset_x, FLAG_OFFSET_Y);
        }
    }
    /// <summary>
    /// 죽은 멤버 깃발 추가
    /// </summary>
    /// <param name="rate"></param>
    public void AddDeathFlag(float rate)
    {
        var death = CreateDeathMemberFlag();
        float per = Mathf.Clamp01(rate);
        float offset_x = Path_Distance * per + FLAG_CORRECT_X;
        death.transform.localPosition = new Vector2(offset_x, FLAG_OFFSET_Y);
        death.transform.SetAsFirstSibling();
    }


    /// <summary>
    /// 스테이지 이동 테스트
    /// </summary>
    public void OnStageSliderChangeValue()
    {
        SetTeamProceeding(Stage_Test_Slider.value);
    }

    /// <summary>
    /// 슬라이더 위치를 기준으로 죽은 멤버 생성
    /// </summary>
    public void OnClickDeathMember()
    {
        AddDeathFlag(Stage_Test_Slider.value);
    }
}
