#nullable disable


[System.Serializable]
public class Raw_Me_Serifu_Data : System.IDisposable
{
	public int serifu_id {get; set;}
	public int player_character_id {get; set;}
	public string text_kr {get; set;}
	public string audio_clip_key {get; set;}

	private bool disposed = false;

	public Raw_Me_Serifu_Data()
	{
		serifu_id = 0;
		player_character_id = 0;
		text_kr = string.Empty;
		audio_clip_key = string.Empty;
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

