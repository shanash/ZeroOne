#nullable disable


[System.Serializable]
public class Raw_Npc_Level_Stat_Data : System.IDisposable
{
	public int npc_level_stat_id {get; set;}
	public NPC_TYPE npc_type {get; set;}
	public TRIBE_TYPE tribe_type {get; set;}
	public ROLE_TYPE role_type {get; set;}
	public double attack_inc {get; set;}
	public double defend_inc {get; set;}
	public double hp_inc {get; set;}
	public double evation_inc {get; set;}
	public double accuracy_inc {get; set;}

	private bool disposed = false;

	public Raw_Npc_Level_Stat_Data()
	{
		npc_level_stat_id = 0;
		npc_type = NPC_TYPE.NONE;
		tribe_type = TRIBE_TYPE.NONE;
		role_type = ROLE_TYPE.NONE;
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

