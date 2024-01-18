

[System.Serializable]
public class Raw_Star_Reward_Data : System.IDisposable
{
	public int star_reward_id {get; set;}
	public int star_reward_group_id {get; set;}
	public int star_point {get; set;}
	public ITEM_TYPE item_type {get; set;}
	public int item_id {get; set;}
	public int item_count {get; set;}

	private bool disposed = false;

	public Raw_Star_Reward_Data()
	{
		star_reward_id = 0;
		star_reward_group_id = 0;
		star_point = 0;
		item_type = ITEM_TYPE.NONE;
		item_id = 0;
		item_count = 0;
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

