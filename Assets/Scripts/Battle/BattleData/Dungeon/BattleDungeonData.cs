using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 모든 던전 데이터의 최상위 데이터
/// 스토리, 이벤트 던전, 레이드 던전 등 다양한 던전의 최상위 클래스
/// </summary>
public class BattleDungeonData : BattleDataBase
{
    public GAME_TYPE Game_Type { get; protected set; }

    /// <summary>
    /// 웨이브 진행 상황
    /// </summary>
    public int Wave { get; protected set; } = 0;

    public BattleDungeonData(GAME_TYPE gtype) {  Game_Type = gtype; }
    /// <summary>
    /// 던전 정보 세팅
    /// </summary>
    /// <param name="dungeon_id"></param>
    public virtual void SetDungeonID(int dungeon_id) { }
    /// <summary>
    /// 던전 데이터 반환
    /// </summary>
    /// <returns></returns>
    public virtual object GetDungeonData() { return null; }

    /// <summary>
    /// 웨이브 인덱스. 
    /// 라벨에 표시할땐 +1 해서 표시
    /// </summary>
    /// <returns></returns>
    public int GetWave()
    {
        return Wave;
    }

    /// <summary>
    /// 최대 웨이브 수
    /// </summary>
    /// <returns></returns>
    public virtual int GetMaxWaveCount()
    {
        return 0;
    }

    public bool HasNextWave()
    {
        return GetWave() < GetMaxWaveCount();
    }

    public virtual bool NextWave() { return false; }

    public virtual void GetMonsterPrefabsPath(ref List<string> path) { }

    public virtual object GetWaveData() {  return null; }

    /// <summary>
    /// NPC의 스킬에 사용될 이펙트 프리팹 경로들을 가져온다
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="list"></param>
    protected void GetNpcSkillEffectPrefabPath(Npc_Data npc, ref List<string> list) 
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
                    if (!string.IsNullOrEmpty(npc_skill.trigger_effect_path) && !list.Contains(npc_skill.trigger_effect_path))
                    {
                        list.Add(npc_skill.trigger_effect_path);
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
