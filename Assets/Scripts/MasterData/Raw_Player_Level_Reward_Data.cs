#nullable disable


[System.Serializable]
public class Raw_Player_Level_Reward_Data : System.IDisposable
{
	public int player_level {get; set;}
	public int increase_stamina {get; set;}

	private bool disposed = false;

	public Raw_Player_Level_Reward_Data()
	{
		player_level = 0;
		increase_stamina = 0;
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

