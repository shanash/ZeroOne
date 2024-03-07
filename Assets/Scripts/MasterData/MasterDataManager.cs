using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                __instance.OnInitialize();
            }
            return __instance;
        }
    }
    private MasterDataManager()
        : base()
    {
    }

    protected void OnInitialize()
    {
        InitLoadCustomData();
    }
    public bool IsLoaded { get { return is_init_load; } }

    #region Level
    public Player_Character_Skill_Level_Data Get_PlayerCharacterSkillLevelData(int lv)
    {
        Check_Player_Character_Skill_Level_Data();
        return _Player_Character_Skill_Level_Data[lv];
    }

    public Player_Character_Skill_Level_Data Get_PlayerCharacterSkillLevelDataByAccumExp(double accum_exp)
    {
        Check_Player_Character_Skill_Level_Data();
        var list = _Player_Character_Skill_Level_Data.Values.OrderBy(x => x.level).ToList();
        return list.FindLast(x => x.accum_exp <= accum_exp);
    }

    /// <summary>
    /// 최대레벨을 넘어가지 않는 스킬레벨데이터를 가져옵니다
    /// </summary>
    /// <param name="accum_exp">총 경험치</param>
    /// <param name="max_level">최대레벨</param>
    /// <returns></returns>
    public Player_Character_Skill_Level_Data Get_PlayerCharacterSkillLevelDataByExpAdjustingMaxLevel(ref double accum_exp, int max_level)
    {
        double _accum_exp = accum_exp;
        Check_Player_Character_Skill_Level_Data();
        var list = _Player_Character_Skill_Level_Data.Values.OrderBy(x => x.level).ToList();
        var item = list.FindLast(x => (x.accum_exp <= _accum_exp && x.level <= max_level));
        if (item.level == max_level)
        {
            accum_exp = item.accum_exp;
        }

        return item;
    }

    public Player_Level_Data Get_PlayerLevelData(int lv)
    {
        Check_Player_Level_Data();
        return _Player_Level_Data[lv];
    }

    public Player_Level_Data Get_PlayerLevelDataByAccumExp(double accum_exp)
    {
        Check_Player_Level_Data();
        var list = _Player_Level_Data.Values.OrderBy(x => x.level).ToList();
        return list.FindLast(x => x.accum_exp <= accum_exp);
    }

    public int Get_PlayerLevelMaxLevel()
    {
        Check_Player_Level_Data();
        int max_lv = _Player_Level_Data.Values.Max(x => x.level);
        return max_lv;
    }

    public Player_Character_Level_Data Get_PlayerCharacterLevelData(int lv)
    {
        Check_Player_Character_Level_Data();
        return _Player_Character_Level_Data[lv];
    }

    public Player_Character_Level_Data Get_PlayerCharacterLevelDataByAccumExp(double accum_exp)
    {
        Check_Player_Character_Level_Data();
        var list = _Player_Character_Level_Data.Values.OrderBy(x => x.level).ToList();
        return list.FindLast(x => x.accum_exp <= accum_exp);
    }

    public Player_Character_Love_Level_Data Get_PlayerCharacterLoveLevelData(int lv)
    {
        Check_Player_Character_Love_Level_Data();
        return _Player_Character_Love_Level_Data[lv];
    }

    public Player_Character_Love_Level_Data Get_PlayerCharacterLoveLevelDataByAccumExp(double accum_exp)
    {
        Check_Player_Character_Love_Level_Data();
        var list = _Player_Character_Love_Level_Data.Values.OrderBy(x => x.level).ToList();
        return list.FindLast(x => x.accum_exp <= accum_exp);
    }
    #endregion

    #region New Item Data

    public Goods_Data Get_GoodsData(GOODS_TYPE gtype)
    {
        Check_Goods_Data();
        return _Goods_Data[gtype];
    }
    public IReadOnlyList<Goods_Data> Get_GoodsDataList()
    {
        Check_Goods_Data();
        return _Goods_Data.Values.ToList();
    }

    public Item_Data Get_ItemData(ITEM_TYPE_V2 itype, int item_id)
    {
        Check_Item_Data();
        return _Item_Data.Values.ToList().Find(x => x.item_type == itype && x.item_id == item_id);
    }
    
    public Item_Data Get_ItemData(int item_id)
    {
        Check_Item_Data();
        Item_Data item;
        _Item_Data.TryGetValue(item_id, out item);
        return item; 
    }

    public IReadOnlyList<Item_Data> Get_ItemDataListByItemType(ITEM_TYPE_V2 itype)
    {
        Check_Item_Data();
        var list = _Item_Data.Values.ToList().FindAll(x => x.item_type == itype);
        list.Sort((a, b) => a.item_id.CompareTo(b.item_id));
        return list;
    }

    public IReadOnlyList<Item_Data> Get_ItemDataList()
    {
        Check_Item_Data();
        return _Item_Data.Values.ToList();
    }

    public Item_Piece_Data Get_ItemPieceData(int item_piece_id)
    {
        Check_Item_Piece_Data();
        return _Item_Piece_Data[item_piece_id];
    }
    public Equipment_Data Get_EquipmentData(EQUIPMENT_TYPE etype, int equipment_id)
    {
        Check_Equipment_Data();
        return _Equipment_Data.Values.ToList().Find(x => x.equipment_type == etype && x.item_id == equipment_id);
    }

    public Equipment_Data Get_EquipmentData(int equipment_id)
    {
        Check_Equipment_Data();
        return _Equipment_Data[equipment_id];
    }


    public IReadOnlyList<Reward_Set_Data> Get_RewardSetDataList(int reward_group_id)
    {
        Check_Reward_Set_Data();
        return _Reward_Set_Data.Values.ToList().FindAll(x => x.reward_group_id == reward_group_id);
    }

    public Charge_Value_Data Get_ChargeValueData(REWARD_TYPE rtype)
    {
        Check_Charge_Value_Data();
        return _Charge_Value_Data[rtype];
    }
    public IReadOnlyList<Charge_Value_Data> Get_ChargeValueDataList()
    {
        Check_Charge_Value_Data();
        return _Charge_Value_Data.Values.ToList();
    }

    public Max_Bound_Info_Data Get_MaxBoundInfoData(REWARD_TYPE rtype)
    {
        Check_Max_Bound_Info_Data();
        return _Max_Bound_Info_Data[rtype];
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

        return _Player_Character_Data[player_character_id];
    }

    public IReadOnlyList<Player_Character_Data> Get_PlayerCharacterDataList()
    {
        Check_Player_Character_Data();
        return _Player_Character_Data.Values.ToList();
    }

    /// <summary>
    /// 최초 지급 캐릭터 반환
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<Player_Character_Data> Get_PlayerCharacterDataListByFirstOpen()
    {
        Check_Player_Character_Data();
        var list = _Player_Character_Data.Values.ToList().FindAll(x => x.first_open_check);
        list.Sort((a, b) => a.player_character_id.CompareTo(b.player_character_id));
        return list;
    }
    
    /// <summary>
    /// 플레이어 캐릭터 전투 정보
    /// </summary>
    /// <param name="battle_info_id"></param>
    /// <param name="star_grade"></param>
    /// <returns></returns>
    public Player_Character_Battle_Data Get_PlayerCharacterBattleData(int battle_info_id, int star_grade)
    {
        Check_Player_Character_Battle_Data();
        return _Player_Character_Battle_Data.Values.ToList().Find(x => x.battle_info_id == battle_info_id && x.star_grade == star_grade);
    }

    /// <summary>
    /// 플레이어 캐릭터 레벨당 스탯 정보<br/>
    /// 각 스탯을 (레벨 - 1)을 곱해서 기본 스탯에 추가한다.
    /// </summary>
    /// <param name="player_character_id"></param>
    /// <param name="star_grade"></param>
    /// <returns></returns>
    public Player_Character_Level_Stat_Data Get_PlayerCharacterLevelStatData(int player_character_id, int star_grade)
    {
        Check_Player_Character_Level_Stat_Data();
        return _Player_Character_Level_Stat_Data.Values.ToList().Find(x => x.player_character_id == player_character_id && x.star_grade == star_grade);
    }

    /// <summary>
    /// 성급 강화 데이터 반환
    /// </summary>
    /// <param name="cur_star_grade"></param>
    /// <returns></returns>
    public Star_Upgrade_Data Get_StarUpgradeData(int cur_star_grade)
    {
        Check_Star_Upgrade_Data();
        return _Star_Upgrade_Data[cur_star_grade];
    }

    /// <summary>
    /// 플레이어 캐릭터 스킬 그룹 데이터 반환
    /// </summary>
    /// <param name="skill_group_id"></param>
    /// <returns></returns>
    public Player_Character_Skill_Group Get_PlayerCharacterSkillGroupData(int skill_group_id)
    {
        Check_Player_Character_Skill_Group();

        return _Player_Character_Skill_Group[skill_group_id];
    }
    /// <summary>
    /// 플레이어 캐릭터 스킬 데이터 반환
    /// </summary>
    /// <param name="pc_skill_id"></param>
    /// <returns></returns>
    public Player_Character_Skill_Data Get_PlayerCharacterSkillData(int pc_skill_id)
    {
        Check_Player_Character_Skill_Data();

        return _Player_Character_Skill_Data[pc_skill_id];
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
        list.AddRange(_Player_Character_Skill_Data.Values.ToList().FindAll(x => x.pc_skill_group_id == skill_group_id));
    }

    /// <summary>
    /// 플레이어 캐릭터의 스킬 그룹에 포함되어 있는 스킬 데이터 리스트 반환
    /// </summary>
    /// <param name="skill_group_id"></param>
    /// <param name="list"></param>
    public List<Player_Character_Skill_Data> Get_PlayerCharacterSkillDataListBySkillGroup(int skill_group_id)
    {
        Check_Player_Character_Skill_Data();
        return _Player_Character_Skill_Data.Values.ToList().FindAll(x => x.pc_skill_group_id == skill_group_id);
    }

    /// <summary>
    /// 플레이어 캐릭터의 일회성 효과 스킬 데이터 반환
    /// </summary>
    /// <param name="pc_skill_onetime_id"></param>
    /// <returns></returns>
    public Player_Character_Skill_Onetime_Data Get_PlayerCharacterSkillOnetimeData(int pc_skill_onetime_id)
    {
        Check_Player_Character_Skill_Onetime_Data();
        return _Player_Character_Skill_Onetime_Data[pc_skill_onetime_id];
    }
    /// <summary>
    /// 플레이어 캐릭터의 지속성 효과 스킬 데이터 반환
    /// </summary>
    /// <param name="pc_skill_duration_id"></param>
    /// <returns></returns>
    public Player_Character_Skill_Duration_Data Get_PlayerCharacterSkillDurationData(int pc_skill_duration_id)
    {
        Check_Player_Character_Skill_Duration_Data();
        return _Player_Character_Skill_Duration_Data[pc_skill_duration_id];
    }

    /// <summary>
    /// 포지션에 따라 아이콘을 표시 해주기 위한 데이터
    /// </summary>
    /// <param name="pos_type"></param>
    /// <returns></returns>
    public Position_Icon_Data Get_PositionIconData(POSITION_TYPE pos_type)
    {
        Check_Position_Icon_Data();

        return _Position_Icon_Data[pos_type];
    }
    /// <summary>
    /// 직군(롤)에 따라 아이콘 표시를 위한 데이터
    /// </summary>
    /// <param name="rtype"></param>
    /// <returns></returns>
    public Role_Icon_Data Get_RoleIconData(ROLE_TYPE rtype)
    {
        Check_Role_Icon_Data();
        return _Role_Icon_Data[rtype];
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

        return _Npc_Data[npc_data_id];
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
        list.AddRange(_Npc_Data.Values.ToList());
    }
    /// <summary>
    /// npc 전투 데이터 가져오기
    /// </summary>
    /// <param name="npc_battle_info_id"></param>
    /// <returns></returns>
    public Npc_Battle_Data Get_NpcBattleData(int npc_battle_info_id)
    {
        Check_Npc_Battle_Data();

        return _Npc_Battle_Data[npc_battle_info_id];
    }
    /// <summary>
    /// NPC의 레벨에 따른 스탯 증가량 정보 데이터
    /// </summary>
    /// <param name="npc_level_stat_id"></param>
    /// <returns></returns>
    public Npc_Level_Stat_Data Get_NpcLevelStatData(int npc_level_stat_id)
    {
        Check_Npc_Level_Stat_Data();
        return _Npc_Level_Stat_Data[npc_level_stat_id];
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

        return _Npc_Skill_Group[npc_skill_group_id];
    }
    /// <summary>
    /// npc의 스킬 데이터 가져오기
    /// </summary>
    /// <param name="npc_skill_id"></param>
    /// <returns></returns>
    public Npc_Skill_Data Get_NpcSkillData(int npc_skill_id)
    {
        Check_Npc_Skill_Data();
        return _Npc_Skill_Data[npc_skill_id];
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
        list.AddRange(_Npc_Skill_Data.Values.ToList().FindAll(x => x.npc_skill_group_id == npc_skill_group_id));
    }
    /// <summary>
    /// npc의 일회성 효과 스킬 데이터 반환
    /// </summary>
    /// <param name="npc_skill_onetime_id"></param>
    /// <returns></returns>
    public Npc_Skill_Onetime_Data Get_NpcSkillOnetimeData(int npc_skill_onetime_id)
    {
        Check_Npc_Skill_Onetime_Data();
        return _Npc_Skill_Onetime_Data[npc_skill_onetime_id];
    }
    /// <summary>
    /// npc의 지속성 효과 스킬 데이터 반환
    /// </summary>
    /// <param name="npc_skill_duration_id"></param>
    /// <returns></returns>
    public Npc_Skill_Duration_Data Get_NpcSkillDurationData(int npc_skill_duration_id)
    {
        Check_Npc_Skill_Duration_Data();
        return _Npc_Skill_Duration_Data[npc_skill_duration_id];
    }

    #endregion

    #region Story Dungeon

    /// <summary>
    /// 에디터에서 사용할 스테이지 데이터 가져오기
    /// </summary>
    /// <param name="stage_id"></param>
    /// <returns></returns>
    public Editor_Stage_Data Get_EditorStageData(int stage_id)
    {
        Check_Editor_Stage_Data();
        return _Editor_Stage_Data[stage_id];
    }
    public void Get_EditorWaveDataList(int wave_group_id, ref List<Editor_Wave_Data> list)
    {
        Check_Editor_Wave_Data();
        list.Clear();
        list.AddRange(_Editor_Wave_Data.Values.ToList().FindAll(x => x.wave_group_id == wave_group_id));
        //  asc 
        list.Sort((a, b) => a.wave_sequence.CompareTo(b.wave_sequence));
    }

    public World_Data Get_WorldData(int world_id)
    {
        Check_World_Data();
        return _World_Data[world_id];
    }
    public World_Data Get_WorldDataByZoneGroupID(int zone_group_id)
    {
        Check_Wave_Data();
        return _World_Data.Values.ToList().Find(x => x.zone_group_id == zone_group_id);
    }

    public IReadOnlyList<World_Data> Get_WorldDataList()
    {
        Check_World_Data();
        return _World_Data.Values.ToList();
    }

    public Zone_Data Get_ZoneData(int zone_id)
    {
        Check_Zone_Data();
        return _Zone_Data[zone_id];
    }
    /// <summary>
    /// 지정 난이도의 존 리스트 반환
    /// </summary>
    /// <param name="dtype"></param>
    /// <returns></returns>
    public List<Zone_Data> Get_ZoneDataListByDifficulty(STAGE_DIFFICULTY_TYPE dtype)
    {
        Check_Zone_Data();
        var list = _Zone_Data.Values.ToList().FindAll(x => x.zone_difficulty == dtype);
        list.Sort((a, b) => a.zone_id.CompareTo(b.zone_id));
        return list;
    }
    

    public Zone_Data Get_ZoneDataByStageGroupID(int stage_group_id)
    {
        Check_Zone_Data();
        return _Zone_Data.Values.ToList().Find(x => x.stage_group_id == stage_group_id);
    }

    public IReadOnlyList<Zone_Data> Get_ZoneDataList(int zone_group_id, STAGE_DIFFICULTY_TYPE diff_type)
    {
        Check_Zone_Data();
        var list = _Zone_Data.Values.ToList().FindAll(x => x.zone_group_id == zone_group_id);
        list.Sort((a, b) => a.zone_ordering.CompareTo(b.zone_ordering));
        return list;
    }

    /// <summary>
    /// 지정 스테이지 정보 가져오기
    /// </summary>
    /// <param name="stage_id"></param>
    /// <returns></returns>
    public Stage_Data Get_StageData(int stage_id)
    {
        Check_Stage_Data();

        return _Stage_Data[stage_id];
    }

    /// <summary>
    /// 해당 스테이지를 기준으로 다음 스테이지를 찾아준다.
    /// </summary>
    /// <param name="stage_id"></param>
    /// <returns></returns>
    public Stage_Data Get_NextStageData(int stage_id)
    {
        Check_Stage_Data();
        var stage_list = _Stage_Data.Values.OrderBy(x => x.stage_id).ToList();
        stage_list.Sort((a, b) => a.stage_id.CompareTo(b.stage_id));
        return stage_list.Find(x => x.stage_id > stage_id);
    }

    public IReadOnlyList<Stage_Data> Get_StageDataListByStageGroupID(int stage_group_id)
    {
        Check_Stage_Data();
        var list = _Stage_Data.Values.ToList().FindAll(x => x.stage_group_id == stage_group_id);
        list.Sort((a, b) => a.stage_ordering.CompareTo(b.stage_ordering));
        return list;
    }


    /// <summary>
    /// 모든 스테이지 정보 가져오기
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<Stage_Data> Get_StageDataList()
    {
        Check_Stage_Data();
        return _Stage_Data.Values.ToList();
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
        list.AddRange(_Wave_Data.Values.ToList().FindAll(x => x.wave_group_id == wave_group_id));
        //  wave_sequence 오름 차순 정렬
        list.Sort((a, b) => a.wave_sequence.CompareTo(b.wave_sequence));
    }

    /// <summary>
    /// 각 스테이지별 지정된 웨이브 그룹ID로 해당 웨이브 데이터 리스트 찾아 반환
    /// </summary>
    /// <param name="wave_group_id"></param>
    /// <returns></returns>
    public IReadOnlyList<Wave_Data> Get_WaveDataList(int wave_group_id)
    {
        Check_Wave_Data();
        var list = _Wave_Data.Values.ToList().FindAll(x => x.wave_group_id == wave_group_id);
        //  wave_sequence 오름 차순 정렬
        list.Sort((a, b) => a.wave_sequence.CompareTo(b.wave_sequence));
        return list;
    }

    #endregion

    #region Dungeon
    public Dungeon_Data Get_DungeonData(GAME_TYPE gtype)
    {
        Check_Dungeon_Data();
        var dungeon_list = _Dungeon_Data.Values.ToList().FindAll(x => x.game_type == gtype);
        var now = DateTime.Now.ToLocalTime();
        Dungeon_Data dungeon = null;
        for (int i = 0; i < dungeon_list.Count; i++)
        {
            var d = dungeon_list[i];
            if (d.schedule_id == 0)
            {
                continue;
            }
            var schedule = Get_ScheduleData(d.schedule_id);
            if (schedule != null)
            {
                var begin_dt = DateTime.Parse(schedule.date_start);
                var end_dt = DateTime.Parse(schedule.date_end);
                if (begin_dt <= now && now <= end_dt)
                {
                    dungeon = d;
                    break;
                }
            }
        }
        return dungeon;
    }
    #endregion

    #region Etc
    public Schedule_Data Get_ScheduleData(int schedule_id)
    {
        Check_Schedule_Data();
        return _Schedule_Data[schedule_id];
    }
    #endregion

    #region Boss Dungeon
    public IReadOnlyList<Boss_Data> Get_BossDataList(int boss_group_id)
    {
        Check_Boss_Data();
        var list = _Boss_Data.Values.ToList().FindAll(x => x.boss_group_id == boss_group_id);
        list.Sort((a, b) => a.boss_id.CompareTo(b.boss_id));
        return list;
    }

    public Boss_Data Get_BossData(int boss_id)
    {
        Check_Boss_Data();
        return _Boss_Data[boss_id];
    }

    public Boss_Data Get_BossDataByBossStageGroupID(int boss_stage_group_id)
    {
        Check_Boss_Data();
        return _Boss_Data.Values.ToList().Find(x => x.boss_stage_group_id == boss_stage_group_id);
    }

    public Boss_Stage_Data Get_BossStageData(int boss_stage_id)
    {
        Check_Boss_Stage_Data();
        return _Boss_Stage_Data[boss_stage_id];
    }

    public Boss_Stage_Data Get_NextBossStageData(int boss_stage_id)
    {
        Check_Boss_Data();
        var stage = Get_BossStageData(boss_stage_id);
        var stage_list = Get_BossStageDataListByBossStageGroupID(stage.boss_stage_group_id).ToList();
        return stage_list.Find(x => x.boss_stage_id > boss_stage_id);
    }

    public IReadOnlyList<Boss_Stage_Data> Get_BossStageDataListByBossStageGroupID(int boss_stage_group_id)
    {
        Check_Boss_Stage_Data();
        var list = _Boss_Stage_Data.Values.ToList().FindAll(x => x.boss_stage_group_id == boss_stage_group_id);
        list.Sort((a, b) => a.stage_ordering.CompareTo(b.stage_ordering));
        return list;
    }

    #endregion

    #region Memorial
    public Me_Resource_Data Get_MemorialData(int memorial_id)
    {
        return _Me_Resource_Data[memorial_id];
    }

    public Me_Resource_Data Get_MemorialData(int player_character_id, int order)
    {
        Check_Me_Resource_Data();
        return _Me_Resource_Data.Values.ToList().Find(x => x.player_character_id == player_character_id && x.order == order);
    }

    public Me_Resource_Data Get_MemorialResourceData(int memorial_id, int player_character_id)
    {
        Check_Me_Resource_Data();
        return _Me_Resource_Data.Values.ToList().Find(x => x.player_character_id == player_character_id && x.memorial_id == memorial_id);
    }

    public void Get_MemorialResourceDataList(ref List<Me_Resource_Data> list)
    {
        Check_Me_Resource_Data();
        list.Clear();
        list.AddRange(_Me_Resource_Data.Values.ToList());
    }

    public void Get_MemorialResourceDataListByPlayerID(int player_character_id, ref List<Me_Resource_Data> list)
    {
        Check_Me_Resource_Data();
        list.Clear();
        list.AddRange(_Me_Resource_Data.Values.ToList().FindAll(x => x.player_character_id == player_character_id));
    }


    public List<Me_Interaction_Data>[,] Get_MemorialInteraction(int player_character_id)
    {
        List<Me_Interaction_Data>[,] result = new List<Me_Interaction_Data>[Enum.GetValues(typeof(TOUCH_GESTURE_TYPE)).Length, Enum.GetValues(typeof(TOUCH_BODY_TYPE)).Length];
        foreach (var data in _Me_Interaction_Data)
        {
            if (data.Value.player_character_id != player_character_id)
                continue;

            int body = (int)data.Value.player_character_id;
            int gesture = (int)data.Value.player_character_id;

            if (result[gesture, body] == null)
            {
                result[gesture, body] = new List<Me_Interaction_Data>();
            }
            result[gesture, body].Add(data.Value);
        }

        return result;
    }

    public Dictionary<int, Me_Chat_Motion_Data> Get_MemorialChatMotion(int player_character_id)
    {
        return _Me_Chat_Motion_Data
            .Where(x => x.Value.player_character_id == player_character_id)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public Me_Chat_Motion_Data Get_MemorialChatMotion_(int chat_motion_id)
    {
        return _Me_Chat_Motion_Data[chat_motion_id];
    }

    public Dictionary<int, Me_Chat_Motion_Data> Get_AniData(List<int> animation_ids)
    {
        return animation_ids
            .Select(chat_motion_id => _Me_Chat_Motion_Data.Values.ToList().Find(data => data.chat_motion_id == chat_motion_id))
            .Where(x => x != null)
            .Distinct()
            .ToDictionary(data => data.chat_motion_id, data => data);
    }

    public Dictionary<int, Me_Serifu_Data> Get_SerifuData(List<int> serifu_ids)
    {
        return serifu_ids
            .Select(serifu_id => _Me_Serifu_Data.Values.ToList().Find(serifu_data => serifu_data.serifu_id == serifu_id))
            .Distinct()
            .ToDictionary(serifu_data => serifu_data.serifu_id, serifu_data => serifu_data);
    }

    public Dictionary<int, Me_Serifu_Data> Get_MemorialSerifu(int player_character_id)
    {
        return _Me_Serifu_Data
            .Where(x => x.Value.player_character_id == player_character_id)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public Dictionary<int, (string Idle_Animation_Name, int[] Bored_Chatmotion_Ids, int Bored_Condition_Count)> Get_MemorialStateAnimation(int player_character_id)
    {
        return _Me_State_Data.Values.ToList()
            .Where(x => x.player_character_id == player_character_id)
            .ToDictionary(x => x.state_id, x => (x.idle_animation_name, x.bored_chatmotion_ids, x.bored_condition_count));
    }
    #endregion

    #region L2D Character
    public List<L2d_Char_Skin_Data> Get_L2dDataList()
    {
        return _L2d_Char_Skin_Data.Values.ToList();
    }

    public L2d_Char_Skin_Data Get_L2DCharSkinData(int char_skin_id)
    {
        return _L2d_Char_Skin_Data[char_skin_id];
    }

    public Dictionary<int, (string Idle_Animation_Name, int[] Bored_Chatmotion_Ids, int Bored_Condition_Count)> Get_L2DCharStateAnimations(int char_skin_id)
    {
        return _L2d_Love_State_Data.Values.ToList()
            .Where(love_state => love_state.l2d_id == char_skin_id)
            .Select(love_state => new
            {
                state_id = love_state.state_id,
                ani_state = _L2d_Skin_Ani_State_Data.FirstOrDefault(x => x.Value.state_id == love_state.state_id)
            })
            //.Where(x => x.ani_state != null)
            .Select(x => new
            {
                x.state_id,
                chat_motion_data = Get_MemorialChatMotion_(x.ani_state.Value.base_ani_id)
            })
            .ToDictionary(
                x => x.state_id,
                x => (x.chat_motion_data.animation_name, new int[0], 0)
            );
    }

    public Dictionary<LOVE_LEVEL_TYPE, L2d_Love_State_Data> Get_L2D_LoveStates(int char_skin_id)
    {
        return _L2d_Love_State_Data.Values.ToList()
            .FindAll(x => x.l2d_id == char_skin_id)
            .ToDictionary(x => x.love_level_type, x => x);
    }

    public L2d_Love_State_Data Get_L2D_LoveState(int char_skin_id, LOVE_LEVEL_TYPE love_lv)
    {
        return _L2d_Love_State_Data.Values.ToList()
            .Find(x => x.l2d_id == char_skin_id && x.love_level_type == love_lv);
    }

    public L2d_Skin_Ani_State_Data Get_L2D_SkinAniState(int state_id)
    {
        return _L2d_Skin_Ani_State_Data[state_id];
    }

    public Dictionary<int, L2d_Skin_Ani_State_Data> Get_L2D_SkinAniStates(List<int> state_ids)
    {
        return state_ids
            .Select(state_id => _L2d_Skin_Ani_State_Data.Values.ToList().Find(x => x.state_id == state_id))
            .Where(skin_ani_state_data => skin_ani_state_data != null)
            .Distinct()
            .ToDictionary(skin_ani_state_data => skin_ani_state_data.state_id, skin_ani_state_data => skin_ani_state_data);
    }

    public List<L2d_Interaction_Base_Data> Get_L2D_InteractionBases(List<int> interaction_group_ids)
    {
        return interaction_group_ids.SelectMany(id => _L2d_Interaction_Base_Data.Values.ToList().FindAll(x => x.interaction_group_id == id)).ToList();
    }

    public Dictionary<Tuple<TOUCH_BODY_TYPE, TOUCH_GESTURE_TYPE, bool>, L2d_Interaction_Base_Data> Get_L2D_InteractionBases(int interaction_group_id, bool is_success_transfer_essence)
    {
        return _L2d_Interaction_Base_Data.Values
            .Where(item => item.interaction_group_id == interaction_group_id)
            .ToDictionary(item => Tuple.Create(item.touch_type_01, item.gescure_type_01, item.check_using_essense), item => item);
    }
    #endregion

    #region Essence Transfer
    public Essence_Status_Data Get_EssenceStatusData(int essence_transfer_percent)
    {
        return _Essence_Status_Data[essence_transfer_percent];
    }

    public List<Essence_Status_Data> Get_EssenceStatusDataRange(int esssence_transfer_percent_start, int essence_transfer_percent_target)
    {
        List<Essence_Status_Data> list = new List<Essence_Status_Data>();
        for (int i = esssence_transfer_percent_start; i <= essence_transfer_percent_target; i++)
        {
            var data = _Essence_Status_Data[i];
            list.Add(data);
        }

        return list;
    }

    public void Sum_EssenceStatusList(List<Essence_Status_Data> list, out int add_phy_atk, out int add_mag_atk, out int add_phy_def, out int add_mag_def, out int add_hp)
    {
        add_phy_atk = add_mag_atk = add_phy_def = add_mag_def = add_hp = 0;

        foreach (var data in list)
        {
            add_phy_atk += data.add_atk;
            add_mag_atk += data.add_matk;
            add_phy_def += data.add_def;
            add_mag_def += data.add_mdef;
            add_hp += data.add_hp;
        }
    }
    #endregion


    #region Attribute
    /// <summary>
    /// 속성 아이콘 정보 
    /// </summary>
    /// <param name="atype"></param>
    /// <returns></returns>
    public Attribute_Icon_Data Get_AttributeIconData(ATTRIBUTE_TYPE atype)
    {
        Check_Attribute_Icon_Data();
        return _Attribute_Icon_Data[atype];
    }
    /// <summary>
    /// 속성의 상성 정보
    /// </summary>
    /// <param name="atype"></param>
    /// <returns></returns>
    public Attribute_Superiority_Data Get_AttributeSuperiorityData(ATTRIBUTE_TYPE atype)
    {
        Check_Attribute_Superiority_Data();
        return _Attribute_Superiority_Data[atype];
    }

    /// <summary>
    /// 속성의 팀 시너지
    /// </summary>
    /// <param name="atype"></param>
    /// <param name="same_count"></param>
    /// <returns></returns>
    public Attribute_Synergy_Data Get_AttributeSynergyData(ATTRIBUTE_TYPE atype, int same_count)
    {
        Check_Attribute_Synergy_Data();
        return _Attribute_Synergy_Data[new Tuple<ATTRIBUTE_TYPE, int>(atype, same_count)];
    }
    #endregion

    #region Language String Data
    public System_Lang_Data Get_SystemLangData(string str_id)
    {
        Check_System_Lang_Data();
        return _System_Lang_Data[str_id];
    }
    public Character_Lang_Data Get_CharacterLangData(string str_id)
    {
        Check_Character_Lang_Data();
        return _Character_Lang_Data[str_id];
    }
    public Skill_Lang_Data Get_SkillLangData(string str_id)
    {
        Check_Skill_Lang_Data();
        return _Skill_Lang_Data[str_id];
    }
    public Item_Lang_Data Get_ItemLangData(string str_id)
    {
        Check_Item_Lang_Data();
        return _Item_Lang_Data[str_id];
    }
    public Dialog_Lang_Data Get_DialogLangData(string str_id)
    {
        Check_Dialog_Lang_Data();
        return _Dialog_Lang_Data[str_id];
    }
    public Story_Lang_Data Get_StoryLangData(string str_id)
    {
        Check_Story_Lang_Data();
        return _Story_Lang_Data[str_id];
    }
    public Voice_Lang_Data Get_VoiceLangData(string str_id)
    {
        Check_Story_Lang_Data();
        return _Voice_Lang_Data[str_id];
    }
    #endregion

    #region Custom Table
    public void InitLoadCustomData()
    {
        Load_RewardGroupList();
    }
    protected Dictionary<int, List<Reward_Set_Data>> _Reward_Group_List_Data
    {
        get;
        private set;
    } = new Dictionary<int, List<Reward_Set_Data>>();
    public void Load_RewardGroupList()
    {
        foreach (var data in _Reward_Set_Data)
        {
            List<Reward_Set_Data> list = _Reward_Group_List_Data[data.Value.reward_group_id];
            if(list != null)
            {
                list.Add(data.Value);
            }
            else 
            {  
                list = new List<Reward_Set_Data>();
                list.Add(data.Value);
                _Reward_Group_List_Data.Add(data.Value.reward_group_id, list);
            }
        }
    }
    public IReadOnlyList<Reward_Set_Data> Get_RewardGroupList(int reward_group_id)
    {
        return _Reward_Group_List_Data[reward_group_id];
    }

    #endregion
}
