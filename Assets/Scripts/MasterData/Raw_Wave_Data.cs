#nullable disable


[System.Serializable]
public class Raw_Wave_Data : System.IDisposable
{
	public int wave_id {get; set;}
	public int wave_group_id {get; set;}
	public int wave_sequence {get; set;}
	public int enemy_appearance_count {get; set;}
	public int[] enemy_appearance_info {get; set;}
	public int[] npc_levels {get; set;}
	public int[] npc_stat_ids {get; set;}
	public int wave_time {get; set;}

	private bool disposed = false;

	public Raw_Wave_Data()
	{
		wave_id = 0;
		wave_group_id = 0;
		wave_sequence = 0;
		enemy_appearance_count = 0;
		wave_time = 0;
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

