public class Npc_Battle_Data : System.IDisposable
{
	///	<summary>
	///	전투 인덱스
	///	</summary>
	public readonly int npc_battle_id;
	///	<summary>
	///	접근 사거리
	///	</summary>
	public readonly double approach;
	///	<summary>
	///	배치 위치
	///	</summary>
	public readonly POSITION_TYPE position_type;
	///	<summary>
	///	스킬 패턴
	///	</summary>
	public readonly int[] skill_pattern;
	///	<summary>
	///	패시브
	///	</summary>
	public readonly int passive_skill_group_id;
	///	<summary>
	///	궁극기
	///	</summary>
	public readonly int special_skill_group_id;
	///	<summary>
	///	레벨
	///	</summary>
	public readonly int npc_level;
	///	<summary>
	///	체력
	///	</summary>
	public readonly double hp;
	///	<summary>
	///	공격력
	///	</summary>
	public readonly double attack;
	///	<summary>
	///	방어력
	///	</summary>
	public readonly double defend;
	///	<summary>
	///	회피
	///	</summary>
	public readonly double evasion;
	///	<summary>
	///	명중
	///	</summary>
	public readonly double accuracy;
	///	<summary>
	///	전투 이동 속도
	///	</summary>
	public readonly double move_speed;
	///	<summary>
	///	전투 대사 인덱스
	///	</summary>
	public readonly string attack_script;

	private bool disposed = false;

	public Npc_Battle_Data(Raw_Npc_Battle_Data raw_data)
	{
		npc_battle_id = raw_data.npc_battle_id;
		approach = raw_data.approach;
		position_type = raw_data.position_type;
		skill_pattern = raw_data.skill_pattern != null ? (int[])raw_data.skill_pattern.Clone() : new int[0];
		passive_skill_group_id = raw_data.passive_skill_group_id;
		special_skill_group_id = raw_data.special_skill_group_id;
		npc_level = raw_data.npc_level;
		hp = raw_data.hp;
		attack = raw_data.attack;
		defend = raw_data.defend;
		evasion = raw_data.evasion;
		accuracy = raw_data.accuracy;
		move_speed = raw_data.move_speed;
		attack_script = raw_data.attack_script;
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
		sb.AppendFormat("[npc_level] = <color=yellow>{0}</color>", npc_level).AppendLine();
		sb.AppendFormat("[hp] = <color=yellow>{0}</color>", hp).AppendLine();
		sb.AppendFormat("[attack] = <color=yellow>{0}</color>", attack).AppendLine();
		sb.AppendFormat("[defend] = <color=yellow>{0}</color>", defend).AppendLine();
		sb.AppendFormat("[evasion] = <color=yellow>{0}</color>", evasion).AppendLine();
		sb.AppendFormat("[accuracy] = <color=yellow>{0}</color>", accuracy).AppendLine();
		sb.AppendFormat("[move_speed] = <color=yellow>{0}</color>", move_speed).AppendLine();
		sb.AppendFormat("[attack_script] = <color=yellow>{0}</color>", attack_script).AppendLine();
		return sb.ToString();
	}
}

