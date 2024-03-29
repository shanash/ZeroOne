
using System.Collections.Generic;
using System.Linq;

public class BattleDungeon_StoryStageData : BattleDungeonData
{
    UserStoryStageData User_Data;
    Stage_Data Stage;
    List<Wave_Data> Wave_Datas = new List<Wave_Data>();
    public BattleDungeon_StoryStageData() : base(GAME_TYPE.STORY_MODE) { }

    public override void SetDungeonID(int dungeon_id)
    {
        var m = MasterDataManager.Instance;
        Stage = m.Get_StageData(dungeon_id);
        m.Get_WaveDataList(Stage.wave_group_id, ref Wave_Datas);

        Dungeon_Limit_Time = Wave_Datas.Sum(x => x.wave_time);

        var mng = GameData.Instance.GetUserStoryStageDataManager();
        User_Data = mng.FindUserStoryStageData(Stage.stage_id);
    }


    /// <summary>
    /// 해당 웨이브 데이터
    /// </summary>
    /// <returns></returns>
    public override object GetWaveData()
    {
        return Wave_Datas[Wave];
    }

    public override int GetMaxWaveCount()
    {
        return Wave_Datas.Count;
    }

    public override bool NextWave()
    {
        Wave += 1;
        if (Wave > Wave_Datas.Count - 1)
        {
            Wave = Wave_Datas.Count - 1;
            return false;   //  마지막 웨이브
        }
        return true;    //  다음 웨이브
    }

    /// <summary>
    /// 각 웨이브별 몬스터의 프리팹 경로를 반환
    /// 중복되는 프리팹 경로는 추가하지 않는다.
    /// </summary>
    /// <returns></returns>
    public override void GetMonsterPrefabsPath(ref List<string> list)
    {
        var m = MasterDataManager.Instance;
        int wave_cnt = Wave_Datas.Count;
        //  웨이브별 npc 프리팹
        for (int w = 0; w < wave_cnt; w++)
        {
            var wdata = Wave_Datas[w];
            int len = wdata.enemy_appearance_info.Length;
            for (int i = 0; i < len; i++)
            {
                int npc_id = wdata.enemy_appearance_info[i];
                var npc = m.Get_NpcData(npc_id);
                if (npc != null)
                {
                    //  npc prefab path add
                    if (!list.Contains(npc.prefab_path))
                    {
                        list.Add(npc.prefab_path);
                    }

                    GetNpcSkillEffectPrefabPath(npc, ref list);
                }
            }
        }
    }

    public override void GetMonsterSkillVoiceAndFxSoundPath(ref List<string> list)
    {
        var m = MasterDataManager.Instance;

        //  bgm 
        string bgm_path = GetBGMPath();
        if (!string.IsNullOrEmpty(bgm_path) && !list.Contains(bgm_path))
        {
            list.Add(bgm_path);
        }

        int wave_cnt = Wave_Datas.Count;
        //  웨이브별 npc 프리팹
        for (int w = 0; w < wave_cnt; w++)
        {
            var wdata = Wave_Datas[w];
            int len = wdata.enemy_appearance_info.Length;
            for (int i = 0; i < len; i++)
            {
                int npc_id = wdata.enemy_appearance_info[i];
                var npc = m.Get_NpcData(npc_id);
                if (npc != null)
                {
                    GetNpcSkillVoiceAndSfxSoundPath(npc, ref list);
                }
            }
        }
    }


    public override int GetPlayerExp()
    {
        var repeat_reward_data_list = MasterDataManager.Instance.Get_RewardSetDataList(GetRepeatRewardGroupID()).ToList();
        if (repeat_reward_data_list.Count > 0)
        {
            var found = repeat_reward_data_list.Find(x => x.reward_type == REWARD_TYPE.EXP_PLAYER);
            if (found != null)
            {
                return found.var1;
            }
        }

        return 0;
    }

    public override int GetPlayerCharacterExp()
    {
        var repeat_reward_data_list = MasterDataManager.Instance.Get_RewardSetDataList(GetRepeatRewardGroupID()).ToList();
        if (repeat_reward_data_list.Count > 0)
        {
            var found = repeat_reward_data_list.Find(x => x.reward_type == REWARD_TYPE.EXP_CHARACTER);
            if (found != null)
            {
                return found.var1;
            }
        }
        return 0;
    }
    public override int GetPlayerCharacterDestinyExp()
    {
        var repeat_reward_data_list = MasterDataManager.Instance.Get_RewardSetDataList(GetRepeatRewardGroupID()).ToList();
        if (repeat_reward_data_list.Count > 0)
        {
            var found = repeat_reward_data_list.Find(x => x.reward_type == REWARD_TYPE.FAVORITE);
            if (found != null)
            {
                return found.var1;
            }
        }
        return 0;
    }

    public override int GetDefaultClearReward()
    {
        var repeat_reward_data_list = MasterDataManager.Instance.Get_RewardSetDataList(GetRepeatRewardGroupID()).ToList();
        if (repeat_reward_data_list.Count > 0)
        {
            var found = repeat_reward_data_list.Find(x => x.reward_type == REWARD_TYPE.GOLD);
            if (found != null)
            {
                return found.var1;
            }
        }
        return 0;
    }

    public override object GetDungeonData()
    {
        return Stage;
    }

    public override object GetUserDungeonData()
    {
        return User_Data;
    }

    public override int GetStarPoint()
    {
        return User_Data.GetStarPoint();
    }

    public override bool IsClearedDungeon()
    {
        return User_Data.IsStageCleared();
    }

    public override int GetFirstRewardGroupID()
    {
        return Stage.first_reward_group_id;
    }
    public override int GetRepeatRewardGroupID()
    {
        return Stage.repeat_reward_group_id;
    }
    public override int GetStarPointRewardGroupID()
    {
        return Stage.star_reward_group_id;
    }

    public override string GetBGMPath()
    {
        return Stage.bgm_path;
    }

    public override bool CalcDundeonLimitTime(float dt)
    {
        Dungeon_Limit_Time -= dt;
        if (Dungeon_Limit_Time < 0f)
        {
            Dungeon_Limit_Time = 0f;
            return true;
        }
        return false;
    }
}
