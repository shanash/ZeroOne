#nullable disable


[System.Serializable]
public class Raw_Player_Character_Data : System.IDisposable
{
	public int player_character_id {get; set;}
	public string name_id {get; set;}
	public int default_star {get; set;}
	public ROLE_TYPE role_type {get; set;}
	public TRIBE_TYPE tribe_type {get; set;}
	public ATTRIBUTE_TYPE attribute_type {get; set;}
	public int profile_age {get; set;}
	public int[] profile_birthday {get; set;}
	public int profile_high {get; set;}
	public string profile_habby {get; set;}
	public int battle_info_id {get; set;}
	public string prefab_path {get; set;}
	public string sd_prefab_path {get; set;}
	public int lobby_basic_id {get; set;}
	public int essence_id {get; set;}
	public int lobby_merry_id {get; set;}
	public string icon_path {get; set;}
	public string Illustration_path {get; set;}
	public string script {get; set;}
	public double scale {get; set;}
	public bool first_open_check {get; set;}

	private bool disposed = false;

	public Raw_Player_Character_Data()
	{
		player_character_id = 0;
		name_id = string.Empty;
		default_star = 0;
		role_type = ROLE_TYPE.NONE;
		tribe_type = TRIBE_TYPE.NONE;
		attribute_type = ATTRIBUTE_TYPE.NONE;
		profile_age = 0;
		profile_high = 0;
		profile_habby = string.Empty;
		battle_info_id = 0;
		prefab_path = string.Empty;
		sd_prefab_path = string.Empty;
		lobby_basic_id = 0;
		essence_id = 0;
		lobby_merry_id = 0;
		icon_path = string.Empty;
		Illustration_path = string.Empty;
		script = string.Empty;
		first_open_check = false;
	}

	public void Dispose()
	{
		Dispose(true);
		System.GC.SuppressFinalize(this);
	}
	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				// todo dispose resouces
			}
			disposed = true;
		}
	}
}

