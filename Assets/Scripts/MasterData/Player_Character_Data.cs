using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Player_Character_Data : System.IDisposable
{
	///	<summary>
	///	캐릭터 고유 인덱스
	///	
	///	</summary>
	public int player_character_id {get; set;}
	///	<summary>
	///	캐릭터 명칭
	///	
	///	</summary>
	public string name_kr {get; set;}
	///	<summary>
	///	캐릭터가 속한 종족
	///	
	///	</summary>
	public TRIBE_TYPE tribe_type {get; set;}
	///	<summary>
	///	전투 정보 인덱스
	///	
	///	</summary>
	public int battle_info_id {get; set;}
	///	<summary>
	///	프리팹
	///	
	///	</summary>
	public string prefab_path {get; set;}
	///	<summary>
	///	캐릭터 아이콘
	///	
	///	</summary>
	public string icon_path {get; set;}
	///	<summary>
	///	캐릭터 설명
	///	
	///	</summary>
	public string script {get; set;}

	private bool disposed = false;

	public Player_Character_Data()
	{
		player_character_id = 0;
		name_kr = string.Empty;
		tribe_type = TRIBE_TYPE.NONE;
		battle_info_id = 0;
		prefab_path = string.Empty;
		icon_path = string.Empty;
		script = string.Empty;
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
		sb.AppendFormat("[player_character_id] = <color=yellow>{0}</color>", player_character_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[tribe_type] = <color=yellow>{0}</color>", tribe_type).AppendLine();
		sb.AppendFormat("[battle_info_id] = <color=yellow>{0}</color>", battle_info_id).AppendLine();
		sb.AppendFormat("[prefab_path] = <color=yellow>{0}</color>", prefab_path).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		sb.AppendFormat("[script] = <color=yellow>{0}</color>", script).AppendLine();
		return sb.ToString();
	}
}

