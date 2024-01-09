[System.Serializable]
public class Player_Character_Data : System.IDisposable
{
    ///	<summary>
    ///	캐릭터 고유 인덱스
    ///	</summary>
    public int player_character_id { get; set; }
    ///	<summary>
    ///	캐릭터 명칭
    ///	</summary>
    public string name_kr { get; set; }
    ///	<summary>
    ///	태생 성급
    ///	</summary>
    public int default_star { get; set; }
    ///	<summary>
    ///	역할군
    ///	</summary>
    public ROLE_TYPE role_type { get; set; }
    ///	<summary>
    ///	캐릭터가 속한 종족
    ///	</summary>
    public TRIBE_TYPE tribe_type { get; set; }
    ///	<summary>
    ///	프로필 정보_나이
    ///	</summary>
    public int profile_age { get; set; }
    ///	<summary>
    ///	프로필 정보_생일
    ///	</summary>
    public int[] profile_birthday { get; set; }
    ///	<summary>
    ///	프로필 정보_키
    ///	</summary>
    public int profile_high { get; set; }
    ///	<summary>
    ///	프로필 정보_취미
    ///	</summary>
    public string profile_habby { get; set; }
    ///	<summary>
    ///	전투 정보 인덱스
    ///	</summary>
    public int battle_info_id { get; set; }
    ///	<summary>
    ///	프리팹
    ///	</summary>
    public string prefab_path { get; set; }
    ///	<summary>
    ///	UI 프리팹
    ///	</summary>
    public string sd_prefab_path { get; set; }
    ///	<summary>
    ///	캐릭터 아이콘
    ///	</summary>
    public string icon_path { get; set; }
    ///	<summary>
    ///	캐릭터 일러스트
    ///	</summary>
    public string Illustration_path { get; set; }
    ///	<summary>
    ///	캐릭터 설명
    ///	</summary>
    public string script { get; set; }

    private bool disposed = false;

    public Player_Character_Data()
    {
        player_character_id = 0;
        name_kr = string.Empty;
        default_star = 0;
        role_type = ROLE_TYPE.NONE;
        tribe_type = TRIBE_TYPE.NONE;
        profile_age = 0;
        profile_high = 0;
        profile_habby = string.Empty;
        battle_info_id = 0;
        prefab_path = string.Empty;
        sd_prefab_path = string.Empty;
        icon_path = string.Empty;
        Illustration_path = string.Empty;
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
        int cnt = 0;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("[player_character_id] = <color=yellow>{0}</color>", player_character_id).AppendLine();
        sb.AppendFormat("[name_kr] = <color=yellow>{0}</color>", name_kr).AppendLine();
        sb.AppendFormat("[default_star] = <color=yellow>{0}</color>", default_star).AppendLine();
        sb.AppendFormat("[role_type] = <color=yellow>{0}</color>", role_type).AppendLine();
        sb.AppendFormat("[tribe_type] = <color=yellow>{0}</color>", tribe_type).AppendLine();
        sb.AppendFormat("[profile_age] = <color=yellow>{0}</color>", profile_age).AppendLine();
        sb.AppendLine("[profile_birthday]");
        if (profile_birthday != null)
        {
            cnt = profile_birthday.Length;
            for (int i = 0; i < cnt; i++)
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

