[System.Serializable]
public class Me_Resource_Data : System.IDisposable
{
	///	<summary>
	///	메모리얼 인덱스
	///	</summary>
	public int memorial_id {get; set;}
	///	<summary>
	///	캐릭터 고유 인덱스
	///	</summary>
	public int player_character_id {get; set;}
	///	<summary>
	///	캐릭터 프리팹 키
	///	</summary>
	public string actor_prefab_key {get; set;}
	///	<summary>
	///	배경 프리팹 키
	///	</summary>
	public string background_prefab_key {get; set;}
	///	<summary>
	///	프리팹 키
	///	</summary>
	public string prefab_key {get; set;}
	///	<summary>
	///	인트로 키
	///	</summary>
	public string intro_key {get; set;}
	///	<summary>
	///	시작 상태
	///	</summary>
	public int state_id {get; set;}
	///	<summary>
	///	순서
	///	</summary>
	public int order {get; set;}

	private bool disposed = false;

	public Me_Resource_Data()
	{
		memorial_id = 0;
		player_character_id = 0;
		actor_prefab_key = string.Empty;
		background_prefab_key = string.Empty;
		prefab_key = string.Empty;
		intro_key = string.Empty;
		state_id = 0;
		order = 0;
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
		sb.AppendFormat("[memorial_id] = <color=yellow>{0}</color>", memorial_id).AppendLine();
		sb.AppendFormat("[player_character_id] = <color=yellow>{0}</color>", player_character_id).AppendLine();
		sb.AppendFormat("[actor_prefab_key] = <color=yellow>{0}</color>", actor_prefab_key).AppendLine();
		sb.AppendFormat("[background_prefab_key] = <color=yellow>{0}</color>", background_prefab_key).AppendLine();
		sb.AppendFormat("[prefab_key] = <color=yellow>{0}</color>", prefab_key).AppendLine();
		sb.AppendFormat("[intro_key] = <color=yellow>{0}</color>", intro_key).AppendLine();
		sb.AppendFormat("[state_id] = <color=yellow>{0}</color>", state_id).AppendLine();
		sb.AppendFormat("[order] = <color=yellow>{0}</color>", order).AppendLine();
		return sb.ToString();
	}
}

