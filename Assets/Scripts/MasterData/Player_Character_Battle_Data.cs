public class Player_Character_Battle_Data : System.IDisposable
{
	///	<summary>
	///	전투 인덱스
	///	</summary>
	public readonly int battle_info_id;
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
	///	pc_group_skill_id를 사용한다.
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
	///	체력
	///	</summary>
	public readonly double hp;
	///	<summary>
	///	물리 공격력
	///	</summary>
	public readonly double attack;
	///	<summary>
	///	마법 공격력
	///	</summary>
	public readonly double m_attack;
	///	<summary>
	///	물리 방어력
	///	</summary>
	public readonly double defend;
	///	<summary>
	///	마법_방어력
	///	</summary>
	public readonly double m_defend;
	///	<summary>
	///	회복량
	///	</summary>
	public readonly double attack_recovery;
	///	<summary>
	///	회피
	///	</summary>
	public readonly double evasion;
	///	<summary>
	///	명중
	///	</summary>
	public readonly double accuracy;
	///	<summary>
	///	자동 회복
	///	</summary>
	public readonly double auto_recovery;
	///	<summary>
	///	전투 이동 속도
	///	</summary>
	public readonly double move_speed;
	///	<summary>
	///	전투 대사 인덱스
	///	</summary>
	public readonly string attack_script;

	private bool disposed = false;

	public Player_Character_Battle_Data(Raw_Player_Character_Battle_Data raw_data)
	{
		battle_info_id = raw_data.battle_info_id;
		approach = raw_data.approach;
		position_type = raw_data.position_type;
		skill_pattern = raw_data.skill_pattern != null ? (int[])raw_data.skill_pattern.Clone() : new int[0];
		passive_skill_group_id = raw_data.passive_skill_group_id;
		special_skill_group_id = raw_data.special_skill_group_id;
		hp = raw_data.hp;
		attack = raw_data.attack;
		m_attack = raw_data.m_attack;
		defend = raw_data.defend;
		m_defend = raw_data.m_defend;
		attack_recovery = raw_data.attack_recovery;
		evasion = raw_data.evasion;
		accuracy = raw_data.accuracy;
		auto_recovery = raw_data.auto_recovery;
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
		sb.AppendFormat("[battle_info_id] = <color=yellow>{0}</color>", battle_info_id).AppendLine();
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
		sb.AppendFormat("[hp] = <color=yellow>{0}</color>", hp).AppendLine();
		sb.AppendFormat("[attack] = <color=yellow>{0}</color>", attack).AppendLine();
		sb.AppendFormat("[m_attack] = <color=yellow>{0}</color>", m_attack).AppendLine();
		sb.AppendFormat("[defend] = <color=yellow>{0}</color>", defend).AppendLine();
		sb.AppendFormat("[m_defend] = <color=yellow>{0}</color>", m_defend).AppendLine();
		sb.AppendFormat("[attack_recovery] = <color=yellow>{0}</color>", attack_recovery).AppendLine();
		sb.AppendFormat("[evasion] = <color=yellow>{0}</color>", evasion).AppendLine();
		sb.AppendFormat("[accuracy] = <color=yellow>{0}</color>", accuracy).AppendLine();
		sb.AppendFormat("[auto_recovery] = <color=yellow>{0}</color>", auto_recovery).AppendLine();
		sb.AppendFormat("[move_speed] = <color=yellow>{0}</color>", move_speed).AppendLine();
		sb.AppendFormat("[attack_script] = <color=yellow>{0}</color>", attack_script).AppendLine();
		return sb.ToString();
	}
}

