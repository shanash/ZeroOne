#nullable disable


[System.Serializable]
public class Raw_Zone_Data : System.IDisposable
{
	public int zone_id {get; set;}
	public string zone_name {get; set;}
	public int zone_group_id {get; set;}
	public int stage_group_id {get; set;}
	public int zone_ordering {get; set;}
	public STAGE_DIFFICULTY_TYPE zone_difficulty {get; set;}
	public string zone_img_path {get; set;}
	public string zone_tooltip {get; set;}
	public LIMIT_TYPE limit_type {get; set;}

	private bool disposed = false;

	public Raw_Zone_Data()
	{
		zone_id = 0;
		zone_name = string.Empty;
		zone_group_id = 0;
		stage_group_id = 0;
		zone_ordering = 0;
		zone_difficulty = STAGE_DIFFICULTY_TYPE.NONE;
		zone_img_path = string.Empty;
		zone_tooltip = string.Empty;
		limit_type = LIMIT_TYPE.NONE;
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

