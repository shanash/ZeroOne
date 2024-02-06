﻿#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Npc_Battle_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	전투 인덱스
	///	</summary>
	public int npc_battle_id => _npc_battle_id;
	int _npc_battle_id;

	///	<summary>
	///	접근 사거리
	///	</summary>
	public double approach => _approach;
	double _approach;

	///	<summary>
	///	배치 위치
	///	</summary>
	public POSITION_TYPE position_type => _position_type;
	POSITION_TYPE _position_type;

	///	<summary>
	///	스킬 패턴
	///	</summary>
	public int[] skill_pattern => _skill_pattern;
	int[] _skill_pattern;

	///	<summary>
	///	패시브
	///	</summary>
	public int passive_skill_group_id => _passive_skill_group_id;
	int _passive_skill_group_id;

	///	<summary>
	///	궁극기
	///	</summary>
	public int special_skill_group_id => _special_skill_group_id;
	int _special_skill_group_id;

	///	<summary>
	///	체력
	///	</summary>
	public double hp => _hp;
	double _hp;

	///	<summary>
	///	물리 공격력
	///	</summary>
	public double p_attack => _p_attack;
	double _p_attack;

	///	<summary>
	///	마법 공격력
	///	</summary>
	public double m_attack => _m_attack;
	double _m_attack;

	///	<summary>
	///	물리 방어력
	///	</summary>
	public double p_defend => _p_defend;
	double _p_defend;

	///	<summary>
	///	마법_방어력
	///	</summary>
	public double m_defend => _m_defend;
	double _m_defend;

	///	<summary>
	///	물리 크리티컬 확률
	///	</summary>
	public double p_cri_rate => _p_cri_rate;
	double _p_cri_rate;

	///	<summary>
	///	마법 크리티컬 확률
	///	</summary>
	public double m_cri_rate => _m_cri_rate;
	double _m_cri_rate;

	///	<summary>
	///	물리 크리티컬 추가 대미지
	///	</summary>
	public double p_cri_power => _p_cri_power;
	double _p_cri_power;

	///	<summary>
	///	마법 크리티컬 추가 대미지
	///	</summary>
	public double m_cri_power => _m_cri_power;
	double _m_cri_power;

	///	<summary>
	///	타격 시 회복량
	///	</summary>
	public double attack_recovery => _attack_recovery;
	double _attack_recovery;

	///	<summary>
	///	회피
	///	</summary>
	public double evasion => _evasion;
	double _evasion;

	///	<summary>
	///	명중
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
	public double resist => _resist;
	double _resist;

	///	<summary>
	///	무게
	///	</summary>
	public double weight => _weight;
	double _weight;

	///	<summary>
	///	전투 이동 속도
	///	</summary>
	public double move_speed => _move_speed;
	double _move_speed;

	///	<summary>
	///	전투 대사 인덱스
	///	</summary>
	public string attack_script => _attack_script;
	string _attack_script;

	private bool disposed = false;

	public Npc_Battle_Data(Raw_Npc_Battle_Data raw_data)
	{
		_npc_battle_id = raw_data.npc_battle_id;
		_approach = raw_data.approach;
		_position_type = raw_data.position_type;
		if(raw_data.skill_pattern != null)
			_skill_pattern = raw_data.skill_pattern.ToArray();
		_passive_skill_group_id = raw_data.passive_skill_group_id;
		_special_skill_group_id = raw_data.special_skill_group_id;
		_hp = raw_data.hp;
		_p_attack = raw_data.p_attack;
		_m_attack = raw_data.m_attack;
		_p_defend = raw_data.p_defend;
		_m_defend = raw_data.m_defend;
		_p_cri_rate = raw_data.p_cri_rate;
		_m_cri_rate = raw_data.m_cri_rate;
		_p_cri_power = raw_data.p_cri_power;
		_m_cri_power = raw_data.m_cri_power;
		_attack_recovery = raw_data.attack_recovery;
		_evasion = raw_data.evasion;
		_accuracy = raw_data.accuracy;
		_heal = raw_data.heal;
		_resist = raw_data.resist;
		_weight = raw_data.weight;
		_move_speed = raw_data.move_speed;
		_attack_script = raw_data.attack_script;
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
		int cnt = 0;
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.AppendFormat("[npc_battle_id] = <color=yellow>{0}</color>", npc_battle_id).AppendLine();
		sb.AppendFormat("[approach] = <color=yellow>{0}</color>", approach).AppendLine();
		sb.AppendFormat("[position_type] = <color=yellow>{0}</color>", position_type).AppendLine();
		sb.AppendLine("[skill_pattern]");
		if(skill_pattern != null)
		{
			cnt = skill_pattern.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", skill_pattern[i]).AppendLine();
			}
		}

		sb.AppendFormat("[passive_skill_group_id] = <color=yellow>{0}</color>", passive_skill_group_id).AppendLine();
		sb.AppendFormat("[special_skill_group_id] = <color=yellow>{0}</color>", special_skill_group_id).AppendLine();
		sb.AppendFormat("[hp] = <color=yellow>{0}</color>", hp).AppendLine();
		sb.AppendFormat("[p_attack] = <color=yellow>{0}</color>", p_attack).AppendLine();
		sb.AppendFormat("[m_attack] = <color=yellow>{0}</color>", m_attack).AppendLine();
		sb.AppendFormat("[p_defend] = <color=yellow>{0}</color>", p_defend).AppendLine();
		sb.AppendFormat("[m_defend] = <color=yellow>{0}</color>", m_defend).AppendLine();
		sb.AppendFormat("[p_cri_rate] = <color=yellow>{0}</color>", p_cri_rate).AppendLine();
		sb.AppendFormat("[m_cri_rate] = <color=yellow>{0}</color>", m_cri_rate).AppendLine();
		sb.AppendFormat("[p_cri_power] = <color=yellow>{0}</color>", p_cri_power).AppendLine();
		sb.AppendFormat("[m_cri_power] = <color=yellow>{0}</color>", m_cri_power).AppendLine();
		sb.AppendFormat("[attack_recovery] = <color=yellow>{0}</color>", attack_recovery).AppendLine();
		sb.AppendFormat("[evasion] = <color=yellow>{0}</color>", evasion).AppendLine();
		sb.AppendFormat("[accuracy] = <color=yellow>{0}</color>", accuracy).AppendLine();
		sb.AppendFormat("[heal] = <color=yellow>{0}</color>", heal).AppendLine();
		sb.AppendFormat("[resist] = <color=yellow>{0}</color>", resist).AppendLine();
		sb.AppendFormat("[weight] = <color=yellow>{0}</color>", weight).AppendLine();
		sb.AppendFormat("[move_speed] = <color=yellow>{0}</color>", move_speed).AppendLine();
		sb.AppendFormat("[attack_script] = <color=yellow>{0}</color>", attack_script).AppendLine();
		return sb.ToString();
	}
}

