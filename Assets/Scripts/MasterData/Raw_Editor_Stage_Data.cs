

[System.Serializable]
public class Raw_Editor_Stage_Data : System.IDisposable
{
	public int stage_id {get; set;}
	public int stage_star_count {get; set;}
	public int stage_wave_count {get; set;}
	public int wave_group_id {get; set;}

	private bool disposed = false;

	public Raw_Editor_Stage_Data()
	{
		stage_id = 0;
		stage_star_count = 0;
		stage_wave_count = 0;
		wave_group_id = 0;
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

