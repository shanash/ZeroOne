#nullable disable


[System.Serializable]
public class Raw_Attribute_Superiority_Data : System.IDisposable
{
	public ATTRIBUTE_TYPE attacker_attribute_type {get; set;}
	public ATTRIBUTE_TYPE defender_attribute_type {get; set;}
	public double final_damage_per {get; set;}

	private bool disposed = false;

	public Raw_Attribute_Superiority_Data()
	{
		attacker_attribute_type = ATTRIBUTE_TYPE.NONE;
		defender_attribute_type = ATTRIBUTE_TYPE.NONE;
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

