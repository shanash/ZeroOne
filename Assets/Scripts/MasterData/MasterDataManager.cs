using System;
using System.Collections.Generic;
using System.Linq;
#nullable disable

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

    public Player_Character_Skill_Level_Data Get_PlayerCharacterSkillLevelData(int lv)
    {
        Check_Player_Character_Skill_Level_Data();
        return _Player_Character_Skill_Level_Data.Find(x => x.level == lv);
    }
    public Player_Character_Skill_Level_Data Get_PlayerCharacterSkillLevelDataByAccumExp(double accum_exp)
    {
        Check_Player_Character_Skill_Level_Data();
        var list = _Player_Character_Skill_Level_Data.OrderBy(x => x.level).ToList();
        return list.FindLast(x => x.accum_exp <= accum_exp);
    }

    public Player_Level_Data Get_PlayerLevelData(int lv)
    {
        Check_Player_Level_Data();
        return _Player_Level_Data.Find(x => x.level == lv);
    }
    public Player_Level_Data Get_PlayerLevelDataByAccumExp(double accum_exp)
    {
        Check_Player_Level_Data();
        var list = _Player_Level_Data.OrderBy(x => x.level).ToList();
        return list.FindLast(x => x.accum_exp <= accum_exp);
    }

    public int Get_PlayerLevelMaxLevel()
    {
        Check_Player_Level_Data();
        int max_lv = _Player_Level_Data.Max(x => x.level);
        return max_lv;
    }

    public Player_Character_Level_Data Get_PlayerCharacterLevelData(int lv)
    {
        Check_Player_Character_Level_Data();
        return _Player_Character_Level_Data.Find(x => x.level == lv);
    }

    public Player_Character_Level_Data Get_PlayerCharacterLevelDataByAccumExp(double accum_exp)
    {
        Check_Player_Character_Level_Data();
        var list = _Player_Character_Level_Data.OrderBy(x => x.level).ToList();
        return list.FindLast(x => x.accum_exp <= accum_exp);
    }

    public Player_Character_Love_Level_Data Get_PlayerCharacterLoveLevelData(int lv)
    {
        Check_Player_Character_Love_Level_Data();
        return _Player_Character_Love_Level_Data.Find(x => x.level == lv);
    }
    public Player_Character_Love_Level_Data Get_PlayerCharacterLoveLevelDataByAccumExp(double accum_exp)
    {
        Check_Player_Character_Love_Level_Data();
        var list = _Player_Character_Love_Level_Data.OrderBy(x => x.level).ToList();
        return list.FindLast(x => x.accum_exp <= accum_exp);
    }

    #endregion

    #region New Item Data

    public Goods_Data Get_GoodsData(GOODS_TYPE gtype)
    {
        Check_Goods_Data();
        return _Goods_Data.Find(x => x.goods_type == gtype);
    }
    public IReadOnlyList<Goods_Data> Get_GoodsDataList()
    {
        Check_Goods_Data();
        return _Goods_Data;
    }

    public Item_Data Get_ItemData(ITEM_TYPE_V2 itype, int item_id)
    {
        Check_Item_Data();
        return _Item_Data.Find(x => x.item_type == itype && x.item_id == item_id);
    }
    
    public IReadOnlyList<Item_Data> Get_ItemDataListByItemType(ITEM_TYPE_V2 itype)
    {
        Check_Item_Data();
        return _Item_Data.FindAll(x => x.item_type == itype);
    }

    public IReadOnlyList<Item_Data> Get_ItemDataList()
    {
        Check_Item_Data();
        return _Item_Data;
    }

    public Item_Piece_Data Get_ItemPieceData(int item_piece_id)
    {
        Check_Item_Piece_Data();
        return _Item_Piece_Data.Find(x => x.item_piece_id == item_piece_id);
    }
    public Equipment_Data Get_EquipmentData(EQUIPMENT_TYPE etype, int equipment_id)
    {
        Check_Equipment_Data();
        return _Equipment_Data.Find(x => x.equipment_type == etype && x.item_id == equipment_id);
    }

    public Equipment_Data Get_EquipmentData(int equipment_id)
    {
        Check_Equipment_Data();
        return _Equipment_Data.Find(x => x.item_id == equipment_id);
    }


    public IReadOnlyList<Reward_Set_Data> Get_RewardSetDataList(int reward_id)
    {
        Check_Reward_Set_Data();
        return _Reward_Set_Data.FindAll(x => x.reward_id == reward_id);
    }

    public Charge_Value_Data Get_ChargeValueData(REWARD_TYPE rtype)
    {
        Check_Charge_Value_Data();
        return _Charge_Value_Data.Find(x => x.reward_type == rtype);
    }
    public IReadOnlyList<Charge_Value_Data> Get_ChargeValueDataList()
    {
        Check_Charge_Value_Data();
        return _Charge_Value_Data.FindAll(x => x.Use_Charge_Data);
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
    ///// <summary>
    ///// 플레이어 캐릭터의 전투 정보
    ///// </summary>
    ///// <param name="battle_info_id"></param>
    ///// <returns></returns>
    //public Player_Character_Battle_Data Get_PlayerCharacterBattleData(int battle_info_id)
    //{
    //    Check_Player_Character_Battle_Data();
    //    return _Player_Character_Battle_Data.Find(x => x.battle_info_id == battle_info_id);
    //}

    /// <summary>
    /// 플레이어 캐릭터 전투 정보
    /// </summary>
    /// <param name="battle_info_id"></param>
    /// <param name="star_grade"></param>
    /// <returns></returns>
    public Player_Character_Battle_Data Get_PlayerCharacterBattleData(int battle_info_id, int star_grade)
    {
        Check_Player_Character_Battle_Data();
        return _Player_Character_Battle_Data.Find(x => x.battle_info_id == battle_info_id && x.star_grade == star_grade);
    }

    /// <summary>
    /// 성급 강화 데이터 반환
    /// </summary>
    /// <param name="cur_star_grade"></param>
    /// <returns></returns>
    public Star_Upgrade_Data Get_StarUpgradeData(int cur_star_grade)
    {
        Check_Star_Upgrade_Data();
        return _Star_Upgrade_Data.Find(x => x.current_star_grade == cur_star_grade);
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
    /// <summary>
    /// 직군(롤)에 따라 아이콘 표시를 위한 데이터
    /// </summary>
    /// <param name="rtype"></param>
    /// <returns></returns>
    public Role_Icon_Data Get_RoleIconData(ROLE_TYPE rtype)
    {
        Check_Role_Icon_Data();
        return _Role_Icon_Data.Find(x => x.role_type == rtype);
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
    /// <param name="stage_id"></param>
    /// <param name="list"></param>
    public void Get_WaveDataList(int stage_id, ref List<Wave_Data> list)
    {

        Check_Wave_Data();
        list.Clear();
        list.AddRange(_Wave_Data.FindAll(x => x.stage_id == stage_id));
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

    public Me_Chat_Motion_Data Get_MemorialChatMotion_(int chat_motion_id)
    {
        return _Me_Chat_Motion_Data
            .Find(x => x.chat_motion_id == chat_motion_id);
    }

    public Dictionary<int, Me_Chat_Motion_Data> Get_AniData(List<int> animation_ids)
    {
        return animation_ids
            .Select(chat_motion_id => _Me_Chat_Motion_Data.Find(data => data.chat_motion_id == chat_motion_id))
            .Where(x => x != null)
            .Distinct()
            .ToDictionary(data => data.chat_motion_id, data => data);
    }

    public Dictionary<int, Me_Serifu_Data> Get_SerifuData(List<int> serifu_ids)
    {
        return serifu_ids
            .Select(serifu_id => _Me_Serifu_Data.Find(serifu_data => serifu_data.serifu_id == serifu_id))
            .Distinct()
            .ToDictionary(serifu_data => serifu_data.serifu_id, serifu_data => serifu_data);
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

    #region L2D Character
    public L2d_Char_Skin_Data Get_L2DCharSkinData(int char_skin_id)
    {
        return _L2d_Char_Skin_Data.Find(x => x.l2d_id == char_skin_id);
    }

    public Dictionary<int, (string Idle_Animation_Name, int[] Bored_Chatmotion_Ids, int Bored_Condition_Count)> Get_L2DCharStateAnimations(int char_skin_id)
    {
        return _L2d_Love_State_Data
            .Where(love_state => love_state.l2d_id == char_skin_id)
            .Select(love_state => new
            {
                state_id = love_state.state_id,
                ani_state = _L2d_Skin_Ani_State_Data.FirstOrDefault(x => x.state_id == love_state.state_id)
            })
            .Where(x => x.ani_state != null)
            .Select(x => new
            {
                x.state_id,
                chat_motion_data = Get_MemorialChatMotion_(x.ani_state.base_ani_id)
            })
            .ToDictionary(
                x => x.state_id,
                x => (x.chat_motion_data.animation_name, new int[0], 0)
            );
    }

    public Dictionary<LOVE_LEVEL_TYPE, L2d_Love_State_Data> Get_L2D_LoveStates(int char_skin_id)
    {
        return _L2d_Love_State_Data
            .FindAll(x => x.l2d_id == char_skin_id)
            .ToDictionary(x => x.love_level_type, x => x);
    }

    public Dictionary<int, L2d_Skin_Ani_State_Data> Get_L2D_SkinAniStates(List<int> state_ids)
    {
        return state_ids
            .Select(state_id => _L2d_Skin_Ani_State_Data.Find(x => x.state_id == state_id))
            .Where(skin_ani_state_data => skin_ani_state_data != null)
            .Distinct()
            .ToDictionary(skin_ani_state_data => skin_ani_state_data.state_id, skin_ani_state_data => skin_ani_state_data);
    }

    public Dictionary<int, List<L2d_Interaction_Base_Data>> Get_L2D_InteractionBases(List<int> interaction_group_ids)
    {
        return interaction_group_ids
            .ToDictionary(
                id => id,
                id => _L2d_Interaction_Base_Data.FindAll(x => x.interaction_group_id == id)
            );
    }
    #endregion

    #region Essence Transfer
    public Essence_Status_Data Get_EssenceStatusData(int essence_transfer_percent)
    {
        return _Essence_Status_Data.Find(x => x.essence_charge_per == essence_transfer_percent);
    }

    public List<Essence_Status_Data> Get_EssenceStatusDataRange(int esssence_transfer_percent_start, int essence_transfer_percent_target)
    {
        List<Essence_Status_Data> list = new List<Essence_Status_Data>();
        for (int i = esssence_transfer_percent_start; i <= essence_transfer_percent_target; i++)
        {
            var data = _Essence_Status_Data.Find(x => x.essence_charge_per == i);
            list.Add(data);
        }

        return list;
    }
    #endregion

    #region Language String Data
    public System_Lang_Data Get_SystemLangData(string str_id)
    {
        Check_System_Lang_Data();
        return _System_Lang_Data.Find(x => x.string_id.Equals(str_id));
    }
    public Character_Lang_Data Get_CharacterLangData(string str_id)
    {
        Check_Character_Lang_Data();
        return _Character_Lang_Data.Find(x => x.string_id.Equals(str_id));
    }
    public Skill_Lang_Data Get_SkillLangData(string str_id)
    {
        Check_Skill_Lang_Data();
        return _Skill_Lang_Data.Find(x => x.string_id.Equals(str_id));
    }
    public Item_Lang_Data Get_ItemLangData(string str_id)
    {
        Check_Item_Lang_Data();
        return _Item_Lang_Data.Find(x => x.string_id.Equals(str_id));
    }
    public Dialog_Lang_Data Get_DialogLangData(string str_id)
    {
        Check_Dialog_Lang_Data();
        return _Dialog_Lang_Data.Find(x => x.string_id.Equals(str_id));
    }
    public Story_Lang_Data Get_StoryLangData(string str_id)
    {
        Check_Story_Lang_Data();
        return _Story_Lang_Data.Find(x => x.string_id.Equals(str_id));
    }
    #endregion
}
