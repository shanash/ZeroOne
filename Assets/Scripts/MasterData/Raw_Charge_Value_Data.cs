#nullable disable


[System.Serializable]
public class Raw_Charge_Value_Data : System.IDisposable
{
	public REWARD_TYPE reward_type {get; set;}
	public CHARGE_TYPE charge_type {get; set;}
	public int charge_count {get; set;}
	public REPEAT_TYPE repeat_type {get; set;}
	public int repeat_time {get; set;}
	public int schedule_id {get; set;}

	private bool disposed = false;

	public Raw_Charge_Value_Data()
	{
		reward_type = REWARD_TYPE.NONE;
		charge_type = CHARGE_TYPE.NONE;
		charge_count = 0;
		repeat_type = REPEAT_TYPE.NONE;
		repeat_time = 0;
		schedule_id = 0;
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

