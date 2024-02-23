#nullable disable


[System.Serializable]
public class Raw_Player_Character_Skill_Group : System.IDisposable
{
	public int pc_skill_group_id {get; set;}
	public string name_id {get; set;}
	public string name_kr {get; set;}
	public double skill_use_delay {get; set;}
	public SKILL_TYPE skill_type {get; set;}
	public string script {get; set;}
	public int target_skill_id {get; set;}
	public string icon {get; set;}
	public string action_name {get; set;}
	public string[] cast_effect_path {get; set;}

	private bool disposed = false;

	public Raw_Player_Character_Skill_Group()
	{
		pc_skill_group_id = 0;
		name_id = string.Empty;
		name_kr = string.Empty;
		skill_type = SKILL_TYPE.NONE;
		script = string.Empty;
		target_skill_id = 0;
		icon = string.Empty;
		action_name = string.Empty;
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

