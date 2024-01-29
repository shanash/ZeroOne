#nullable disable


[System.Serializable]
public class Raw_L2d_Char_Skin_Data : System.IDisposable
{
	public int l2d_id {get; set;}
	public string l2d_skin_path {get; set;}
	public string l2d_bg_path {get; set;}
	public string l2d_intro_path {get; set;}

	private bool disposed = false;

	public Raw_L2d_Char_Skin_Data()
	{
		l2d_id = 0;
		l2d_skin_path = string.Empty;
		l2d_bg_path = string.Empty;
		l2d_intro_path = string.Empty;
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

