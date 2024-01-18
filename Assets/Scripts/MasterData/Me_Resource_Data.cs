public class Me_Resource_Data : System.IDisposable
{
	///	<summary>
	///	메모리얼 인덱스
	///	</summary>
	public readonly int memorial_id;
	///	<summary>
	///	캐릭터 고유 인덱스
	///	</summary>
	public readonly int player_character_id;
	///	<summary>
	///	캐릭터 프리팹 키
	///	</summary>
	public readonly string actor_prefab_key;
	///	<summary>
	///	배경 프리팹 키
	///	</summary>
	public readonly string background_prefab_key;
	///	<summary>
	///	프리팹 키
	///	</summary>
	public readonly string prefab_key;
	///	<summary>
	///	인트로 키
	///	</summary>
	public readonly string intro_key;
	///	<summary>
	///	시작 상태
	///	</summary>
	public readonly int state_id;
	///	<summary>
	///	순서
	///	</summary>
	public readonly int order;

	private bool disposed = false;

	public Me_Resource_Data(Raw_Me_Resource_Data raw_data)
	{
		memorial_id = raw_data.memorial_id;
		player_character_id = raw_data.player_character_id;
		actor_prefab_key = raw_data.actor_prefab_key;
		background_prefab_key = raw_data.background_prefab_key;
		prefab_key = raw_data.prefab_key;
		intro_key = raw_data.intro_key;
		state_id = raw_data.state_id;
		order = raw_data.order;
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

