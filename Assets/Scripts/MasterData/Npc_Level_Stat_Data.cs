#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Npc_Level_Stat_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	스텟 인덱스
	///	</summary>
	public int npc_level_stat_id => _npc_level_stat_id;
	int _npc_level_stat_id;

	///	<summary>
	///	체력 증가
	///	</summary>
	public double life => _life;
	double _life;

	///	<summary>
	///	물리 공격력 증가
	///	</summary>
	public double physics_attack => _physics_attack;
	double _physics_attack;

	///	<summary>
	///	마법 공격력 증가
	///	</summary>
	public double magic_attack => _magic_attack;
	double _magic_attack;

	///	<summary>
	///	물리 방어력 증가
	///	</summary>
	public double physics_defend => _physics_defend;
	double _physics_defend;

	///	<summary>
	///	마법 방어력 증가
	///	</summary>
	public double magic_defend => _magic_defend;
	double _magic_defend;

	///	<summary>
	///	물리 크리티컬 확률
	///	</summary>
	public double physics_critical_chance => _physics_critical_chance;
	double _physics_critical_chance;

	///	<summary>
	///	마법 크리티컬 확률
	///	</summary>
	public double magic_critical_chance => _magic_critical_chance;
	double _magic_critical_chance;

	///	<summary>
	///	물리 크리티컬 추가 대미지
	///	</summary>
	public double physics_critical_power_add => _physics_critical_power_add;
	double _physics_critical_power_add;

	///	<summary>
	///	마법 크리티컬 추가 대미지
	///	</summary>
	public double magic_critical_power_add => _magic_critical_power_add;
	double _magic_critical_power_add;

	///	<summary>
	///	타격 시 회복량
	///	</summary>
	public double attack_life_recovery => _attack_life_recovery;
	double _attack_life_recovery;

	///	<summary>
	///	회피 증가
	///	</summary>
	public double evasion => _evasion;
	double _evasion;

	///	<summary>
	///	명중 증가
	///	</summary>
	public double accuracy => _accuracy;
	double _accuracy;

	///	<summary>
	///	힐량
	///	</summary>
	public double heal => _heal;
	double _heal;

	///	<summary>
	///	강인함
	///	</summary>
	public double resist  => _resist ;
	double _resist ;

	///	<summary>
	///	무게
	///	</summary>
	public double weight => _weight;
	double _weight;

	///	<summary>
	///	자동 회복
	///	</summary>
	public double auto_recovery => _auto_recovery;
	double _auto_recovery;

	///	<summary>
	///	전투 이동 속도
	///	</summary>
	public double move_speed => _move_speed;
	double _move_speed;

	private bool disposed = false;

	public Npc_Level_Stat_Data(Raw_Npc_Level_Stat_Data raw_data)
	{
		_npc_level_stat_id = raw_data.npc_level_stat_id;
		_life = raw_data.life;
		_physics_attack = raw_data.physics_attack;
		_magic_attack = raw_data.magic_attack;
		_physics_defend = raw_data.physics_defend;
		_magic_defend = raw_data.magic_defend;
		_physics_critical_chance = raw_data.physics_critical_chance;
		_magic_critical_chance = raw_data.magic_critical_chance;
		_physics_critical_power_add = raw_data.physics_critical_power_add;
		_magic_critical_power_add = raw_data.magic_critical_power_add;
		_attack_life_recovery = raw_data.attack_life_recovery;
		_evasion = raw_data.evasion;
		_accuracy = raw_data.accuracy;
		_heal = raw_data.heal;
		_resist  = raw_data.resist ;
		_weight = raw_data.weight;
		_auto_recovery = raw_data.auto_recovery;
		_move_speed = raw_data.move_speed;
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
		sb.AppendFormat("[life] = <color=yellow>{0}</color>", life).AppendLine();
		sb.AppendFormat("[physics_attack] = <color=yellow>{0}</color>", physics_attack).AppendLine();
		sb.AppendFormat("[magic_attack] = <color=yellow>{0}</color>", magic_attack).AppendLine();
		sb.AppendFormat("[physics_defend] = <color=yellow>{0}</color>", physics_defend).AppendLine();
		sb.AppendFormat("[magic_defend] = <color=yellow>{0}</color>", magic_defend).AppendLine();
		sb.AppendFormat("[physics_critical_chance] = <color=yellow>{0}</color>", physics_critical_chance).AppendLine();
		sb.AppendFormat("[magic_critical_chance] = <color=yellow>{0}</color>", magic_critical_chance).AppendLine();
		sb.AppendFormat("[physics_critical_power_add] = <color=yellow>{0}</color>", physics_critical_power_add).AppendLine();
		sb.AppendFormat("[magic_critical_power_add] = <color=yellow>{0}</color>", magic_critical_power_add).AppendLine();
		sb.AppendFormat("[attack_life_recovery] = <color=yellow>{0}</color>", attack_life_recovery).AppendLine();
		sb.AppendFormat("[evasion] = <color=yellow>{0}</color>", evasion).AppendLine();
		sb.AppendFormat("[accuracy] = <color=yellow>{0}</color>", accuracy).AppendLine();
		sb.AppendFormat("[heal] = <color=yellow>{0}</color>", heal).AppendLine();
		sb.AppendFormat("[resist ] = <color=yellow>{0}</color>", resist ).AppendLine();
		sb.AppendFormat("[weight] = <color=yellow>{0}</color>", weight).AppendLine();
		sb.AppendFormat("[auto_recovery] = <color=yellow>{0}</color>", auto_recovery).AppendLine();
		sb.AppendFormat("[move_speed] = <color=yellow>{0}</color>", move_speed).AppendLine();
		return sb.ToString();
	}
}

