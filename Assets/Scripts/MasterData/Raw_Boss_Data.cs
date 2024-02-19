#nullable disable


[System.Serializable]
public class Raw_Boss_Data : System.IDisposable
{
	public int boss_id {get; set;}
	public int boss_group_id {get; set;}
	public string boss_name {get; set;}
	public int boss_stage_group_id {get; set;}
	public string boss_story_info {get; set;}
	public string boss_skill_info {get; set;}

	private bool disposed = false;

	public Raw_Boss_Data()
	{
		boss_id = 0;
		boss_group_id = 0;
		boss_name = string.Empty;
		boss_stage_group_id = 0;
		boss_story_info = string.Empty;
		boss_skill_info = string.Empty;
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

