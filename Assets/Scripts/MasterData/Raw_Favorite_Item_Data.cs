

[System.Serializable]
public class Raw_Favorite_Item_Data : System.IDisposable
{
	public int favorite_item_id {get; set;}
	public string item_name_kr {get; set;}
	public int use_effect {get; set;}
	public string item_tooltip_text {get; set;}
	public int sell_price {get; set;}
	public string icon_path {get; set;}

	private bool disposed = false;

	public Raw_Favorite_Item_Data()
	{
		favorite_item_id = 0;
		item_name_kr = string.Empty;
		use_effect = 0;
		item_tooltip_text = string.Empty;
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

