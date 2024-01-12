﻿using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Player_Character_Skill_Duration_Data : System.IDisposable
{
	///	<summary>
	///	지속성 스킬 효과 인덱스
	///	</summary>
	public int pc_skill_duration_id {get; set;}
	///	<summary>
	///	지속성 효과 타입
	///	</summary>
	public DURATION_EFFECT_TYPE duration_effect_type {get; set;}
	///	<summary>
	///	지속성 방식 타입
	///	</summary>
	public PERSISTENCE_TYPE persistence_type {get; set;}
	///	<summary>
	///	지속 시간
	///	지속시간 : 초단위
	///	</summary>
	public double time {get; set;}
	///	<summary>
	///	지속 횟수
	///	</summary>
	public int count {get; set;}
	///	<summary>
	///	반복 일회성 효과
	///	</summary>
	public int[] repeat_pc_onetime_ids {get; set;}
	///	<summary>
	///	반복 주기
	///	</summary>
	public double repeat_interval {get; set;}
	///	<summary>
	///	종료 일회성 효과
	///	</summary>
	public int[] finish_pc_onetime_ids {get; set;}
	///	<summary>
	///	스탯 멀티플 타입
	///	</summary>
	public STAT_MULTIPLE_TYPE multiple_type {get; set;}
	///	<summary>
	///	절대값
	///	</summary>
	public int value {get; set;}
	///	<summary>
	///	배율
	///	</summary>
	public double multiple {get; set;}
	///	<summary>
	///	확률
	///	적용 확률
	///	10000분율을 기준으로 한다.
	///	</summary>
	public double rate {get; set;}
	///	<summary>
	///	이펙트 프리팹
	///	</summary>
	public string effect_path {get; set;}
	///	<summary>
	///	중첩 가능
	///	</summary>
	public bool is_overlapable {get; set;}

	private bool disposed = false;

	public Player_Character_Skill_Duration_Data()
	{
		pc_skill_duration_id = 0;
		duration_effect_type = DURATION_EFFECT_TYPE.NONE;
		persistence_type = PERSISTENCE_TYPE.NONE;
		count = 0;
		multiple_type = STAT_MULTIPLE_TYPE.NONE;
		value = 0;
		effect_path = string.Empty;
		is_overlapable = false;
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
		sb.AppendFormat("[pc_skill_duration_id] = <color=yellow>{0}</color>", pc_skill_duration_id).AppendLine();
		sb.AppendFormat("[duration_effect_type] = <color=yellow>{0}</color>", duration_effect_type).AppendLine();
		sb.AppendFormat("[persistence_type] = <color=yellow>{0}</color>", persistence_type).AppendLine();
		sb.AppendFormat("[time] = <color=yellow>{0}</color>", time).AppendLine();
		sb.AppendFormat("[count] = <color=yellow>{0}</color>", count).AppendLine();
		sb.AppendLine("[repeat_pc_onetime_ids]");
		if(repeat_pc_onetime_ids != null)
		{
			cnt = repeat_pc_onetime_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", repeat_pc_onetime_ids[i]).AppendLine();
			}
		}

		sb.AppendFormat("[repeat_interval] = <color=yellow>{0}</color>", repeat_interval).AppendLine();
		sb.AppendLine("[finish_pc_onetime_ids]");
		if(finish_pc_onetime_ids != null)
		{
			cnt = finish_pc_onetime_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", finish_pc_onetime_ids[i]).AppendLine();
			}
		}

		sb.AppendFormat("[multiple_type] = <color=yellow>{0}</color>", multiple_type).AppendLine();
		sb.AppendFormat("[value] = <color=yellow>{0}</color>", value).AppendLine();
		sb.AppendFormat("[multiple] = <color=yellow>{0}</color>", multiple).AppendLine();
		sb.AppendFormat("[rate] = <color=yellow>{0}</color>", rate).AppendLine();
		sb.AppendFormat("[effect_path] = <color=yellow>{0}</color>", effect_path).AppendLine();
		sb.AppendFormat("[is_overlapable] = <color=yellow>{0}</color>", is_overlapable).AppendLine();
		return sb.ToString();
	}
}

