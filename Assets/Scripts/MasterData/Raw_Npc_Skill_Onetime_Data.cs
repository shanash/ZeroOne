

[System.Serializable]
public class Raw_Npc_Skill_Onetime_Data : System.IDisposable
{
	public int npc_skill_onetime_id {get; set;}
	public ONETIME_EFFECT_TYPE onetime_effect_type {get; set;}
	public STAT_MULTIPLE_TYPE multiple_type {get; set;}
	public int value {get; set;}
	public double multiple {get; set;}
	public string effect_path {get; set;}

	private bool disposed = false;

	public Raw_Npc_Skill_Onetime_Data()
	{
		npc_skill_onetime_id = 0;
		onetime_effect_type = ONETIME_EFFECT_TYPE.NONE;
		multiple_type = STAT_MULTIPLE_TYPE.NONE;
		value = 0;
		effect_path = string.Empty;
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

