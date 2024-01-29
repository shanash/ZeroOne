#nullable disable


[System.Serializable]
public class Raw_Max_Bound_Info_Data : System.IDisposable
{
	public REWARD_TYPE reward_type {get; set;}
	public double base_max {get; set;}

	private bool disposed = false;

	public Raw_Max_Bound_Info_Data()
	{
		reward_type = REWARD_TYPE.NONE;
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

