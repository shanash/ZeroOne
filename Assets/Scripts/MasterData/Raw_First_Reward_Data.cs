

[System.Serializable]
public class Raw_First_Reward_Data : System.IDisposable
{
	public int first_reward_id {get; set;}
	public int first_reward_group_id {get; set;}
	public ITEM_TYPE item_type {get; set;}
	public int item_id {get; set;}
	public string item_count {get; set;}

	private bool disposed = false;

	public Raw_First_Reward_Data()
	{
		first_reward_id = 0;
		first_reward_group_id = 0;
		item_type = ITEM_TYPE.NONE;
		item_id = 0;
		item_count = string.Empty;
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

