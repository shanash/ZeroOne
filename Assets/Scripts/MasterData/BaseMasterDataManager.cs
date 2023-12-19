using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json;

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
	protected List<Memoritem_Data> _Memoritem_Data
	{
		get;
		private set;
	}
	protected List<Expendable_Item_Data> _Expendable_Item_Data
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
		await LoadMaster_Memoritem_Data();
		await LoadMaster_Expendable_Item_Data();
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
		await LoadMaster_Player_Character_Skill_Group();
		await LoadMaster_Player_Character_Skill_Data();
		await LoadMaster_Player_Character_Skill_Onetime_Data();
		await LoadMaster_Player_Character_Skill_Duration_Data();
		await LoadMaster_Player_Character_Data();
		await LoadMaster_Player_Character_Battle_Data();
		await LoadMaster_Position_Icon_Data();
		await LoadMaster_Role_Icon_Data();
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
		_Editor_Stage_Data = JsonConvert.DeserializeObject<List<Editor_Stage_Data>>(json);
	}

	protected async Task LoadMaster_Editor_Wave_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Editor_Wave_Data");
		_Editor_Wave_Data = JsonConvert.DeserializeObject<List<Editor_Wave_Data>>(json);
	}

	protected async Task LoadMaster_Item_Type_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Item_Type_Data");
		_Item_Type_Data = JsonConvert.DeserializeObject<List<Item_Type_Data>>(json);
	}

	protected async Task LoadMaster_Character_Piece_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Character_Piece_Data");
		_Character_Piece_Data = JsonConvert.DeserializeObject<List<Character_Piece_Data>>(json);
	}

	protected async Task LoadMaster_Exp_Potion_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Exp_Potion_Data");
		_Exp_Potion_Data = JsonConvert.DeserializeObject<List<Exp_Potion_Data>>(json);
	}

	protected async Task LoadMaster_Sta_Potion_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Sta_Potion_Data");
		_Sta_Potion_Data = JsonConvert.DeserializeObject<List<Sta_Potion_Data>>(json);
	}

	protected async Task LoadMaster_Memoritem_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Memoritem_Data");
		_Memoritem_Data = JsonConvert.DeserializeObject<List<Memoritem_Data>>(json);
	}

	protected async Task LoadMaster_Expendable_Item_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Expendable_Item_Data");
		_Expendable_Item_Data = JsonConvert.DeserializeObject<List<Expendable_Item_Data>>(json);
	}

	protected async Task LoadMaster_Me_Resource_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Resource_Data");
		_Me_Resource_Data = JsonConvert.DeserializeObject<List<Me_Resource_Data>>(json);
	}

	protected async Task LoadMaster_Me_State_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_State_Data");
		_Me_State_Data = JsonConvert.DeserializeObject<List<Me_State_Data>>(json);
	}

	protected async Task LoadMaster_Me_Interaction_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Interaction_Data");
		_Me_Interaction_Data = JsonConvert.DeserializeObject<List<Me_Interaction_Data>>(json);
	}

	protected async Task LoadMaster_Me_Chat_Motion_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Chat_Motion_Data");
		_Me_Chat_Motion_Data = JsonConvert.DeserializeObject<List<Me_Chat_Motion_Data>>(json);
	}

	protected async Task LoadMaster_Me_Serifu_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Me_Serifu_Data");
		_Me_Serifu_Data = JsonConvert.DeserializeObject<List<Me_Serifu_Data>>(json);
	}

	protected async Task LoadMaster_Npc_Skill_Group()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Group");
		_Npc_Skill_Group = JsonConvert.DeserializeObject<List<Npc_Skill_Group>>(json);
	}

	protected async Task LoadMaster_Npc_Skill_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Data");
		_Npc_Skill_Data = JsonConvert.DeserializeObject<List<Npc_Skill_Data>>(json);
	}

	protected async Task LoadMaster_Npc_Skill_Onetime_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Onetime_Data");
		_Npc_Skill_Onetime_Data = JsonConvert.DeserializeObject<List<Npc_Skill_Onetime_Data>>(json);
	}

	protected async Task LoadMaster_Npc_Skill_Duration_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Skill_Duration_Data");
		_Npc_Skill_Duration_Data = JsonConvert.DeserializeObject<List<Npc_Skill_Duration_Data>>(json);
	}

	protected async Task LoadMaster_Npc_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Data");
		_Npc_Data = JsonConvert.DeserializeObject<List<Npc_Data>>(json);
	}

	protected async Task LoadMaster_Npc_Battle_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Npc_Battle_Data");
		_Npc_Battle_Data = JsonConvert.DeserializeObject<List<Npc_Battle_Data>>(json);
	}

	protected async Task LoadMaster_Player_Character_Skill_Group()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Group");
		_Player_Character_Skill_Group = JsonConvert.DeserializeObject<List<Player_Character_Skill_Group>>(json);
	}

	protected async Task LoadMaster_Player_Character_Skill_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Data");
		_Player_Character_Skill_Data = JsonConvert.DeserializeObject<List<Player_Character_Skill_Data>>(json);
	}

	protected async Task LoadMaster_Player_Character_Skill_Onetime_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Onetime_Data");
		_Player_Character_Skill_Onetime_Data = JsonConvert.DeserializeObject<List<Player_Character_Skill_Onetime_Data>>(json);
	}

	protected async Task LoadMaster_Player_Character_Skill_Duration_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Skill_Duration_Data");
		_Player_Character_Skill_Duration_Data = JsonConvert.DeserializeObject<List<Player_Character_Skill_Duration_Data>>(json);
	}

	protected async Task LoadMaster_Player_Character_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Data");
		_Player_Character_Data = JsonConvert.DeserializeObject<List<Player_Character_Data>>(json);
	}

	protected async Task LoadMaster_Player_Character_Battle_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Player_Character_Battle_Data");
		_Player_Character_Battle_Data = JsonConvert.DeserializeObject<List<Player_Character_Battle_Data>>(json);
	}

	protected async Task LoadMaster_Position_Icon_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Position_Icon_Data");
		_Position_Icon_Data = JsonConvert.DeserializeObject<List<Position_Icon_Data>>(json);
	}

	protected async Task LoadMaster_Role_Icon_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Role_Icon_Data");
		_Role_Icon_Data = JsonConvert.DeserializeObject<List<Role_Icon_Data>>(json);
	}

	protected async Task LoadMaster_Stage_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Stage_Data");
		_Stage_Data = JsonConvert.DeserializeObject<List<Stage_Data>>(json);
	}

	protected async Task LoadMaster_Wave_Data()
	{
		string json = await LoadJsonDataAsync("Assets/AssetResources/Master/Wave_Data");
		_Wave_Data = JsonConvert.DeserializeObject<List<Wave_Data>>(json);
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

	protected async void Check_Memoritem_Data()
	{
		if(_Memoritem_Data == null)
		{
			await LoadMaster_Memoritem_Data();
		}
	}

	protected async void Check_Expendable_Item_Data()
	{
		if(_Expendable_Item_Data == null)
		{
			await LoadMaster_Expendable_Item_Data();
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
