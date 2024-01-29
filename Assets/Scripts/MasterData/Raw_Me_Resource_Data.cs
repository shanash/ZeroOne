#nullable disable


[System.Serializable]
public class Raw_Me_Resource_Data : System.IDisposable
{
	public int memorial_id {get; set;}
	public int player_character_id {get; set;}
	public string actor_prefab_key {get; set;}
	public string background_prefab_key {get; set;}
	public string prefab_key {get; set;}
	public string intro_key {get; set;}
	public int state_id {get; set;}
	public int order {get; set;}

	private bool disposed = false;

	public Raw_Me_Resource_Data()
	{
		memorial_id = 0;
		player_character_id = 0;
		actor_prefab_key = string.Empty;
		background_prefab_key = string.Empty;
		prefab_key = string.Empty;
		intro_key = string.Empty;
		state_id = 0;
		order = 0;
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

