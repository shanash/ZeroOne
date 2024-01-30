#nullable disable


[System.Serializable]
public class Raw_L2d_Love_State_Data : System.IDisposable
{
	public int id {get; set;}
	public int l2d_id {get; set;}
	public LOVE_LEVEL_TYPE love_level_type {get; set;}
	public int state_id {get; set;}

	private bool disposed = false;

	public Raw_L2d_Love_State_Data()
	{
		id = 0;
		l2d_id = 0;
		love_level_type = LOVE_LEVEL_TYPE.NONE;
		state_id = 0;
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

