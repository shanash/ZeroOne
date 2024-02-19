#nullable disable


[System.Serializable]
public class Raw_Dungeon_Data : System.IDisposable
{
	public int dungeon_id {get; set;}
	public GAME_TYPE game_type {get; set;}
	public int dungeon_group_id {get; set;}
	public int schedule_id {get; set;}
	public int entrance_limit_count {get; set;}
	public GAME_TYPE open_game_type {get; set;}
	public int open_dungeon_id {get; set;}

	private bool disposed = false;

	public Raw_Dungeon_Data()
	{
		dungeon_id = 0;
		game_type = GAME_TYPE.NONE;
		dungeon_group_id = 0;
		schedule_id = 0;
		entrance_limit_count = 0;
		open_game_type = GAME_TYPE.NONE;
		open_dungeon_id = 0;
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

