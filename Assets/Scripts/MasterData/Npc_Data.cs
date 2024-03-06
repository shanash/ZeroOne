#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Npc_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	npc id
	///	</summary>
	public int npc_data_id => _npc_data_id;
	int _npc_data_id;

	///	<summary>
	///	이름
	///	</summary>
	public string name_id => _name_id;
	string _name_id;

	///	<summary>
	///	이름
	///	</summary>
	public string name_kr => _name_kr;
	string _name_kr;

	///	<summary>
	///	설명
	///	</summary>
	public string desc_id => _desc_id;
	string _desc_id;

	///	<summary>
	///	종족
	///	</summary>
	public TRIBE_TYPE tribe_type => _tribe_type;
	TRIBE_TYPE _tribe_type;

	///	<summary>
	///	npc 타입
	///	</summary>
	public NPC_TYPE npc_type => _npc_type;
	NPC_TYPE _npc_type;

	///	<summary>
	///	공격 속성
	///	</summary>
	public ATTRIBUTE_TYPE attribute_type => _attribute_type;
	ATTRIBUTE_TYPE _attribute_type;

	///	<summary>
	///	전투 정보 인덱스
	///	</summary>
	public int npc_battle_id => _npc_battle_id;
	int _npc_battle_id;

	///	<summary>
	///	프리팹
	///	</summary>
	public string prefab_path => _prefab_path;
	string _prefab_path;

	///	<summary>
	///	아이콘
	///	</summary>
	public string icon_path => _icon_path;
	string _icon_path;

	///	<summary>
	///	크기
	///	</summary>
	public double scale => _scale;
	double _scale;

	private bool disposed = false;

	public Npc_Data(Raw_Npc_Data raw_data)
	{
		_npc_data_id = raw_data.npc_data_id;
		_name_id = raw_data.name_id;
		_name_kr = raw_data.name_kr;
		_desc_id = raw_data.desc_id;
		_tribe_type = raw_data.tribe_type;
		_npc_type = raw_data.npc_type;
		_attribute_type = raw_data.attribute_type;
		_npc_battle_id = raw_data.npc_battle_id;
		_prefab_path = raw_data.prefab_path;
		_icon_path = raw_data.icon_path;
		_scale = raw_data.scale;
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
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[desc_id] = <color=yellow>{0}</color>", desc_id).AppendLine();
		sb.AppendFormat("[tribe_type] = <color=yellow>{0}</color>", tribe_type).AppendLine();
		sb.AppendFormat("[npc_type] = <color=yellow>{0}</color>", npc_type).AppendLine();
		sb.AppendFormat("[attribute_type] = <color=yellow>{0}</color>", attribute_type).AppendLine();
		sb.AppendFormat("[npc_battle_id] = <color=yellow>{0}</color>", npc_battle_id).AppendLine();
		sb.AppendFormat("[prefab_path] = <color=yellow>{0}</color>", prefab_path).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		sb.AppendFormat("[scale] = <color=yellow>{0}</color>", scale).AppendLine();
		return sb.ToString();
	}
}

