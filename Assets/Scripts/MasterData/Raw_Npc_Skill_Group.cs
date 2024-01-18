﻿

[System.Serializable]
public class Raw_Npc_Skill_Group : System.IDisposable
{
	public int npc_skill_group_id {get; set;}
	public string name_kr {get; set;}
	public double skill_use_delay {get; set;}
	public SKILL_TYPE skill_type {get; set;}
	public string icon {get; set;}
	public string action_name {get; set;}
	public string cast_effect_path {get; set;}
	public double effect_duration {get; set;}

	private bool disposed = false;

	public Raw_Npc_Skill_Group()
	{
		npc_skill_group_id = 0;
		name_kr = string.Empty;
		skill_type = SKILL_TYPE.NONE;
		icon = string.Empty;
		action_name = string.Empty;
		cast_effect_path = string.Empty;
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
