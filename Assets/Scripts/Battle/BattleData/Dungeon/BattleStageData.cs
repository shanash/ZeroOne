using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BattleStageData : BattleDungeonData
{
    Stage_Data Stage;
    List<Wave_Data> Wave_Datas = new List<Wave_Data>();

    public BattleStageData() : base(GAME_TYPE.STORY_MODE) { }

    

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

    /// <summary>
    /// npc의 스킬에 사용될 이펙트 프리팹 경로들을 가져온다
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="list"></param>
    void GetNpcSkillEffectPrefabPath(Npc_Data npc, ref List<string> list)
    {
        var m = MasterDataManager.Instance;
        List<Npc_Skill_Data> skill_list = new List<Npc_Skill_Data>();

        //  battle data
        var bdata = m.Get_NpcBattleData(npc.npc_battle_id);
        if (bdata != null)
        {
            int grp_cnt = bdata.skill_pattern.Length;

            for (int g = 0; g < grp_cnt; g++)
            {
                //  skill group
                var skill_group = m.Get_NpcSkillGroup(bdata.skill_pattern[g]);
                if (skill_group == null)
                {
                    UnityEngine.Debug.Assert(false);
                    continue;
                }
                //  skill list
                m.Get_NpcSkillDataListBySkillGroup(skill_group.npc_skill_group_id, ref skill_list);

                int skill_cnt = skill_list.Count;
                for (int s = 0; s < skill_cnt; s++)
                {
                    var npc_skill = skill_list[s];
                    //  npc skill effect
                    if (!string.IsNullOrEmpty(npc_skill.effect_path) && !list.Contains(npc_skill.effect_path))
                    {
                        list.Add(npc_skill.effect_path);
                    }

                    //  onetime skill list
                    for (int o = 0; o < npc_skill.onetime_effect_ids.Length; o++)
                    {
                        int onetime_skill_id = npc_skill.onetime_effect_ids[o];
                        if (onetime_skill_id == 0)
                        {
                            continue;
                        }
                        var onetime_data = m.Get_NpcSkillOnetimeData(onetime_skill_id);
                        if (!string.IsNullOrEmpty(onetime_data.effect_path) && !list.Contains(onetime_data.effect_path))
                        {
                            list.Add(onetime_data.effect_path);
                        }
                    }
                    //  duration skill list
                    for (int d = 0; d < npc_skill.duration_effect_ids.Length; d++)
                    {
                        int duration_skill_id = npc_skill.duration_effect_ids[d];
                        if (duration_skill_id == 0)
                        {
                            continue;
                        }
                        var duration_data = m.Get_NpcSkillDurationData(duration_skill_id);
                        if (!string.IsNullOrEmpty(duration_data.effect_path) && !list.Contains(duration_data.effect_path))
                        {
                            list.Add(duration_data.effect_path);
                        }
                    }
                }
            }
        }
    }
}
