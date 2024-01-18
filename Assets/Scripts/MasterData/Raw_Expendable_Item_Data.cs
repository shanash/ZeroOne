

[System.Serializable]
public class Raw_Expendable_Item_Data : System.IDisposable
{
	public int expendable_item_id {get; set;}
	public string name_kr {get; set;}
	public string tooltip_text {get; set;}
	public int sell_price {get; set;}
	public string icon_path {get; set;}

	private bool disposed = false;

	public Raw_Expendable_Item_Data()
	{
		expendable_item_id = 0;
		name_kr = string.Empty;
		tooltip_text = string.Empty;
		sell_price = 0;
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

