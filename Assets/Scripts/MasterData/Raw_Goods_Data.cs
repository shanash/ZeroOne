#nullable disable


[System.Serializable]
public class Raw_Goods_Data : System.IDisposable
{
	public GOODS_TYPE goods_type {get; set;}
	public string name_id {get; set;}
	public string desc_id {get; set;}
	public double max_bound {get; set;}
	public string icon_path {get; set;}

	private bool disposed = false;

	public Raw_Goods_Data()
	{
		goods_type = GOODS_TYPE.NONE;
		name_id = string.Empty;
		desc_id = string.Empty;
		icon_path = string.Empty;
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

