#nullable disable


[System.Serializable]
public class Raw_Essence_Status_Data : System.IDisposable
{
	public int essence_charge_per {get; set;}
	public int add_atk {get; set;}
	public int add_matk {get; set;}
	public int add_def {get; set;}
	public int add_mdef {get; set;}
	public int add_hp {get; set;}

	private bool disposed = false;

	public Raw_Essence_Status_Data()
	{
		essence_charge_per = 0;
		add_atk = 0;
		add_matk = 0;
		add_def = 0;
		add_mdef = 0;
		add_hp = 0;
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

