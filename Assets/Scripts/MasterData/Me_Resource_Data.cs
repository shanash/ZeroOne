#if UNITY_5_3_OR_NEWER
using FluffyDuck.Util;
#endif
using System.Linq;
#nullable disable

public class Me_Resource_Data : System.IDisposable
{
	///	<summary>
	///	메모리얼 인덱스
	///	</summary>
	public int memorial_id => _memorial_id;
	int _memorial_id;

	///	<summary>
	///	캐릭터 고유 인덱스
	///	</summary>
	public int player_character_id => _player_character_id;
	int _player_character_id;

	///	<summary>
	///	캐릭터 프리팹 키
	///	</summary>
	public string actor_prefab_key => _actor_prefab_key;
	string _actor_prefab_key;

	///	<summary>
	///	배경 프리팹 키
	///	</summary>
	public string background_prefab_key => _background_prefab_key;
	string _background_prefab_key;

	///	<summary>
	///	프리팹 키
	///	</summary>
	public string prefab_key => _prefab_key;
	string _prefab_key;

	///	<summary>
	///	인트로 키
	///	</summary>
	public string intro_key => _intro_key;
	string _intro_key;

	///	<summary>
	///	시작 상태
	///	</summary>
	public int state_id => _state_id;
	int _state_id;

	///	<summary>
	///	순서
	///	</summary>
	public int order => _order;
	int _order;

	private bool disposed = false;

	public Me_Resource_Data(Raw_Me_Resource_Data raw_data)
	{
		_memorial_id = raw_data.memorial_id;
		_player_character_id = raw_data.player_character_id;
		_actor_prefab_key = raw_data.actor_prefab_key;
		_background_prefab_key = raw_data.background_prefab_key;
		_prefab_key = raw_data.prefab_key;
		_intro_key = raw_data.intro_key;
		_state_id = raw_data.state_id;
		_order = raw_data.order;
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

