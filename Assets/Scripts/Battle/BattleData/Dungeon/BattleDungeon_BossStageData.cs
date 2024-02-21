using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDungeon_BossStageData : BattleDungeonData
{
    UserBossStageData User_Data;
    Boss_Stage_Data Stage;
    List<Wave_Data> Wave_Datas = new List<Wave_Data>();

    public BattleDungeon_BossStageData() : base(GAME_TYPE.BOSS_DUNGEON_MODE) { }

    public override void SetDungeonID(int dungeon_id)
    {
        var m = MasterDataManager.Instance;
        Stage = m.Get_BossStageData(dungeon_id);
        m.Get_WaveDataList(Stage.wave_group_id, ref Wave_Datas);

        var mng = GameData.Instance.GetUserBossStageDataManager();
        User_Data = mng.FindUserBossDungeonData(Stage.boss_stage_id);
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

    public override int GetPlayerCharacterExp()
    {
        if (Stage != null)
        {
            return Stage.character_exp;
        }
        return 0;
    }
    public override int GetPlayerCharacterDestinyExp()
    {
        if (Stage != null)
        {
            return Stage.destiny_exp;
        }
        return 0;
    }

    public override int GetDefaultClearReward()
    {
        if (Stage != null)
        {
            return Stage.gold;
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
}