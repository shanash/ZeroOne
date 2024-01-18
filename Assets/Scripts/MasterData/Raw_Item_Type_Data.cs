

[System.Serializable]
public class Raw_Item_Type_Data : System.IDisposable
{
	public ITEM_TYPE item_type {get; set;}
	public string name_kr {get; set;}
	public string tooltip_text {get; set;}
	public bool sellable {get; set;}
	public double max_bounds {get; set;}
	public string icon_path {get; set;}

	private bool disposed = false;

	public Raw_Item_Type_Data()
	{
		item_type = ITEM_TYPE.NONE;
		name_kr = string.Empty;
		tooltip_text = string.Empty;
		sellable = false;
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

