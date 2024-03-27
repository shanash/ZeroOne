#nullable disable


[System.Serializable]
public class Raw_Stage_Data : System.IDisposable
{
	public int stage_id {get; set;}
	public int stage_group_id {get; set;}
	public int wave_group_id {get; set;}
	public int stage_ordering {get; set;}
	public string stage_name_id {get; set;}
	public string stage_name {get; set;}
	public int use_stamina {get; set;}
	public int character_exp {get; set;}
	public int destiny_exp {get; set;}
	public int gold {get; set;}
	public int repeat_reward_group_id {get; set;}
	public int first_reward_group_id {get; set;}
	public int star_reward_group_id {get; set;}
	public int schedule_id {get; set;}
	public int entrance_limit_count {get; set;}
	public REWARD_TYPE reward_type {get; set;}
	public int reward_id {get; set;}
	public string entrance_dialogue {get; set;}
	public string outrance_dialogue {get; set;}
	public string background_image_path {get; set;}
	public string bgm_path {get; set;}

	private bool disposed = false;

	public Raw_Stage_Data()
	{
		stage_id = 0;
		stage_group_id = 0;
		wave_group_id = 0;
		stage_ordering = 0;
		stage_name_id = string.Empty;
		stage_name = string.Empty;
		use_stamina = 0;
		character_exp = 0;
		destiny_exp = 0;
		gold = 0;
		repeat_reward_group_id = 0;
		first_reward_group_id = 0;
		star_reward_group_id = 0;
		schedule_id = 0;
		entrance_limit_count = 0;
		reward_type = REWARD_TYPE.NONE;
		reward_id = 0;
		entrance_dialogue = string.Empty;
		outrance_dialogue = string.Empty;
		background_image_path = string.Empty;
		bgm_path = string.Empty;
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

