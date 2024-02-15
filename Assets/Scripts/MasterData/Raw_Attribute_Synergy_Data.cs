#nullable disable


[System.Serializable]
public class Raw_Attribute_Synergy_Data : System.IDisposable
{
	public ATTRIBUTE_TYPE attribute_type {get; set;}
	public int same_attribute_count {get; set;}
	public STAT_MULTIPLE_TYPE multiple_type {get; set;}
	public double add_damage_per {get; set;}

	private bool disposed = false;

	public Raw_Attribute_Synergy_Data()
	{
		attribute_type = ATTRIBUTE_TYPE.NONE;
		same_attribute_count = 0;
		multiple_type = STAT_MULTIPLE_TYPE.NONE;
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

