#nullable disable


[System.Serializable]
public class Raw_Item_Data : System.IDisposable
{
	public int item_id {get; set;}
	public string name_id {get; set;}
	public ITEM_TYPE_V2 item_type {get; set;}
	public int max_num {get; set;}
	public int int_var1 {get; set;}
	public int int_var2 {get; set;}
	public int expire_time {get; set;}
	public int schedule_id {get; set;}
	public string icon_path {get; set;}

	private bool disposed = false;

	public Raw_Item_Data()
	{
		item_id = 0;
		name_id = string.Empty;
		item_type = ITEM_TYPE_V2.NONE;
		max_num = 0;
		int_var1 = 0;
		int_var2 = 0;
		expire_time = 0;
		schedule_id = 0;
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

