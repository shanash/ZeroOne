#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Player_Character_Data : System.IDisposable
{
	///	<summary>
	///	캐릭터 고유 인덱스
	///	</summary>
	public int player_character_id => _player_character_id;
	int _player_character_id;

	///	<summary>
	///	캐릭터 명칭
	///	</summary>
	public string name_kr => _name_kr;
	string _name_kr;

	///	<summary>
	///	태생 성급
	///	</summary>
	public int default_star => _default_star;
	int _default_star;

	///	<summary>
	///	역할군
	///	</summary>
	public ROLE_TYPE role_type => _role_type;
	ROLE_TYPE _role_type;

	///	<summary>
	///	캐릭터가 속한 종족
	///	</summary>
	public TRIBE_TYPE tribe_type => _tribe_type;
	TRIBE_TYPE _tribe_type;

	///	<summary>
	///	프로필 정보_나이
	///	</summary>
	public int profile_age => _profile_age;
	int _profile_age;

	///	<summary>
	///	프로필 정보_생일
	///	</summary>
	public int[] profile_birthday => _profile_birthday;
	int[] _profile_birthday;

	///	<summary>
	///	프로필 정보_키
	///	</summary>
	public int profile_high => _profile_high;
	int _profile_high;

	///	<summary>
	///	프로필 정보_취미
	///	</summary>
	public string profile_habby => _profile_habby;
	string _profile_habby;

	///	<summary>
	///	전투 정보 인덱스
	///	</summary>
	public int battle_info_id => _battle_info_id;
	int _battle_info_id;

	///	<summary>
	///	전투용 프리팹
	///	</summary>
	public string prefab_path => _prefab_path;
	string _prefab_path;

	///	<summary>
	///	결과 UI 프리팹
	///	</summary>
	public string sd_prefab_path => _sd_prefab_path;
	string _sd_prefab_path;

	///	<summary>
	///	캐릭터 아이콘
	///	</summary>
	public string icon_path => _icon_path;
	string _icon_path;

	///	<summary>
	///	캐릭터 일러스트
	///	</summary>
	public string Illustration_path => _Illustration_path;
	string _Illustration_path;

	///	<summary>
	///	캐릭터 설명
	///	</summary>
	public string script => _script;
	string _script;

	private bool disposed = false;

	public Player_Character_Data(Raw_Player_Character_Data raw_data)
	{
		_player_character_id = raw_data.player_character_id;
		_name_kr = raw_data.name_kr;
		_default_star = raw_data.default_star;
		_role_type = raw_data.role_type;
		_tribe_type = raw_data.tribe_type;
		_profile_age = raw_data.profile_age;
		if(raw_data.profile_birthday != null)
			_profile_birthday = raw_data.profile_birthday.ToArray();
		_profile_high = raw_data.profile_high;
		_profile_habby = raw_data.profile_habby;
		_battle_info_id = raw_data.battle_info_id;
		_prefab_path = raw_data.prefab_path;
		_sd_prefab_path = raw_data.sd_prefab_path;
		_icon_path = raw_data.icon_path;
		_Illustration_path = raw_data.Illustration_path;
		_script = raw_data.script;
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
		sb.AppendFormat("[player_character_id] = <color=yellow>{0}</color>", player_character_id).AppendLine();
		sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
		sb.AppendFormat("[default_star] = <color=yellow>{0}</color>", default_star).AppendLine();
		sb.AppendFormat("[role_type] = <color=yellow>{0}</color>", role_type).AppendLine();
		sb.AppendFormat("[tribe_type] = <color=yellow>{0}</color>", tribe_type).AppendLine();
		sb.AppendFormat("[profile_age] = <color=yellow>{0}</color>", profile_age).AppendLine();
		sb.AppendLine("[profile_birthday]");
		if(profile_birthday != null)
		{
			cnt = profile_birthday.Length;
			for(int i = 0; i< cnt; i++)
			{
				sb.Append("\t").AppendFormat("<color=yellow>{0}</color>", profile_birthday[i]).AppendLine();
			}
		}

		sb.AppendFormat("[profile_high] = <color=yellow>{0}</color>", profile_high).AppendLine();
		sb.AppendFormat("[profile_habby] = <color=yellow>{0}</color>", profile_habby).AppendLine();
		sb.AppendFormat("[battle_info_id] = <color=yellow>{0}</color>", battle_info_id).AppendLine();
		sb.AppendFormat("[prefab_path] = <color=yellow>{0}</color>", prefab_path).AppendLine();
		sb.AppendFormat("[sd_prefab_path] = <color=yellow>{0}</color>", sd_prefab_path).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		sb.AppendFormat("[Illustration_path] = <color=yellow>{0}</color>", Illustration_path).AppendLine();
		sb.AppendFormat("[script] = <color=yellow>{0}</color>", script).AppendLine();
		return sb.ToString();
	}
}

