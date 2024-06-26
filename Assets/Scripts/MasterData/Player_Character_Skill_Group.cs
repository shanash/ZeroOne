﻿#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Player_Character_Skill_Group : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	스킬 그룹 인덱스
	///	</summary>
	public int pc_skill_group_id => _pc_skill_group_id;
	int _pc_skill_group_id;

	///	<summary>
	///	스킬 이름 아이디
	///	</summary>
	public string name_id => _name_id;
	string _name_id;

	///	<summary>
	///	스킬 이름
	///	</summary>
	public string name_kr => _name_kr;
	string _name_kr;

	///	<summary>
	///	다음 스킬 사용 
	///	가능 쿨타임
	///	</summary>
	public double skill_use_delay => _skill_use_delay;
	double _skill_use_delay;

	///	<summary>
	///	스킬 타입
	///	</summary>
	public SKILL_TYPE skill_type => _skill_type;
	SKILL_TYPE _skill_type;

	///	<summary>
	///	스킬 설명
	///	</summary>
	public string script => _script;
	string _script;

	///	<summary>
	///	궁극기 타겟 스킬 id
	///	궁극기 사용시 타겟팅을 먼저 해야 하는데, 해당 타겟을 위한 스킬 ID
	///	</summary>
	public int target_skill_id => _target_skill_id;
	int _target_skill_id;

	///	<summary>
	///	스킬 아이콘
	///	</summary>
	public string icon => _icon;
	string _icon;

	///	<summary>
	///	액션
	///	</summary>
	public string action_name => _action_name;
	string _action_name;

	///	<summary>
	///	캐스팅 이펙트
	///	</summary>
	public string[] cast_effect_path => _cast_effect_path;
	string[] _cast_effect_path;

	///	<summary>
	///	Skill SFX
	///	</summary>
	public string skill_sfx_path => _skill_sfx_path;
	string _skill_sfx_path;

	///	<summary>
	///	목소리 1배속
	///	</summary>
	public string skill_voice_path_1 => _skill_voice_path_1;
	string _skill_voice_path_1;

	///	<summary>
	///	목소리 2배속
	///	</summary>
	public string skill_voice_path_2 => _skill_voice_path_2;
	string _skill_voice_path_2;

	///	<summary>
	///	목소리 3배속
	///	</summary>
	public string skill_voice_path_3 => _skill_voice_path_3;
	string _skill_voice_path_3;

	private bool disposed = false;

	public Player_Character_Skill_Group(Raw_Player_Character_Skill_Group raw_data)
	{
		_pc_skill_group_id = raw_data.pc_skill_group_id;
		_name_id = raw_data.name_id;
		_name_kr = raw_data.name_kr;
		_skill_use_delay = raw_data.skill_use_delay;
		_skill_type = raw_data.skill_type;
		_script = raw_data.script;
		_target_skill_id = raw_data.target_skill_id;
		_icon = raw_data.icon;
		_action_name = raw_data.action_name;
		if(raw_data.cast_effect_path != null)
			_cast_effect_path = raw_data.cast_effect_path.ToArray();
		_skill_sfx_path = raw_data.skill_sfx_path;
		_skill_voice_path_1 = raw_data.skill_voice_path_1;
		_skill_voice_path_2 = raw_data.skill_voice_path_2;
		_skill_voice_path_3 = raw_data.skill_voice_path_3;
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
		sb.AppendFormat("[pc_skill_group_id] = <color=yellow>{0}</color>", pc_skill_group_id).AppendLine();
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[skill_use_delay] = <color=yellow>{0}</color>", skill_use_delay).AppendLine();
		sb.AppendFormat("[skill_type] = <color=yellow>{0}</color>", skill_type).AppendLine();
		sb.AppendFormat("[script] = <color=yellow>{0}</color>", script).AppendLine();
		sb.AppendFormat("[target_skill_id] = <color=yellow>{0}</color>", target_skill_id).AppendLine();
		sb.AppendFormat("[icon] = <color=yellow>{0}</color>", icon).AppendLine();
		sb.AppendFormat("[action_name] = <color=yellow>{0}</color>", action_name).AppendLine();
		sb.AppendLine("[cast_effect_path]");
		if(cast_effect_path != null)
		{
			cnt = cast_effect_path.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", cast_effect_path[i]).AppendLine();
			}
		}

		sb.AppendFormat("[skill_sfx_path] = <color=yellow>{0}</color>", skill_sfx_path).AppendLine();
		sb.AppendFormat("[skill_voice_path_1] = <color=yellow>{0}</color>", skill_voice_path_1).AppendLine();
		sb.AppendFormat("[skill_voice_path_2] = <color=yellow>{0}</color>", skill_voice_path_2).AppendLine();
		sb.AppendFormat("[skill_voice_path_3] = <color=yellow>{0}</color>", skill_voice_path_3).AppendLine();
		return sb.ToString();
	}
}

