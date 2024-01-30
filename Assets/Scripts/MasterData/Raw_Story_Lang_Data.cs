#nullable disable


[System.Serializable]
public class Raw_Story_Lang_Data : System.IDisposable
{
	public string string_id {get; set;}
	public string kor {get; set;}
	public string eng {get; set;}
	public string jpn {get; set;}

	private bool disposed = false;

	public Raw_Story_Lang_Data()
	{
		string_id = string.Empty;
		kor = string.Empty;
		eng = string.Empty;
		jpn = string.Empty;
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

