

[System.Serializable]
public class Raw_Me_State_Data : System.IDisposable
{
	public int state_id {get; set;}
	public int player_character_id {get; set;}
	public string idle_animation_name {get; set;}
	public int[] bored_chatmotion_ids {get; set;}
	public int bored_condition_count {get; set;}

	private bool disposed = false;

	public Raw_Me_State_Data()
	{
		state_id = 0;
		player_character_id = 0;
		idle_animation_name = string.Empty;
		bored_condition_count = 0;
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

