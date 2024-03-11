#nullable disable


[System.Serializable]
public class Raw_Equipment_Data : System.IDisposable
{
	public int item_id {get; set;}
	public string name_id {get; set;}
	public string desc_id {get; set;}
	public EQUIPMENT_TYPE equipment_type {get; set;}
	public int max_num {get; set;}
	public int def {get; set;}
	public int max_hp {get; set;}
	public int evasion {get; set;}
	public int recover_hp {get; set;}
	public int drain_hp {get; set;}
	public int hit {get; set;}
	public int matk {get; set;}
	public int atk {get; set;}
	public int hit_mcri {get; set;}
	public int hit_cri {get; set;}
	public int heal_up {get; set;}
	public int expire_time {get; set;}
	public int schedule_id {get; set;}
	public string icon_path {get; set;}

	private bool disposed = false;

	public Raw_Equipment_Data()
	{
		item_id = 0;
		name_id = string.Empty;
		desc_id = string.Empty;
		equipment_type = EQUIPMENT_TYPE.NONE;
		max_num = 0;
		def = 0;
		max_hp = 0;
		evasion = 0;
		recover_hp = 0;
		drain_hp = 0;
		hit = 0;
		matk = 0;
		atk = 0;
		hit_mcri = 0;
		hit_cri = 0;
		heal_up = 0;
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

