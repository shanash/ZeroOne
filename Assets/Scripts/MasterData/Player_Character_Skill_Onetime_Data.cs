using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Player_Character_Skill_Onetime_Data : System.IDisposable
{
	///	<summary>
	///	일회성 스킬 효과 인덱스
	///	</summary>
	public int pc_skill_onetime_id {get; set;}
	///	<summary>
	///	일회성 효과 타입
	///	</summary>
	public ONETIME_EFFECT_TYPE onetime_effect_type {get; set;}
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
	///	이펙트 프리팹
	///	</summary>
	public string effect_path {get; set;}

	private bool disposed = false;

	public Player_Character_Skill_Onetime_Data()
	{
		pc_skill_onetime_id = 0;
		onetime_effect_type = ONETIME_EFFECT_TYPE.NONE;
		multiple_type = STAT_MULTIPLE_TYPE.NONE;
		value = 0;
		effect_path = string.Empty;
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
		sb.AppendFormat("[pc_skill_onetime_id] = <color=yellow>{0}</color>", pc_skill_onetime_id).AppendLine();
		sb.AppendFormat("[onetime_effect_type] = <color=yellow>{0}</color>", onetime_effect_type).AppendLine();
		sb.AppendFormat("[multiple_type] = <color=yellow>{0}</color>", multiple_type).AppendLine();
		sb.AppendFormat("[value] = <color=yellow>{0}</color>", value).AppendLine();
		sb.AppendFormat("[multiple] = <color=yellow>{0}</color>", multiple).AppendLine();
		sb.AppendFormat("[effect_path] = <color=yellow>{0}</color>", effect_path).AppendLine();
		return sb.ToString();
	}
}

