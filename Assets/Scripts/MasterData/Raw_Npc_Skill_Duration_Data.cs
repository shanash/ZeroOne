#nullable disable


[System.Serializable]
public class Raw_Npc_Skill_Duration_Data : System.IDisposable
{
	public int npc_skill_duration_id {get; set;}
	public DURATION_EFFECT_TYPE duration_effect_type {get; set;}
	public PERSISTENCE_TYPE persistence_type {get; set;}
	public double time {get; set;}
	public int count {get; set;}
	public int[] repeat_npc_onetime_ids {get; set;}
	public double repeat_interval {get; set;}
	public int[] finish_npc_onetime_ids {get; set;}
	public STAT_MULTIPLE_TYPE multiple_type {get; set;}
	public int value {get; set;}
	public double multiple {get; set;}
	public double rate {get; set;}
	public string effect_path {get; set;}
	public bool is_overlapable {get; set;}

	private bool disposed = false;

	public Raw_Npc_Skill_Duration_Data()
	{
		npc_skill_duration_id = 0;
		duration_effect_type = DURATION_EFFECT_TYPE.NONE;
		persistence_type = PERSISTENCE_TYPE.NONE;
		count = 0;
		multiple_type = STAT_MULTIPLE_TYPE.NONE;
		value = 0;
		effect_path = string.Empty;
		is_overlapable = false;
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

