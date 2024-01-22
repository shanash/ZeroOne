using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;
using System.Linq;

public class BaseMasterDataManager
{

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
	protected List<Item_Type_Data> _Item_Type_Data
	{
		get;
		private set;
	}
	protected List<Character_Piece_Data> _Character_Piece_Data
	{
		get;
		private set;
	}
	protected List<Exp_Potion_Data> _Exp_Potion_Data
	{
		get;
		private set;
	}
	protected List<Sta_Potion_Data> _Sta_Potion_Data
	{
		get;
		private set;
	}
	protected List<Favorite_Item_Data> _Favorite_Item_Data
	{
		get;
		private set;
	}
	protected List<Expendable_Item_Data> _Expendable_Item_Data
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
	protected List<First_Reward_Data> _First_Reward_Data
	{
		get;
		private set;
	}
	protected List<Repeat_Reward_Data> _Repeat_Reward_Data
	{
		get;
		private set;
	}
	protected List<Star_Reward_Data> _Star_Reward_Data
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
		await LoadMaster_Editor_Stage_Data();
		await LoadMaster_Editor_Wave_Data();
		await LoadMaster_Item_Type_Data();
		await LoadMaster_Character_Piece_Data();
		await LoadMaster_Exp_Potion_Data();
		await LoadMaster_Sta_Potion_Data();
		await LoadMaster_Favorite_Item_Data();
		await LoadMaster_Expendable_Item_Data();
		await LoadMaster_Goods_Data();
		await LoadMaster_Item_Data();
		await LoadMaster_Item_Piece_Data();
		await LoadMaster_Equipment_Data();
		await LoadMaster_L2d_Char_Skin_Data();
		await LoadMaster_L2d_Love_State_Data();
		await LoadMaster_L2d_Skin_Ani_State_Data();
		await LoadMaster_Player_Level_Data();
		await LoadMaster_Player_Character_Level_Data();
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
		await LoadMaster_First_Reward_Data();
		await LoadMaster_Repeat_Reward_Data();
		await LoadMaster_Star_Reward_Data();
		await LoadMaster_Reward_Set_Data();
		await LoadMaster_World_Data();
		await LoadMaster_Zone_Data();
		await LoadMaster_Stage_Data();
		await LoadMaster_Wave_Data();
		is_init_load = true;
	}


	async Task<string> LoadJsonDataAsync(string path)
	{
		var handle = Addressables.LoadAssetAsync<TextAsset>(path);
		TextAsset txt_asset = await handle.Task;
		return txt_asset.text;
	}


	protected async Task LoadMaster_Editor_Stage_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Editor_Stage_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Editor_Stage_Data>>(json);
		_Editor_Stage_Data = raw_data_list.Select(raw_data => new Editor_Stage_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Editor_Wave_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Editor_Wave_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Editor_Wave_Data>>(json);
		_Editor_Wave_Data = raw_data_list.Select(raw_data => new Editor_Wave_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Item_Type_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Item_Type_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Item_Type_Data>>(json);
		_Item_Type_Data = raw_data_list.Select(raw_data => new Item_Type_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Character_Piece_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Character_Piece_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Character_Piece_Data>>(json);
		_Character_Piece_Data = raw_data_list.Select(raw_data => new Character_Piece_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Exp_Potion_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Exp_Potion_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Exp_Potion_Data>>(json);
		_Exp_Potion_Data = raw_data_list.Select(raw_data => new Exp_Potion_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Sta_Potion_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Sta_Potion_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Sta_Potion_Data>>(json);
		_Sta_Potion_Data = raw_data_list.Select(raw_data => new Sta_Potion_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Favorite_Item_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Favorite_Item_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Favorite_Item_Data>>(json);
		_Favorite_Item_Data = raw_data_list.Select(raw_data => new Favorite_Item_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Expendable_Item_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Expendable_Item_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Expendable_Item_Data>>(json);
		_Expendable_Item_Data = raw_data_list.Select(raw_data => new Expendable_Item_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Goods_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Goods_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Goods_Data>>(json);
		_Goods_Data = raw_data_list.Select(raw_data => new Goods_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Item_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Item_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Item_Data>>(json);
		_Item_Data = raw_data_list.Select(raw_data => new Item_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Item_Piece_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Item_Piece_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Item_Piece_Data>>(json);
		_Item_Piece_Data = raw_data_list.Select(raw_data => new Item_Piece_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Equipment_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Equipment_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Equipment_Data>>(json);
		_Equipment_Data = raw_data_list.Select(raw_data => new Equipment_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_L2d_Char_Skin_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Char_Skin_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Char_Skin_Data>>(json);
		_L2d_Char_Skin_Data = raw_data_list.Select(raw_data => new L2d_Char_Skin_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_L2d_Love_State_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Love_State_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Love_State_Data>>(json);
		_L2d_Love_State_Data = raw_data_list.Select(raw_data => new L2d_Love_State_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_L2d_Skin_Ani_State_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/L2d_Skin_Ani_State_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_L2d_Skin_Ani_State_Data>>(json);
		_L2d_Skin_Ani_State_Data = raw_data_list.Select(raw_data => new L2d_Skin_Ani_State_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Level_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Level_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Level_Data>>(json);
		_Player_Level_Data = raw_data_list.Select(raw_data => new Player_Level_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Level_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Level_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Level_Data>>(json);
		_Player_Character_Level_Data = raw_data_list.Select(raw_data => new Player_Character_Level_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Me_Resource_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Resource_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Resource_Data>>(json);
		_Me_Resource_Data = raw_data_list.Select(raw_data => new Me_Resource_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Me_State_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_State_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_State_Data>>(json);
		_Me_State_Data = raw_data_list.Select(raw_data => new Me_State_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Me_Interaction_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Interaction_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Interaction_Data>>(json);
		_Me_Interaction_Data = raw_data_list.Select(raw_data => new Me_Interaction_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Me_Chat_Motion_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Chat_Motion_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Chat_Motion_Data>>(json);
		_Me_Chat_Motion_Data = raw_data_list.Select(raw_data => new Me_Chat_Motion_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Me_Serifu_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Serifu_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Me_Serifu_Data>>(json);
		_Me_Serifu_Data = raw_data_list.Select(raw_data => new Me_Serifu_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Skill_Group()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Group");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Group>>(json);
		_Npc_Skill_Group = raw_data_list.Select(raw_data => new Npc_Skill_Group(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Skill_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Data>>(json);
		_Npc_Skill_Data = raw_data_list.Select(raw_data => new Npc_Skill_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Skill_Onetime_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Onetime_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Onetime_Data>>(json);
		_Npc_Skill_Onetime_Data = raw_data_list.Select(raw_data => new Npc_Skill_Onetime_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Skill_Duration_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Duration_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Skill_Duration_Data>>(json);
		_Npc_Skill_Duration_Data = raw_data_list.Select(raw_data => new Npc_Skill_Duration_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Data>>(json);
		_Npc_Data = raw_data_list.Select(raw_data => new Npc_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Battle_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Battle_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Battle_Data>>(json);
		_Npc_Battle_Data = raw_data_list.Select(raw_data => new Npc_Battle_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Npc_Level_Stat_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Level_Stat_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Npc_Level_Stat_Data>>(json);
		_Npc_Level_Stat_Data = raw_data_list.Select(raw_data => new Npc_Level_Stat_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Skill_Group()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Group");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Group>>(json);
		_Player_Character_Skill_Group = raw_data_list.Select(raw_data => new Player_Character_Skill_Group(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Skill_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Data>>(json);
		_Player_Character_Skill_Data = raw_data_list.Select(raw_data => new Player_Character_Skill_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Skill_Onetime_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Onetime_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Onetime_Data>>(json);
		_Player_Character_Skill_Onetime_Data = raw_data_list.Select(raw_data => new Player_Character_Skill_Onetime_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Skill_Duration_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Duration_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Skill_Duration_Data>>(json);
		_Player_Character_Skill_Duration_Data = raw_data_list.Select(raw_data => new Player_Character_Skill_Duration_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Data>>(json);
		_Player_Character_Data = raw_data_list.Select(raw_data => new Player_Character_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Player_Character_Battle_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Battle_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Player_Character_Battle_Data>>(json);
		_Player_Character_Battle_Data = raw_data_list.Select(raw_data => new Player_Character_Battle_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Position_Icon_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Position_Icon_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Position_Icon_Data>>(json);
		_Position_Icon_Data = raw_data_list.Select(raw_data => new Position_Icon_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Role_Icon_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Role_Icon_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Role_Icon_Data>>(json);
		_Role_Icon_Data = raw_data_list.Select(raw_data => new Role_Icon_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_First_Reward_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/First_Reward_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_First_Reward_Data>>(json);
		_First_Reward_Data = raw_data_list.Select(raw_data => new First_Reward_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Repeat_Reward_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Repeat_Reward_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Repeat_Reward_Data>>(json);
		_Repeat_Reward_Data = raw_data_list.Select(raw_data => new Repeat_Reward_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Star_Reward_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Star_Reward_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Star_Reward_Data>>(json);
		_Star_Reward_Data = raw_data_list.Select(raw_data => new Star_Reward_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Reward_Set_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Reward_Set_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Reward_Set_Data>>(json);
		_Reward_Set_Data = raw_data_list.Select(raw_data => new Reward_Set_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_World_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/World_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_World_Data>>(json);
		_World_Data = raw_data_list.Select(raw_data => new World_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Zone_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Zone_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Zone_Data>>(json);
		_Zone_Data = raw_data_list.Select(raw_data => new Zone_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Stage_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Stage_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Stage_Data>>(json);
		_Stage_Data = raw_data_list.Select(raw_data => new Stage_Data(raw_data)).ToList();
	}

	protected async Task LoadMaster_Wave_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Wave_Data");
		var raw_data_list = JsonConvert.DeserializeObject<List<Raw_Wave_Data>>(json);
		_Wave_Data = raw_data_list.Select(raw_data => new Wave_Data(raw_data)).ToList();
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

	protected async void Check_Item_Type_Data()
	{
		if(_Item_Type_Data == null)
		{
			await LoadMaster_Item_Type_Data();
		}
	}

	protected async void Check_Character_Piece_Data()
	{
		if(_Character_Piece_Data == null)
		{
			await LoadMaster_Character_Piece_Data();
		}
	}

	protected async void Check_Exp_Potion_Data()
	{
		if(_Exp_Potion_Data == null)
		{
			await LoadMaster_Exp_Potion_Data();
		}
	}

	protected async void Check_Sta_Potion_Data()
	{
		if(_Sta_Potion_Data == null)
		{
			await LoadMaster_Sta_Potion_Data();
		}
	}

	protected async void Check_Favorite_Item_Data()
	{
		if(_Favorite_Item_Data == null)
		{
			await LoadMaster_Favorite_Item_Data();
		}
	}

	protected async void Check_Expendable_Item_Data()
	{
		if(_Expendable_Item_Data == null)
		{
			await LoadMaster_Expendable_Item_Data();
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

	protected async void Check_First_Reward_Data()
	{
		if(_First_Reward_Data == null)
		{
			await LoadMaster_First_Reward_Data();
		}
	}

	protected async void Check_Repeat_Reward_Data()
	{
		if(_Repeat_Reward_Data == null)
		{
			await LoadMaster_Repeat_Reward_Data();
		}
	}

	protected async void Check_Star_Reward_Data()
	{
		if(_Star_Reward_Data == null)
		{
			await LoadMaster_Star_Reward_Data();
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

}
