using FluffyDuck.Util;
using System.Linq;

public class Npc_Level_Stat_Data : System.IDisposable
{
	///	<summary>
	///	스텟 인덱스
	///	</summary>
	public int npc_level_stat_id => _npc_level_stat_id;
	int _npc_level_stat_id;

	///	<summary>
	///	NPC Type
	///	</summary>
	public NPC_TYPE npc_type => _npc_type;
	NPC_TYPE _npc_type;

	///	<summary>
	///	종족 타입
	///	</summary>
	public TRIBE_TYPE tribe_type => _tribe_type;
	TRIBE_TYPE _tribe_type;

	///	<summary>
	///	롤 타입
	///	</summary>
	public ROLE_TYPE role_type => _role_type;
	ROLE_TYPE _role_type;

	///	<summary>
	///	공격력 증가
	///	</summary>
	public double attack_inc => _attack_inc;
	double _attack_inc;

	///	<summary>
	///	방어력 증가
	///	</summary>
	public double defend_inc => _defend_inc;
	double _defend_inc;

	///	<summary>
	///	체력 증가
	///	</summary>
	public double hp_inc => _hp_inc;
	double _hp_inc;

	///	<summary>
	///	회피 증가
	///	</summary>
	public double evation_inc => _evation_inc;
	double _evation_inc;

	///	<summary>
	///	명중 증가
	///	</summary>
	public double accuracy_inc => _accuracy_inc;
	double _accuracy_inc;

	private bool disposed = false;

	public Npc_Level_Stat_Data(Raw_Npc_Level_Stat_Data raw_data)
	{
		_npc_level_stat_id = raw_data.npc_level_stat_id;
		_npc_type = raw_data.npc_type;
		_tribe_type = raw_data.tribe_type;
		_role_type = raw_data.role_type;
		_attack_inc = raw_data.attack_inc;
		_defend_inc = raw_data.defend_inc;
		_hp_inc = raw_data.hp_inc;
		_evation_inc = raw_data.evation_inc;
		_accuracy_inc = raw_data.accuracy_inc;
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

