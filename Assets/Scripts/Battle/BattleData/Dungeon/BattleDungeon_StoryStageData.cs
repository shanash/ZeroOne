using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BattleDungeon_StoryStageData : BattleDungeonData
{
    Stage_Data Stage;
    List<Wave_Data> Wave_Datas = new List<Wave_Data>();

    public BattleDungeon_StoryStageData() : base(GAME_TYPE.STORY_MODE) { }

    

    public override void SetDungeonID(int dungeon_id)
    {
        var m = MasterDataManager.Instance;
        Stage = m.Get_StageData(dungeon_id);
        m.Get_WaveDataList(Stage.wave_group_id, ref Wave_Datas);
    }



    /// <summary>
    /// 해당 웨이브 데이터
    /// </summary>
    /// <returns></returns>
    public override object GetWaveData()
    {
        return Wave_Datas[Wave];
    }
    /// <summary>
    /// 최대 웨이브 수
    /// </summary>
    /// <returns></returns>
    public int GetMaxWaveCount()
    {
        return Wave_Datas.Count;
    }

    public bool NextWave()
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

    
}
