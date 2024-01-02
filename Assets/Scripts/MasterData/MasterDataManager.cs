using System;
using System.Collections.Generic;
using System.Linq;

public class MasterDataManager : BaseMasterDataManager
{
    static MasterDataManager __instance = null;
    public static MasterDataManager Instance
    {
        get
        {
            if (__instance == null)
            {
                __instance = new MasterDataManager();
            }
            return __instance;
        }
    }
    private MasterDataManager()
        : base()
    {
    }

    public bool IsLoaded { get { return is_init_load; } }

    #region Level
    public Player_Level_Data Get_PlayerLevelData(int lv)
    {
        Check_Player_Level_Data();
        return _Player_Level_Data.Find(x => x.level == lv);
    }
    public Player_Level_Data Get_PlayerLevelDataByAccumExp(int accum_exp)
    {
        Check_Player_Level_Data();
        var list = _Player_Level_Data.OrderBy(x => x.level).ToList();
        return list.FindLast(x => x.accum_exp <= accum_exp);
    }

    public Player_Character_Level_Data Get_PlayerCharacterLevelData(int lv)
    {
        Check_Player_Character_Level_Data();
        return _Player_Character_Level_Data.Find(x => x.level == lv);
    }

    public Player_Character_Level_Data Get_PlayerCharacterLevelDataByAccumExp(int accum_exp)
    {
        Check_Player_Character_Level_Data();
        var list = _Player_Character_Level_Data.OrderBy(x => x.level).ToList();
        return list.FindLast(x => x.accum_exp <= accum_exp);
    }

    #endregion

    #region Item
    /// <summary>
    /// 아이템 타입 데이터 반환
    /// </summary>
    /// <param name="itype"></param>
    /// <returns></returns>
    public Item_Type_Data Get_ItemTypeData(ITEM_TYPE itype)
    {
        Check_Item_Type_Data();
        return _Item_Type_Data.Find(x => x.item_type == itype);
    }
    /// <summary>
    /// 캐릭터 조각 데이터 반환
    /// </summary>
    /// <param name="player_character_id"></param>
    /// <returns></returns>
    public Character_Piece_Data Get_CharacterPieceData(int player_character_id)
    {
        Check_Character_Piece_Data();

        return _Character_Piece_Data.Find(x => x.player_character_id == player_character_id);
    }
    /// <summary>
    /// 경험치 물약 데이터 반환
    /// </summary>
    /// <param name="exp_potion_id"></param>
    /// <returns></returns>
    public Exp_Potion_Data Get_ExpPotionData(int exp_potion_id)
    {
        Check_Exp_Potion_Data();
        return _Exp_Potion_Data.Find(x => x.exp_potion_id == exp_potion_id);
    }
    /// <summary>
    /// 스태미너 물약 데이터 반환
    /// </summary>
    /// <param name="sta_potion_id"></param>
    /// <returns></returns>
    public Sta_Potion_Data Get_StaPotionData(int sta_potion_id)
    {
        Check_Sta_Potion_Data();
        return _Sta_Potion_Data.Find(x => x.sta_potion_id == sta_potion_id);
    }
    /// <summary>
    /// 메모리템(메모리얼에서 사용될 아이템) 데이터 반환
    /// </summary>
    /// <param name="memoritem_id"></param>
    /// <returns></returns>
    public Memoritem_Data Get_MemoritemData(int memoritem_id)
    {
        Check_Memoritem_Data();
        return _Memoritem_Data.Find(x => x.memoritem_id == memoritem_id);
    }
    /// <summary>
    /// 소비성 아이템 데이터 반환
    /// </summary>
    /// <param name="expendable_item_id"></param>
    /// <returns></returns>
    public Expendable_Item_Data Get_ExpendableItemData(int expendable_item_id)
    {
        Check_Expendable_Item_Data();
        return _Expendable_Item_Data.Find(x => x.expendable_item_id == expendable_item_id);
    }
    /// <summary>
    /// 호감도 아이템 데이터 반환
    /// </summary>
    /// <param name="favorite_item_id"></param>
    /// <returns></returns>
    public Favorite_Item_Data Get_FavoriteItemData(int favorite_item_id)
    {
        Check_Favorite_Item_Data();
        return _Favorite_Item_Data.Find(x => x.favorite_item_id == favorite_item_id);
    }
    /// <summary>
    /// 초회 보상 데이터 반환
    /// </summary>
    /// <param name="first_reward_id"></param>
    /// <returns></returns>
    public First_Reward_Data Get_FirstRewardData(int first_reward_id)
    {
        Check_First_Reward_Data();
        return _First_Reward_Data.Find(x => x.frist_reward_id == first_reward_id);
    }
    /// <summary>
    /// 반복 보상 데이터 반환
    /// </summary>
    /// <param name="repeat_reward_id"></param>
    /// <returns></returns>
    public Repeat_Reward_Data Get_RepeatRewardData(int repeat_reward_id)
    {
        Check_Repeat_Reward_Data();
        return _Repeat_Reward_Data.Find(x => x.repeat_reward_id ==  repeat_reward_id);
    }
    

    #endregion

    #region Player Character
    /// <summary>
    /// 지정한 플레이어 캐릭터 데이터 가져오기
    /// </summary>
    /// <param name="player_character_id"></param>
    /// <returns></returns>
    public Player_Character_Data Get_PlayerCharacterData(int player_character_id)
    {
        Check_Player_Character_Data();

        return _Player_Character_Data.Find(x => x.player_character_id == player_character_id);
    }

    /// <summary>
    /// 플레이어 캐릭터 모두 가져오기
    /// </summary>
    /// <param name="list"></param>
    public void Get_PlayerCharacterDataList(ref List<Player_Character_Data> list)
    {
        Check_Player_Character_Data();

        list.Clear();
        list.AddRange(_Player_Character_Data);
    }
    /// <summary>
    /// 플레이어 캐릭터의 전투 정보
    /// </summary>
    /// <param name="battle_info_id"></param>
    /// <returns></returns>
    public Player_Character_Battle_Data Get_PlayerCharacterBattleData(int battle_info_id)
    {
        Check_Player_Character_Battle_Data();
        return _Player_Character_Battle_Data.Find(x => x.battle_info_id == battle_info_id);
    }

    /// <summary>
    /// 플레이어 캐릭터 스킬 그룹 데이터 반환
    /// </summary>
    /// <param name="skill_group_id"></param>
    /// <returns></returns>
    public Player_Character_Skill_Group Get_PlayerCharacterSkillGroupData(int skill_group_id)
    {
        Check_Player_Character_Skill_Group();

        return _Player_Character_Skill_Group.Find(x => x.pc_skill_group_id == skill_group_id);
    }
    /// <summary>
    /// 플레이어 캐릭터 스킬 데이터 반환
    /// </summary>
    /// <param name="pc_skill_id"></param>
    /// <returns></returns>
    public Player_Character_Skill_Data Get_PlayerCharacterSkillData(int pc_skill_id)
    {
        Check_Player_Character_Skill_Data();

        return _Player_Character_Skill_Data.Find(x => x.pc_skill_id == pc_skill_id);
    }
    /// <summary>
    /// 플레이어 캐릭터의 스킬 그룹에 포함되어 있는 스킬 데이터 리스트 반환
    /// </summary>
    /// <param name="skill_group_id"></param>
    /// <param name="list"></param>
    public void Get_PlayerCharacterSkillDataListBySkillGroup(int skill_group_id, ref List<Player_Character_Skill_Data> list)
    {
        Check_Player_Character_Skill_Data();

        list.Clear();
        list.AddRange(_Player_Character_Skill_Data.FindAll(x => x.pc_skill_group_id == skill_group_id));
    }
    /// <summary>
    /// 플레이어 캐릭터의 일회성 효과 스킬 데이터 반환
    /// </summary>
    /// <param name="pc_skill_onetime_id"></param>
    /// <returns></returns>
    public Player_Character_Skill_Onetime_Data Get_PlayerCharacterSkillOnetimeData(int pc_skill_onetime_id)
    {
        Check_Player_Character_Skill_Onetime_Data();
        return _Player_Character_Skill_Onetime_Data.Find(x => x.pc_skill_onetime_id == pc_skill_onetime_id);
    }
    /// <summary>
    /// 플레이어 캐릭터의 지속성 효과 스킬 데이터 반환
    /// </summary>
    /// <param name="pc_skill_duration_id"></param>
    /// <returns></returns>
    public Player_Character_Skill_Duration_Data Get_PlayerCharacterSkillDurationData(int pc_skill_duration_id)
    {
        Check_Player_Character_Skill_Duration_Data();
        return _Player_Character_Skill_Duration_Data.Find(x => x.pc_skill_duration_id == pc_skill_duration_id);
    }

    /// <summary>
    /// 포지션에 따라 아이콘을 표시 해주기 위한 데이터
    /// </summary>
    /// <param name="pos_type"></param>
    /// <returns></returns>
    public Position_Icon_Data Get_PositionIconData(POSITION_TYPE pos_type)
    {
        Check_Position_Icon_Data();

        return _Position_Icon_Data.Find(x => x.position_type == pos_type);
    }
    #endregion

    #region None Player Character
    /// <summary>
    /// NPC 데이터 가져오기
    /// </summary>
    /// <param name="npc_data_id"></param>
    /// <returns></returns>
    public Npc_Data Get_NpcData(int npc_data_id)
    {
        Check_Npc_Data();

        return _Npc_Data.Find(x => x.npc_data_id == npc_data_id);
    }

    /// <summary>
    /// 지정한 npc id를 이용하여 모함되는 모든 npc 리스트 반환
    /// </summary>
    /// <param name="npc_ids"></param>
    /// <param name="list"></param>
    public void Get_NpcDataList(int[] npc_ids, ref List<Npc_Data> list)
    {
        Check_Npc_Data();
        list.Clear();
        int len = npc_ids.Length;
        for (int i = 0; i < len; i++)
        {
            int npc_id = npc_ids[i];
            var npc = Get_NpcData(npc_id);
            if (npc != null)
            {
                list.Add(npc);
            }
        }
    }

    /// <summary>
    /// Npc 데이터 모두 가져오기 
    /// 차후 npc의 속성,특징,종족별로 가져오기 기능도 필요.
    /// </summary>
    /// <param name="list"></param>
    public void Get_NpcDataList(ref List<Npc_Data> list)
    {
        Check_Npc_Data();

        list.Clear();
        list.AddRange(_Npc_Data);
    }
    /// <summary>
    /// npc 전투 데이터 가져오기
    /// </summary>
    /// <param name="npc_battle_info_id"></param>
    /// <returns></returns>
    public Npc_Battle_Data Get_NpcBattleData(int npc_battle_info_id)
    {
        Check_Npc_Battle_Data();

        return _Npc_Battle_Data.Find(x => x.npc_battle_id == npc_battle_info_id);
    }
    /// <summary>
    /// NPC의 레벨에 따른 스탯 증가량 정보 데이터
    /// </summary>
    /// <param name="npc_level_stat_id"></param>
    /// <returns></returns>
    public Npc_Level_Stat_Data Get_NpcLevelStatData(int npc_level_stat_id)
    {
        Check_Npc_Level_Stat_Data();
        return _Npc_Level_Stat_Data.Find(x => x.npc_level_stat_id == npc_level_stat_id);
    }

    /// <summary>
    /// npc의 스킬 그룹 정보 가져오기
    /// 
    /// </summary>
    /// <param name="npc_skill_group_id"></param>
    /// <returns></returns>
    public Npc_Skill_Group Get_NpcSkillGroup(int npc_skill_group_id)
    {
        Check_Npc_Skill_Group();

        return _Npc_Skill_Group.Find(x => x.npc_skill_group_id == npc_skill_group_id);
    }
    /// <summary>
    /// npc의 스킬 데이터 가져오기
    /// </summary>
    /// <param name="npc_skill_id"></param>
    /// <returns></returns>
    public Npc_Skill_Data Get_NpcSkillData(int npc_skill_id)
    {
        Check_Npc_Skill_Data();
        return _Npc_Skill_Data.Find(x => x.npc_skill_id == npc_skill_id);
    }
    /// <summary>
    /// npc의 스킬 그룹에 포함되어 있는 모든 스킬 데이터 가져오기
    /// </summary>
    /// <param name="npc_skill_group_id"></param>
    /// <param name="list"></param>
    public void Get_NpcSkillDataListBySkillGroup(int npc_skill_group_id, ref List<Npc_Skill_Data> list)
    {
        Check_Npc_Skill_Data();

        list.Clear();
        list.AddRange(_Npc_Skill_Data.FindAll(x => x.npc_skill_group_id == npc_skill_group_id));
    }
    /// <summary>
    /// npc의 일회성 효과 스킬 데이터 반환
    /// </summary>
    /// <param name="npc_skill_onetime_id"></param>
    /// <returns></returns>
    public Npc_Skill_Onetime_Data Get_NpcSkillOnetimeData(int npc_skill_onetime_id)
    {
        Check_Npc_Skill_Onetime_Data();
        return _Npc_Skill_Onetime_Data.Find(x => x.npc_skill_onetime_id == npc_skill_onetime_id);
    }
    /// <summary>
    /// npc의 지속성 효과 스킬 데이터 반환
    /// </summary>
    /// <param name="npc_skill_duration_id"></param>
    /// <returns></returns>
    public Npc_Skill_Duration_Data Get_NpcSkillDurationData(int npc_skill_duration_id)
    {
        Check_Npc_Skill_Duration_Data();
        return _Npc_Skill_Duration_Data.Find(x => x.npc_skill_duration_id == npc_skill_duration_id);
    }

    #endregion

    #region Stage

    /// <summary>
    /// 에디터에서 사용할 스테이지 데이터 가져오기
    /// </summary>
    /// <param name="stage_id"></param>
    /// <returns></returns>
    public Editor_Stage_Data Get_EditorStageData(int stage_id)
    {
        Check_Editor_Stage_Data();
        return _Editor_Stage_Data.Find(x => x.stage_id == stage_id);
    }
    public void Get_EditorWaveDataList(int wave_group_id, ref List<Editor_Wave_Data> list)
    {
        Check_Editor_Wave_Data();
        list.Clear();
        list.AddRange(_Editor_Wave_Data.FindAll(x => x.wave_group_id == wave_group_id));
        //  asc 
        list.Sort((a, b) => a.wave_sequence.CompareTo(b.wave_sequence));
    }

    public World_Data Get_WorldData(int world_id)
    {
        Check_World_Data();
        return _World_Data.Find(x => x.world_id == world_id);
    }

    public void Get_WorldDataList(ref List<World_Data> list)
    {
        Check_World_Data();
        list.Clear();
        list.AddRange(_World_Data);
        list.Sort((a, b) => a.world_id.CompareTo(b.world_id));
    }

    public Zone_Data Get_ZoneData(int zone_id)
    {
        Check_Zone_Data();
        return _Zone_Data.Find(x => x.zone_id == zone_id);
    }

    public void Get_ZoneDataList(int world_id, STAGE_DIFFICULTY_TYPE diff_type, ref List<Zone_Data> list)
    {
        Check_Zone_Data();
        list.Clear();
        list.AddRange(_Zone_Data.FindAll(x => x.in_world_id == world_id && x.zone_difficulty == diff_type));
        list.Sort((a, b) => a.zone_ordering.CompareTo(b.zone_ordering));
    }

    /// <summary>
    /// 지정 스테이지 정보 가져오기
    /// </summary>
    /// <param name="stage_id"></param>
    /// <returns></returns>
    public Stage_Data Get_StageData(int stage_id)
    {
        
        Check_Stage_Data();

        return _Stage_Data.Find(x => x.stage_id == stage_id);
    }
    public void Get_StageDataList(int zone_id, ref List<Stage_Data> list)
    {
        Check_Stage_Data();
        list.Clear();
        list.AddRange(_Stage_Data.FindAll(x => x.zone_id == zone_id));
        list.Sort((a, b) => a.stage_ordering.CompareTo(b.stage_ordering));
    }


    /// <summary>
    /// 모든 스테이지 정보 가져오기
    /// 차후 지역별 스테이지 정보를 가져오는 방식이 될 수도 있다.
    /// </summary>
    /// <param name="list"></param>
    public void Get_StageDataList(ref List<Stage_Data> list)
    {
        Check_Stage_Data();

        list.Clear();
        list.AddRange(_Stage_Data);
    }
    /// <summary>
    /// 각 스테이지별로 지정된 웨이브의 그룹id로 해당 웨이브 데이터 리스트를 찾아 반환
    /// </summary>
    /// <param name="wave_group_id"></param>
    /// <param name="list"></param>
    public void Get_WaveDataList(int wave_group_id, ref List<Wave_Data> list)
    {
        
        Check_Wave_Data();
        list.Clear();
        list.AddRange(_Wave_Data.FindAll(x => x.wave_group_id == wave_group_id));
        //  wave_sequence 오름 차순 정렬
        list.Sort((a, b) => a.wave_sequence.CompareTo(b.wave_sequence));
    }
    #endregion

    #region Memorial
    public Me_Resource_Data Get_MemorialData(int memorial_id)
    {
        return _Me_Resource_Data.Find(x => x.memorial_id == memorial_id);
    }

    public Me_Resource_Data Get_MemorialData(int player_character_id, int order)
    {
        Check_Me_Resource_Data();
        return _Me_Resource_Data.Find(x => x.player_character_id == player_character_id && x.order == order);
    }

    public Me_Resource_Data Get_MemorialResourceData(int memorial_id, int player_character_id)
    {
        Check_Me_Resource_Data();
        return _Me_Resource_Data.Find(x => x.player_character_id == player_character_id && x.memorial_id == memorial_id);
    }

    public void Get_MemorialResourceDataList(ref List<Me_Resource_Data> list)
    {
        Check_Me_Resource_Data();
        list.Clear();
        list.AddRange(_Me_Resource_Data);
    }

    public void Get_MemorialResourceDataListByPlayerID(int player_character_id, ref List<Me_Resource_Data> list)
    {
        Check_Me_Resource_Data();
        list.Clear();
        list.AddRange(_Me_Resource_Data.FindAll(x => x.player_character_id == player_character_id));
    }


    public List<Me_Interaction_Data>[,] Get_MemorialInteraction(int player_character_id)
    {
        List<Me_Interaction_Data>[,] result = new List<Me_Interaction_Data>[Enum.GetValues(typeof(TOUCH_GESTURE_TYPE)).Length, Enum.GetValues(typeof(TOUCH_BODY_TYPE)).Length];
        foreach (var data in _Me_Interaction_Data)
        {
            if (data.player_character_id != player_character_id)
                continue;

            int body = (int)data.touch_body_type;
            int gesture = (int)data.touch_gesture_type;

            if (result[gesture, body] == null)
            {
                result[gesture, body] = new List<Me_Interaction_Data>();
            }
            result[gesture, body].Add(data);
        }

        return result;
    }

    public Dictionary<int, Me_Chat_Motion_Data> Get_MemorialChatMotion(int player_character_id)
    {
        return _Me_Chat_Motion_Data
            .Where(x => x.player_character_id == player_character_id)
            .ToDictionary(x => x.chat_motion_id, x => x);
    }

    public Dictionary<int, Me_Serifu_Data> Get_MemorialSerifu(int player_character_id)
    {
        return _Me_Serifu_Data
            .Where(x => x.player_character_id == player_character_id)
            .ToDictionary(x => x.serifu_id, x => x);
    }

    public Dictionary<int, (string Idle_Animation_Name, int[] Bored_Chatmotion_Ids, int Bored_Condition_Count)> Get_MemorialStateAnimation(int player_character_id)
    {
        return _Me_State_Data
            .Where(x => x.player_character_id == player_character_id)
            .ToDictionary(x => x.state_id, x => (x.idle_animation_name, x.bored_chatmotion_ids, x.bored_condition_count));
    }
    #endregion
}
