using FluffyDuck.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserBossDungeonDataManager : ManagerBase
{
    List<UserBossDungeonData> User_Boss_Dungeon_Data_List = new List<UserBossDungeonData>();

    public UserBossDungeonDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype) { }

    protected override void Destroy()
    {
        int cnt = User_Boss_Dungeon_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Boss_Dungeon_Data_List[i].Dispose();
        }
        User_Boss_Dungeon_Data_List.Clear();
    }

    /// <summary>
    /// 보스전 던전 데이터는, 나중에 추가되거나 제외되는 던전이 있을 수 있으니<br/>
    /// 초기화에서 모든 던전 데이터를 추가해두는 것으로 하자.
    /// </summary>
    public override void InitDataManager()
    {
        var m = MasterDataManager.Instance;
        var boss_list = m.Get_BossDataList();
        if (boss_list.Count == 0)
        {
            return;
        }

        //  마스터에 등록되어 있는 모든 보스 던전 데이터 추가 (보스가 등록되어 있는 던전만 추가)
        for (int b = 0; b < boss_list.Count; b++)
        {
            var boss = boss_list[b];
            var stage_list = m.Get_BossStageDataListByBossStageGroupID(boss.boss_stage_group_id);
            //  각 보스의 첫번째 던전만 등록해둔다. 두번째 이상 스테이지는 승리하면 오픈되는 형식으로.
            //  첫번째 던전만 존재한다면 두번째부터는 진행 가능.
            if (stage_list.Count > 0)
            {
                var stage = stage_list.FirstOrDefault();
                if (stage != null)
                {
                    AddUserBossDungeonData(stage.boss_stage_id);
                }
            }
        }
        Save();
    }

    /// <summary>
    /// 사용자 보스 던전 데이터 찾기
    /// </summary>
    /// <param name="boss_dungeon_id"></param>
    /// <returns></returns>
    public UserBossDungeonData FindUserBossDungeonData(int boss_dungeon_id)
    {
        return User_Boss_Dungeon_Data_List.Find(x => x.Boss_Dungeon_ID == boss_dungeon_id);
    }

    /// <summary>
    /// 지정 보스의 던전 리스트 찾기
    /// </summary>
    /// <param name="boss_stage_group_id"></param>
    /// <returns></returns>
    public List<UserBossDungeonData> FindUserBossDungeonDataList(int boss_stage_group_id)
    {
        return User_Boss_Dungeon_Data_List.FindAll(x => x.GetBossStageGroupID() == boss_stage_group_id);
    }

    /// <summary>
    /// 사용자 던전 데이터 추가
    /// </summary>
    /// <param name="boss_dungeon_id"></param>
    /// <returns></returns>
    UserBossDungeonData AddUserBossDungeonData(int boss_dungeon_id)
    {
        if (boss_dungeon_id == 0)
        {
            return null;
        }
        var stage = FindUserBossDungeonData(boss_dungeon_id);
        if (stage == null)
        {
            stage = new UserBossDungeonData();
            stage.SetBossDungeonID(boss_dungeon_id);
            User_Boss_Dungeon_Data_List.Add(stage);
            Is_Update_Data = true;
        }
        return stage;
    }

    public void SetStarPoint(int boss_dungeon_id, int star_pt)
    {
        var stage = FindUserBossDungeonData(boss_dungeon_id);
        if (stage != null)
        {
            stage.SetStarPoint(star_pt);
        }
    }
    /// <summary>
    /// 보스 던전 승리
    /// </summary>
    /// <param name="boss_stage_id"></param>
    /// <returns></returns>
    public ERROR_CODE BossStageWin(int boss_stage_id)
    {
        var stage = FindUserBossDungeonData(boss_stage_id);
        if (stage == null)
        {
            return ERROR_CODE.FAILED;
        }

        stage.AddWinCount();
        OpenNextBossStage(boss_stage_id);
        return ERROR_CODE.SUCCESS;
    }

    /// <summary>
    /// 해당 보스 던전의 다음 스테이지 오픈<br/>
    /// 해당 보스의 마지막 던전일 경우, 다음 스테이지 오픈은 없음
    /// </summary>
    /// <param name="boss_stage_id"></param>
    void OpenNextBossStage(int boss_stage_id)
    {
        var m = MasterDataManager.Instance;
        var next_stage = m.Get_NextBossStageData(boss_stage_id);
        if (next_stage == null)
        {
            return;
        }
        AddUserBossDungeonData(next_stage.boss_stage_id);
    }

    /// <summary>
    /// 해당 스테이지 도전이 가능한지 여부 판단<br/>
    /// 이전 스테이지가 아직 클리어 되지 않았다면 오픈안됨
    /// </summary>
    /// <param name="boss_stage_id"></param>
    /// <returns></returns>
    public bool IsEnableBossDungeonEntrance(int boss_stage_id)
    {
        return User_Boss_Dungeon_Data_List.Exists(x => x.Boss_Dungeon_ID == boss_stage_id);
    }

    /// <summary>
    /// 보스 던전 오픈 여부 <br/>
    /// 각 보스별 오픈 조건이 다른데, 각각의 오픈 조건이 완료되었는지 체크한 후<br/>
    /// 조건이 충족되었을 경우에 오픈 반환<br/>
    /// 지금은 조건이 없기 때문에 M2까지는 강제 오픈. M2 이후 수정 필요
    /// </summary>
    /// <param name="boss_id"></param>
    /// <returns></returns>
    public bool IsEnableOpenBossDungeon(int boss_id)
    {
        var boss = MasterDataManager.Instance.Get_BossData(boss_id);
        return true;
    }

    /// <summary>
    /// 지정 보스의 마지막 오픈 던전 반환
    /// </summary>
    /// <param name="boss_id"></param>
    /// <returns></returns>
    public UserBossDungeonData GetLastOpenDungeon(int boss_id)
    {
        var boss = MasterDataManager.Instance.Get_BossData(boss_id);
        var find_all = FindUserBossDungeonDataList(boss.boss_stage_group_id);
        if (find_all.Count > 0)
        {
            find_all.Sort((a, b) => a.Boss_Dungeon_ID.CompareTo(b.Boss_Dungeon_ID));
            return find_all.Last();
        }
        return null;
    }

    /// <summary>
    /// 해당 보스 던전에 진입 가능한지 여부 체크<br/>
    /// 각 보스당 1일 입장 제한 횟수가 지정되어 있음<br/>
    /// 입장 제한 횟수는 각 던전에서 개별로 카운팅 되는 것이 아닌<br/>
    /// 보스별로 입장 제한 횟수가 결정되기 때문에<br/>
    /// 각 보스의 던전 총합으로 체크 가능
    /// </summary>
    /// <param name="boss_id"></param>
    /// <returns></returns>
    public bool IsEnableEntranceBossDungeon(int boss_id)
    {
        var boss = MasterDataManager.Instance.Get_BossData(boss_id);
        if (boss != null)
        {
            var find_all = FindUserBossDungeonDataList(boss.boss_stage_group_id);
            int sum = find_all.Sum(x => x.GetDailyWinCount());
            return sum < boss.enter_limit_count;
        }
        return false;
    }

    public override ERROR_CODE CheckDateAndTimeCharge()
    {
        int cnt = User_Boss_Dungeon_Data_List.Count;
        ERROR_CODE result = ERROR_CODE.NOT_WORK;
        for (int i = 0; i < cnt; i++)
        {
            ERROR_CODE code = User_Boss_Dungeon_Data_List[i].CheckDateAndTimeCharge();
            if (code != ERROR_CODE.NOT_WORK)
            {
                result = code;
            }
        }
        return result;
    }

    public override JsonData Serialized()
    {
        var json = new JsonData();

        var arr = new JsonData();
        int cnt = User_Boss_Dungeon_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Boss_Dungeon_Data_List[i];
            var jdata = item.Serialized();
            if (jdata == null)
            {
                continue;
            }
            arr.Add(jdata);
        }
        if (arr.IsArray && arr.Count > 0)
        {
            json[NODE_DUNGEON_DATA_LIST] = arr;
        }

        if (json.Keys.Count > 0)
        {
            return json;
        }
        return null;
    }
    public override bool Deserialized(JsonData json)
    {
        if (json == null)
        {
            return false;
        }

        if (json.ContainsKey(NODE_DUNGEON_DATA_LIST))
        {
            var arr = json[NODE_DUNGEON_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];

                    int boss_dungeon_id = 0;
                    if (int.TryParse(jdata[NODE_BOSS_DUNGEON_ID].ToString(), out boss_dungeon_id))
                    {
                        UserBossDungeonData item = FindUserBossDungeonData(boss_dungeon_id);
                        if (item == null)
                        {
                            item = AddUserBossDungeonData(boss_dungeon_id);
                        }
                        item?.Deserialized(jdata);
                    }
                }
            }
        }

        InitUpdateData();
        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_DUNGEON_DATA_LIST = "dlist";

    protected const string NODE_BOSS_DUNGEON_ID = "bdid";

}
