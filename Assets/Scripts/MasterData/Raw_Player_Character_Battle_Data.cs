#nullable disable


[System.Serializable]
public class Raw_Player_Character_Battle_Data : System.IDisposable
{
	public int battle_info_id {get; set;}
	public int star_grade {get; set;}
	public double approach {get; set;}
	public POSITION_TYPE position_type {get; set;}
	public int[] skill_pattern {get; set;}
	public int passive_skill_group_id {get; set;}
	public int special_skill_group_id {get; set;}
	public double life {get; set;}
	public double physics_attack {get; set;}
	public double magic_attack {get; set;}
	public double physics_defend {get; set;}
	public double magic_defend {get; set;}
	public double physics_critical_chance {get; set;}
	public double magic_critical_chance {get; set;}
	public double physics_critical_power_add {get; set;}
	public double magic_critical_power_add {get; set;}
	public double attack_life_recovery {get; set;}
	public double evasion {get; set;}
	public double accuracy {get; set;}
	public double heal {get; set;}
	public double resist  {get; set;}
	public double weight {get; set;}
	public double auto_recovery {get; set;}
	public double move_speed {get; set;}
	public string attack_script {get; set;}

	private bool disposed = false;

	public Raw_Player_Character_Battle_Data()
	{
		battle_info_id = 0;
		star_grade = 0;
		position_type = POSITION_TYPE.NONE;
		passive_skill_group_id = 0;
		special_skill_group_id = 0;
		attack_script = string.Empty;
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

