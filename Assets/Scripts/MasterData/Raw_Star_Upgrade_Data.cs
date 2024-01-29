#nullable disable


[System.Serializable]
public class Raw_Star_Upgrade_Data : System.IDisposable
{
	public int current_star_grade {get; set;}
	public int need_char_piece {get; set;}
	public int need_gold {get; set;}

	private bool disposed = false;

	public Raw_Star_Upgrade_Data()
	{
		current_star_grade = 0;
		need_char_piece = 0;
		need_gold = 0;
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

