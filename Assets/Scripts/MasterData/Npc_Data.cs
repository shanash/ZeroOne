public class Npc_Data : System.IDisposable
{
	///	<summary>
	///	npc id
	///	</summary>
	public readonly int npc_data_id;
	///	<summary>
	///	이름
	///	</summary>
	public readonly string name_kr;
	///	<summary>
	///	종족
	///	</summary>
	public readonly TRIBE_TYPE tribe_type;
	///	<summary>
	///	npc 타입
	///	</summary>
	public readonly NPC_TYPE npc_type;
	///	<summary>
	///	전투 정보 인덱스
	///	</summary>
	public readonly int npc_battle_id;
	///	<summary>
	///	프리팹
	///	</summary>
	public readonly string prefab_path;
	///	<summary>
	///	아이콘
	///	</summary>
	public readonly string icon_path;

	private bool disposed = false;

	public Npc_Data(Raw_Npc_Data raw_data)
	{
		npc_data_id = raw_data.npc_data_id;
		name_kr = raw_data.name_kr;
		tribe_type = raw_data.tribe_type;
		npc_type = raw_data.npc_type;
		npc_battle_id = raw_data.npc_battle_id;
		prefab_path = raw_data.prefab_path;
		icon_path = raw_data.icon_path;
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
		sb.AppendFormat("[npc_battle_id] = <color=yellow>{0}</color>", npc_battle_id).AppendLine();
		sb.AppendFormat("[prefab_path] = <color=yellow>{0}</color>", prefab_path).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		return sb.ToString();
	}
}

