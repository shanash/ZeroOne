#nullable disable


[System.Serializable]
public class Raw_Boss_Stage_Data : System.IDisposable
{
	public int boss_stage_id {get; set;}
	public int boss_stage_group_id {get; set;}
	public int wave_group_id {get; set;}
	public int stage_ordering {get; set;}
	public string stage_name {get; set;}
	public int character_exp {get; set;}
	public int destiny_exp {get; set;}
	public int gold {get; set;}
	public int repeat_reward_group_id {get; set;}
	public int first_reward_group_id {get; set;}
	public int recomment_level {get; set;}
	public string bgm_path {get; set;}

	private bool disposed = false;

	public Raw_Boss_Stage_Data()
	{
		boss_stage_id = 0;
		boss_stage_group_id = 0;
		wave_group_id = 0;
		stage_ordering = 0;
		stage_name = string.Empty;
		character_exp = 0;
		destiny_exp = 0;
		gold = 0;
		repeat_reward_group_id = 0;
		first_reward_group_id = 0;
		recomment_level = 0;
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

