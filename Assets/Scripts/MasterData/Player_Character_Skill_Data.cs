﻿public class Player_Character_Skill_Data : System.IDisposable
{
	///	<summary>
	///	스킬 인덱스
	///	</summary>
	public readonly int pc_skill_id;
	///	<summary>
	///	스킬 그룹 ID
	///	</summary>
	public readonly int pc_skill_group_id;
	///	<summary>
	///	타겟팀
	///	0 : 아군
	///	1 : 적군
	///	</summary>
	public readonly TARGET_TYPE target_type;
	///	<summary>
	///	타겟룰
	///	</summary>
	public readonly TARGET_RULE_TYPE target_rule_type;
	///	<summary>
	///	타겟 순서
	///	순서는 0부터 시작
	///	순서가 필요한 타겟룰에서만 사용
	///	</summary>
	public readonly int target_order;
	///	<summary>
	///	타겟수
	///	</summary>
	public readonly int target_count;
	///	<summary>
	///	타겟 범위
	///	</summary>
	public readonly double target_range;
	///	<summary>
	///	이펙트 카운트 타입
	///	</summary>
	public readonly EFFECT_COUNT_TYPE effect_count_type;
	///	<summary>
	///	세컨 타겟 룰
	///	</summary>
	public readonly SECOND_TARGET_RULE_TYPE second_target_rule;
	///	<summary>
	///	최대 세컨 타겟 카운트
	///	기준이 되는 타겟은 포함하지 않는 갯수
	///	</summary>
	public readonly int max_second_target_count;
	///	<summary>
	///	세컨 타겟 반경
	///	</summary>
	public readonly double second_target_range;
	///	<summary>
	///	효과 비중
	///	힛 횟수에 따라 비중 조절
	///	총 합이 100이 되어야 한다.
	///	실제 애니메이션의 이벤트 횟수보다 배열의 수가 많으면 안된다.
	///	</summary>
	public readonly int[] effect_weight;
	///	<summary>
	///	일회성 효과
	///	</summary>
	public readonly int[] onetime_effect_ids;
	///	<summary>
	///	지속성 효과
	///	</summary>
	public readonly int[] duration_effect_ids;
	///	<summary>
	///	세컨 타겟용 일회성 효과
	///	</summary>
	public readonly int[] second_target_onetime_effect_ids;
	///	<summary>
	///	세컨 타겟용 지속성 효과
	///	</summary>
	public readonly int[] second_target_duration_effect_ids;
	///	<summary>
	///	이벤트 이름
	///	</summary>
	public readonly string event_name;
	///	<summary>
	///	트리거 이펙트 프리팹
	///	스킬의 이펙트는 트리거 역할을 해줘야 함
	///	이펙트 패스 정보가 없는 경우는 즉시 스킬을 시전/적용하는 방식이고
	///	이펙트 패스 정보가 있는 경우 해당 이펙트를 이용하여 트리거를 발생하는 방식을 적용한다.
	///	</summary>
	public readonly string trigger_effect_path;
	///	<summary>
	///	빈 이펙트 프리팹
	///	</summary>
	public readonly string empty_effect_path;

	private bool disposed = false;

	public Player_Character_Skill_Data(Raw_Player_Character_Skill_Data raw_data)
	{
		pc_skill_id = raw_data.pc_skill_id;
		pc_skill_group_id = raw_data.pc_skill_group_id;
		target_type = raw_data.target_type;
		target_rule_type = raw_data.target_rule_type;
		target_order = raw_data.target_order;
		target_count = raw_data.target_count;
		target_range = raw_data.target_range;
		effect_count_type = raw_data.effect_count_type;
		second_target_rule = raw_data.second_target_rule;
		max_second_target_count = raw_data.max_second_target_count;
		second_target_range = raw_data.second_target_range;
		effect_weight = raw_data.effect_weight != null ? (int[])raw_data.effect_weight.Clone() : new int[0];
		onetime_effect_ids = raw_data.onetime_effect_ids != null ? (int[])raw_data.onetime_effect_ids.Clone() : new int[0];
		duration_effect_ids = raw_data.duration_effect_ids != null ? (int[])raw_data.duration_effect_ids.Clone() : new int[0];
		second_target_onetime_effect_ids = raw_data.second_target_onetime_effect_ids != null ? (int[])raw_data.second_target_onetime_effect_ids.Clone() : new int[0];
		second_target_duration_effect_ids = raw_data.second_target_duration_effect_ids != null ? (int[])raw_data.second_target_duration_effect_ids.Clone() : new int[0];
		event_name = raw_data.event_name;
		trigger_effect_path = raw_data.trigger_effect_path;
		empty_effect_path = raw_data.empty_effect_path;
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
		sb.AppendFormat("[pc_skill_id] = <color=yellow>{0}</color>", pc_skill_id).AppendLine();
		sb.AppendFormat("[pc_skill_group_id] = <color=yellow>{0}</color>", pc_skill_group_id).AppendLine();
		sb.AppendFormat("[target_type] = <color=yellow>{0}</color>", target_type).AppendLine();
		sb.AppendFormat("[target_rule_type] = <color=yellow>{0}</color>", target_rule_type).AppendLine();
		sb.AppendFormat("[target_order] = <color=yellow>{0}</color>", target_order).AppendLine();
		sb.AppendFormat("[target_count] = <color=yellow>{0}</color>", target_count).AppendLine();
		sb.AppendFormat("[target_range] = <color=yellow>{0}</color>", target_range).AppendLine();
		sb.AppendFormat("[effect_count_type] = <color=yellow>{0}</color>", effect_count_type).AppendLine();
		sb.AppendFormat("[second_target_rule] = <color=yellow>{0}</color>", second_target_rule).AppendLine();
		sb.AppendFormat("[max_second_target_count] = <color=yellow>{0}</color>", max_second_target_count).AppendLine();
		sb.AppendFormat("[second_target_range] = <color=yellow>{0}</color>", second_target_range).AppendLine();
		sb.AppendLine("[effect_weight]");
		if(effect_weight != null)
		{
			cnt = effect_weight.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", effect_weight[i]).AppendLine();
			}
		}

		sb.AppendLine("[onetime_effect_ids]");
		if(onetime_effect_ids != null)
		{
			cnt = onetime_effect_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", onetime_effect_ids[i]).AppendLine();
			}
		}

		sb.AppendLine("[duration_effect_ids]");
		if(duration_effect_ids != null)
		{
			cnt = duration_effect_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", duration_effect_ids[i]).AppendLine();
			}
		}

		sb.AppendLine("[second_target_onetime_effect_ids]");
		if(second_target_onetime_effect_ids != null)
		{
			cnt = second_target_onetime_effect_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", second_target_onetime_effect_ids[i]).AppendLine();
			}
		}

		sb.AppendLine("[second_target_duration_effect_ids]");
		if(second_target_duration_effect_ids != null)
		{
			cnt = second_target_duration_effect_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", second_target_duration_effect_ids[i]).AppendLine();
			}
		}

		sb.AppendFormat("[event_name] = <color=yellow>{0}</color>", event_name).AppendLine();
		sb.AppendFormat("[trigger_effect_path] = <color=yellow>{0}</color>", trigger_effect_path).AppendLine();
		sb.AppendFormat("[empty_effect_path] = <color=yellow>{0}</color>", empty_effect_path).AppendLine();
		return sb.ToString();
	}
}

