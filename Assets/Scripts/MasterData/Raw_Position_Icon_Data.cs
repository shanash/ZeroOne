

[System.Serializable]
public class Raw_Position_Icon_Data : System.IDisposable
{
	public POSITION_TYPE position_type {get; set;}
	public string name_kr {get; set;}
	public string icon {get; set;}

	private bool disposed = false;

	public Raw_Position_Icon_Data()
	{
		position_type = POSITION_TYPE.NONE;
		name_kr = string.Empty;
		icon = string.Empty;
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

