

[System.Serializable]
public class Raw_World_Data : System.IDisposable
{
	public int world_id {get; set;}
	public string world_name {get; set;}

	private bool disposed = false;

	public Raw_World_Data()
	{
		world_id = 0;
		world_name = string.Empty;
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

