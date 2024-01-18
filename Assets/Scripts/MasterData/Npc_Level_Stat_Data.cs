public class Npc_Level_Stat_Data : System.IDisposable
{
	///	<summary>
	///	스텟 인덱스
	///	</summary>
	public readonly int npc_level_stat_id;
	///	<summary>
	///	NPC Type
	///	</summary>
	public readonly NPC_TYPE npc_type;
	///	<summary>
	///	종족 타입
	///	</summary>
	public readonly TRIBE_TYPE tribe_type;
	///	<summary>
	///	롤 타입
	///	</summary>
	public readonly ROLE_TYPE role_type;
	///	<summary>
	///	공격력 증가
	///	</summary>
	public readonly double attack_inc;
	///	<summary>
	///	방어력 증가
	///	</summary>
	public readonly double defend_inc;
	///	<summary>
	///	체력 증가
	///	</summary>
	public readonly double hp_inc;
	///	<summary>
	///	회피 증가
	///	</summary>
	public readonly double evation_inc;
	///	<summary>
	///	명중 증가
	///	</summary>
	public readonly double accuracy_inc;

	private bool disposed = false;

	public Npc_Level_Stat_Data(Raw_Npc_Level_Stat_Data raw_data)
	{
		npc_level_stat_id = raw_data.npc_level_stat_id;
		npc_type = raw_data.npc_type;
		tribe_type = raw_data.tribe_type;
		role_type = raw_data.role_type;
		attack_inc = raw_data.attack_inc;
		defend_inc = raw_data.defend_inc;
		hp_inc = raw_data.hp_inc;
		evation_inc = raw_data.evation_inc;
		accuracy_inc = raw_data.accuracy_inc;
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

