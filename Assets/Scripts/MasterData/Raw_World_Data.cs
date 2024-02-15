#nullable disable


[System.Serializable]
public class Raw_World_Data : System.IDisposable
{
	public int world_id {get; set;}
	public string world_name {get; set;}
	public int zone_group_id {get; set;}

	private bool disposed = false;

	public Raw_World_Data()
	{
		world_id = 0;
		world_name = string.Empty;
		zone_group_id = 0;
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

