#nullable disable


[System.Serializable]
public class Raw_Npc_Data : System.IDisposable
{
	public int npc_data_id {get; set;}
	public string name_id {get; set;}
	public string name_kr {get; set;}
	public string desc_id {get; set;}
	public TRIBE_TYPE tribe_type {get; set;}
	public NPC_TYPE npc_type {get; set;}
	public ATTRIBUTE_TYPE attribute_type {get; set;}
	public int npc_battle_id {get; set;}
	public string prefab_path {get; set;}
	public string icon_path {get; set;}
	public double scale {get; set;}

	private bool disposed = false;

	public Raw_Npc_Data()
	{
		npc_data_id = 0;
		name_id = string.Empty;
		name_kr = string.Empty;
		desc_id = string.Empty;
		tribe_type = TRIBE_TYPE.NONE;
		npc_type = NPC_TYPE.NONE;
		attribute_type = ATTRIBUTE_TYPE.NONE;
		npc_battle_id = 0;
		prefab_path = string.Empty;
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

