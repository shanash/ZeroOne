#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Player_Character_Data : System.IDisposable
{
	///	<summary>
	///	<b>key_1</b><br/>
	///	캐릭터 고유 인덱스
	///	</summary>
	public int player_character_id => _player_character_id;
	int _player_character_id;

	///	<summary>
	///	캐릭터 명칭
	///	</summary>
	public string name_id => _name_id;
	string _name_id;

	///	<summary>
	///	툴 팁용 캐릭터 설명 ID
	///	</summary>
	public string desc_id => _desc_id;
	string _desc_id;

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
	///	종족
	///	</summary>
	public TRIBE_TYPE tribe_type => _tribe_type;
	TRIBE_TYPE _tribe_type;

	///	<summary>
	///	공격 속성
	///	</summary>
	public ATTRIBUTE_TYPE attribute_type => _attribute_type;
	ATTRIBUTE_TYPE _attribute_type;

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
	///	로비 스탠딩
	///	</summary>
	public int lobby_basic_id => _lobby_basic_id;
	int _lobby_basic_id;

	///	<summary>
	///	근원전달 스탠딩
	///	</summary>
	public int essence_id => _essence_id;
	int _essence_id;

	///	<summary>
	///	로비 서약 스탠딩
	///	</summary>
	public int lobby_merry_id => _lobby_merry_id;
	int _lobby_merry_id;

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

	///	<summary>
	///	크기
	///	</summary>
	public double scale => _scale;
	double _scale;

	///	<summary>
	///	초기 오픈 캐릭터
	///	</summary>
	public bool first_open_check => _first_open_check;
	bool _first_open_check;

	///	<summary>
	///	전투 편성창 선택 보이스
	///	</summary>
	public string battle_selcet_voice => _battle_selcet_voice;
	string _battle_selcet_voice;

	///	<summary>
	///	사망 보이스
	///	</summary>
	public string die_voice => _die_voice;
	string _die_voice;

	///	<summary>
	///	승리 보이스
	///	</summary>
	public string win_voice => _win_voice;
	string _win_voice;

	private bool disposed = false;

	public Player_Character_Data(Raw_Player_Character_Data raw_data)
	{
		_player_character_id = raw_data.player_character_id;
		_name_id = raw_data.name_id;
		_desc_id = raw_data.desc_id;
		_default_star = raw_data.default_star;
		_role_type = raw_data.role_type;
		_tribe_type = raw_data.tribe_type;
		_attribute_type = raw_data.attribute_type;
		_profile_age = raw_data.profile_age;
		if(raw_data.profile_birthday != null)
			_profile_birthday = raw_data.profile_birthday.ToArray();
		_profile_high = raw_data.profile_high;
		_profile_habby = raw_data.profile_habby;
		_battle_info_id = raw_data.battle_info_id;
		_prefab_path = raw_data.prefab_path;
		_sd_prefab_path = raw_data.sd_prefab_path;
		_lobby_basic_id = raw_data.lobby_basic_id;
		_essence_id = raw_data.essence_id;
		_lobby_merry_id = raw_data.lobby_merry_id;
		_icon_path = raw_data.icon_path;
		_Illustration_path = raw_data.Illustration_path;
		_script = raw_data.script;
		_scale = raw_data.scale;
		_first_open_check = raw_data.first_open_check;
		_battle_selcet_voice = raw_data.battle_selcet_voice;
		_die_voice = raw_data.die_voice;
		_win_voice = raw_data.win_voice;
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
		sb.AppendFormat("[name_id] = <color=yellow>{0}</color>", name_id).AppendLine();
		sb.AppendFormat("[desc_id] = <color=yellow>{0}</color>", desc_id).AppendLine();
		sb.AppendFormat("[default_star] = <color=yellow>{0}</color>", default_star).AppendLine();
		sb.AppendFormat("[role_type] = <color=yellow>{0}</color>", role_type).AppendLine();
		sb.AppendFormat("[tribe_type] = <color=yellow>{0}</color>", tribe_type).AppendLine();
		sb.AppendFormat("[attribute_type] = <color=yellow>{0}</color>", attribute_type).AppendLine();
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
		sb.AppendFormat("[lobby_basic_id] = <color=yellow>{0}</color>", lobby_basic_id).AppendLine();
		sb.AppendFormat("[essence_id] = <color=yellow>{0}</color>", essence_id).AppendLine();
		sb.AppendFormat("[lobby_merry_id] = <color=yellow>{0}</color>", lobby_merry_id).AppendLine();
		sb.AppendFormat("[icon_path] = <color=yellow>{0}</color>", icon_path).AppendLine();
		sb.AppendFormat("[Illustration_path] = <color=yellow>{0}</color>", Illustration_path).AppendLine();
		sb.AppendFormat("[script] = <color=yellow>{0}</color>", script).AppendLine();
		sb.AppendFormat("[scale] = <color=yellow>{0}</color>", scale).AppendLine();
		sb.AppendFormat("[first_open_check] = <color=yellow>{0}</color>", first_open_check).AppendLine();
		sb.AppendFormat("[battle_selcet_voice] = <color=yellow>{0}</color>", battle_selcet_voice).AppendLine();
		sb.AppendFormat("[die_voice] = <color=yellow>{0}</color>", die_voice).AppendLine();
		sb.AppendFormat("[win_voice] = <color=yellow>{0}</color>", win_voice).AppendLine();
		return sb.ToString();
	}
}

