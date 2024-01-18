

[System.Serializable]
public class Raw_Reward_Set_Data : System.IDisposable
{
	public int reward_id {get; set;}
	public REWARD_TYPE reward_type {get; set;}
	public int var1 {get; set;}
	public int var2 {get; set;}
	public int drop_type {get; set;}
	public int drop_per {get; set;}
	public bool is_use {get; set;}
	public int sort_order {get; set;}

	private bool disposed = false;

	public Raw_Reward_Set_Data()
	{
		reward_id = 0;
		reward_type = REWARD_TYPE.NONE;
		var1 = 0;
		var2 = 0;
		drop_type = 0;
		drop_per = 0;
		is_use = false;
		sort_order = 0;
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

