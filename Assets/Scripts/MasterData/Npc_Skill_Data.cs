using FluffyDuck.Util;
using System.Linq;

public class Npc_Skill_Data : System.IDisposable
{
	///	<summary>
	///	스킬 인덱스
	///	</summary>
	public int npc_skill_id => _npc_skill_id;
	int _npc_skill_id;

	///	<summary>
	///	스킬 그룹 ID
	///	</summary>
	public int npc_skill_group_id => _npc_skill_group_id;
	int _npc_skill_group_id;

	///	<summary>
	///	타겟 팀
	///	0 : 아군
	///	1 : 적군
	///	</summary>
	public TARGET_TYPE target_type => _target_type;
	TARGET_TYPE _target_type;

	///	<summary>
	///	타겟룰
	///	</summary>
	public TARGET_RULE_TYPE target_rule_type => _target_rule_type;
	TARGET_RULE_TYPE _target_rule_type;

	///	<summary>
	///	타겟 순서
	///	순서는 0부터 시작
	///	순서가 필요한 타겟룰에서만 사용
	///	</summary>
	public int target_order => _target_order;
	int _target_order;

	///	<summary>
	///	타겟수
	///	</summary>
	public int target_count => _target_count;
	int _target_count;

	///	<summary>
	///	타겟 범위
	///	</summary>
	public double target_range => _target_range;
	double _target_range;

	///	<summary>
	///	이펙트 카운트 타입
	///	</summary>
	public EFFECT_COUNT_TYPE effect_count_type => _effect_count_type;
	EFFECT_COUNT_TYPE _effect_count_type;

	///	<summary>
	///	세컨 타겟 룰
	///	</summary>
	public SECOND_TARGET_RULE_TYPE second_target_rule => _second_target_rule;
	SECOND_TARGET_RULE_TYPE _second_target_rule;

	///	<summary>
	///	세컨 타겟 카운트
	///	</summary>
	public int max_second_target_count => _max_second_target_count;
	int _max_second_target_count;

	///	<summary>
	///	세컨 타겟 반경
	///	</summary>
	public double second_target_range => _second_target_range;
	double _second_target_range;

	///	<summary>
	///	효과 비중
	///	힛 횟수에 따라 비중 조절
	///	총 합이 100이 되어야 한다.
	///	실제 애니메이션의 이벤트 횟수보다 배열의 수가 많으면 안된다.
	///	</summary>
	public int[] effect_weight => _effect_weight;
	int[] _effect_weight;

	///	<summary>
	///	일회성 효과
	///	</summary>
	public int[] onetime_effect_ids => _onetime_effect_ids;
	int[] _onetime_effect_ids;

	///	<summary>
	///	지속성 효과
	///	</summary>
	public int[] duration_effect_ids => _duration_effect_ids;
	int[] _duration_effect_ids;

	///	<summary>
	///	세컨 타겟용 일회성 효과
	///	</summary>
	public int[] second_target_onetime_effect_ids => _second_target_onetime_effect_ids;
	int[] _second_target_onetime_effect_ids;

	///	<summary>
	///	세컨 타겟용 지속성 효과
	///	</summary>
	public int[] second_target_duration_effect_ids => _second_target_duration_effect_ids;
	int[] _second_target_duration_effect_ids;

	///	<summary>
	///	이벤트 이름
	///	</summary>
	public string event_name => _event_name;
	string _event_name;

	///	<summary>
	///	트리거 이펙트 프리팹
	///	</summary>
	public string trigger_effect_path => _trigger_effect_path;
	string _trigger_effect_path;

	private bool disposed = false;

	public Npc_Skill_Data(Raw_Npc_Skill_Data raw_data)
	{
		_npc_skill_id = raw_data.npc_skill_id;
		_npc_skill_group_id = raw_data.npc_skill_group_id;
		_target_type = raw_data.target_type;
		_target_rule_type = raw_data.target_rule_type;
		_target_order = raw_data.target_order;
		_target_count = raw_data.target_count;
		_target_range = raw_data.target_range;
		_effect_count_type = raw_data.effect_count_type;
		_second_target_rule = raw_data.second_target_rule;
		_max_second_target_count = raw_data.max_second_target_count;
		_second_target_range = raw_data.second_target_range;
		_effect_weight = raw_data.effect_weight.ToArray();
		_onetime_effect_ids = raw_data.onetime_effect_ids.ToArray();
		_duration_effect_ids = raw_data.duration_effect_ids.ToArray();
		_second_target_onetime_effect_ids = raw_data.second_target_onetime_effect_ids.ToArray();
		_second_target_duration_effect_ids = raw_data.second_target_duration_effect_ids.ToArray();
		_event_name = raw_data.event_name;
		_trigger_effect_path = raw_data.trigger_effect_path;
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
		sb.AppendFormat("[npc_skill_id] = <color=yellow>{0}</color>", npc_skill_id).AppendLine();
		sb.AppendFormat("[npc_skill_group_id] = <color=yellow>{0}</color>", npc_skill_group_id).AppendLine();
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
		return sb.ToString();
	}
}

