[System.Serializable]
public class Npc_Level_Stat_Data : System.IDisposable
{
	///	<summary>
	///	스텟 인덱스
	///	</summary>
	public int npc_level_stat_id {get; set;}
	///	<summary>
	///	NPC Type
	///	</summary>
	public NPC_TYPE npc_type {get; set;}
	///	<summary>
	///	종족 타입
	///	</summary>
	public TRIBE_TYPE tribe_type {get; set;}
	///	<summary>
	///	롤 타입
	///	</summary>
	public ROLE_TYPE role_type {get; set;}
	///	<summary>
	///	공격력 증가
	///	</summary>
	public double attack_inc {get; set;}
	///	<summary>
	///	방어력 증가
	///	</summary>
	public double defend_inc {get; set;}
	///	<summary>
	///	체력 증가
	///	</summary>
	public double hp_inc {get; set;}
	///	<summary>
	///	회피 증가
	///	</summary>
	public double evation_inc {get; set;}
	///	<summary>
	///	명중 증가
	///	</summary>
	public double accuracy_inc {get; set;}

	private bool disposed = false;

	public Npc_Level_Stat_Data()
	{
		npc_level_stat_id = 0;
		npc_type = NPC_TYPE.NONE;
		tribe_type = TRIBE_TYPE.NONE;
		role_type = ROLE_TYPE.NONE;
		attack_inc = 0;
		defend_inc = 0;
		hp_inc = 0;
		evation_inc = 0;
		accuracy_inc = 0;
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
	public override string ToString()
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.AppendFormat("[npc_level_stat_id] = <color=yellow>{0}</color>", npc_level_stat_id).AppendLine();
		sb.AppendFormat("[npc_type] = <color=yellow>{0}</color>", npc_type).AppendLine();
		sb.AppendFormat("[tribe_type] = <color=yellow>{0}</color>", tribe_type).AppendLine();
		sb.AppendFormat("[role_type] = <color=yellow>{0}</color>", role_type).AppendLine();
		sb.AppendFormat("[attack_inc] = <color=yellow>{0}</color>", attack_inc).AppendLine();
		sb.AppendFormat("[defend_inc] = <color=yellow>{0}</color>", defend_inc).AppendLine();
		sb.AppendFormat("[hp_inc] = <color=yellow>{0}</color>", hp_inc).AppendLine();
		sb.AppendFormat("[evation_inc] = <color=yellow>{0}</color>", evation_inc).AppendLine();
		sb.AppendFormat("[accuracy_inc] = <color=yellow>{0}</color>", accuracy_inc).AppendLine();
		return sb.ToString();
	}
}

