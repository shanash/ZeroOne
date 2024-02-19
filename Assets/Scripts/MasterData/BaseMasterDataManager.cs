using System.Collections.Generic;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif
using System.Threading.Tasks;
#if UNITY_5_3_OR_NEWER
using UnityEngine.AddressableAssets;
#endif
using System;
#nullable disable
using Newtonsoft.Json;
using System.Linq;

public class BaseMasterDataManager
{

	///	<summary>
	///	 <b>key_1 ATTRIBUTE_TYPE : attacker_attribute_type </b><br/>
	///	</summary>
	protected Dictionary<ATTRIBUTE_TYPE, Attribute_Superiority_Data> _Attribute_Superiority_Data
	{
		get;
		private set;
	} = new Dictionary<ATTRIBUTE_TYPE, Attribute_Superiority_Data>();
	///	<summary>
	///	 <b>key_1 ATTRIBUTE_TYPE : attribute_type </b><br/>
	///	 <b>key_2 int : same_attribute_count </b><br/>
	///	</summary>
	protected Dictionary<Tuple<ATTRIBUTE_TYPE, int>, Attribute_Synergy_Data> _Attribute_Synergy_Data
	{
		get;
		private set;
	} = new Dictionary<Tuple<ATTRIBUTE_TYPE, int>, Attribute_Synergy_Data>();
	///	<summary>
	///	 <b>key_1 int : boss_id </b><br/>
	///	</summary>
	protected Dictionary<int, Boss_Data> _Boss_Data
	{
		get;
		private set;
	} = new Dictionary<int, Boss_Data>();
	///	<summary>
	///	 <b>key_1 int : boss_stage_id </b><br/>
	///	</summary>
	protected Dictionary<int, Boss_Stage_Data> _Boss_Stage_Data
	{
		get;
		private set;
	} = new Dictionary<int, Boss_Stage_Data>();
	///	<summary>
	///	 <b>key_1 REWARD_TYPE : reward_type </b><br/>
	///	</summary>
	protected Dictionary<REWARD_TYPE, Charge_Value_Data> _Charge_Value_Data
	{
		get;
		private set;
	} = new Dictionary<REWARD_TYPE, Charge_Value_Data>();
	///	<summary>
	///	 <b>key_1 int : dungeon_id </b><br/>
	///	</summary>
	protected Dictionary<int, Dungeon_Data> _Dungeon_Data
	{
		get;
		private set;
	} = new Dictionary<int, Dungeon_Data>();
	///	<summary>
	///	 <b>key_1 int : stage_id </b><br/>
	///	</summary>
	protected Dictionary<int, Editor_Stage_Data> _Editor_Stage_Data
	{
		get;
		private set;
	} = new Dictionary<int, Editor_Stage_Data>();
	///	<summary>
	///	 <b>key_1 int : wave_group_id </b><br/>
	///	 <b>key_2 int : wave_sequence </b><br/>
	///	</summary>
	protected Dictionary<Tuple<int, int>, Editor_Wave_Data> _Editor_Wave_Data
	{
		get;
		private set;
	} = new Dictionary<Tuple<int, int>, Editor_Wave_Data>();
	///	<summary>
	///	 <b>key_1 int : essence_charge_per </b><br/>
	///	</summary>
	protected Dictionary<int, Essence_Status_Data> _Essence_Status_Data
	{
		get;
		private set;
	} = new Dictionary<int, Essence_Status_Data>();
	///	<summary>
	///	 <b>key_1 GOODS_TYPE : goods_type </b><br/>
	///	</summary>
	protected Dictionary<GOODS_TYPE, Goods_Data> _Goods_Data
	{
		get;
		private set;
	} = new Dictionary<GOODS_TYPE, Goods_Data>();
	///	<summary>
	///	 <b>key_1 int : item_id </b><br/>
	///	</summary>
	protected Dictionary<int, Item_Data> _Item_Data
	{
		get;
		private set;
	} = new Dictionary<int, Item_Data>();
	///	<summary>
	///	 <b>key_1 int : item_piece_id </b><br/>
	///	</summary>
	protected Dictionary<int, Item_Piece_Data> _Item_Piece_Data
	{
		get;
		private set;
	} = new Dictionary<int, Item_Piece_Data>();
	///	<summary>
	///	 <b>key_1 int : item_id </b><br/>
	///	</summary>
	protected Dictionary<int, Equipment_Data> _Equipment_Data
	{
		get;
		private set;
	} = new Dictionary<int, Equipment_Data>();
	///	<summary>
	///	 <b>key_1 int : l2d_id </b><br/>
	///	</summary>
	protected Dictionary<int, L2d_Char_Skin_Data> _L2d_Char_Skin_Data
	{
		get;
		private set;
	} = new Dictionary<int, L2d_Char_Skin_Data>();
	///	<summary>
	///	 <b>key_1 int : id </b><br/>
	///	</summary>
	protected Dictionary<int, L2d_Love_State_Data> _L2d_Love_State_Data
	{
		get;
		private set;
	} = new Dictionary<int, L2d_Love_State_Data>();
	///	<summary>
	///	 <b>key_1 int : state_id </b><br/>
	///	</summary>
	protected Dictionary<int, L2d_Skin_Ani_State_Data> _L2d_Skin_Ani_State_Data
	{
		get;
		private set;
	} = new Dictionary<int, L2d_Skin_Ani_State_Data>();
	///	<summary>
	///	 <b>key_1 int : interaction_group_id </b><br/>
	///	 <b>key_2 TOUCH_BODY_TYPE : touch_type_01 </b><br/>
	///	</summary>
	protected Dictionary<Tuple<int, TOUCH_BODY_TYPE>, L2d_Interaction_Base_Data> _L2d_Interaction_Base_Data
	{
		get;
		private set;
	} = new Dictionary<Tuple<int, TOUCH_BODY_TYPE>, L2d_Interaction_Base_Data>();
	///	<summary>
	///	 <b>key_1 int : level </b><br/>
	///	</summary>
	protected Dictionary<int, Player_Level_Data> _Player_Level_Data
	{
		get;
		private set;
	} = new Dictionary<int, Player_Level_Data>();
	///	<summary>
	///	 <b>key_1 int : level </b><br/>
	///	</summary>
	protected Dictionary<int, Player_Character_Level_Data> _Player_Character_Level_Data
	{
		get;
		private set;
	} = new Dictionary<int, Player_Character_Level_Data>();
	///	<summary>
	///	 <b>key_1 int : level </b><br/>
	///	</summary>
	protected Dictionary<int, Player_Character_Skill_Level_Data> _Player_Character_Skill_Level_Data
	{
		get;
		private set;
	} = new Dictionary<int, Player_Character_Skill_Level_Data>();
	///	<summary>
	///	 <b>key_1 int : level </b><br/>
	///	</summary>
	protected Dictionary<int, Player_Character_Love_Level_Data> _Player_Character_Love_Level_Data
	{
		get;
		private set;
	} = new Dictionary<int, Player_Character_Love_Level_Data>();
	///	<summary>
	///	 <b>key_1 REWARD_TYPE : reward_type </b><br/>
	///	</summary>
	protected Dictionary<REWARD_TYPE, Max_Bound_Info_Data> _Max_Bound_Info_Data
	{
		get;
		private set;
	} = new Dictionary<REWARD_TYPE, Max_Bound_Info_Data>();
	///	<summary>
	///	 <b>key_1 int : memorial_id </b><br/>
	///	</summary>
	protected Dictionary<int, Me_Resource_Data> _Me_Resource_Data
	{
		get;
		private set;
	} = new Dictionary<int, Me_Resource_Data>();
	///	<summary>
	///	 <b>key_1 int : state_id </b><br/>
	///	</summary>
	protected Dictionary<int, Me_State_Data> _Me_State_Data
	{
		get;
		private set;
	} = new Dictionary<int, Me_State_Data>();
	///	<summary>
	///	 <b>key_1 int : interaction_id </b><br/>
	///	</summary>
	protected Dictionary<int, Me_Interaction_Data> _Me_Interaction_Data
	{
		get;
		private set;
	} = new Dictionary<int, Me_Interaction_Data>();
	///	<summary>
	///	 <b>key_1 int : chat_motion_id </b><br/>
	///	</summary>
	protected Dictionary<int, Me_Chat_Motion_Data> _Me_Chat_Motion_Data
	{
		get;
		private set;
	} = new Dictionary<int, Me_Chat_Motion_Data>();
	///	<summary>
	///	 <b>key_1 int : serifu_id </b><br/>
	///	</summary>
	protected Dictionary<int, Me_Serifu_Data> _Me_Serifu_Data
	{
		get;
		private set;
	} = new Dictionary<int, Me_Serifu_Data>();
	///	<summary>
	///	 <b>key_1 int : npc_skill_group_id </b><br/>
	///	</summary>
	protected Dictionary<int, Npc_Skill_Group> _Npc_Skill_Group
	{
		get;
		private set;
	} = new Dictionary<int, Npc_Skill_Group>();
	///	<summary>
	///	 <b>key_1 int : npc_skill_id </b><br/>
	///	</summary>
	protected Dictionary<int, Npc_Skill_Data> _Npc_Skill_Data
	{
		get;
		private set;
	} = new Dictionary<int, Npc_Skill_Data>();
	///	<summary>
	///	 <b>key_1 int : npc_skill_onetime_id </b><br/>
	///	</summary>
	protected Dictionary<int, Npc_Skill_Onetime_Data> _Npc_Skill_Onetime_Data
	{
		get;
		private set;
	} = new Dictionary<int, Npc_Skill_Onetime_Data>();
	///	<summary>
	///	 <b>key_1 int : npc_skill_duration_id </b><br/>
	///	</summary>
	protected Dictionary<int, Npc_Skill_Duration_Data> _Npc_Skill_Duration_Data
	{
		get;
		private set;
	} = new Dictionary<int, Npc_Skill_Duration_Data>();
	///	<summary>
	///	 <b>key_1 int : npc_data_id </b><br/>
	///	</summary>
	protected Dictionary<int, Npc_Data> _Npc_Data
	{
		get;
		private set;
	} = new Dictionary<int, Npc_Data>();
	///	<summary>
	///	 <b>key_1 int : npc_battle_id </b><br/>
	///	</summary>
	protected Dictionary<int, Npc_Battle_Data> _Npc_Battle_Data
	{
		get;
		private set;
	} = new Dictionary<int, Npc_Battle_Data>();
	///	<summary>
	///	 <b>key_1 int : npc_level_stat_id </b><br/>
	///	</summary>
	protected Dictionary<int, Npc_Level_Stat_Data> _Npc_Level_Stat_Data
	{
		get;
		private set;
	} = new Dictionary<int, Npc_Level_Stat_Data>();
	///	<summary>
	///	 <b>key_1 int : pc_skill_group_id </b><br/>
	///	</summary>
	protected Dictionary<int, Player_Character_Skill_Group> _Player_Character_Skill_Group
	{
		get;
		private set;
	} = new Dictionary<int, Player_Character_Skill_Group>();
	///	<summary>
	///	 <b>key_1 int : pc_skill_id </b><br/>
	///	</summary>
	protected Dictionary<int, Player_Character_Skill_Data> _Player_Character_Skill_Data
	{
		get;
		private set;
	} = new Dictionary<int, Player_Character_Skill_Data>();
	///	<summary>
	///	 <b>key_1 int : pc_skill_onetime_id </b><br/>
	///	</summary>
	protected Dictionary<int, Player_Character_Skill_Onetime_Data> _Player_Character_Skill_Onetime_Data
	{
		get;
		private set;
	} = new Dictionary<int, Player_Character_Skill_Onetime_Data>();
	///	<summary>
	///	 <b>key_1 int : pc_skill_duration_id </b><br/>
	///	</summary>
	protected Dictionary<int, Player_Character_Skill_Duration_Data> _Player_Character_Skill_Duration_Data
	{
		get;
		private set;
	} = new Dictionary<int, Player_Character_Skill_Duration_Data>();
	///	<summary>
	///	 <b>key_1 int : player_character_id </b><br/>
	///	</summary>
	protected Dictionary<int, Player_Character_Data> _Player_Character_Data
	{
		get;
		private set;
	} = new Dictionary<int, Player_Character_Data>();
	///	<summary>
	///	 <b>key_1 int : battle_info_id </b><br/>
	///	 <b>key_2 int : star_grade </b><br/>
	///	</summary>
	protected Dictionary<Tuple<int, int>, Player_Character_Battle_Data> _Player_Character_Battle_Data
	{
		get;
		private set;
	} = new Dictionary<Tuple<int, int>, Player_Character_Battle_Data>();
	///	<summary>
	///	 <b>key_1 POSITION_TYPE : position_type </b><br/>
	///	</summary>
	protected Dictionary<POSITION_TYPE, Position_Icon_Data> _Position_Icon_Data
	{
		get;
		private set;
	} = new Dictionary<POSITION_TYPE, Position_Icon_Data>();
	///	<summary>
	///	 <b>key_1 ROLE_TYPE : role_type </b><br/>
	///	</summary>
	protected Dictionary<ROLE_TYPE, Role_Icon_Data> _Role_Icon_Data
	{
		get;
		private set;
	} = new Dictionary<ROLE_TYPE, Role_Icon_Data>();
	///	<summary>
	///	 <b>key_1 ATTRIBUTE_TYPE : attribute_type </b><br/>
	///	</summary>
	protected Dictionary<ATTRIBUTE_TYPE, Attribute_Icon_Data> _Attribute_Icon_Data
	{
		get;
		private set;
	} = new Dictionary<ATTRIBUTE_TYPE, Attribute_Icon_Data>();
	///	<summary>
	///	 <b>key_1 int : player_character_id </b><br/>
	///	 <b>key_2 int : star_grade </b><br/>
	///	</summary>
	protected Dictionary<Tuple<int, int>, Player_Character_Level_Stat_Data> _Player_Character_Level_Stat_Data
	{
		get;
		private set;
	} = new Dictionary<Tuple<int, int>, Player_Character_Level_Stat_Data>();
	///	<summary>
	///	 <b>key_1 int : reward_id </b><br/>
	///	</summary>
	protected Dictionary<int, Reward_Set_Data> _Reward_Set_Data
	{
		get;
		private set;
	} = new Dictionary<int, Reward_Set_Data>();
	///	<summary>
	///	 <b>key_1 int : schedule_id </b><br/>
	///	</summary>
	protected Dictionary<int, Schedule_Data> _Schedule_Data
	{
		get;
		private set;
	} = new Dictionary<int, Schedule_Data>();
	///	<summary>
	///	 <b>key_1 int : world_id </b><br/>
	///	</summary>
	protected Dictionary<int, World_Data> _World_Data
	{
		get;
		private set;
	} = new Dictionary<int, World_Data>();
	///	<summary>
	///	 <b>key_1 int : zone_id </b><br/>
	///	</summary>
	protected Dictionary<int, Zone_Data> _Zone_Data
	{
		get;
		private set;
	} = new Dictionary<int, Zone_Data>();
	///	<summary>
	///	 <b>key_1 int : stage_id </b><br/>
	///	</summary>
	protected Dictionary<int, Stage_Data> _Stage_Data
	{
		get;
		private set;
	} = new Dictionary<int, Stage_Data>();
	///	<summary>
	///	 <b>key_1 int : wave_id </b><br/>
	///	</summary>
	protected Dictionary<int, Wave_Data> _Wave_Data
	{
		get;
		private set;
	} = new Dictionary<int, Wave_Data>();
	///	<summary>
	///	 <b>key_1 int : current_star_grade </b><br/>
	///	</summary>
	protected Dictionary<int, Star_Upgrade_Data> _Star_Upgrade_Data
	{
		get;
		private set;
	} = new Dictionary<int, Star_Upgrade_Data>();
	///	<summary>
	///	 <b>key_1 string : string_id </b><br/>
	///	</summary>
	protected Dictionary<string, System_Lang_Data> _System_Lang_Data
	{
		get;
		private set;
	} = new Dictionary<string, System_Lang_Data>();
	///	<summary>
	///	 <b>key_1 string : string_id </b><br/>
	///	</summary>
	protected Dictionary<string, Character_Lang_Data> _Character_Lang_Data
	{
		get;
		private set;
	} = new Dictionary<string, Character_Lang_Data>();
	///	<summary>
	///	 <b>key_1 string : string_id </b><br/>
	///	</summary>
	protected Dictionary<string, Skill_Lang_Data> _Skill_Lang_Data
	{
		get;
		private set;
	} = new Dictionary<string, Skill_Lang_Data>();
	///	<summary>
	///	 <b>key_1 string : string_id </b><br/>
	///	</summary>
	protected Dictionary<string, Item_Lang_Data> _Item_Lang_Data
	{
		get;
		private set;
	} = new Dictionary<string, Item_Lang_Data>();
	///	<summary>
	///	 <b>key_1 string : string_id </b><br/>
	///	</summary>
	protected Dictionary<string, Dialog_Lang_Data> _Dialog_Lang_Data
	{
		get;
		private set;
	} = new Dictionary<string, Dialog_Lang_Data>();
	///	<summary>
	///	 <b>key_1 string : string_id </b><br/>
	///	</summary>
	protected Dictionary<string, Story_Lang_Data> _Story_Lang_Data
	{
		get;
		private set;
	} = new Dictionary<string, Story_Lang_Data>();


	protected bool is_init_load = false;

	protected BaseMasterDataManager()
	{
		if(!is_init_load)
		{
			InitLoadMasterData();
		}
	}


	private async void InitLoadMasterData()
	{
		await LoadMaster_Attribute_Superiority_Data();
		await LoadMaster_Attribute_Synergy_Data();
		await LoadMaster_Boss_Data();
		await LoadMaster_Boss_Stage_Data();
		await LoadMaster_Charge_Value_Data();
		await LoadMaster_Dungeon_Data();
		await LoadMaster_Editor_Stage_Data();
		await LoadMaster_Editor_Wave_Data();
		await LoadMaster_Essence_Status_Data();
		await LoadMaster_Goods_Data();
		await LoadMaster_Item_Data();
		await LoadMaster_Item_Piece_Data();
		await LoadMaster_Equipment_Data();
		await LoadMaster_L2d_Char_Skin_Data();
		await LoadMaster_L2d_Love_State_Data();
		await LoadMaster_L2d_Skin_Ani_State_Data();
		await LoadMaster_L2d_Interaction_Base_Data();
		await LoadMaster_Player_Level_Data();
		await LoadMaster_Player_Character_Level_Data();
		await LoadMaster_Player_Character_Skill_Level_Data();
		await LoadMaster_Player_Character_Love_Level_Data();
		await LoadMaster_Max_Bound_Info_Data();
		await LoadMaster_Me_Resource_Data();
		await LoadMaster_Me_State_Data();
		await LoadMaster_Me_Interaction_Data();
		await LoadMaster_Me_Chat_Motion_Data();
		await LoadMaster_Me_Serifu_Data();
		await LoadMaster_Npc_Skill_Group();
		await LoadMaster_Npc_Skill_Data();
		await LoadMaster_Npc_Skill_Onetime_Data();
		await LoadMaster_Npc_Skill_Duration_Data();
		await LoadMaster_Npc_Data();
		await LoadMaster_Npc_Battle_Data();
		await LoadMaster_Npc_Level_Stat_Data();
		await LoadMaster_Player_Character_Skill_Group();
		await LoadMaster_Player_Character_Skill_Data();
		await LoadMaster_Player_Character_Skill_Onetime_Data();
		await LoadMaster_Player_Character_Skill_Duration_Data();
		await LoadMaster_Player_Character_Data();
		await LoadMaster_Player_Character_Battle_Data();
		await LoadMaster_Position_Icon_Data();
		await LoadMaster_Role_Icon_Data();
		await LoadMaster_Attribute_Icon_Data();
		await LoadMaster_Player_Character_Level_Stat_Data();
		await LoadMaster_Reward_Set_Data();
		await LoadMaster_Schedule_Data();
		await LoadMaster_World_Data();
		await LoadMaster_Zone_Data();
		await LoadMaster_Stage_Data();
		await LoadMaster_Wave_Data();
		await LoadMaster_Star_Upgrade_Data();
		await LoadMaster_System_Lang_Data();
		await LoadMaster_Character_Lang_Data();
		await LoadMaster_Skill_Lang_Data();
		await LoadMaster_Item_Lang_Data();
		await LoadMaster_Dialog_Lang_Data();
		await LoadMaster_Story_Lang_Data();
		is_init_load = true;
	}


#if UNITY_5_3_OR_NEWER
	async Task<string> LoadJsonDataAsync(string path)
	{
		var handle = Addressables.LoadAssetAsync<TextAsset>(path);
		TextAsset txt_asset = await handle.Task;
		return txt_asset.text;
	}


#else
	async Task<string> LoadJsonDataAsync(string path)
	{
		using (var reader = new StreamReader(path))
		{
			return await reader.ReadToEndAsync();
		}
	}


#endif
	protected async Task LoadMaster_Attribute_Superiority_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Attribute_Superiority_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Attribute_Superiority_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Attribute_Superiority_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Attribute_Superiority_Data.Add(raw_data.attacker_attribute_type, new Attribute_Superiority_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Attribute_Synergy_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Attribute_Synergy_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Attribute_Synergy_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Attribute_Synergy_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Attribute_Synergy_Data.Add(new Tuple<ATTRIBUTE_TYPE, int>(raw_data.attribute_type, raw_data.same_attribute_count), new Attribute_Synergy_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Boss_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Boss_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Boss_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Boss_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Boss_Data.Add(raw_data.boss_id, new Boss_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Boss_Stage_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Boss_Stage_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Boss_Stage_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Boss_Stage_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Boss_Stage_Data.Add(raw_data.boss_stage_id, new Boss_Stage_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Charge_Value_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Charge_Value_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Charge_Value_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Charge_Value_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Charge_Value_Data.Add(raw_data.reward_type, new Charge_Value_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Dungeon_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Dungeon_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Dungeon_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Dungeon_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Dungeon_Data.Add(raw_data.dungeon_id, new Dungeon_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Editor_Stage_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Editor_Stage_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Editor_Stage_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Editor_Stage_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Editor_Stage_Data.Add(raw_data.stage_id, new Editor_Stage_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Editor_Wave_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Editor_Wave_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Editor_Wave_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Editor_Wave_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Editor_Wave_Data.Add(new Tuple<int, int>(raw_data.wave_group_id, raw_data.wave_sequence), new Editor_Wave_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Essence_Status_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Essence_Status_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Essence_Status_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Essence_Status_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Essence_Status_Data.Add(raw_data.essence_charge_per, new Essence_Status_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Goods_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Goods_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Goods_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Goods_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Goods_Data.Add(raw_data.goods_type, new Goods_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Item_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Item_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Item_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Item_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Item_Data.Add(raw_data.item_id, new Item_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Item_Piece_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Item_Piece_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Item_Piece_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Item_Piece_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Item_Piece_Data.Add(raw_data.item_piece_id, new Item_Piece_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Equipment_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Equipment_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Equipment_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Equipment_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Equipment_Data.Add(raw_data.item_id, new Equipment_Data(raw_data));
		}
	}

	protected async Task LoadMaster_L2d_Char_Skin_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Char_Skin_Data");
#else
		string json = await LoadJsonDataAsync("../Master/L2d_Char_Skin_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Char_Skin_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_L2d_Char_Skin_Data.Add(raw_data.l2d_id, new L2d_Char_Skin_Data(raw_data));
		}
	}

	protected async Task LoadMaster_L2d_Love_State_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Love_State_Data");
#else
		string json = await LoadJsonDataAsync("../Master/L2d_Love_State_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Love_State_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_L2d_Love_State_Data.Add(raw_data.id, new L2d_Love_State_Data(raw_data));
		}
	}

	protected async Task LoadMaster_L2d_Skin_Ani_State_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Skin_Ani_State_Data");
#else
		string json = await LoadJsonDataAsync("../Master/L2d_Skin_Ani_State_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Skin_Ani_State_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_L2d_Skin_Ani_State_Data.Add(raw_data.state_id, new L2d_Skin_Ani_State_Data(raw_data));
		}
	}

	protected async Task LoadMaster_L2d_Interaction_Base_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Interaction_Base_Data");
#else
		string json = await LoadJsonDataAsync("../Master/L2d_Interaction_Base_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Interaction_Base_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_L2d_Interaction_Base_Data.Add(new Tuple<int, TOUCH_BODY_TYPE>(raw_data.interaction_group_id, raw_data.touch_type_01), new L2d_Interaction_Base_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Level_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Level_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Level_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Level_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Level_Data.Add(raw_data.level, new Player_Level_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Character_Level_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Level_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Level_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Level_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Character_Level_Data.Add(raw_data.level, new Player_Character_Level_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Character_Skill_Level_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Level_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Skill_Level_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Level_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Character_Skill_Level_Data.Add(raw_data.level, new Player_Character_Skill_Level_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Character_Love_Level_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Love_Level_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Love_Level_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Love_Level_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Character_Love_Level_Data.Add(raw_data.level, new Player_Character_Love_Level_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Max_Bound_Info_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Max_Bound_Info_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Max_Bound_Info_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Max_Bound_Info_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Max_Bound_Info_Data.Add(raw_data.reward_type, new Max_Bound_Info_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Me_Resource_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Resource_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Me_Resource_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Resource_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Me_Resource_Data.Add(raw_data.memorial_id, new Me_Resource_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Me_State_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_State_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Me_State_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_State_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Me_State_Data.Add(raw_data.state_id, new Me_State_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Me_Interaction_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Interaction_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Me_Interaction_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Interaction_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Me_Interaction_Data.Add(raw_data.interaction_id, new Me_Interaction_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Me_Chat_Motion_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Chat_Motion_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Me_Chat_Motion_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Chat_Motion_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Me_Chat_Motion_Data.Add(raw_data.chat_motion_id, new Me_Chat_Motion_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Me_Serifu_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Serifu_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Me_Serifu_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Serifu_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Me_Serifu_Data.Add(raw_data.serifu_id, new Me_Serifu_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Npc_Skill_Group()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Group");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Skill_Group.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Group>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Npc_Skill_Group.Add(raw_data.npc_skill_group_id, new Npc_Skill_Group(raw_data));
		}
	}

	protected async Task LoadMaster_Npc_Skill_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Skill_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Npc_Skill_Data.Add(raw_data.npc_skill_id, new Npc_Skill_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Npc_Skill_Onetime_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Onetime_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Skill_Onetime_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Onetime_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Npc_Skill_Onetime_Data.Add(raw_data.npc_skill_onetime_id, new Npc_Skill_Onetime_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Npc_Skill_Duration_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Duration_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Skill_Duration_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Duration_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Npc_Skill_Duration_Data.Add(raw_data.npc_skill_duration_id, new Npc_Skill_Duration_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Npc_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Npc_Data.Add(raw_data.npc_data_id, new Npc_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Npc_Battle_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Battle_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Battle_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Battle_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Npc_Battle_Data.Add(raw_data.npc_battle_id, new Npc_Battle_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Npc_Level_Stat_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Level_Stat_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Level_Stat_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Level_Stat_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Npc_Level_Stat_Data.Add(raw_data.npc_level_stat_id, new Npc_Level_Stat_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Character_Skill_Group()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Group");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Skill_Group.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Group>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Character_Skill_Group.Add(raw_data.pc_skill_group_id, new Player_Character_Skill_Group(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Character_Skill_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Skill_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Character_Skill_Data.Add(raw_data.pc_skill_id, new Player_Character_Skill_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Character_Skill_Onetime_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Onetime_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Skill_Onetime_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Onetime_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Character_Skill_Onetime_Data.Add(raw_data.pc_skill_onetime_id, new Player_Character_Skill_Onetime_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Character_Skill_Duration_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Duration_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Skill_Duration_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Duration_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Character_Skill_Duration_Data.Add(raw_data.pc_skill_duration_id, new Player_Character_Skill_Duration_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Character_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Character_Data.Add(raw_data.player_character_id, new Player_Character_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Character_Battle_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Battle_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Battle_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Battle_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Character_Battle_Data.Add(new Tuple<int, int>(raw_data.battle_info_id, raw_data.star_grade), new Player_Character_Battle_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Position_Icon_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Position_Icon_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Position_Icon_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Position_Icon_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Position_Icon_Data.Add(raw_data.position_type, new Position_Icon_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Role_Icon_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Role_Icon_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Role_Icon_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Role_Icon_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Role_Icon_Data.Add(raw_data.role_type, new Role_Icon_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Attribute_Icon_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Attribute_Icon_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Attribute_Icon_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Attribute_Icon_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Attribute_Icon_Data.Add(raw_data.attribute_type, new Attribute_Icon_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Player_Character_Level_Stat_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Level_Stat_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Level_Stat_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Level_Stat_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Player_Character_Level_Stat_Data.Add(new Tuple<int, int>(raw_data.player_character_id, raw_data.star_grade), new Player_Character_Level_Stat_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Reward_Set_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Reward_Set_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Reward_Set_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Reward_Set_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Reward_Set_Data.Add(raw_data.reward_id, new Reward_Set_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Schedule_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Schedule_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Schedule_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Schedule_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Schedule_Data.Add(raw_data.schedule_id, new Schedule_Data(raw_data));
		}
	}

	protected async Task LoadMaster_World_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/World_Data");
#else
		string json = await LoadJsonDataAsync("../Master/World_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_World_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_World_Data.Add(raw_data.world_id, new World_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Zone_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Zone_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Zone_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Zone_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Zone_Data.Add(raw_data.zone_id, new Zone_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Stage_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Stage_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Stage_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Stage_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Stage_Data.Add(raw_data.stage_id, new Stage_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Wave_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Wave_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Wave_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Wave_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Wave_Data.Add(raw_data.wave_id, new Wave_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Star_Upgrade_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Star_Upgrade_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Star_Upgrade_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Star_Upgrade_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Star_Upgrade_Data.Add(raw_data.current_star_grade, new Star_Upgrade_Data(raw_data));
		}
	}

	protected async Task LoadMaster_System_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/System_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/System_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_System_Lang_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_System_Lang_Data.Add(raw_data.string_id, new System_Lang_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Character_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Character_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Character_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Character_Lang_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Character_Lang_Data.Add(raw_data.string_id, new Character_Lang_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Skill_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Skill_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Skill_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Skill_Lang_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Skill_Lang_Data.Add(raw_data.string_id, new Skill_Lang_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Item_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Item_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Item_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Item_Lang_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Item_Lang_Data.Add(raw_data.string_id, new Item_Lang_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Dialog_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Dialog_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Dialog_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Dialog_Lang_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Dialog_Lang_Data.Add(raw_data.string_id, new Dialog_Lang_Data(raw_data));
		}
	}

	protected async Task LoadMaster_Story_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Story_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Story_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Story_Lang_Data>>(json);
		foreach (var raw_data in raw_data_list)
		{
			_Story_Lang_Data.Add(raw_data.string_id, new Story_Lang_Data(raw_data));
		}
	}

	protected async void Check_Attribute_Superiority_Data()
	{
		if(_Attribute_Superiority_Data == null)
		{
			await LoadMaster_Attribute_Superiority_Data();
		}
	}

	protected async void Check_Attribute_Synergy_Data()
	{
		if(_Attribute_Synergy_Data == null)
		{
			await LoadMaster_Attribute_Synergy_Data();
		}
	}

	protected async void Check_Boss_Data()
	{
		if(_Boss_Data == null)
		{
			await LoadMaster_Boss_Data();
		}
	}

	protected async void Check_Boss_Stage_Data()
	{
		if(_Boss_Stage_Data == null)
		{
			await LoadMaster_Boss_Stage_Data();
		}
	}

	protected async void Check_Charge_Value_Data()
	{
		if(_Charge_Value_Data == null)
		{
			await LoadMaster_Charge_Value_Data();
		}
	}

	protected async void Check_Dungeon_Data()
	{
		if(_Dungeon_Data == null)
		{
			await LoadMaster_Dungeon_Data();
		}
	}

	protected async void Check_Editor_Stage_Data()
	{
		if(_Editor_Stage_Data == null)
		{
			await LoadMaster_Editor_Stage_Data();
		}
	}

	protected async void Check_Editor_Wave_Data()
	{
		if(_Editor_Wave_Data == null)
		{
			await LoadMaster_Editor_Wave_Data();
		}
	}

	protected async void Check_Essence_Status_Data()
	{
		if(_Essence_Status_Data == null)
		{
			await LoadMaster_Essence_Status_Data();
		}
	}

	protected async void Check_Goods_Data()
	{
		if(_Goods_Data == null)
		{
			await LoadMaster_Goods_Data();
		}
	}

	protected async void Check_Item_Data()
	{
		if(_Item_Data == null)
		{
			await LoadMaster_Item_Data();
		}
	}

	protected async void Check_Item_Piece_Data()
	{
		if(_Item_Piece_Data == null)
		{
			await LoadMaster_Item_Piece_Data();
		}
	}

	protected async void Check_Equipment_Data()
	{
		if(_Equipment_Data == null)
		{
			await LoadMaster_Equipment_Data();
		}
	}

	protected async void Check_L2d_Char_Skin_Data()
	{
		if(_L2d_Char_Skin_Data == null)
		{
			await LoadMaster_L2d_Char_Skin_Data();
		}
	}

	protected async void Check_L2d_Love_State_Data()
	{
		if(_L2d_Love_State_Data == null)
		{
			await LoadMaster_L2d_Love_State_Data();
		}
	}

	protected async void Check_L2d_Skin_Ani_State_Data()
	{
		if(_L2d_Skin_Ani_State_Data == null)
		{
			await LoadMaster_L2d_Skin_Ani_State_Data();
		}
	}

	protected async void Check_L2d_Interaction_Base_Data()
	{
		if(_L2d_Interaction_Base_Data == null)
		{
			await LoadMaster_L2d_Interaction_Base_Data();
		}
	}

	protected async void Check_Player_Level_Data()
	{
		if(_Player_Level_Data == null)
		{
			await LoadMaster_Player_Level_Data();
		}
	}

	protected async void Check_Player_Character_Level_Data()
	{
		if(_Player_Character_Level_Data == null)
		{
			await LoadMaster_Player_Character_Level_Data();
		}
	}

	protected async void Check_Player_Character_Skill_Level_Data()
	{
		if(_Player_Character_Skill_Level_Data == null)
		{
			await LoadMaster_Player_Character_Skill_Level_Data();
		}
	}

	protected async void Check_Player_Character_Love_Level_Data()
	{
		if(_Player_Character_Love_Level_Data == null)
		{
			await LoadMaster_Player_Character_Love_Level_Data();
		}
	}

	protected async void Check_Max_Bound_Info_Data()
	{
		if(_Max_Bound_Info_Data == null)
		{
			await LoadMaster_Max_Bound_Info_Data();
		}
	}

	protected async void Check_Me_Resource_Data()
	{
		if(_Me_Resource_Data == null)
		{
			await LoadMaster_Me_Resource_Data();
		}
	}

	protected async void Check_Me_State_Data()
	{
		if(_Me_State_Data == null)
		{
			await LoadMaster_Me_State_Data();
		}
	}

	protected async void Check_Me_Interaction_Data()
	{
		if(_Me_Interaction_Data == null)
		{
			await LoadMaster_Me_Interaction_Data();
		}
	}

	protected async void Check_Me_Chat_Motion_Data()
	{
		if(_Me_Chat_Motion_Data == null)
		{
			await LoadMaster_Me_Chat_Motion_Data();
		}
	}

	protected async void Check_Me_Serifu_Data()
	{
		if(_Me_Serifu_Data == null)
		{
			await LoadMaster_Me_Serifu_Data();
		}
	}

	protected async void Check_Npc_Skill_Group()
	{
		if(_Npc_Skill_Group == null)
		{
			await LoadMaster_Npc_Skill_Group();
		}
	}

	protected async void Check_Npc_Skill_Data()
	{
		if(_Npc_Skill_Data == null)
		{
			await LoadMaster_Npc_Skill_Data();
		}
	}

	protected async void Check_Npc_Skill_Onetime_Data()
	{
		if(_Npc_Skill_Onetime_Data == null)
		{
			await LoadMaster_Npc_Skill_Onetime_Data();
		}
	}

	protected async void Check_Npc_Skill_Duration_Data()
	{
		if(_Npc_Skill_Duration_Data == null)
		{
			await LoadMaster_Npc_Skill_Duration_Data();
		}
	}

	protected async void Check_Npc_Data()
	{
		if(_Npc_Data == null)
		{
			await LoadMaster_Npc_Data();
		}
	}

	protected async void Check_Npc_Battle_Data()
	{
		if(_Npc_Battle_Data == null)
		{
			await LoadMaster_Npc_Battle_Data();
		}
	}

	protected async void Check_Npc_Level_Stat_Data()
	{
		if(_Npc_Level_Stat_Data == null)
		{
			await LoadMaster_Npc_Level_Stat_Data();
		}
	}

	protected async void Check_Player_Character_Skill_Group()
	{
		if(_Player_Character_Skill_Group == null)
		{
			await LoadMaster_Player_Character_Skill_Group();
		}
	}

	protected async void Check_Player_Character_Skill_Data()
	{
		if(_Player_Character_Skill_Data == null)
		{
			await LoadMaster_Player_Character_Skill_Data();
		}
	}

	protected async void Check_Player_Character_Skill_Onetime_Data()
	{
		if(_Player_Character_Skill_Onetime_Data == null)
		{
			await LoadMaster_Player_Character_Skill_Onetime_Data();
		}
	}

	protected async void Check_Player_Character_Skill_Duration_Data()
	{
		if(_Player_Character_Skill_Duration_Data == null)
		{
			await LoadMaster_Player_Character_Skill_Duration_Data();
		}
	}

	protected async void Check_Player_Character_Data()
	{
		if(_Player_Character_Data == null)
		{
			await LoadMaster_Player_Character_Data();
		}
	}

	protected async void Check_Player_Character_Battle_Data()
	{
		if(_Player_Character_Battle_Data == null)
		{
			await LoadMaster_Player_Character_Battle_Data();
		}
	}

	protected async void Check_Position_Icon_Data()
	{
		if(_Position_Icon_Data == null)
		{
			await LoadMaster_Position_Icon_Data();
		}
	}

	protected async void Check_Role_Icon_Data()
	{
		if(_Role_Icon_Data == null)
		{
			await LoadMaster_Role_Icon_Data();
		}
	}

	protected async void Check_Attribute_Icon_Data()
	{
		if(_Attribute_Icon_Data == null)
		{
			await LoadMaster_Attribute_Icon_Data();
		}
	}

	protected async void Check_Player_Character_Level_Stat_Data()
	{
		if(_Player_Character_Level_Stat_Data == null)
		{
			await LoadMaster_Player_Character_Level_Stat_Data();
		}
	}

	protected async void Check_Reward_Set_Data()
	{
		if(_Reward_Set_Data == null)
		{
			await LoadMaster_Reward_Set_Data();
		}
	}

	protected async void Check_Schedule_Data()
	{
		if(_Schedule_Data == null)
		{
			await LoadMaster_Schedule_Data();
		}
	}

	protected async void Check_World_Data()
	{
		if(_World_Data == null)
		{
			await LoadMaster_World_Data();
		}
	}

	protected async void Check_Zone_Data()
	{
		if(_Zone_Data == null)
		{
			await LoadMaster_Zone_Data();
		}
	}

	protected async void Check_Stage_Data()
	{
		if(_Stage_Data == null)
		{
			await LoadMaster_Stage_Data();
		}
	}

	protected async void Check_Wave_Data()
	{
		if(_Wave_Data == null)
		{
			await LoadMaster_Wave_Data();
		}
	}

	protected async void Check_Star_Upgrade_Data()
	{
		if(_Star_Upgrade_Data == null)
		{
			await LoadMaster_Star_Upgrade_Data();
		}
	}

	protected async void Check_System_Lang_Data()
	{
		if(_System_Lang_Data == null)
		{
			await LoadMaster_System_Lang_Data();
		}
	}

	protected async void Check_Character_Lang_Data()
	{
		if(_Character_Lang_Data == null)
		{
			await LoadMaster_Character_Lang_Data();
		}
	}

	protected async void Check_Skill_Lang_Data()
	{
		if(_Skill_Lang_Data == null)
		{
			await LoadMaster_Skill_Lang_Data();
		}
	}

	protected async void Check_Item_Lang_Data()
	{
		if(_Item_Lang_Data == null)
		{
			await LoadMaster_Item_Lang_Data();
		}
	}

	protected async void Check_Dialog_Lang_Data()
	{
		if(_Dialog_Lang_Data == null)
		{
			await LoadMaster_Dialog_Lang_Data();
		}
	}

	protected async void Check_Story_Lang_Data()
	{
		if(_Story_Lang_Data == null)
		{
			await LoadMaster_Story_Lang_Data();
		}
	}

}
