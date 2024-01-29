#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Npc_Skill_Duration_Data : System.IDisposable
{
	///	<summary>
	///	지속성 스킬 효과 인덱스
	///	</summary>
	public int npc_skill_duration_id => _npc_skill_duration_id;
	int _npc_skill_duration_id;

	///	<summary>
	///	지속성 효과 타입
	///	</summary>
	public DURATION_EFFECT_TYPE duration_effect_type => _duration_effect_type;
	DURATION_EFFECT_TYPE _duration_effect_type;

	///	<summary>
	///	지속성 방식 타입
	///	</summary>
	public PERSISTENCE_TYPE persistence_type => _persistence_type;
	PERSISTENCE_TYPE _persistence_type;

	///	<summary>
	///	지속 시간
	///	</summary>
	public double time => _time;
	double _time;

	///	<summary>
	///	지속 횟수
	///	</summary>
	public int count => _count;
	int _count;

	///	<summary>
	///	반복 일회성 효과
	///	</summary>
	public int[] repeat_npc_onetime_ids => _repeat_npc_onetime_ids;
	int[] _repeat_npc_onetime_ids;

	///	<summary>
	///	반복 주기
	///	</summary>
	public double repeat_interval => _repeat_interval;
	double _repeat_interval;

	///	<summary>
	///	종료 일회성 효과
	///	</summary>
	public int[] finish_npc_onetime_ids => _finish_npc_onetime_ids;
	int[] _finish_npc_onetime_ids;

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
	///	확률
	///	적용 확률
	///	10000분율을 기준으로 한다.
	///	</summary>
	public double rate => _rate;
	double _rate;

	///	<summary>
	///	이펙트 프리팹
	///	</summary>
	public string effect_path => _effect_path;
	string _effect_path;

	///	<summary>
	///	중첩 가능
	///	</summary>
	public bool is_overlapable => _is_overlapable;
	bool _is_overlapable;

	private bool disposed = false;

	public Npc_Skill_Duration_Data(Raw_Npc_Skill_Duration_Data raw_data)
	{
		_npc_skill_duration_id = raw_data.npc_skill_duration_id;
		_duration_effect_type = raw_data.duration_effect_type;
		_persistence_type = raw_data.persistence_type;
		_time = raw_data.time;
		_count = raw_data.count;
		if(raw_data.repeat_npc_onetime_ids != null)
			_repeat_npc_onetime_ids = raw_data.repeat_npc_onetime_ids.ToArray();
		_repeat_interval = raw_data.repeat_interval;
		if(raw_data.finish_npc_onetime_ids != null)
			_finish_npc_onetime_ids = raw_data.finish_npc_onetime_ids.ToArray();
		_multiple_type = raw_data.multiple_type;
		_value = raw_data.value;
		_multiple = raw_data.multiple;
		_rate = raw_data.rate;
		_effect_path = raw_data.effect_path;
		_is_overlapable = raw_data.is_overlapable;
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
		sb.AppendFormat("[npc_skill_duration_id] = <color=yellow>{0}</color>", npc_skill_duration_id).AppendLine();
		sb.AppendFormat("[duration_effect_type] = <color=yellow>{0}</color>", duration_effect_type).AppendLine();
		sb.AppendFormat("[persistence_type] = <color=yellow>{0}</color>", persistence_type).AppendLine();
		sb.AppendFormat("[time] = <color=yellow>{0}</color>", time).AppendLine();
		sb.AppendFormat("[count] = <color=yellow>{0}</color>", count).AppendLine();
		sb.AppendLine("[repeat_npc_onetime_ids]");
		if(repeat_npc_onetime_ids != null)
		{
			cnt = repeat_npc_onetime_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", repeat_npc_onetime_ids[i]).AppendLine();
			}
		}

		sb.AppendFormat("[repeat_interval] = <color=yellow>{0}</color>", repeat_interval).AppendLine();
		sb.AppendLine("[finish_npc_onetime_ids]");
		if(finish_npc_onetime_ids != null)
		{
			cnt = finish_npc_onetime_ids.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", finish_npc_onetime_ids[i]).AppendLine();
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

