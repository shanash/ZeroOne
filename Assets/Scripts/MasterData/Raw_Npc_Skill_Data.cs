

[System.Serializable]
public class Raw_Npc_Skill_Data : System.IDisposable
{
	public int npc_skill_id {get; set;}
	public int npc_skill_group_id {get; set;}
	public TARGET_TYPE target_type {get; set;}
	public TARGET_RULE_TYPE target_rule_type {get; set;}
	public int target_order {get; set;}
	public int target_count {get; set;}
	public double target_range {get; set;}
	public EFFECT_COUNT_TYPE effect_count_type {get; set;}
	public SECOND_TARGET_RULE_TYPE second_target_rule {get; set;}
	public int max_second_target_count {get; set;}
	public double second_target_range {get; set;}
	public int[] effect_weight {get; set;}
	public int[] onetime_effect_ids {get; set;}
	public int[] duration_effect_ids {get; set;}
	public int[] second_target_onetime_effect_ids {get; set;}
	public int[] second_target_duration_effect_ids {get; set;}
	public string event_name {get; set;}
	public string trigger_effect_path {get; set;}
	public string empty_effect_path {get; set;}

	private bool disposed = false;

	public Raw_Npc_Skill_Data()
	{
		npc_skill_id = 0;
		npc_skill_group_id = 0;
		target_type = TARGET_TYPE.MY_TEAM;
		target_rule_type = TARGET_RULE_TYPE.RANDOM;
		target_order = 0;
		target_count = 0;
		effect_count_type = EFFECT_COUNT_TYPE.NONE;
		second_target_rule = SECOND_TARGET_RULE_TYPE.NONE;
		max_second_target_count = 0;
		event_name = string.Empty;
		trigger_effect_path = string.Empty;
		empty_effect_path = string.Empty;
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

