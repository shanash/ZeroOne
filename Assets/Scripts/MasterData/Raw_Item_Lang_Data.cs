#nullable disable


[System.Serializable]
public class Raw_Item_Lang_Data : System.IDisposable
{
	public int index_id {get; set;}
	public string string_id {get; set;}
	public string kor {get; set;}
	public string eng {get; set;}
	public string jpn {get; set;}

	private bool disposed = false;

	public Raw_Item_Lang_Data()
	{
		index_id = 0;
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

