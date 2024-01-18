

[System.Serializable]
public class Raw_Character_Piece_Data : System.IDisposable
{
	public int player_character_id {get; set;}
	public string name_kr {get; set;}
	public string tooltip_text {get; set;}
	public int sell_price {get; set;}
	public int make_need {get; set;}
	public string icon_path {get; set;}

	private bool disposed = false;

	public Raw_Character_Piece_Data()
	{
		player_character_id = 0;
		name_kr = string.Empty;
		tooltip_text = string.Empty;
		sell_price = 0;
		make_need = 0;
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

