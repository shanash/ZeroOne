#nullable disable


[System.Serializable]
public class Raw_Me_Chat_Motion_Data : System.IDisposable
{
	public int chat_motion_id {get; set;}
	public int player_character_id {get; set;}
	public string animation_name {get; set;}
	public int[] serifu_ids {get; set;}

	private bool disposed = false;

	public Raw_Me_Chat_Motion_Data()
	{
		chat_motion_id = 0;
		player_character_id = 0;
		animation_name = string.Empty;
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

