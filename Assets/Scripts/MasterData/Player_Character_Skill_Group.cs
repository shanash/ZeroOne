﻿using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Player_Character_Skill_Group : System.IDisposable
{
	///	<summary>
	///	스킬 그룹 인덱스
	///	</summary>
	public int pc_skill_group_id {get; set;}
	///	<summary>
	///	스킬 이름
	///	</summary>
	public string name_kr {get; set;}
	///	<summary>
	///	다음 스킬 사용 
	///	가능 쿨타임
	///	</summary>
	public double skill_use_delay {get; set;}
	///	<summary>
	///	스킬 설명
	///	</summary>
	public string script {get; set;}
	///	<summary>
	///	스킬 아이콘
	///	</summary>
	public string icon {get; set;}
	///	<summary>
	///	액션
	///	</summary>
	public string action_name {get; set;}

	private bool disposed = false;

	public Player_Character_Skill_Group()
	{
		pc_skill_group_id = 0;
		name_kr = string.Empty;
		script = string.Empty;
		icon = string.Empty;
		action_name = string.Empty;
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
		sb.AppendFormat("[pc_skill_group_id] = <color=yellow>{0}</color>", pc_skill_group_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[skill_use_delay] = <color=yellow>{0}</color>", skill_use_delay).AppendLine();
		sb.AppendFormat("[script] = <color=yellow>{0}</color>", script).AppendLine();
		sb.AppendFormat("[icon] = <color=yellow>{0}</color>", icon).AppendLine();
		sb.AppendFormat("[action_name] = <color=yellow>{0}</color>", action_name).AppendLine();
		return sb.ToString();
	}
}

