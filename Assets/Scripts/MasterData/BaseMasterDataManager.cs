﻿using System.Collections.Generic;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif
using System.Threading.Tasks;
#if UNITY_5_3_OR_NEWER
using UnityEngine.AddressableAssets;
#endif
#nullable disable
using Newtonsoft.Json;
using System.Linq;

public class BaseMasterDataManager
{

	protected List<Charge_Value_Data> _Charge_Value_Data
	{
		get;
		private set;
	}
	protected List<Editor_Stage_Data> _Editor_Stage_Data
	{
		get;
		private set;
	}
	protected List<Editor_Wave_Data> _Editor_Wave_Data
	{
		get;
		private set;
	}
	protected List<Essence_Status_Data> _Essence_Status_Data
	{
		get;
		private set;
	}
	protected List<Goods_Data> _Goods_Data
	{
		get;
		private set;
	}
	protected List<Item_Data> _Item_Data
	{
		get;
		private set;
	}
	protected List<Item_Piece_Data> _Item_Piece_Data
	{
		get;
		private set;
	}
	protected List<Equipment_Data> _Equipment_Data
	{
		get;
		private set;
	}
	protected List<L2d_Char_Skin_Data> _L2d_Char_Skin_Data
	{
		get;
		private set;
	}
	protected List<L2d_Love_State_Data> _L2d_Love_State_Data
	{
		get;
		private set;
	}
	protected List<L2d_Skin_Ani_State_Data> _L2d_Skin_Ani_State_Data
	{
		get;
		private set;
	}
	protected List<L2d_Interaction_Base_Data> _L2d_Interaction_Base_Data
	{
		get;
		private set;
	}
	protected List<Player_Level_Data> _Player_Level_Data
	{
		get;
		private set;
	}
	protected List<Player_Character_Level_Data> _Player_Character_Level_Data
	{
		get;
		private set;
	}
	protected List<Player_Character_Skill_Level_Data> _Player_Character_Skill_Level_Data
	{
		get;
		private set;
	}
	protected List<Max_Bound_Info_Data> _Max_Bound_Info_Data
	{
		get;
		private set;
	}
	protected List<Me_Resource_Data> _Me_Resource_Data
	{
		get;
		private set;
	}
	protected List<Me_State_Data> _Me_State_Data
	{
		get;
		private set;
	}
	protected List<Me_Interaction_Data> _Me_Interaction_Data
	{
		get;
		private set;
	}
	protected List<Me_Chat_Motion_Data> _Me_Chat_Motion_Data
	{
		get;
		private set;
	}
	protected List<Me_Serifu_Data> _Me_Serifu_Data
	{
		get;
		private set;
	}
	protected List<Npc_Skill_Group> _Npc_Skill_Group
	{
		get;
		private set;
	}
	protected List<Npc_Skill_Data> _Npc_Skill_Data
	{
		get;
		private set;
	}
	protected List<Npc_Skill_Onetime_Data> _Npc_Skill_Onetime_Data
	{
		get;
		private set;
	}
	protected List<Npc_Skill_Duration_Data> _Npc_Skill_Duration_Data
	{
		get;
		private set;
	}
	protected List<Npc_Data> _Npc_Data
	{
		get;
		private set;
	}
	protected List<Npc_Battle_Data> _Npc_Battle_Data
	{
		get;
		private set;
	}
	protected List<Npc_Level_Stat_Data> _Npc_Level_Stat_Data
	{
		get;
		private set;
	}
	protected List<Player_Character_Skill_Group> _Player_Character_Skill_Group
	{
		get;
		private set;
	}
	protected List<Player_Character_Skill_Data> _Player_Character_Skill_Data
	{
		get;
		private set;
	}
	protected List<Player_Character_Skill_Onetime_Data> _Player_Character_Skill_Onetime_Data
	{
		get;
		private set;
	}
	protected List<Player_Character_Skill_Duration_Data> _Player_Character_Skill_Duration_Data
	{
		get;
		private set;
	}
	protected List<Player_Character_Data> _Player_Character_Data
	{
		get;
		private set;
	}
	protected List<Player_Character_Battle_Data> _Player_Character_Battle_Data
	{
		get;
		private set;
	}
	protected List<Position_Icon_Data> _Position_Icon_Data
	{
		get;
		private set;
	}
	protected List<Role_Icon_Data> _Role_Icon_Data
	{
		get;
		private set;
	}
	protected List<Reward_Set_Data> _Reward_Set_Data
	{
		get;
		private set;
	}
	protected List<World_Data> _World_Data
	{
		get;
		private set;
	}
	protected List<Zone_Data> _Zone_Data
	{
		get;
		private set;
	}
	protected List<Stage_Data> _Stage_Data
	{
		get;
		private set;
	}
	protected List<Wave_Data> _Wave_Data
	{
		get;
		private set;
	}
	protected List<Star_Upgrade_Data> _Star_Upgrade_Data
	{
		get;
		private set;
	}
	protected List<System_Lang_Data> _System_Lang_Data
	{
		get;
		private set;
	}
	protected List<Character_Lang_Data> _Character_Lang_Data
	{
		get;
		private set;
	}
	protected List<Skill_Lang_Data> _Skill_Lang_Data
	{
		get;
		private set;
	}
	protected List<Item_Lang_Data> _Item_Lang_Data
	{
		get;
		private set;
	}
	protected List<Dialog_Lang_Data> _Dialog_Lang_Data
	{
		get;
		private set;
	}
	protected List<Story_Lang_Data> _Story_Lang_Data
	{
		get;
		private set;
	}


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
		await LoadMaster_Charge_Value_Data();
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
		await LoadMaster_Reward_Set_Data();
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
	protected async Task LoadMaster_Charge_Value_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Charge_Value_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Charge_Value_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Charge_Value_Data>>(json);
		_Charge_Value_Data = raw_data_list.Select(raw_data => new Charge_Value_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Editor_Stage_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Editor_Stage_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Editor_Stage_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Editor_Stage_Data>>(json);
		_Editor_Stage_Data = raw_data_list.Select(raw_data => new Editor_Stage_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Editor_Wave_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Editor_Wave_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Editor_Wave_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Editor_Wave_Data>>(json);
		_Editor_Wave_Data = raw_data_list.Select(raw_data => new Editor_Wave_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Essence_Status_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Essence_Status_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Essence_Status_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Essence_Status_Data>>(json);
		_Essence_Status_Data = raw_data_list.Select(raw_data => new Essence_Status_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Goods_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Goods_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Goods_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Goods_Data>>(json);
		_Goods_Data = raw_data_list.Select(raw_data => new Goods_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Item_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Item_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Item_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Item_Data>>(json);
		_Item_Data = raw_data_list.Select(raw_data => new Item_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Item_Piece_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Item_Piece_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Item_Piece_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Item_Piece_Data>>(json);
		_Item_Piece_Data = raw_data_list.Select(raw_data => new Item_Piece_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Equipment_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Equipment_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Equipment_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Equipment_Data>>(json);
		_Equipment_Data = raw_data_list.Select(raw_data => new Equipment_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_L2d_Char_Skin_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Char_Skin_Data");
#else
		string json = await LoadJsonDataAsync("../Master/L2d_Char_Skin_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Char_Skin_Data>>(json);
		_L2d_Char_Skin_Data = raw_data_list.Select(raw_data => new L2d_Char_Skin_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_L2d_Love_State_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Love_State_Data");
#else
		string json = await LoadJsonDataAsync("../Master/L2d_Love_State_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Love_State_Data>>(json);
		_L2d_Love_State_Data = raw_data_list.Select(raw_data => new L2d_Love_State_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_L2d_Skin_Ani_State_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Skin_Ani_State_Data");
#else
		string json = await LoadJsonDataAsync("../Master/L2d_Skin_Ani_State_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Skin_Ani_State_Data>>(json);
		_L2d_Skin_Ani_State_Data = raw_data_list.Select(raw_data => new L2d_Skin_Ani_State_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_L2d_Interaction_Base_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Interaction_Base_Data");
#else
		string json = await LoadJsonDataAsync("../Master/L2d_Interaction_Base_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Interaction_Base_Data>>(json);
		_L2d_Interaction_Base_Data = raw_data_list.Select(raw_data => new L2d_Interaction_Base_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Level_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Level_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Level_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Level_Data>>(json);
		_Player_Level_Data = raw_data_list.Select(raw_data => new Player_Level_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Level_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Level_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Level_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Level_Data>>(json);
		_Player_Character_Level_Data = raw_data_list.Select(raw_data => new Player_Character_Level_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Skill_Level_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Level_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Skill_Level_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Level_Data>>(json);
		_Player_Character_Skill_Level_Data = raw_data_list.Select(raw_data => new Player_Character_Skill_Level_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Max_Bound_Info_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Max_Bound_Info_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Max_Bound_Info_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Max_Bound_Info_Data>>(json);
		_Max_Bound_Info_Data = raw_data_list.Select(raw_data => new Max_Bound_Info_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Me_Resource_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Resource_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Me_Resource_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Resource_Data>>(json);
		_Me_Resource_Data = raw_data_list.Select(raw_data => new Me_Resource_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Me_State_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_State_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Me_State_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_State_Data>>(json);
		_Me_State_Data = raw_data_list.Select(raw_data => new Me_State_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Me_Interaction_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Interaction_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Me_Interaction_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Interaction_Data>>(json);
		_Me_Interaction_Data = raw_data_list.Select(raw_data => new Me_Interaction_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Me_Chat_Motion_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Chat_Motion_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Me_Chat_Motion_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Chat_Motion_Data>>(json);
		_Me_Chat_Motion_Data = raw_data_list.Select(raw_data => new Me_Chat_Motion_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Me_Serifu_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Serifu_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Me_Serifu_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Serifu_Data>>(json);
		_Me_Serifu_Data = raw_data_list.Select(raw_data => new Me_Serifu_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Skill_Group()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Group");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Skill_Group.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Group>>(json);
		_Npc_Skill_Group = raw_data_list.Select(raw_data => new Npc_Skill_Group(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Skill_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Skill_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Data>>(json);
		_Npc_Skill_Data = raw_data_list.Select(raw_data => new Npc_Skill_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Skill_Onetime_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Onetime_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Skill_Onetime_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Onetime_Data>>(json);
		_Npc_Skill_Onetime_Data = raw_data_list.Select(raw_data => new Npc_Skill_Onetime_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Skill_Duration_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Duration_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Skill_Duration_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Duration_Data>>(json);
		_Npc_Skill_Duration_Data = raw_data_list.Select(raw_data => new Npc_Skill_Duration_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Data>>(json);
		_Npc_Data = raw_data_list.Select(raw_data => new Npc_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Battle_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Battle_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Battle_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Battle_Data>>(json);
		_Npc_Battle_Data = raw_data_list.Select(raw_data => new Npc_Battle_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Level_Stat_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Level_Stat_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Npc_Level_Stat_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Level_Stat_Data>>(json);
		_Npc_Level_Stat_Data = raw_data_list.Select(raw_data => new Npc_Level_Stat_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Skill_Group()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Group");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Skill_Group.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Group>>(json);
		_Player_Character_Skill_Group = raw_data_list.Select(raw_data => new Player_Character_Skill_Group(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Skill_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Skill_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Data>>(json);
		_Player_Character_Skill_Data = raw_data_list.Select(raw_data => new Player_Character_Skill_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Skill_Onetime_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Onetime_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Skill_Onetime_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Onetime_Data>>(json);
		_Player_Character_Skill_Onetime_Data = raw_data_list.Select(raw_data => new Player_Character_Skill_Onetime_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Skill_Duration_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Duration_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Skill_Duration_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Duration_Data>>(json);
		_Player_Character_Skill_Duration_Data = raw_data_list.Select(raw_data => new Player_Character_Skill_Duration_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Data>>(json);
		_Player_Character_Data = raw_data_list.Select(raw_data => new Player_Character_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Battle_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Battle_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Player_Character_Battle_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Battle_Data>>(json);
		_Player_Character_Battle_Data = raw_data_list.Select(raw_data => new Player_Character_Battle_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Position_Icon_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Position_Icon_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Position_Icon_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Position_Icon_Data>>(json);
		_Position_Icon_Data = raw_data_list.Select(raw_data => new Position_Icon_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Role_Icon_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Role_Icon_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Role_Icon_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Role_Icon_Data>>(json);
		_Role_Icon_Data = raw_data_list.Select(raw_data => new Role_Icon_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Reward_Set_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Reward_Set_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Reward_Set_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Reward_Set_Data>>(json);
		_Reward_Set_Data = raw_data_list.Select(raw_data => new Reward_Set_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_World_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/World_Data");
#else
		string json = await LoadJsonDataAsync("../Master/World_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_World_Data>>(json);
		_World_Data = raw_data_list.Select(raw_data => new World_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Zone_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Zone_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Zone_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Zone_Data>>(json);
		_Zone_Data = raw_data_list.Select(raw_data => new Zone_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Stage_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Stage_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Stage_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Stage_Data>>(json);
		_Stage_Data = raw_data_list.Select(raw_data => new Stage_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Wave_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Wave_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Wave_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Wave_Data>>(json);
		_Wave_Data = raw_data_list.Select(raw_data => new Wave_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Star_Upgrade_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Star_Upgrade_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Star_Upgrade_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Star_Upgrade_Data>>(json);
		_Star_Upgrade_Data = raw_data_list.Select(raw_data => new Star_Upgrade_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_System_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/System_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/System_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_System_Lang_Data>>(json);
		_System_Lang_Data = raw_data_list.Select(raw_data => new System_Lang_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Character_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Character_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Character_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Character_Lang_Data>>(json);
		_Character_Lang_Data = raw_data_list.Select(raw_data => new Character_Lang_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Skill_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Skill_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Skill_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Skill_Lang_Data>>(json);
		_Skill_Lang_Data = raw_data_list.Select(raw_data => new Skill_Lang_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Item_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Item_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Item_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Item_Lang_Data>>(json);
		_Item_Lang_Data = raw_data_list.Select(raw_data => new Item_Lang_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Dialog_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Dialog_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Dialog_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Dialog_Lang_Data>>(json);
		_Dialog_Lang_Data = raw_data_list.Select(raw_data => new Dialog_Lang_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Story_Lang_Data()
	{
#if UNITY_5_3_OR_NEWER
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Story_Lang_Data");
#else
		string json = await LoadJsonDataAsync("../Master/Story_Lang_Data.json");
#endif
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Story_Lang_Data>>(json);
		_Story_Lang_Data = raw_data_list.Select(raw_data => new Story_Lang_Data(raw_data)).ToList();
	}

	protected async void Check_Charge_Value_Data()
	{
		if(_Charge_Value_Data == null)
		{
			await LoadMaster_Charge_Value_Data();
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

	protected async void Check_Reward_Set_Data()
	{
		if(_Reward_Set_Data == null)
		{
			await LoadMaster_Reward_Set_Data();
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
