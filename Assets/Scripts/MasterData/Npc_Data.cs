using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Npc_Data : System.IDisposable
{
	///	<summary>
	///	npc id
	///	</summary>
	public int npc_data_id {get; set;}
	///	<summary>
	///	이름
	///	</summary>
	public string name_kr {get; set;}
	///	<summary>
	///	종족
	///	</summary>
	public TRIBE_TYPE tribe_type {get; set;}
	///	<summary>
	///	npc 타입
	///	</summary>
	public NPC_TYPE npc_type {get; set;}
	///	<summary>
	///	프리팹
	///	</summary>
	public string prefab_path {get; set;}
	///	<summary>
	///	전투 정보 인덱스
	///	</summary>
	public int npc_battle_id {get; set;}

	private bool disposed = false;

	public Npc_Data()
	{
		npc_data_id = 0;
		name_kr = string.Empty;
		tribe_type = TRIBE_TYPE.NONE;
		npc_type = NPC_TYPE.NONE;
		prefab_path = string.Empty;
		npc_battle_id = 0;
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
		sb.AppendFormat("[npc_data_id] = <color=yellow>{0}</color>", npc_data_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[tribe_type] = <color=yellow>{0}</color>", tribe_type).AppendLine();
		sb.AppendFormat("[npc_type] = <color=yellow>{0}</color>", npc_type).AppendLine();
		sb.AppendFormat("[prefab_path] = <color=yellow>{0}</color>", prefab_path).AppendLine();
		sb.AppendFormat("[npc_battle_id] = <color=yellow>{0}</color>", npc_battle_id).AppendLine();
		return sb.ToString();
	}
}

