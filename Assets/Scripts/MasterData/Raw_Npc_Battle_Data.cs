#nullable disable


[System.Serializable]
public class Raw_Npc_Battle_Data : System.IDisposable
{
	public int npc_battle_id {get; set;}
	public double approach {get; set;}
	public POSITION_TYPE position_type {get; set;}
	public int[] skill_pattern {get; set;}
	public int passive_skill_group_id {get; set;}
	public int special_skill_group_id {get; set;}
	public double hp {get; set;}
	public double p_attack {get; set;}
	public double m_attack {get; set;}
	public double p_defend {get; set;}
	public double m_defend {get; set;}
	public double p_cri_rate {get; set;}
	public double m_cri_rate {get; set;}
	public double p_cri_power {get; set;}
	public double m_cri_power {get; set;}
	public double attack_recovery {get; set;}
	public double evasion {get; set;}
	public double accuracy {get; set;}
	public double heal {get; set;}
	public double resist {get; set;}
	public double weight {get; set;}
	public double move_speed {get; set;}
	public string attack_script {get; set;}

	private bool disposed = false;

	public Raw_Npc_Battle_Data()
	{
		npc_battle_id = 0;
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

