#nullable disable


[System.Serializable]
public class Raw_Role_Icon_Data : System.IDisposable
{
	public ROLE_TYPE role_type {get; set;}
	public string role_name_id {get; set;}
	public string name_kr {get; set;}
	public string icon {get; set;}
	public string card_icon {get; set;}
	public string tag_bg_path {get; set;}

	private bool disposed = false;

	public Raw_Role_Icon_Data()
	{
		role_type = ROLE_TYPE.NONE;
		role_name_id = string.Empty;
		name_kr = string.Empty;
		icon = string.Empty;
		card_icon = string.Empty;
		tag_bg_path = string.Empty;
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

