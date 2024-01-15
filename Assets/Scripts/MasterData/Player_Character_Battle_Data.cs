using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Player_Character_Battle_Data : System.IDisposable
{
	///	<summary>
	///	전투 인덱스
	///	</summary>
	public int battle_info_id {get; set;}
	///	<summary>
	///	접근 사거리
	///	</summary>
	public double approach {get; set;}
	///	<summary>
	///	배치 위치
	///	</summary>
	public POSITION_TYPE position_type {get; set;}
	///	<summary>
	///	스킬 패턴
	///	pc_group_skill_id를 사용한다.
	///	</summary>
	public int[] skill_pattern {get; set;}
	///	<summary>
	///	패시브
	///	</summary>
	public int passive_skill_group_id {get; set;}
	///	<summary>
	///	궁극기
	///	</summary>
	public int super_skill_group_id {get; set;}
	///	<summary>
	///	체력
	///	</summary>
	public double hp {get; set;}
	///	<summary>
	///	물리 공격력
	///	</summary>
	public double attack {get; set;}
	///	<summary>
	///	마법 공격력
	///	</summary>
	public double m_attack {get; set;}
	///	<summary>
	///	물리 방어력
	///	</summary>
	public double defend {get; set;}
	///	<summary>
	///	마법_방어력
	///	</summary>
	public double m_defend {get; set;}
	///	<summary>
	///	회복량
	///	</summary>
	public double attack_recovery {get; set;}
	///	<summary>
	///	회피
	///	</summary>
	public double evasion {get; set;}
	///	<summary>
	///	명중
	///	</summary>
	public double accuracy {get; set;}
	///	<summary>
	///	자동 회복
	///	</summary>
	public double auto_recovery {get; set;}
	///	<summary>
	///	전투 이동 속도
	///	</summary>
	public double move_speed {get; set;}
	///	<summary>
	///	전투 대사 인덱스
	///	</summary>
	public string attack_script {get; set;}

	private bool disposed = false;

	public Player_Character_Battle_Data()
	{
		battle_info_id = 0;
		position_type = POSITION_TYPE.NONE;
		passive_skill_group_id = 0;
		super_skill_group_id = 0;
		attack_script = string.Empty;
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
		sb.AppendFormat("[super_skill_group_id] = <color=yellow>{0}</color>", super_skill_group_id).AppendLine();
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

