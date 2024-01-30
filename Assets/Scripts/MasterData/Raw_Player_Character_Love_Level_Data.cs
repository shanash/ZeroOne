#nullable disable


[System.Serializable]
public class Raw_Player_Character_Love_Level_Data : System.IDisposable
{
	public int level {get; set;}
	public double accum_exp {get; set;}
	public double need_exp {get; set;}
	public LOVE_LEVEL_TYPE love_level_type {get; set;}

	private bool disposed = false;

	public Raw_Player_Character_Love_Level_Data()
	{
		level = 0;
		love_level_type = LOVE_LEVEL_TYPE.NONE;
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

