public class Player_Character_Data : System.IDisposable
{
	///	<summary>
	///	캐릭터 고유 인덱스
	///	</summary>
	public readonly int player_character_id;
	///	<summary>
	///	캐릭터 명칭
	///	</summary>
	public readonly string name_kr;
	///	<summary>
	///	태생 성급
	///	</summary>
	public readonly int default_star;
	///	<summary>
	///	역할군
	///	</summary>
	public readonly ROLE_TYPE role_type;
	///	<summary>
	///	캐릭터가 속한 종족
	///	</summary>
	public readonly TRIBE_TYPE tribe_type;
	///	<summary>
	///	프로필 정보_나이
	///	</summary>
	public readonly int profile_age;
	///	<summary>
	///	프로필 정보_생일
	///	</summary>
	public readonly int[] profile_birthday;
	///	<summary>
	///	프로필 정보_키
	///	</summary>
	public readonly int profile_high;
	///	<summary>
	///	프로필 정보_취미
	///	</summary>
	public readonly string profile_habby;
	///	<summary>
	///	전투 정보 인덱스
	///	</summary>
	public readonly int battle_info_id;
	///	<summary>
	///	전투용 프리팹
	///	</summary>
	public readonly string prefab_path;
	///	<summary>
	///	결과 UI 프리팹
	///	</summary>
	public readonly string sd_prefab_path;
	///	<summary>
	///	캐릭터 아이콘
	///	</summary>
	public readonly string icon_path;
	///	<summary>
	///	캐릭터 일러스트
	///	</summary>
	public readonly string Illustration_path;
	///	<summary>
	///	캐릭터 설명
	///	</summary>
	public readonly string script;

	private bool disposed = false;

	public Player_Character_Data(Raw_Player_Character_Data raw_data)
	{
		player_character_id = raw_data.player_character_id;
		name_kr = raw_data.name_kr;
		default_star = raw_data.default_star;
		role_type = raw_data.role_type;
		tribe_type = raw_data.tribe_type;
		profile_age = raw_data.profile_age;
		profile_birthday = raw_data.profile_birthday != null ? (int[])raw_data.profile_birthday.Clone() : new int[0];
		profile_high = raw_data.profile_high;
		profile_habby = raw_data.profile_habby;
		battle_info_id = raw_data.battle_info_id;
		prefab_path = raw_data.prefab_path;
		sd_prefab_path = raw_data.sd_prefab_path;
		icon_path = raw_data.icon_path;
		Illustration_path = raw_data.Illustration_path;
		script = raw_data.script;
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

