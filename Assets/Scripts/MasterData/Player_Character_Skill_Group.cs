public class Player_Character_Skill_Group : System.IDisposable
{
	///	<summary>
	///	스킬 그룹 인덱스
	///	</summary>
	public readonly int pc_skill_group_id;
	///	<summary>
	///	스킬 이름
	///	</summary>
	public readonly string name_kr;
	///	<summary>
	///	다음 스킬 사용 
	///	가능 쿨타임
	///	</summary>
	public readonly double skill_use_delay;
	///	<summary>
	///	스킬 타입
	///	</summary>
	public readonly SKILL_TYPE skill_type;
	///	<summary>
	///	스킬 설명
	///	</summary>
	public readonly string script;
	///	<summary>
	///	스킬 아이콘
	///	</summary>
	public readonly string icon;
	///	<summary>
	///	액션
	///	</summary>
	public readonly string action_name;
	///	<summary>
	///	캐스팅 이펙트
	///	</summary>
	public readonly string cast_effect_path;

	private bool disposed = false;

	public Player_Character_Skill_Group(Raw_Player_Character_Skill_Group raw_data)
	{
		pc_skill_group_id = raw_data.pc_skill_group_id;
		name_kr = raw_data.name_kr;
		skill_use_delay = raw_data.skill_use_delay;
		skill_type = raw_data.skill_type;
		script = raw_data.script;
		icon = raw_data.icon;
		action_name = raw_data.action_name;
		cast_effect_path = raw_data.cast_effect_path;
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
		sb.AppendFormat("[skill_type] = <color=yellow>{0}</color>", skill_type).AppendLine();
		sb.AppendFormat("[script] = <color=yellow>{0}</color>", script).AppendLine();
		sb.AppendFormat("[icon] = <color=yellow>{0}</color>", icon).AppendLine();
		sb.AppendFormat("[action_name] = <color=yellow>{0}</color>", action_name).AppendLine();
		sb.AppendFormat("[cast_effect_path] = <color=yellow>{0}</color>", cast_effect_path).AppendLine();
		return sb.ToString();
	}
}

