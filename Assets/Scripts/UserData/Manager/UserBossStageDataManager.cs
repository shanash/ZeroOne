using FluffyDuck.Util;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserBossStageDataManager : ManagerBase
{
    /// <summary>
    /// 기간 충전 횟수(정해진 스케쥴에 따라 충전되는 횟수)<br/>
    /// Count는 입장 가능 총 횟수임. 스케쥴에 따라 충전되면 Count = Max_Count로 초기화 되어야 함
    /// </summary>
    SecureVar<int> Count = null;
    /// <summary>
    /// 마지막 입장 시점.(이 시점을 기준으로 충전)<br/>
    /// 사용하지 않았을 경우, 재 충전시 Datetime.Mindate로 설정함
    /// </summary>
    string Last_Used_Dt = string.Empty;

    DateTime Last_Used_Datetime;

    List<UserBossStageData> User_Boss_Dungeon_Data_List = new List<UserBossStageData>();

    Dungeon_Data Dungeon;
    Schedule_Data Schedule;
    Max_Bound_Info_Data Max_Bound_Data;

    

    public UserBossStageDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype) { }

    protected override void Reset()
    {
        if (Count == null)
        {
            Count = new SecureVar<int>();
        }
        Last_Used_Dt = string.Empty;
        Last_Used_Datetime = DateTime.MinValue;
    }

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
        if (!InitDungeonData())
        {
            return;
        }


        //  보스 던전은 보스 데이터를 조회한다.
        var boss_list = m.Get_BossDataList(Dungeon.dungeon_group_id);
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

    bool InitDungeonData()
    {
        var m = MasterDataManager.Instance;

        Dungeon = m.Get_DungeonData(GAME_TYPE.BOSS_DUNGEON_MODE);
        if (Dungeon == null)
        {
            return false;
        }
        Schedule = m.Get_ScheduleData(Dungeon.schedule_id);
        if (Schedule == null)
        {
            return false;
        }
        Max_Bound_Data = m.Get_MaxBoundInfoData(REWARD_TYPE.BOSS_DUNGEON_TICKET);
        return true;
    }

    public Dungeon_Data GetDungeonData()
    {
        return Dungeon;
    }

    /// <summary>
    /// 사용자 보스 던전 데이터 찾기
    /// </summary>
    /// <param name="boss_dungeon_id"></param>
    /// <returns></returns>
    public UserBossStageData FindUserBossDungeonData(int boss_dungeon_id)
    {
        return User_Boss_Dungeon_Data_List.Find(x => x.Boss_Dungeon_ID == boss_dungeon_id);
    }

    /// <summary>
    /// 지정 보스의 던전 리스트 찾기
    /// </summary>
    /// <param name="boss_stage_group_id"></param>
    /// <returns></returns>
    public List<UserBossStageData> FindUserBossDungeonDataList(int boss_stage_group_id)
    {
        return User_Boss_Dungeon_Data_List.FindAll(x => x.GetBossStageGroupID() == boss_stage_group_id);
    }

    /// <summary>
    /// 사용자 던전 데이터 추가
    /// </summary>
    /// <param name="boss_dungeon_id"></param>
    /// <returns></returns>
    UserBossStageData AddUserBossDungeonData(int boss_dungeon_id)
    {
        if (boss_dungeon_id == 0)
        {
            return null;
        }
        var stage = FindUserBossDungeonData(boss_dungeon_id);
        if (stage == null)
        {
            stage = new UserBossStageData();
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
    /// 보스 던전 스킵 요청
    /// </summary>
    /// <param name="boss_stage_id"></param>
    /// <returns></returns>
    public ERROR_CODE SkipBossStage(int boss_stage_id)
    {
        //  입장 횟수 부족
        if (!IsEnableEntranceBossDungeon())
        {
            return ERROR_CODE.NOT_ENOUGH_ITEM;
        }
        var dungeon = FindUserBossDungeonData(boss_stage_id);
        if (dungeon != null)
        {
            dungeon.AddDailyChallengeCount();
            return BossStageWin(boss_stage_id);
        }
        return ERROR_CODE.FAILED;
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
        if (!IsEnableEntranceBossDungeon())
        {
            return ERROR_CODE.FAILED;
        }
        //  입장 횟수 차감
        int cnt = GetCount();
        cnt += 1;
        Count.Set(cnt);

        var now = DateTime.Now.ToLocalTime();
        Last_Used_Datetime = now;
        Last_Used_Dt = Last_Used_Datetime.ToString(GameDefine.DATE_TIME_FORMAT);
        Is_Update_Data = true;

        stage.AddWinCount();
        OpenNextBossStage(boss_stage_id);
        return ERROR_CODE.SUCCESS;
    }
    /// <summary>
    /// 마지막 사용 시간 가져오기
    /// </summary>
    /// <returns></returns>
    public DateTime GetLastUseDateTime()
    {
        if (!string.IsNullOrEmpty(Last_Used_Dt))
        {
            if (Last_Used_Datetime == DateTime.MinValue)
            {
                Last_Used_Datetime = DateTime.Parse(Last_Used_Dt);
            }
            return Last_Used_Datetime;
        }
        return DateTime.MinValue;
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
    public bool IsEnableBossStageEntrance(int boss_stage_id)
    {
        return User_Boss_Dungeon_Data_List.Exists(x => x.Boss_Dungeon_ID == boss_stage_id);
    }

    /// <summary>
    /// 지정 보스의 마지막 오픈 던전 반환
    /// </summary>
    /// <param name="boss_id"></param>
    /// <returns></returns>
    public UserBossStageData GetLastOpenDungeon(int boss_id)
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


    public int GetCount()
    {
        return Count.Get();
    }

    public int GetEntranceCount()
    {
        return GetMaxEntranceCount() - GetCount();
    }

    public int GetMaxEntranceCount()
    {
        if (Max_Bound_Data != null)
        {
            return (int)Max_Bound_Data.base_max;
        }
        return 3;
    }

    /// <summary>
    /// 컨텐츠 오픈에 필요한 스테이지 클리어 ID
    /// </summary>
    /// <returns></returns>
    public int GetNeedClearDungeonStageID()
    {
        return Dungeon.open_dungeon_id;
    }

    /// <summary>
    /// 보스 던전 컨텐츠가 오픈되었는지 여부 반환<br/>
    /// 시간 오픈 체크<br/>
    /// 던전 클리어 오픈 체크
    /// </summary>
    /// <returns></returns>
    public bool IsBossDungeonOpen()
    {
        if (!IsDungeonOpenTime())
        {
            return false;
        }
        //  던전 클리어 오픈 체크
        if (Dungeon.open_game_type == GAME_TYPE.STORY_MODE)
        {
            var story_mng = GameData.Instance.GetUserStoryStageDataManager();
            var stage = story_mng.FindUserStoryStageData(Dungeon.open_dungeon_id);
            if (stage != null)
            {
                return stage.IsStageCleared();
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    /// <summary>
    /// 보스 던전은 이미 오픈된 상태<br/>
    /// 보스 리스트 중에서 해당 보스가 오픈되었는지 여부 판단
    /// </summary>
    /// <param name="boss_id"></param>
    /// <returns></returns>
    public bool IsBossOpen(int boss_id)
    {
        if (!IsBossDungeonOpen())
        {
            return false;
        }

        //  던전 오픈 조건 체크
        var boss = MasterDataManager.Instance.Get_BossData(boss_id);
        if (boss != null)
        {
            if (boss.open_game_type == GAME_TYPE.STORY_MODE)
            {
                var story_mng = GameData.Instance.GetUserStoryStageDataManager();
                var stage = story_mng.FindUserStoryStageData(boss.open_dungeon_id);
                if (stage != null)
                {
                    return stage.IsStageCleared();
                }
                else 
                {
                    return false; 
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 던전 컨텐츠의 스케쥴 오픈 여부 확인
    /// </summary>
    /// <returns></returns>
    bool IsDungeonOpenTime()
    {
        bool is_open = true;
        if (Schedule == null)
        {
            return false;
        }
        var begin_dt = DateTime.Parse(Schedule.date_start);
        var end_dt = DateTime.Parse(Schedule.date_end);
        var now = DateTime.Now.ToLocalTime();
        if (begin_dt > now || now > end_dt)
        {
            is_open = false;
        }

        return is_open;
    }


    /// <summary>
    /// 보스 던전에 진입 가능한지 여부 체크(입장권이 남아 있는지 여부)<br/>
    /// 보스 던전에 총 1일 입장 제한 횟수가 있음<br/>
    /// 각 보스당 1일 입장 제한 횟수가 지정되는 것이 아님<br/>
    /// </summary>
    /// <returns></returns>
    public bool IsEnableEntranceBossDungeon()
    {
        return GetEntranceCount() > 0;
    }


    public override ERROR_CODE CheckDateAndTimeCharge()
    {
        ERROR_CODE code = ERROR_CODE.NOT_WORK;
        //  던전이 오픈 기간이 아니면 아무것도 안함
        if (!IsDungeonOpenTime())
        {
            return ERROR_CODE.NOT_WORK;
        }

        var now = DateTime.Now.ToLocalTime();
        var last_dt = GetLastUseDateTime();
        if (last_dt != DateTime.MinValue && last_dt.Date < now.Date)
        {
            var open_time = TimeSpan.Parse(Schedule.time_open);
            //  마지막 접속 시간을 기준으로 익일 새벽 시간을 찾는다.
            var refresh_dt = last_dt.Date.Add(open_time);
            if (refresh_dt <= last_dt)
            {
                refresh_dt = refresh_dt.AddDays(1);
            }

            //  현재 접속 시간과 갱신 시간을 체크해서, 현재 시간이 갱신시간을 넘었을 경우 갱신 해줌.
            if (now >= refresh_dt)
            {
                //  갱신일이 아니라면, 갱신하지 않음.
                if (!IsExistDayOfWeeks((int)refresh_dt.DayOfWeek))
                {
                    return code;
                }
                for (int i = 0; i < User_Boss_Dungeon_Data_List.Count; i++)
                {
                    User_Boss_Dungeon_Data_List[i].ResetDailyData();
                }
                Count.Set(0);
                Last_Used_Dt = string.Empty;
                Last_Used_Datetime = DateTime.MinValue;
                Is_Update_Data = true;
                code = ERROR_CODE.SUCCESS;
            }
        }

        return code;
    }

    /// <summary>
    /// 해당 요일이 갱신 지정일로 정해져 있는지 여부 반환
    /// </summary>
    /// <param name="day_of_week"></param>
    /// <returns></returns>
    bool IsExistDayOfWeeks(int day_of_week)
    {
        bool is_exist = false;
        for (int i = 0; i < Schedule.day_of_weeks.Length; i++)
        {
            if (Schedule.day_of_weeks[i] == day_of_week)
            {
                is_exist = true;
                break;
            }
        }
        return is_exist;
    }

    public override JsonData Serialized()
    {
        var json = new JsonData();


        json[NODE_ENTRANCE_COUNT] = GetCount();
        if (!string.IsNullOrEmpty(Last_Used_Dt))
        {
            json[NODE_LAST_USED_DT] = Last_Used_Dt;
        }

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
        InitDungeonData();

        if (json.ContainsKey(NODE_ENTRANCE_COUNT))
        {
            Count.Set(ParseInt(json, NODE_ENTRANCE_COUNT));
        }
        if (json.ContainsKey(NODE_LAST_USED_DT))
        {
            Last_Used_Dt = ParseString(json, NODE_LAST_USED_DT);
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
                        UserBossStageData item = FindUserBossDungeonData(boss_dungeon_id);
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
        CheckDateAndTimeCharge();
        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_ENTRANCE_COUNT = "cnt";
    protected const string NODE_LAST_USED_DT = "lastdt";
    protected const string NODE_DUNGEON_DATA_LIST = "dlist";

    protected const string NODE_BOSS_DUNGEON_ID = "bdid";

}
