#nullable disable


[System.Serializable]
public class Raw_Schedule_Data : System.IDisposable
{
	public int schedule_id {get; set;}
	public string date_start {get; set;}
	public string date_end {get; set;}
	public string time_open {get; set;}
	public string time_close {get; set;}
	public int day_of_week {get; set;}

	private bool disposed = false;

	public Raw_Schedule_Data()
	{
		schedule_id = 0;
		date_start = string.Empty;
		date_end = string.Empty;
		time_open = string.Empty;
		time_close = string.Empty;
		day_of_week = 0;
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

