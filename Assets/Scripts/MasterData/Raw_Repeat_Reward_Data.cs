

[System.Serializable]
public class Raw_Repeat_Reward_Data : System.IDisposable
{
	public int repeat_reward_id {get; set;}
	public int repeat_reward_group_id {get; set;}
	public ITEM_TYPE item_type {get; set;}
	public int item_id {get; set;}
	public int min_count {get; set;}
	public int max_count {get; set;}

	private bool disposed = false;

	public Raw_Repeat_Reward_Data()
	{
		repeat_reward_id = 0;
		repeat_reward_group_id = 0;
		item_type = ITEM_TYPE.NONE;
		item_id = 0;
		min_count = 0;
		max_count = 0;
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

