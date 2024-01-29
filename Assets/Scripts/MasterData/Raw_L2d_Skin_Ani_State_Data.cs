#nullable disable


[System.Serializable]
public class Raw_L2d_Skin_Ani_State_Data : System.IDisposable
{
	public int state_id {get; set;}
	public int base_ani_id {get; set;}
	public int interaction_group_id {get; set;}

	private bool disposed = false;

	public Raw_L2d_Skin_Ani_State_Data()
	{
		state_id = 0;
		base_ani_id = 0;
		interaction_group_id = 0;
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

