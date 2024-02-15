#nullable disable


[System.Serializable]
public class Raw_Stage_Data : System.IDisposable
{
	public int stage_id {get; set;}
	public int stage_group_id {get; set;}
	public int wave_group_id {get; set;}
	public int stage_ordering {get; set;}
	public string stage_name {get; set;}
	public int use_stamina {get; set;}
	public int character_exp {get; set;}
	public int destiny_exp {get; set;}
	public int gold {get; set;}
	public int repeat_reward_group_id {get; set;}
	public int first_reward_group_id {get; set;}
	public int star_reward_group_id {get; set;}

	private bool disposed = false;

	public Raw_Stage_Data()
	{
		stage_id = 0;
		stage_group_id = 0;
		wave_group_id = 0;
		stage_ordering = 0;
		stage_name = string.Empty;
		use_stamina = 0;
		character_exp = 0;
		destiny_exp = 0;
		gold = 0;
		repeat_reward_group_id = 0;
		first_reward_group_id = 0;
		star_reward_group_id = 0;
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

