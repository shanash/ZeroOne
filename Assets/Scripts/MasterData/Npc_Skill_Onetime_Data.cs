﻿using FluffyDuck.Util;
using System.Linq;

public class Npc_Skill_Onetime_Data : System.IDisposable
{
	///	<summary>
	///	일회성 스킬 효과 인덱스
	///	</summary>
	public int npc_skill_onetime_id => _npc_skill_onetime_id;
	int _npc_skill_onetime_id;

	///	<summary>
	///	일회성 효과 타입
	///	</summary>
	public ONETIME_EFFECT_TYPE onetime_effect_type => _onetime_effect_type;
	ONETIME_EFFECT_TYPE _onetime_effect_type;

	///	<summary>
	///	스탯 멀티플 타입
	///	</summary>
	public STAT_MULTIPLE_TYPE multiple_type => _multiple_type;
	STAT_MULTIPLE_TYPE _multiple_type;

	///	<summary>
	///	절대값
	///	</summary>
	public int value => _value;
	int _value;

	///	<summary>
	///	배율
	///	</summary>
	public double multiple => _multiple;
	double _multiple;

	///	<summary>
	///	이펙트 프리팹
	///	</summary>
	public string effect_path => _effect_path;
	string _effect_path;

	private bool disposed = false;

	public Npc_Skill_Onetime_Data(Raw_Npc_Skill_Onetime_Data raw_data)
	{
		_npc_skill_onetime_id = raw_data.npc_skill_onetime_id;
		_onetime_effect_type = raw_data.onetime_effect_type;
		_multiple_type = raw_data.multiple_type;
		_value = raw_data.value;
		_multiple = raw_data.multiple;
		_effect_path = raw_data.effect_path;
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
		sb.AppendFormat("[npc_skill_onetime_id] = <color=yellow>{0}</color>", npc_skill_onetime_id).AppendLine();
		sb.AppendFormat("[onetime_effect_type] = <color=yellow>{0}</color>", onetime_effect_type).AppendLine();
		sb.AppendFormat("[multiple_type] = <color=yellow>{0}</color>", multiple_type).AppendLine();
		sb.AppendFormat("[value] = <color=yellow>{0}</color>", value).AppendLine();
		sb.AppendFormat("[multiple] = <color=yellow>{0}</color>", multiple).AppendLine();
		sb.AppendFormat("[effect_path] = <color=yellow>{0}</color>", effect_path).AppendLine();
		return sb.ToString();
	}
}

