using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Npc_Skill_Group : System.IDisposable
{
	///	<summary>
	///	npc 스킬 그룹 인덱스
	///	</summary>
	public int npc_skill_group_id {get; set;}
	///	<summary>
	///	스킬 이름
	///	</summary>
	public string name_kr {get; set;}
	///	<summary>
	///	쿨타임
	///	</summary>
	public double skill_use_delay {get; set;}
	///	<summary>
	///	액션
	///	</summary>
	public string action_name {get; set;}

	private bool disposed = false;

	public Npc_Skill_Group()
	{
		npc_skill_group_id = 0;
		name_kr = string.Empty;
		action_name = string.Empty;
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
		sb.AppendFormat("[npc_skill_group_id] = <color=yellow>{0}</color>", npc_skill_group_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[skill_use_delay] = <color=yellow>{0}</color>", skill_use_delay).AppendLine();
		sb.AppendFormat("[action_name] = <color=yellow>{0}</color>", action_name).AppendLine();
		return sb.ToString();
	}
}

