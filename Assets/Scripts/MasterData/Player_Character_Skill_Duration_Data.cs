public class Player_Character_Skill_Duration_Data : System.IDisposable
{
	///	<summary>
	///	지속성 스킬 효과 인덱스
	///	</summary>
	public readonly int pc_skill_duration_id;
	///	<summary>
	///	지속성 효과 타입
	///	</summary>
	public readonly DURATION_EFFECT_TYPE duration_effect_type;
	///	<summary>
	///	지속성 방식 타입
	///	</summary>
	public readonly PERSISTENCE_TYPE persistence_type;
	///	<summary>
	///	지속 시간
	///	지속시간 : 초단위
	///	</summary>
	public readonly double time;
	///	<summary>
	///	지속 횟수
	///	</summary>
	public readonly int count;
	///	<summary>
	///	반복 일회성 효과
	///	</summary>
	public readonly int[] repeat_pc_onetime_ids;
	///	<summary>
	///	반복 주기
	///	</summary>
	public readonly double repeat_interval;
	///	<summary>
	///	종료 일회성 효과
	///	</summary>
	public readonly int[] finish_pc_onetime_ids;
	///	<summary>
	///	스탯 멀티플 타입
	///	</summary>
	public readonly STAT_MULTIPLE_TYPE multiple_type;
	///	<summary>
	///	절대값
	///	</summary>
	public readonly int value;
	///	<summary>
	///	배율
	///	</summary>
	public readonly double multiple;
	///	<summary>
	///	확률
	///	적용 확률
	///	10000분율을 기준으로 한다.
	///	</summary>
	public readonly double rate;
	///	<summary>
	///	이펙트 프리팹
	///	</summary>
	public readonly string effect_path;
	///	<summary>
	///	중첩 가능
	///	</summary>
	public readonly bool is_overlapable;

	private bool disposed = false;

	public Player_Character_Skill_Duration_Data(Raw_Player_Character_Skill_Duration_Data raw_data)
	{
		pc_skill_duration_id = raw_data.pc_skill_duration_id;
		duration_effect_type = raw_data.duration_effect_type;
		persistence_type = raw_data.persistence_type;
		time = raw_data.time;
		count = raw_data.count;
		repeat_pc_onetime_ids = raw_data.repeat_pc_onetime_ids != null ? (int[])raw_data.repeat_pc_onetime_ids.Clone() : new int[0];
		repeat_interval = raw_data.repeat_interval;
		finish_pc_onetime_ids = raw_data.finish_pc_onetime_ids != null ? (int[])raw_data.finish_pc_onetime_ids.Clone() : new int[0];
		multiple_type = raw_data.multiple_type;
		value = raw_data.value;
		multiple = raw_data.multiple;
		rate = raw_data.rate;
		effect_path = raw_data.effect_path;
		is_overlapable = raw_data.is_overlapable;
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

