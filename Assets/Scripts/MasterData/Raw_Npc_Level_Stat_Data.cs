#nullable disable


[System.Serializable]
public class Raw_Npc_Level_Stat_Data : System.IDisposable
{
	public int npc_level_stat_id {get; set;}
	public double life {get; set;}
	public double physics_attack {get; set;}
	public double magic_attack {get; set;}
	public double physics_defend {get; set;}
	public double magic_defend {get; set;}
	public double physics_critical_chance {get; set;}
	public double magic_critical_chance {get; set;}
	public double physics_critical_power_add {get; set;}
	public double magic_critical_power_add {get; set;}
	public double attack_life_recovery {get; set;}
	public double evasion {get; set;}
	public double accuracy {get; set;}
	public double heal {get; set;}
	public double resist  {get; set;}
	public double weight {get; set;}
	public double auto_recovery {get; set;}
	public double move_speed {get; set;}

	private bool disposed = false;

	public Raw_Npc_Level_Stat_Data()
	{
		npc_level_stat_id = 0;
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

