using System.Collections.Generic;

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

    /// <summary>
    /// 던전 제한 시간
    /// </summary>
    public float Dungeon_Limit_Time { get; protected set; } = 0f;

    public BattleDungeonData(GAME_TYPE gtype) { Game_Type = gtype; }
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
    /// 사용자 던전 데이터 반환
    /// </summary>
    /// <returns></returns>
    public virtual object GetUserDungeonData() { return null; }

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
    /// <summary>
    /// 다음 웨이브로 이동
    /// </summary>
    /// <returns>다음 웨이브가 있으면 <b>true</b>, 마지막 웨이브면 <b>false</b></returns>
    public virtual bool NextWave() { return false; }
    /// <summary>
    /// 몬스터 프리팹 경로 반환
    /// </summary>
    /// <param name="path"></param>
    public virtual void GetMonsterPrefabsPath(ref List<string> path) { }
    /// <summary>
    /// 몬스터 궁극기 스킬 보이스 반환
    /// </summary>
    /// <param name="path"></param>
    public virtual void GetMonsterSkillVoiceAndFxSoundPath(ref List<string> path) { }
    

    public virtual object GetWaveData() { return null; }

    /// <summary>
    /// 던전 제한 시간 계산<br/>
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>제한시간이 남아 있으면 <b>false</b>, 제한시간이 모두 소진되었으면 <b>true</b></returns>
    public virtual bool CalcDundeonLimitTime(float dt) { return false; }

    /// <summary>
    /// NPC의 스킬에 사용될 보이스 및 이벡트 사운드 경로들을 가져온다
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="sound_list"></param>
    protected void GetNpcSkillVoiceAndSfxSoundPath(Npc_Data npc, ref List<string> sound_list)
    {
        var m = MasterDataManager.Instance;
        List<Npc_Skill_Data> skill_list = new List<Npc_Skill_Data>();

        //  battle data
        var bdata = m.Get_NpcBattleData(npc.npc_battle_id);
        if (bdata != null)
        {
            List<int> skill_group_ids = new List<int>();
            skill_group_ids.AddRange(bdata.skill_pattern);
            skill_group_ids.Add(bdata.special_skill_group_id);
            int grp_cnt = skill_group_ids.Count;

            for (int g = 0; g < grp_cnt; g++)
            {
                int gid = skill_group_ids[g];
                if (gid == 0)
                {
                    continue;
                }
                //  skill group
                var skill_group = m.Get_NpcSkillGroup(gid);
                if (skill_group == null)
                {
                    UnityEngine.Debug.Assert(false);
                    continue;
                }
                //  skill group cast voice & cas sfx
                if (!string.IsNullOrEmpty(skill_group.skill_sfx_path) && !sound_list.Contains(skill_group.skill_sfx_path))
                {
                    sound_list.Add(skill_group.skill_sfx_path);
                }
                if (!string.IsNullOrEmpty(skill_group.skill_voice_path_1) && !sound_list.Contains(skill_group.skill_voice_path_1))
                {
                    sound_list.Add(skill_group.skill_voice_path_1);
                }
                if (!string.IsNullOrEmpty(skill_group.skill_voice_path_2) && !sound_list.Contains(skill_group.skill_voice_path_2))
                {
                    sound_list.Add(skill_group.skill_voice_path_2);
                }
                if (!string.IsNullOrEmpty(skill_group.skill_voice_path_3) && !sound_list.Contains(skill_group.skill_voice_path_3))
                {
                    sound_list.Add(skill_group.skill_voice_path_3);
                }

                //  skill list
                m.Get_NpcSkillDataListBySkillGroup(skill_group.npc_skill_group_id, ref skill_list);

                //int skill_cnt = skill_list.Count;
                //for (int s = 0; s < skill_cnt; s++)
                //{
                //    var npc_skill = skill_list[s];
                //    //  npc skill effect
                //    if (!string.IsNullOrEmpty(npc_skill.trigger_effect_path) && !prefab_list.Contains(npc_skill.trigger_effect_path))
                //    {
                //        prefab_list.Add(npc_skill.trigger_effect_path);
                //    }

                //    //  onetime skill list
                //    for (int o = 0; o < npc_skill.onetime_effect_ids.Length; o++)
                //    {
                //        int onetime_skill_id = npc_skill.onetime_effect_ids[o];
                //        if (onetime_skill_id == 0)
                //        {
                //            continue;
                //        }
                //        var onetime_data = m.Get_NpcSkillOnetimeData(onetime_skill_id);
                //        if (!string.IsNullOrEmpty(onetime_data.effect_path) && !prefab_list.Contains(onetime_data.effect_path))
                //        {
                //            prefab_list.Add(onetime_data.effect_path);
                //        }
                //    }
                //    //  duration skill list
                //    for (int d = 0; d < npc_skill.duration_effect_ids.Length; d++)
                //    {
                //        int duration_skill_id = npc_skill.duration_effect_ids[d];
                //        if (duration_skill_id == 0)
                //        {
                //            continue;
                //        }
                //        var duration_data = m.Get_NpcSkillDurationData(duration_skill_id);
                //        if (!string.IsNullOrEmpty(duration_data.effect_path) && !prefab_list.Contains(duration_data.effect_path))
                //        {
                //            prefab_list.Add(duration_data.effect_path);
                //        }
                //    }
                //}
            }
        }
    }

    /// <summary>
    /// NPC의 스킬에 사용될 이펙트 프리팹 경로들을 가져온다
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="prefab_list"></param>
    protected void GetNpcSkillEffectPrefabPath(Npc_Data npc, ref List<string> prefab_list)
    {
        var m = MasterDataManager.Instance;
        List<Npc_Skill_Data> skill_list = new List<Npc_Skill_Data>();

        //  battle data
        var bdata = m.Get_NpcBattleData(npc.npc_battle_id);
        if (bdata != null)
        {
            List<int> skill_group_ids = new List<int>();
            skill_group_ids.AddRange(bdata.skill_pattern);
            skill_group_ids.Add(bdata.special_skill_group_id);
            int grp_cnt = skill_group_ids.Count;

            for (int g = 0; g < grp_cnt; g++)
            {
                int gid = skill_group_ids[g];
                if (gid == 0)
                {
                    continue;
                }
                //  skill group
                var skill_group = m.Get_NpcSkillGroup(gid);
                if (skill_group == null)
                {
                    UnityEngine.Debug.Assert(false);
                    continue;
                }
                //  skill group cast effect
                if (skill_group.cast_effect_path != null)
                {
                    for (int c = 0; c < skill_group.cast_effect_path.Length; c++)
                    {
                        string cast_path = skill_group.cast_effect_path[c];
                        if (!string.IsNullOrEmpty(cast_path) && !prefab_list.Contains(cast_path))
                        {
                            prefab_list.Add(cast_path);
                        }
                    }
                }

                //  skill list
                m.Get_NpcSkillDataListBySkillGroup(skill_group.npc_skill_group_id, ref skill_list);

                int skill_cnt = skill_list.Count;
                for (int s = 0; s < skill_cnt; s++)
                {
                    var npc_skill = skill_list[s];
                    //  npc skill effect
                    if (!string.IsNullOrEmpty(npc_skill.trigger_effect_path) && !prefab_list.Contains(npc_skill.trigger_effect_path))
                    {
                        prefab_list.Add(npc_skill.trigger_effect_path);
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
                        if (!string.IsNullOrEmpty(onetime_data.effect_path) && !prefab_list.Contains(onetime_data.effect_path))
                        {
                            prefab_list.Add(onetime_data.effect_path);
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
                        if (!string.IsNullOrEmpty(duration_data.effect_path) && !prefab_list.Contains(duration_data.effect_path))
                        {
                            prefab_list.Add(duration_data.effect_path);
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// 해당 던전 완료시 획득 가능한 플레이어 경험치<br/>
    /// 일반 스테이지 : use_stamina
    /// </summary>
    /// <returns></returns>
    public virtual int GetPlayerExp() { return 0; }

    /// <summary>
    /// 해당 던전 완료시 획득 가능한 캐릭터 경험치<br/>
    /// 일반 스테이지 : character_exp
    /// </summary>
    /// <returns></returns>
    public virtual int GetPlayerCharacterExp() { return 0; }

    /// <summary>
    /// 해당 던전 클리어 시 획득 가능한 캐릭터 호감도 경험치<br/>
    /// 일반 스테이지 : destiny_exp
    /// </summary>
    /// <returns></returns>
    public virtual int GetPlayerCharacterDestinyExp() { return 0; }

    /// <summary>
    /// 해당 던전 클리어시 획득 가능한 기본 보상<br/>
    /// 일반 스테이지 : 골드
    /// </summary>
    /// <returns></returns>
    public virtual int GetDefaultClearReward() { return 0; }

    /// <summary>
    /// 완료된 던전인지 여부 반환(1회 이상 클리어한 던전인지)
    /// </summary>
    /// <returns></returns>
    public virtual bool IsClearedDungeon() { return false; }
    /// <summary>
    /// 별 포인트가 있는 던전에서, 획득한 별 포인트
    /// </summary>
    /// <returns></returns>
    public virtual int GetStarPoint() {  return 0; }

    /// <summary>
    /// 초회 보상 그룹 ID 반환
    /// </summary>
    /// <returns></returns>
    public virtual int GetFirstRewardGroupID() { return 0; }
    /// <summary>
    /// 반복 보상 그룹 ID 반환
    /// </summary>
    /// <returns></returns>
    public virtual int GetRepeatRewardGroupID() { return 0; }
    /// <summary>
    /// 별 보상 그룹 ID 반환
    /// </summary>
    /// <returns></returns>
    public virtual int GetStarPointRewardGroupID() { return 0; }    

    /// <summary>
    /// 던전 BGM 경로 반환
    /// </summary>
    /// <returns></returns>
    public virtual string GetBGMPath() { return string.Empty; }
}
