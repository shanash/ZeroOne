

[System.Serializable]
public class Raw_Item_Piece_Data : System.IDisposable
{
	public int item_piece_id {get; set;}
	public string name_id {get; set;}
	public int target_id {get; set;}
	public int max_num {get; set;}
	public int make_count {get; set;}
	public int expire_time {get; set;}
	public int expire_schedule_id {get; set;}
	public string icon_path {get; set;}

	private bool disposed = false;

	public Raw_Item_Piece_Data()
	{
		item_piece_id = 0;
		name_id = string.Empty;
		target_id = 0;
		max_num = 0;
		make_count = 0;
		expire_time = 0;
		expire_schedule_id = 0;
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

